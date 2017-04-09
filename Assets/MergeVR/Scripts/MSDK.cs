// =======================================================================================
// Copyright (c) 2014, Merge Labs Inc.
// =======================================================================================
using System;
using UnityEngine;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Collections;

namespace Merge
{
	public enum MergeConnectionState
	{
		/// Indicates that the controller is disconnected.
		Disconnected,
		/// Indicates that the device is scanning for controllers.
		Scanning,
		/// Indicates that the device is connecting to a controller.
		Connecting,
		/// Indicates that the device is connected to a controller.
		Connected,
		/// Indicates that an error has occurred.
		Error,
	};

	/// <summary>
	/// MSDK provides access to the Merge Universal Motion Controller.
	/// </summary>
	public class MSDK : Singleton<MSDK>
	{
		public MSDK()
		{
			// Configure our Singleton features - we want them all enabled!
			//
			// This will spawn a GameObject with MSDK attached if one doesn't already exist in
			// the scene.
			singletonAutoCreateInstance = true;
			// This will make sure one and only one copy of the MSDK component is in the scene at
			// any given time.
			singletonEnforceSingleInstance = true;
			// This will keep the MSDK component's object alive through scene changes.
			singletonPersistInstance = true;
		}

		/// <summary>
		/// Returns the controller's current connection state.
		/// </summary>
		/// <value>The state of the controller.</value>
		public static MergeConnectionState State
		{
			get
			{
				return instance.GetController().cData.Connected == true ? MergeConnectionState.Connected : MergeConnectionState.Error;
			}
		}

		/// <summary>
		/// Returns the controller's current orientation in space, as a quaternion.
		/// </summary>
		/// <value>The orientation of the controller.</value>
		public static Quaternion Orientation
		{
			get
			{
				return instance != null ? GetOrientation(0) : Quaternion.identity;
			}
		}
		// TODO: Pull real gyro data in
		/// <summary>
		/// Returns the controller's gyroscope reading.
		/// </summary>
		/// <value>The controller's gyroscope reading.</value>
		public static Vector3 Gyro
		{
			get
			{
				return instance != null ? Orientation.eulerAngles - instance.lastEuler : Vector3.zero;
//				return instance != null ? Instance.GetController().cData.Gyro : Vector3.zero;
//				return instance != null ? Vector3.zero : Vector3.zero;
			}
		}

		/// <summary>
		/// Returns the controller's accelerometer reading.
		/// </summary>
		/// <value>The controller's current accelorometer reading.</value>
		public static Vector3 Accel
		{
			get
			{
				return instance != null ? GetAccel(0) : Vector3.zero;
			}
		}

		// TODO: Implement IsTouching
		/// <summary>
		/// If true, the user is currently touching the controller's touchpad.
		/// </summary>
		/// <value><c>true</c> if the user is touching the touchpad; otherwise, <c>false</c>.</value>
		public static bool IsTouching
		{
			get
			{
				return instance != null ? false : false;
			}
		}

		// TODO: Implement TouchDown
		/// <summary>
		/// If true, the user just started touching the touchpad.
		/// </summary>
		/// <value><c>true</c> if user just started touching the touchpad; otherwise, <c>false</c>.</value>
		public static bool TouchDown
		{
			get
			{
				return instance != null ? false : false;
			}
		}

		// TODO: Implement TouchUp
		/// <summary>
		/// If true, the user just stopped touching the touchpad.
		/// </summary>
		/// <value><c>true</c> if the user just stopped touching the touchpad; otherwise, <c>false</c>.</value>
		public static bool TouchUp
		{
			get
			{
				return instance != null ? false : false;
			}
		}

		// TODO: Implement TouchPos
		/// <summary>
		/// Returns the position of the user's touch.
		/// </summary>
		/// <value>The position of the user's touch.</value>
		public static Vector2 TouchPos
		{
			get
			{
				return instance != null ? Vector2.zero : Vector2.zero;
			}
		}

		// TODO: Implement Recentering
		/// <summary>
		/// If true, the user is currently performing the recentering gesture.
		/// </summary>
		/// <value><c>true</c> if recentering; otherwise, <c>false</c>.</value>
		public static bool Recentering
		{
			get
			{
				return instance != null ? false : false;
			}
		}

		// TODO: Implement Recentered
		/// <summary>
		/// If true, the user just completed the recenter gesture.
		/// </summary>
		/// <value><c>true</c> if recentered; otherwise, <c>false</c>.</value>
		public static bool Recentered
		{
			get
			{
				return instance != null ? false : false;
			}
		}

