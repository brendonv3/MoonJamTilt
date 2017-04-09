using UnityEngine;
using System.Collections;

using Merge;

/// <summary>
/// This is a utility class which serves as a Facade to all motion controller input. Instead of calling for input from one specific class,
/// use this class instead to support different motion controllers all in one place.
/// </summary>
public class GenericMotionController : MonoBehaviour
{
	private static GenericMotionController instance;

	public static MotionControllerConnectionState State
	{
		get
		{
			return (MotionControllerConnectionState)(int)MSDK.State;
		}
	}

	public static Quaternion Orientation
	{
		get
		{
			return MSDK.Orientation;
		}
	}
	public static Vector3 Gyro
	{
		get
		{
			return MSDK.Gyro;
		}
	}
	public static Vector3 Accel
	{
		get
		{
			return MSDK.Accel;
		}
	}
	// TODO: Implement IsTouching
	public static bool IsTouching
	{
		get
		{
			return MSDK.IsTouching;
		}
	}
	// TODO: Implement TouchDown
	public static bool TouchDown
	{
		get
		{
			return MSDK.TouchDown;
		}
	}
	// TODO: Implement TouchUp
	public static bool TouchUp
	{
		get
		{
			return MSDK.TouchUp;
		}
	}
	// TODO: Implment TouchPos
	public static Vector2 TouchPos
	{
		get
		{
			return MSDK.TouchPos;
		}
	}
	// TODO: Implement Recentering
	public static bool Recentering
	{
		get
		{
			return MSDK.Recentering;
		}
	}
	// TODO: Implement Recentered
	public static bool Recentered
	{
		get
		{
			return MSDK.Recentered;
		}
	}
	// TODO: Implement ClickButton
	public static bool ClickButton
	{
		get
		{
			return MSDK.ClickButton;
		}
	}
	// TODO: Implement ClickButtonDown
	public static bool ClickButtonDown
	{
		get
		{
			return MSDK.ClickButtonDown;
		}
	}
	// TODO: ClickButtonUp
	public static bool ClickButtonUp 
	{
		get
		{
			return MSDK.ClickButtonUp;
		}
	}
	public static bool AppButton
	{
		get
		{
			return MSDK.AppButton;
		}
	}
	public static bool AppButtonDown
	{
		get
		{
			return MSDK.AppButtonDown;
		}
	}
	public static bool AppButtonUp
	{
		get
		{
			return MSDK.AppButtonUp;
		}
	}

	void Awake()
	{
		if (instance != null)
		{
			this.enabled = false;
			return;
		}
		instance = this;
	}

	void OnDestroy()
	{
		instance = null;
	}
}

public enum MotionControllerConnectionState
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

