using UnityEngine;
using UnityEngine.UI;
using Merge;

public class Controllers2 : MonoBehaviour
{
	public Text completeDataText;

	public GameObject controller;

	// Update is called once per frame
	void Update()
	{
		controller.transform.rotation = GenericMotionController.Orientation;

		UpdateUI();
	}

	void UpdateUI()
	{
		if (MSDK.State == MergeConnectionState.Connected)
		{
			Vector3 cachedControllerLinear = MSDK.Accel;

			completeDataText.text = "\n Joystick " + Merge.MSDK.JoystickX + ", " + Merge.MSDK.JoystickY;

			completeDataText.text += "\n Click: " + MSDK.ClickButton;
			completeDataText.text += "\n App: " + MSDK.AppButton;
			completeDataText.text += "\n Trigger1: " + MSDK.TriggerOneButton;
			completeDataText.text += "\n Trigger2: " + MSDK.TriggerTwoButton;

			completeDataText.text += "\n Linear: " + cachedControllerLinear.ToString();
			completeDataText.text += "\n Orientation: " + MSDK.Orientation;
			completeDataText.text += "\n Euler: " + MSDK.Orientation.eulerAngles;
		}
		else
		{
			completeDataText.text = "Waiting for Controller to connect...";
		}
	}
}


