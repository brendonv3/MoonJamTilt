using System;
using UnityEngine;

/*
 * MergeController.cs - Defines the MergeVR Controller and provides access to the controller
 * data; which buttons are pressed, the linear acceleration vector, the joystick position,
 * etc.
 */

namespace Merge
{
	/// <summary>
	/// Represents the controller data. It must match the struct in the plugins, so do not change.
	/// </summary>
	public struct ControllerData
	{
		/// <summary>
		/// The UUID.
		/// </summary>
		public string UUID;

//		public Vector3 Gyro;

		/// <summary>
		/// The fused sensor orientation.
		/// </summary>
		public Quaternion FusedSensorOrientation;

		/// <summary>
		/// The linear acceleration.
		/// </summary>
		public Vector3 LinearAcceleration;

		/// <summary>
		/// The state of all the buttons
		/// </summary>
		public int Buttons;

		/// <summary>
		/// The RSSI.
		/// </summary>
		public float RSSI;

		/// <summary>
		/// Whether or not the controller is connected
		/// </summary>
		public bool Connected;

		/// <summary>
		/// The x joy stick.
		/// </summary>
		public float xJoyStick;
		/// <summary>
		/// The y joy stick.
		/// </summary>
		public float yJoyStick;

		/// <summary>
		/// The battery level.
		/// </summary>
		public int BatteryLevel;

		/// <summary>
		/// The event code.
		/// </summary>
		public int EventCode;

		/// <summary>
		/// The event message.
		/// </summary>
		public string EventMessage;

		/// <summary>
		/// Whether or not the controller is calibrated.
		/// </summary>
		public bool calibrated;

		/// <summary>
		/// The firmware version.
		/// </summary>
		public string firmwareVersion;

		/// <summary>
		/// The calibration status.
		/// </summary>
		public int calibration_status;

		/// <summary>
		/// The cardboard status.
		/// </summary>
		public int cardboard_status;
	}

	/// <summary>
	/// Represents a button on the merge controller and it's 3 states.
	/// </summary>
	public struct MergeButton
	{
		/// <summary>
		/// True if the button is in it's KeyDown state, false otherwise.
		/// </summary>
		public bool KeyDown;
		/// <summary>
		/// True if the button is in it's OnKeyDown state, false otherwise.
		/// </summary>
		public bool OnKeyDown;
		/// <summary>
		/// True if the button is in it's OnKeyUp state, false otherwise.
		/// </summary>
		public bool OnKeyUp;

		/// <summary>
		/// Sets the state of the key.
		/// </summary>
		/// <value><c>true</c> if key is pressed; otherwise, <c>false</c>.</value>
		public bool key
		{
			set
			{
				if (value && !KeyDown)
				{ //Trigger KeyDown & OnKeyDown
					OnKeyDown = true;
					OnKeyUp = false;
					KeyDown = true;
				}
				else if (value && KeyDown)
				{ //Reset OnKeyDown
					OnKeyDown = false;
				}
				else if (!value && !KeyDown)
				{ //Reset OnKeyUp
					OnKeyUp = false;
				}
				else if (!value && KeyDown)
				{ //Trigger OnKeyUp
					OnKeyDown = false;
					OnKeyUp = true;
					KeyDown = false;
				}
			}
		}
	};

	/// <summary>
	/// Represents the Merge Universal Motion Controller.
	/// </summary>
	public class MergeController
	{
		// button mask flags
		[Flags]
		enum ButtonFlag
		{
			Z = 0x00000001, // Click/Z
			App = 0x00000002,
			Home = 0x00000004, // Home/Up
			Power = 0x00000020, // Power Button
			Square = 0x00000040, // Square
			Trigger1 = 0x00000008, // Trigger 1
			Trigger2 = 0x00000010, // Trigger 2
			Down = 0x00000080 // Down
		}
//		[Flags]
//		enum ButtonFlag
//		{
//			Up = 0x00000001, // Click/Z
//			Circle = 0x00000002,
//			Down = 0x00000004, // Home/Up
//			Square = 0x00000020, // Power Button
//			Trigger1 = 0x00000040, // Square
//			Trigger2 = 0x00000008, // Trigger 1
//			Z = 0x00000010, // Trigger 2
//			Home = 0x00000080 // Down
//		}