		/// <summary>
		/// If true, the click button (touchpad button) is currently being pressed.
		/// </summary>
		/// <value><c>true</c> if the click button (touchpad button) is currently being pressed; otherwise, <c>false</c>.</value>
		public static bool ClickButton
		{
			get
			{
				return instance != null ? GetClickButton(0) : false;
			}
		}

		/// <summary>
		/// If true, the click button (touchpad button) was just pressed.
		/// </summary>
		/// <value><c>true</c> if the click button (touchpad button) was just pressed; otherwise, <c>false</c>.</value>
		public static bool ClickButtonDown
		{
			get
			{
				return instance != null ? GetClickButtonDown(0) : false;
			}
		}

		/// <summary>
		/// If true, the click button (touchpad button) was just released.
		/// </summary>
		/// <value><c>true</c> if the click button (touchpad button) was just released; otherwise, <c>false</c>.</value>
		public static bool ClickButtonUp 
		{
			get
			{
				return instance != null ? GetClickButtonUp(0) : false;
			}
		}

		/// <summary>
		/// If true, the app button is currently being pressed.
		/// </summary>
		/// <value><c>true</c> if the app button is currently being pressed; otherwise, <c>false</c>.</value>
		public static bool AppButton
		{
			get
			{
				return instance != null ? GetAppButton(0) : false;
			}
		}

		/// <summary>
		/// If true, the app button was just pressed.
		/// </summary>
		/// <value><c>true</c> if the app button was just pressed; otherwise, <c>false</c>.</value>
		public static bool AppButtonDown
		{
			get
			{
				return instance != null ? GetAppButtonDown(0) : false;
			}
		}

		/// <summary>
		/// If true, the app button was just released.
		/// </summary>
		/// <value><c>true</c> if the app button was just released; otherwise, <c>false</c>.</value>
		public static bool AppButtonUp
		{
			get
			{
				return instance != null ? GetAppButtonUp(0) : false;
			}
		}

		/// <summary>
		/// Returns the specified controller's current connection state.
		/// </summary>
		/// <returns>The specified controller's current connection state.</returns>
		/// <param name="controllerIndex">The index of the controller.</param>
		public static MergeConnectionState GetState(int controllerIndex)
		{
			return instance.GetController(controllerIndex).cData.Connected == true ? MergeConnectionState.Connected : MergeConnectionState.Error;
		}

		/// <summary>
		/// Returns the specified controller's current orientation in space, as a quaternion.
		/// </summary>
		/// <returns>The specified controller's current orientation in space.</returns>
		/// <param name="controllerIndex">The index of the controller.</param>
		public static Quaternion GetOrientation(int controllerIndex)
		{
			Quaternion rawRotation = Instance.GetController(controllerIndex).cData.FusedSensorOrientation;
			return instance != null ? Quaternion.Euler(new Vector3(
				rawRotation.eulerAngles.x,
				rawRotation.eulerAngles.y + Instance.yawOffset,
				rawRotation.eulerAngles.z)) : Quaternion.identity;
		}

		/// <summary>
		/// Returns the specified controller's gyroscope reading.
		/// </summary>
		/// <returns>The specified controller's gyroscope reading.</returns>
		/// <param name="controllerIndex">The index of the controller.</param>
		public static Vector3 GetGyro(int controllerIndex)
		{
			return instance != null ? Vector3.zero : Vector3.zero;
		}

