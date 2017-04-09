using System;
using System.Runtime.InteropServices;
using SimpleJSON;
using UnityEngine;
using System.Collections;
using WinDataProcess;

namespace Merge
{
	/// <summary>
	/// Handles I/O to the Native plugins providing controller support to the SDK.  
	/// 
	/// Important methods:
	/// 
	/// IsControllerConnected - Check to see if ANY controller is connected, this will ensure at least
	/// controller #0 is active.
	/// 
	/// QueryNumControllersConnected - Returns the number of controllers that are actively connected.
	/// 
	/// QueryControllerData - Returns a snapshot of a given controller's state.  Pass in a connected controller's
	/// index, which you can validate via QueryNumControllersConnected().
	/// </summary>
	public class MergeCom
	{
		/// <summary>
		/// Go through all the controllers, calling UpdateController with their latest data.
		/// </summary>
		static public void Update()
		{
			if (IsControllerConnected(0))
			{
				int curControllerCount = QueryNumControllersConnected();

				// For each connected controller...
				for (int i = 0; i < curControllerCount; i++)
				{
					// Update the controller data.
					MergeController controller = Merge.MSDK.Instance.controllerList[i];
					ControllerData dat = QueryControllerData(i);
					controller.UpdateController(dat);

					// Question - can controllers become UN-calibrated?
					if (dat.calibration_status == 0)
					{
						dat.calibrated = true;
					}
				}
			}
		}

		static public void OnApplicationQuit()
		{
			PerformMergeVRExit();
		}

		////////////
		// Platform-agnostic helper functions.
		// Wraps the #ifdefs for each platform, so that the main logic only 
		// needs to call these functions.
		////////////

		#if (UNITY_ANDROID && !UNITY_EDITOR)
			public static AndroidJavaClass androidClass;
			private int myInt;
			private static AndroidJavaObject activityContext = null;
		#elif UNITY_IPHONE && !(UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX)
			[DllImport( "__Internal")] private static extern int mergeVRInit ();
			[DllImport( "__Internal")] private static extern int mergeVRGetControllerCount ();
			[DllImport( "__Internal")] private static extern ControllerData mergeVRGetControllerData (int sensorIndex);
			[DllImport( "__Internal")] private static extern int mergeVRExit ();
//			[DllImport( "__Internal")] private static extern BundleIdentifierData mergeGetBundleIdentifer ();
			[DllImport( "__Internal")] private static extern void mergeVRSetMaxControllerCount (int count);
		#elif (UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX)
			// TODO: Might be able to remove these
			[DllImport("libMergeVROSX")] private static extern int mergeVRInitMac();
			[DllImport("libMergeVROSX")] private static extern int mergeVRInit();
			[DllImport("libMergeVROSX")] private static extern int mergeVRGetControllerCount();
			[DllImport("libMergeVROSX")] private static extern ControllerData mergeVRGetControllerData(int sensorIndex);
			[DllImport("libMergeVROSX")] private static extern int mergeVRExit();
//			[DllImport("libMergeVROSX")] private static extern BundleIdentifierData mergeGetBundleIdentifer();
			[DllImport("libMergeVROSX")] private static extern void mergeVRSetMaxControllerCount(int count);
		#elif (UNITY_EDITOR_WIN)
		[DllImport("BLE.dll",CallingConvention = CallingConvention.StdCall)] static extern int GetDeviceState();
		[DllImport("BLE.dll",CallingConvention = CallingConvention.StdCall)] static extern int GetStructData(IntPtr PData,uint iSize);
		#endif

		/// <summary>
		/// Determines if the specified controllers is connected.
		/// </summary>
		/// <returns><c>true</c> if the specified controllers is connected; otherwise, <c>false</c>.</returns>
		/// <param name="index">The index of the controller.</param>
		public static bool IsControllerConnected(int index)
		{
			return (QueryNumControllersConnected() > index);
		}

		/// <summary>
		/// Sets the requested controller count.
		/// </summary>
		/// <param name="count">The number of requested controllers.</param>
		public static void SetRequestedControllerCount(int count)
		{
			#if (UNITY_ANDROID && !UNITY_EDITOR)
			//call not required for Android
			return;
			#elif (UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX || UNITY_IPHONE)
			mergeVRSetMaxControllerCount(count);
			#elif (UNITY_EDITOR_WIN)
			//WTP2.0
			//Current windows only support 1 controller, later version will support more;
			return;
			#else
			return;
			#endif
		}