		/// <summary>
		/// In order to keep the usage of the SDK streamlined and less verbose,
		/// MergeController can now act as a 'dummy' controller, simply returning
		/// false for every inquiry.  This is due to a very large number of calls to
		/// MSDK.getController() without checking the return value.  If there are no
		/// controllers connected, and getController() is called, we'll return the
		/// dummy controller.
		/// </summary>
		private bool placeholder = false;

		/// <summary>
		/// The controller data for this controller.
		/// </summary>
		public ControllerData cData;

		/// <summary>
		/// Gets a value indicating whether this <see cref="Merge.MergeController"/> is calibrated.
		/// </summary>
		/// <value><c>true</c> if calibrated; otherwise, <c>false</c>.</value>
		public bool calibrated
		{
			get
			{ 
				if (cData.calibration_status == 0)
					return false;
				else
					return true;
			}
		}

		// The circle button.
		MergeButton buttonApp = new MergeButton();
		// The up button.
		MergeButton buttonHome = new MergeButton();
		// The square button.
		MergeButton buttonSquare = new MergeButton();
		// The down button.
		MergeButton buttonDown = new MergeButton();
		// The 1st trigger button.
		MergeButton buttonTrigger1 = new MergeButton();
		// The 2nd trigger button.
		MergeButton buttonTrigger2 = new MergeButton();
		// The z button.
		MergeButton buttonZ = new MergeButton();
		// The home button.
		MergeButton buttonPower = new MergeButton();
		// The threshold that triggers getbuttondjoystick event
		private float joystickButtonThreshold = 0.5f;
		// Joystick up event flag.
		MergeButton buttonJoyStickUp = new MergeButton();
		// Joystick down event flag.
		MergeButton buttonJoyStickDown = new MergeButton();
		// Joystick right event flag.
		MergeButton buttonJoyStickRight = new MergeButton();
		// Joystick left event flag.
		MergeButton buttonJoyStickLeft = new MergeButton();

		/// <summary>
		/// Initializes a new instance of the <see cref="Merge.MergeController"/> class.
		/// </summary>
		/// <param name="initControllerIndex">Init controller index.</param>
		public MergeController(int initControllerIndex)
		{
			// If we pass in -1 for the controller index, we're requesting a dummy
			// 'placeholder' controller that will return false or zeroes for everything.
			if (initControllerIndex == -1)
			{
				placeholder = true;
			}
		}

		/// <summary>
		/// Gets this controller's joystick y position.
		/// </summary>
		/// <value>The controller joystick y.</value>
		public float ControllerJoystickY
		{
			get{ return GetControllerJoystickY(); }
		}

		private float GetControllerJoystickY()
		{
			if (placeholder)
			{
				return 0f;
			}
			else
			{
				return cData.yJoyStick;
			}
		}

		/// <summary>
		/// Gets this controller's joystick x position.
		/// </summary>
		/// <value>The controller joystick x.</value>
		public float ControllerJoystickX
		{
			get{ return GetControllerJoystickX(); }
		}

		private float GetControllerJoystickX()
		{
			if (placeholder)
			{
				return 0f;
			}
			else
			{
				return cData.xJoyStick;
			}
		}

		/// <summary>
		/// Updates the controller.
		/// </summary>
		/// <param name="updateCData">The controller data struct to be filled with the controller data.</param>
		public void UpdateController(ControllerData updateCData)
		{
			cData = updateCData;

			buttonApp.key = GetButton("App");
			buttonHome.key = GetButton("Home");
			buttonSquare.key = GetButton("Square");
			buttonDown.key = GetButton("Down");
			buttonTrigger1.key = GetButton("Trigger1");
			buttonTrigger2.key = GetButton("Trigger2");
			buttonZ.key = GetButton("Z");
			buttonPower.key = GetButton("Power");

			buttonJoyStickUp.key = GetButton("JoystickUp");
			buttonJoyStickDown.key = GetButton("JoystickDown");
			buttonJoyStickRight.key = GetButton("JoystickRight");
			buttonJoyStickLeft.key = GetButton("JoystickLeft");
		}

