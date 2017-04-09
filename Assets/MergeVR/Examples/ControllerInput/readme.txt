This is the MergeVR Controller example.

The framework requires the Cardboard SDK for Unity  (available at https://developers.google.com/cardboard/unity/download - install it first before installing our package).

Controller2 script is attached to the Scene object - this script initializes the mergevr controllers in Start - Merge.MSDK.Instance.InitControllers();

Scene waits for a controller to connect, after one connects - then gives user chance to hit circle for one controller or wait for next controller.

Once either single or double controllers are active - models on screen match orientation of controller and debug window shows state of each controller.

ControllerDataLog script shows example of what data you can read and how to do it.