		/// <summary>
		/// Returns specified the controller's accelerometer reading.
		/// </summary>
		/// <returns>The specified the controller's accelerometer reading.</returns>
		/// <param name="controllerIndex">The index of the controller.</param>
		public static Vector3 GetAccel(int controllerIndex)
		{
			return instance != null ? instance.GetController(controllerIndex).cData.LinearAcceleration : Vector3.zero;
		}
		// TODO: Implement IsTouching
		/// <summary>
		/// If true, the user is currently touching the specified controller's touchpad.
		/// </summary>
		/// <returns><c>true</c>, if the user is currently touching the specified controller's touchpad, <c>false</c> otherwise.</returns>
		/// <param name="controllerIndex">The index of the controller.</param>
		public static bool GetIsTouching(int controllerIndex)
		{
			return instance != null ? false : false;
		}
		// TODO: Implement TouchDown
		/// <summary>
		/// If true, the user just started touching the specified controller's touchpad.
		/// </summary>
		/// <returns><c>true</c>, if the user just started touching the specified controller's touchpad, <c>false</c> otherwise.</returns>
		/// <param name="controllerIndex">The index of the controller.</param>
		public static bool GetTouchDown(int controllerIndex)
		{
			return instance != null ? false : false;
		}
		// TODO: Implement TouchUp
		/// <summary>
		/// If true, the user just stopped touching the specified controller's touchpad.
		/// </summary>
		/// <returns><c>true</c>, if the user just stopped touching the specified controller's touchpad, <c>false</c> otherwise.</returns>
		/// <param name="controllerIndex">The index of the controller.</param>
		public static bool GetTouchUp(int controllerIndex)
		{
			return instance != null ? false : false;
		}
		// TODO: Implment TouchPos
		/// <summary>
		/// Returns the position of the user's touch on the specified controller.
		/// </summary>
		/// <returns>The position of the user's touch on the specified controller.</returns>
		/// <param name="controllerIndex">The index of the controller.</param>
		public static Vector2 GetTouchPos(int controllerIndex)
		{
			return instance != null ? Vector2.zero : Vector2.zero;
		}

		/// <summary>
		/// If true, the click button (touchpad button) is currently being pressed on the specified controller.
		/// </summary>
		/// <returns><c>true</c>, if the click button (touchpad button) is currently being pressed on the specified controller, <c>false</c> otherwise.</returns>
		/// <param name="controllerIndex">The index of the controller.</param>
		public static bool GetClickButton(int controllerIndex)
		{
			return instance != null ? instance.GetController(controllerIndex).GetButton("z") : false;
		}

		/// <summary>
		/// If true, the click button (touchpad button) was just pressed on the specified controller.
		/// </summary>
		/// <returns><c>true</c>, if the click button (touchpad button) was just pressed on the specified controller, <c>false</c> otherwise.</returns>
		/// <param name="controllerIndex">The index of the controller.</param>
		public static bool GetClickButtonDown(int controllerIndex)
		{
			return instance != null ? instance.GetController(controllerIndex).GetButtonDown("z") : false;
		}

		/// <summary>
		/// If true, the click button (touchpad button) was just released on the specified controller.
		/// </summary>
		/// <returns><c>true</c>, if the click button (touchpad button) was just released on the specified controller, <c>false</c> otherwise.</returns>
		/// <param name="controllerIndex">The index of the controller.</param>
		public static bool GetClickButtonUp(int controllerIndex)
		{
			return instance != null ? instance.GetController(controllerIndex).GetButtonUp("z") : false;
		}

		/// <summary>
		/// If true, the app button is currently being pressed on the specified controller.
		/// </summary>
		/// <returns><c>true</c>, if the app button is currently being pressed on the specified controller, <c>false</c> otherwise.</returns>
		/// <param name="controllerIndex">The index of the controller.</param>
		public static bool GetAppButton(int controllerIndex)
		{
			return instance != null ? instance.GetController(controllerIndex).GetButton("app") : false;
		}

		/// <summary>
		/// If true, the app button was just pressed on the specified controller.
		/// </summary>
		/// <returns><c>true</c>, if the app button was just pressed on the specified controller, <c>false</c> otherwise.</returns>
		/// <param name="controllerIndex">The index of the controller.</param>
		public static bool GetAppButtonDown(int controllerIndex)
		{
			return instance != null ? instance.GetController(controllerIndex).GetButtonDown("app") : false;
		}

		/// <summary>
		/// If true, the app button was just released on the specified controller.
		/// </summary>
		/// <returns><c>true</c>, if the app button was just released on the specified controller, <c>false</c> otherwise.</returns>
		/// <param name="controllerIndex">The index of the controller.</param>
		public static bool GetAppButtonUp(int controllerIndex)
		{
			return instance != null ? instance.GetController(controllerIndex).GetButtonUp("app") : false;
		}

		/// <summary>
		/// Gets the joystick's Y position.
		/// </summary>
		/// <value>The  joystick's Y position.</value>
		public static float JoystickY { get	{ return GetJoystickY(0); }	}

		/// <summary>
		/// Gets the joystick's X position.
		/// </summary>
		/// <value>The joystick's X position.</value>
		public static float JoystickX {	get	{ return GetJoystickX(0); }	}