		/// <summary>
		/// Queries the number of controllers connected.
		/// </summary>
		/// <returns>The number of controllers connected.</returns>
		public static int QueryNumControllersConnected()
		{
			#if (UNITY_ANDROID && !UNITY_EDITOR) 
			int ControllerCount;

			if (androidClass == null)
				ControllerCount=0;
			else
				ControllerCount = androidClass.CallStatic<int> ("mergeVRGetControllerCount");	

			return ControllerCount;
			#elif (UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX || UNITY_IPHONE)
			return mergeVRGetControllerCount();
			#elif (UNITY_EDITOR_WIN)
			//WTP2.0
			return  WinControllerCount();
			#else
				return 0;
			#endif
		}

		/// <summary>
		/// Queries the controller data.
		/// </summary>
		/// <returns>The controller data.</returns>
		/// <param name="controllerNum">Controller number.</param>
		public static ControllerData QueryControllerData(int controllerNum)
		{
			//Debug.Log ("queryControllerData- Getting Controller "+controllerNum+"'s data.  There are "+QueryNumControllersConnected()+" controllers.");
			#if (UNITY_ANDROID && !UNITY_EDITOR)
			object[] args = { controllerNum };
			String controllerDataStr = androidClass.CallStatic<String> ("mergeVRGetControllerData", args);
			var V = JSON.Parse (controllerDataStr);
			ControllerData controllerDataToReturn = new ControllerData ();
			controllerDataToReturn.UUID = V ["UUID"];
			controllerDataToReturn.Buttons = V ["Buttons"].AsInt;
			controllerDataToReturn.RSSI = V ["RSSI"].AsFloat;
			controllerDataToReturn.Connected = V ["Connected"].AsBool;
			controllerDataToReturn.calibrated = V ["Calibrated"].AsBool;
			controllerDataToReturn.FusedSensorOrientation = new Quaternion (V ["MergeVRQuaternion_X"].AsFloat, V ["MergeVRQuaternion_Y"].AsFloat, V ["MergeVRQuaternion_Z"].AsFloat, V ["MergeVRQuaternion_W"].AsFloat);
			controllerDataToReturn.LinearAcceleration = new Vector3 (V ["LinearAcceleration_X"].AsFloat, V ["LinearAcceleration_Y"].AsFloat, V ["LinearAcceleration_Z"].AsFloat);			
			controllerDataToReturn.xJoyStick = V ["xJoyStick"].AsFloat;
			controllerDataToReturn.yJoyStick = V ["yJoyStick"].AsFloat;
			controllerDataToReturn.firmwareVersion = V ["FirmwareVersion"];
			controllerDataToReturn.BatteryLevel = V ["Battery"].AsInt;
//			controllerDataToReturn = androidClass.CallStatic<ControllerData> ("mergeVRGetControllerDataStruct", args);
			return controllerDataToReturn;
			#elif (!UNITY_EDITOR && (UNITY_IPHONE)) || (UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX)
			ControllerData controllerData = mergeVRGetControllerData(controllerNum);
			return controllerData;
			#elif (UNITY_EDITOR_WIN)
			//WTP2.0
			return GetControllerData();
			#else
				ControllerData controllerDataPC = new ControllerData();
				return controllerDataPC;
			#endif
		}

