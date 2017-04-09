// Copyright 2016 Google Inc. All rights reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using UnityEngine;
using UnityEngine.UI;
using Merge;

public class MergeControllerDemoManager : MonoBehaviour
{
	public GameObject controllerPivot;

	public Material cubeInactiveMaterial;
	public Material cubeHoverMaterial;
	public Material cubeActiveMaterial;

	private Renderer controllerCursorRenderer;

	// Currently selected GameObject.
	private GameObject selectedObject;

	// True if we are dragging the currently selected GameObject.
	private bool dragging;

	void Update()
	{
		//		print("Google: " + GvrController.Gyro + ", Merge: " + MSDK.Gyro);
		UpdatePointer();
		//		UpdateStatusMessage();
	}

	private void UpdatePointer()
	{
		if (MSDK.State != MergeConnectionState.Connected)
		{
			controllerPivot.SetActive(false);
		}
		controllerPivot.SetActive(true);
		controllerPivot.transform.rotation = MSDK.Orientation;

		if (dragging)
		{
			if (MSDK.AppButtonUp)
			{
				EndDragging();
			}
		}
		else
		{
			RaycastHit hitInfo;
			Vector3 rayDirection = MSDK.Orientation * Vector3.forward;
			if (Physics.Raycast(Vector3.zero, rayDirection, out hitInfo))
			{
				if (hitInfo.collider && hitInfo.collider.gameObject)
				{
					SetSelectedObject(hitInfo.collider.gameObject);
				}
			}
			else
			{
				SetSelectedObject(null);
			}
			if (MSDK.AppButtonDown && selectedObject != null)
			{
				StartDragging();
			}
		}
	}

	private void SetSelectedObject(GameObject obj)
	{
		if (null != selectedObject)
		{
			selectedObject.GetComponent<Renderer>().material = cubeInactiveMaterial;
		}
		if (null != obj)
		{
			obj.GetComponent<Renderer>().material = cubeHoverMaterial;
		}
		selectedObject = obj;
	}

	private void StartDragging()
	{
		dragging = true;
		selectedObject.GetComponent<Renderer>().material = cubeActiveMaterial;

		// Reparent the active cube so it's part of the ControllerPivot object. That will
		// make it move with the controller.
		selectedObject.transform.SetParent(controllerPivot.transform, true);
	}

	private void EndDragging()
	{
		dragging = false;
		selectedObject.GetComponent<Renderer>().material = cubeHoverMaterial;

		// Stop dragging the cube along.
		selectedObject.transform.SetParent(null, true);
	}
}