		/// <summary>
		/// If true, the square button is currently being pressed.
		/// </summary>
		/// <value><c>true</c> if the square button is currently being pressed; otherwise, <c>false</c>.</value>
		public static bool SquareButton { get { return GetSquareButton(0); } }

		/// <summary>
		/// If true, the square button was just pressed.
		/// </summary>
		/// <value><c>true</c> if the square button was just pressed; otherwise, <c>false</c>.</value>
		public static bool SquareButtonDown	{ get { return GetSquareButtonDown(0); } }

		/// <summary>
		/// If true, the square button was just released.
		/// </summary>
		/// <value><c>true</c> if the square button was just released; otherwise, <c>false</c>.</value>
		public static bool SquareButtonUp { get { return GetSquareButtonUp(0); } }

		/// <summary>
		/// If true, the down button is currently being pressed.
		/// </summary>
		/// <value><c>true</c> if the down button is currently being pressed; otherwise, <c>false</c>.</value>
		public static bool DownButton { get { return GetDownButton(0); } }

		/// <summary>
		/// If true, the down button was just pressed.
		/// </summary>
		/// <value><c>true</c> if the down button was just pressed; otherwise, <c>false</c>.</value>
		public static bool DownButtonDown { get { return GetDownButtonDown(0); } }

		/// <summary>
		/// If true, the down button was just released.
		/// </summary>
		/// <value><c>true</c> if the down button was just released; otherwise, <c>false</c>.</value>
		public static bool DownButtonUp { get { return GetDownButtonUp(0); } }

		/// <summary>
		/// If true, the trigger1 button is being pressed.
		/// </summary>
		/// <value><c>true</c> if the trigger1 button is being pressed; otherwise, <c>false</c>.</value>
		public static bool TriggerOneButton { get { return GetTriggerOneButton(0); } }

		/// <summary>
		/// If true, the trigger1 button was just pressed.
		/// </summary>
		/// <value><c>true</c> if the trigger1 button was just pressed; otherwise, <c>false</c>.</value>
		public static bool TriggerOneButtonDown { get { return GetTriggerOneButtonDown(0); } }

		/// <summary>
		/// If true, the trigger1 button was just released.
		/// </summary>
		/// <value><c>true</c> if the trigger1 button was just released; otherwise, <c>false</c>.</value>
		public static bool TriggerOneButtonUp { get { return GetTriggerOneButtonUp(0); } }

		/// <summary>
		/// If true, the trigger2 button is being pressed.
		/// </summary>
		/// <value><c>true</c> if the trigger2 button is being pressed; otherwise, <c>false</c>.</value>
		public static bool TriggerTwoButton { get { return GetTriggerTwoButton(0); } }

		/// <summary>
		/// If true, the trigger2 button was just pressed.
		/// </summary>
		/// <value><c>true</c> if the trigger2 button was just pressed; otherwise, <c>false</c>.</value>
		public static bool TriggerTwoButtonDown { get { return GetTriggerTwoButtonDown(0); } }

		/// <summary>
		/// If true, the trigger2 button was just released.
		/// </summary>
		/// <value><c>true</c> if the trigger2 button was just released; otherwise, <c>false</c>.</value>
		public static bool TriggerTwoButtonUp { get { return GetTriggerTwoButtonUp(0); } }

		/// <summary>
		/// If true, the square button is being pressed on the specified controller.
		/// </summary>
		/// <returns><c>true</c>, if the square button is being pressed on the specified controller, <c>false</c> otherwise.</returns>
		/// <param name="controllerIndex">Controller index.</param>
		public static bool GetSquareButton(int controllerIndex)
		{
			return instance != null ? instance.GetController(controllerIndex).GetButton("square") : false;
		}

		/// <summary>
		/// If true, the square button was just released on the specified controller.
		/// </summary>
		/// <returns><c>true</c>, if the square button was just released on the specified controller, <c>false</c> otherwise.</returns>
		/// <param name="controllerIndex">Controller index.</param>
		public static bool GetSquareButtonUp(int controllerIndex)
		{
			return instance != null ? instance.GetController(controllerIndex).GetButtonUp("square") : false;
		}

		/// <summary>
		/// If true, the square button was just pressed on the specified controller.
		/// </summary>
		/// <returns><c>true</c>, if the square button was just pressed on the specified controller, <c>false</c> otherwise.</returns>
		/// <param name="controllerIndex">Controller index.</param>
		public static bool GetSquareButtonDown(int controllerIndex)
		{
			return instance != null ? instance.GetController(controllerIndex).GetButtonDown("square") : false;
		}