		/// <summary>
		/// Initializes controllers and begins searching for controllers.
		/// </summary>
		public static void PerformMergeVRControllerInit()
		{
			#if (UNITY_ANDROID && !UNITY_EDITOR)

			AndroidJNI.AttachCurrentThread ();
			androidClass = new AndroidJavaClass ("com.mergelabs.MergeVR.MergeVRBridge");
			
			using (AndroidJavaClass activityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
				{

				activityContext = activityClass.GetStatic<AndroidJavaObject> ("currentActivity");
				
				androidClass.CallStatic ("setContext", activityContext);
				androidClass.CallStatic<int> ("mergeVRInit");

				}

			#elif (!UNITY_EDITOR && (UNITY_IPHONE)) || (UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX)
			mergeVRInit();
			#elif (UNITY_EDITOR_WIN)
			//WTP2.0
			InitWin();
			#endif	
		}

		/// <summary>
		/// Performs the merge VR exit and releases pointers to controllers.
		/// </summary>
		/// <returns>Exit code 0 if succeeds, -1 otherwise.</returns>
		public static int PerformMergeVRExit()
		{
			#if (UNITY_ANDROID && !UNITY_EDITOR) 
			return androidClass.CallStatic<int> ("mergeVRExit");
			#elif (!UNITY_EDITOR && (UNITY_IPHONE)) || (UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX)
			return mergeVRExit();
			#elif (UNITY_EDITOR_WIN)
			//WTP2.0
			//Will perform exit later
			return 0;
			#else
				return 0;
			#endif
		}


		#if UNITY_EDITOR_WIN
		//Temp Windows Data Process
		static ControllerData lastData = new ControllerData ();
		static MergeDataProcess dataProcess = new MergeDataProcess ();
		const int BuffSize = 20000;
		static IntPtr PBuffer;
		static bool isWinConnect = false;
		static void InitWin () {
			PBuffer = Marshal.AllocHGlobal(BuffSize);
			isWinConnect = GetControllerState ();
		}
		static int WinControllerCount(){
			if (!isWinConnect) {
				isWinConnect = GetControllerState ();
				return isWinConnect?1:0;
			} else {
				return 1;
			}
		}
		static public bool GetControllerState(){
			int tptp = GetDeviceState ();
			Debug.LogWarning ("Get Controller State Start = "+tptp);

			if (tptp == 0){
				return true;
			} else{
				return false;
			}
		}
		static public ControllerData GetControllerData()
		{
			int iRet;
			do {
				iRet = GetStructData (PBuffer, BuffSize);
			} while (iRet == 1000);
			if (iRet > 0) {
				IntPtr PLast = (IntPtr)((uint)PBuffer + (iRet - 1) * 20);
				lastData = ProcessData (PLast); 
			} else if (iRet == 0) {
				//no data
			} else {
				Debug.LogError ("Get Controller Data Error!! Should Not Happen!");
			}
			return lastData;
		}
		static ControllerData ProcessData(IntPtr PLast){
			ControllerData rcData = new ControllerData ();
			rcData.FusedSensorOrientation.w = dataProcess.ProcessData (new byte[] {
				Marshal.ReadByte (PLast, 1),
				Marshal.ReadByte (PLast, 0)
			}, MergeDataProcess.DataTypeEnum.qua);
			rcData.FusedSensorOrientation.x = dataProcess.ProcessData (new byte[] {
				Marshal.ReadByte (PLast, 3),
				Marshal.ReadByte (PLast, 2)
			}, MergeDataProcess.DataTypeEnum.qua);
			rcData.FusedSensorOrientation.y = dataProcess.ProcessData (new byte[] {
				Marshal.ReadByte (PLast, 5),
				Marshal.ReadByte (PLast, 4)
			}, MergeDataProcess.DataTypeEnum.qua);
			rcData.FusedSensorOrientation.z = dataProcess.ProcessData (new byte[] {
				Marshal.ReadByte (PLast, 7),
				Marshal.ReadByte (PLast, 6)
			}, MergeDataProcess.DataTypeEnum.qua);

			rcData.LinearAcceleration.x = dataProcess.ProcessData (new byte[] {
				Marshal.ReadByte (PLast, 9),
				Marshal.ReadByte (PLast, 8)
			}, MergeDataProcess.DataTypeEnum.lia);
			rcData.LinearAcceleration.y = dataProcess.ProcessData (new byte[] {
				Marshal.ReadByte (PLast, 11),
				Marshal.ReadByte (PLast, 10)
			}, MergeDataProcess.DataTypeEnum.lia);
			rcData.LinearAcceleration.z = dataProcess.ProcessData (new byte[] {
				Marshal.ReadByte (PLast, 13),
				Marshal.ReadByte (PLast, 12)
			}, MergeDataProcess.DataTypeEnum.lia);

			rcData.Buttons = dataProcess.ProcessBtn (new byte[] {
				Marshal.ReadByte (PLast, 15),
				Marshal.ReadByte (PLast, 14)
			});
			rcData.xJoyStick = dataProcess.ProcessData (new byte[] {
				Marshal.ReadByte (PLast, 19),
				Marshal.ReadByte (PLast, 18)
			}, MergeDataProcess.DataTypeEnum.joy);
			rcData.yJoyStick = dataProcess.ProcessData (new byte[] {
				Marshal.ReadByte (PLast, 17),
				Marshal.ReadByte (PLast, 16)
			}, MergeDataProcess.DataTypeEnum.joy);
			rcData.Connected = true;
			return rcData;
		}
		#endif
	}
}