		/// <summary>
		/// Returns true the first frame the user releases the virtual button identified by buttonName.
		/// </summary>
		/// <returns><c>true</c>, if button up was gotten, <c>false</c> otherwise.</returns>
		/// <param name="buttonName">The button whose up date we are checking.</param>
		public bool GetButtonUp(string buttonName)
		{
			bool keyUp = false;

			switch (buttonName.ToLower())
			{
				case "app":
					keyUp = buttonApp.OnKeyUp;
					break;

				case "z":
					keyUp = buttonZ.OnKeyUp;
					break;

				case "home":
					keyUp = buttonHome.OnKeyUp;
					break;

				case "square":
					keyUp = buttonSquare.OnKeyUp;
					break;

				case "down":
					keyUp = buttonDown.OnKeyUp;
					break;

				case "trigger1":
					keyUp = buttonTrigger1.OnKeyUp;
					break;

				case "trigger2":
					keyUp = buttonTrigger2.OnKeyUp;
					break;

				case "power":
					keyUp = buttonPower.OnKeyUp;
					break;
			}
			return keyUp;
		}

		/// <summary>
		/// Returns true during the frame the user pressed down the virtual button identified by buttonName.
		/// </summary>
		/// <returns><c>true</c>, if button down was gotten, <c>false</c> otherwise.</returns>
		/// <param name="buttonName">The button whose down state we are checking.</param>
		public bool GetButtonDown(string buttonName)
		{
			bool keyDown = false; //default

			switch (buttonName.ToLower())
			{
				case "joystickup":
					keyDown = buttonJoyStickUp.OnKeyDown;
					break;

				case "joystickdown":
					keyDown = buttonJoyStickDown.OnKeyDown;
					break;

				case "joystickright":
					keyDown = buttonJoyStickRight.OnKeyDown;
					break;

				case "joystickleft":
					keyDown = buttonJoyStickLeft.OnKeyDown;
					break;

				case "app":
					keyDown = buttonApp.OnKeyDown;
					break;

				case "z":
					keyDown = buttonZ.OnKeyDown;
					break;

				case "home":
					keyDown = buttonHome.OnKeyDown;
					break;

				case "square":
					keyDown = buttonSquare.OnKeyDown;
					break;

				case "down":
					keyDown = buttonDown.OnKeyDown;
					break;

				case "trigger1":
					keyDown = buttonTrigger1.OnKeyDown;
					break;

				case "trigger2":
					keyDown = buttonTrigger2.OnKeyDown;
					break;

				case "power":
					keyDown = buttonPower.OnKeyDown;
					break;
			}
			return keyDown;
		}

		/// <summary>
		/// Returns true while the virtual button identified by buttonName is held down.
		/// </summary>
		/// <returns><c>true</c>, if button was gotten, <c>false</c> otherwise.</returns>
		/// <param name="buttonName">The button whose down state we are checking.</param>
		public bool GetButton(string buttonName)
		{
			bool key = false; //default

			ButtonFlag buttonMask = (ButtonFlag)cData.Buttons;
			
			switch (buttonName.ToLower())
			{
				case "joystickup":
					if (cData.yJoyStick > joystickButtonThreshold)
						key = true;
					break;

				case "joystickdown":
					if (cData.yJoyStick < -joystickButtonThreshold)
						key = true;
					break;

				case "joystickright":
					if (cData.xJoyStick > joystickButtonThreshold)
						key = true;
					break;

				case "joystickleft":
					if (cData.xJoyStick < -joystickButtonThreshold)
						key = true;
					break;

				case "app":
					if ((buttonMask & ButtonFlag.App) == ButtonFlag.App)
					{
						key = true;
					}
					break;

				case "home":
					if ((buttonMask & ButtonFlag.Home) == ButtonFlag.Home)
					{
						key = true;
					}
					break;

				case "square":
					if ((buttonMask & ButtonFlag.Square) == ButtonFlag.Square)
					{
						key = true;
					}
					break;

				case "down":
					if ((buttonMask & ButtonFlag.Down) == ButtonFlag.Down)
					{
						key = true;
					}
					break;

				case "trigger1":
					if ((buttonMask & ButtonFlag.Trigger1) == ButtonFlag.Trigger1)
					{
						key = true;
					}
					break;

				case "trigger2":
					if ((buttonMask & ButtonFlag.Trigger2) == ButtonFlag.Trigger2)
					{
						key = true;
					}
					break;

				case "z":
					if ((buttonMask & ButtonFlag.Z) == ButtonFlag.Z)
					{
						key = true;
					}
					break;

				case "power":
					if ((buttonMask & ButtonFlag.Power) == ButtonFlag.Power)
					{
						key = true;
					}
					break;

				default:
					break;
			}
			return key; //default
		}
	}
}