		/// <summary>
		/// If true, the down button is being pressed on the specified controller.
		/// </summary>
		/// <returns><c>true</c>, if the down button is being pressed on the specified controller, <c>false</c> otherwise.</returns>
		/// <param name="controllerIndex">Controller index.</param>
		public static bool GetDownButton(int controllerIndex)
		{
			return instance != null ? instance.GetController(controllerIndex).GetButton("down") : false;
		}

		/// <summary>
		/// If true, the down button was just pressed on the specified controller.
		/// </summary>
		/// <returns><c>true</c>, if the down button was just pressed on the specified controller, <c>false</c> otherwise.</returns>
		/// <param name="controllerIndex">Controller index.</param>
		public static bool GetDownButtonDown(int controllerIndex)
		{
			return instance != null ? instance.GetController(controllerIndex).GetButtonDown("down") : false;
		}

		/// <summary>
		/// If true, the down button was just released on the specified controller.
		/// </summary>
		/// <returns><c>true</c>, if the down button was just released on the specified controller, <c>false</c> otherwise.</returns>
		/// <param name="controllerIndex">Controller index.</param>
		public static bool GetDownButtonUp(int controllerIndex)
		{
			return instance != null ? instance.GetController(controllerIndex).GetButtonUp("down") : false;
		}

		/// <summary>
		/// If true, the trigger1 button is being pressed on the specified controller.
		/// </summary>
		/// <returns><c>true</c>, if the trigger1 button is being pressed on the specified controller, <c>false</c> otherwise.</returns>
		/// <param name="controllerIndex">Controller index.</param>
		public static bool GetTriggerOneButton(int controllerIndex)
		{
			return instance != null ? instance.GetController(controllerIndex).GetButton("trigger1") : false;
		}

		/// <summary>
		/// If true, the trigger1 button was just pressed on the specified controller.
		/// </summary>
		/// <returns><c>true</c>, if the trigger1 button was just pressed on the specified controller, <c>false</c> otherwise.</returns>
		/// <param name="controllerIndex">Controller index.</param>
		public static bool GetTriggerOneButtonDown(int controllerIndex)
		{
			return instance != null ? instance.GetController(controllerIndex).GetButtonDown("trigger1") : false;
		}

		/// <summary>
		/// If true, the trigger1 button was just released on the specified controller.
		/// </summary>
		/// <returns><c>true</c>, if the trigger1 button was just released on the specified controller, <c>false</c> otherwise.</returns>
		/// <param name="controllerIndex">Controller index.</param>
		public static bool GetTriggerOneButtonUp(int controllerIndex)
		{
			return instance != null ? instance.GetController(controllerIndex).GetButtonUp("trigger1") : false;
		}

		/// <summary>
		/// If true, the trigger2 button is being pressed on the specified controller.
		/// </summary>
		/// <returns><c>true</c>, if the trigger2 button is being pressed on the specified controller, <c>false</c> otherwise.</returns>
		/// <param name="controllerIndex">Controller index.</param>
		public static bool GetTriggerTwoButton(int controllerIndex)
		{
			return instance != null ? instance.GetController(controllerIndex).GetButton("trigger2") : false;
		}

		/// <summary>
		/// If true, the trigger2 button was just pressed on the specified controller.
		/// </summary>
		/// <returns><c>true</c>, if the trigger2 button was just pressed on the specified controller, <c>false</c> otherwise.</returns>
		/// <param name="controllerIndex">Controller index.</param>
		public static bool GetTriggerTwoButtonDown(int controllerIndex)
		{
			return instance != null ? instance.GetController(controllerIndex).GetButtonDown("trigger2") : false;
		}

		/// <summary>
		/// If true, the trigger2 button was just released on the specified controller.
		/// </summary>
		/// <returns><c>true</c>, if the trigger2 button was just released on the specified controller, <c>false</c> otherwise.</returns>
		/// <param name="controllerIndex">Controller index.</param>
		public static bool GetTriggerTwoButtonUp(int controllerIndex)
		{
			return instance != null ? instance.GetController(controllerIndex).GetButtonUp("trigger2") : false;
		}

		/// <summary>
		/// Gets the specified controller's joystick's Y position.
		/// </summary>
		/// <returns>The joystick's Y position.</returns>
		/// <param name="controllerIndex">Controller index.</param>
		public static float GetJoystickY(int controllerIndex)
		{
			return instance != null ? instance.GetController(controllerIndex).ControllerJoystickY : 0f;
		}

		/// <summary>
		/// Gets the specified controller's joystick's X position.
		/// </summary>
		/// <returns>The joystick's X position.</returns>
		/// <param name="controllerIndex">Controller index.</param>
		public static float GetJoystickX(int controllerIndex)
		{
			return instance != null ? instance.GetController(controllerIndex).ControllerJoystickX : 0f;
		}

		/// <summary>
		/// Gets the specified controller's joystick X and Y position as a Vector2.
		/// </summary>
		/// <returns>The joystick's X and Y position as a Vector2.</returns>
		/// <param name="controllerIndex">Controller index.</param>
		public static Vector2 GetJoystickPosition(int controllerIndex)
		{
			return new Vector2(GetJoystickX(controllerIndex), GetJoystickY(controllerIndex));
		}

		/// <summary>
		/// A flag to keep track if the controllers have been initialized or not.
		/// </summary>
		private static bool ControllersInitialized = false;

		/// <summary>
		/// A list of all the MergeVR Controllers connected to our device.
		/// </summary>
		public List< MergeController > controllerList = new List< MergeController >();

		/// <summary>
		/// A placeholder for when no controllers are connected.
		/// </summary>
		private MergeController placeholderController;

		/// <summary>
		/// Initializes the controllers. Must be called before controllers can be used.
		/// </summary>
		private void InitControllers()
		{
			if (!ControllersInitialized)
			{ 
				Merge.MergeCom.PerformMergeVRControllerInit();

				ControllersInitialized = true;
			}

			return;
		}

		/// <summary>
		/// Sets the requested controller count. 1 by default.
		/// </summary>
		/// <param name="count">Count.</param>
		public void SetRequestedControllerCount(int count)
		{
			if (ControllersInitialized)
			{
				Merge.MergeCom.SetRequestedControllerCount(count);
			}

			return;
		}

		/// <summary>
		/// Gets the number of controllers connected.
		/// </summary>
		/// <value>The number of controllers connected.</value>
		public int ControllerCount
		{
			get
			{
				if (ControllersInitialized)
					return Merge.MergeCom.QueryNumControllersConnected();
				else
					return 0;
			}
		}

		/// <summary>
		/// Gets the first controller.
		/// </summary>
		/// <returns>The first controller.</returns>
		public MergeController GetController()
		{
			return GetController(0);
		}

		/// <summary>
		/// Gets the specified controller.
		/// </summary>
		/// <returns>The specified controller.</returns>
		/// <param name="index">The index of the controller.</param>
		public MergeController GetController(int controllerIndex)
		{
			//check if mergecontroller object is defined for this controller
			if (controllerList.Count > controllerIndex && controllerList[controllerIndex] != null)
			{
				// controller already exists so return it
				return controllerList[controllerIndex];
			}
			else
			{
				// The controller doesn't exist in the list.  Let's see if we have one
				// actually connected.
				var controllerCount = ControllerCount;
				if (controllerCount >= (controllerIndex + 1))
				{
					// Yes, we have real controllers available for use.
					MergeController newController = new MergeController(controllerIndex);
					controllerList.Add(newController);
					Debug.Log("Controller added: " + controllerIndex);
					return newController;
				}
				else
				{
					// We don't have any real controllers connected.  Return a "placeholder"
					// MergeController object, instead.
					if (placeholderController == null)
					{
						placeholderController = new MergeController(-1);
					}
					return placeholderController;
				}
			}	
		}

		void Awake()
		{
			InitControllers();
		}

		private float yawOffset = 0f; 
		void Update()
		{
			Merge.MergeCom.Update();

			// Recenter
			if (Instance.GetController().GetButton("home"))
			{
				
				GvrViewer.Instance.Recenter ();

				Quaternion rawRotation = GetController().cData.FusedSensorOrientation;

				//var controllerRot = controllerRotation;	// rawRotation
				yawOffset = Mathf.DeltaAngle(rawRotation.eulerAngles.y, 0f);
			}
		}

		// This is temporarily here to help mimic daydreams gyro data.
		// TODO: Remove this when actual gyro data is passed in
		Vector3 lastEuler = Vector3.zero;
		void LateUpdate()
		{
			lastEuler = Orientation.eulerAngles;
		}
			
		void OnApplicationQuit()
		{
			Merge.MergeCom.OnApplicationQuit();
		}
	}
}
