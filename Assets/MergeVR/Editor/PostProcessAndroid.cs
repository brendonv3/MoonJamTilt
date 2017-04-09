using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using System.Text;
using System.Text.RegularExpressions;



public class PostProcessAndroid : MonoBehaviour {

	#if UNITY_ANDROID
	
	
	[PostProcessBuild]

	public static void OnPostProcessBuild(BuildTarget target, string path)
	{
		string xmlManifestPath = string.Format ("{0}{1}", Application.dataPath,"/Plugins/Android/AndroidManifest.xml");

		Debug.Log (xmlManifestPath);
		
		string cleanAppID = PlayerSettings.bundleIdentifier.Replace(".", string.Empty);

		string intentScheme = "mergevr" + cleanAppID.ToLower();

		string searchTag = "mergeVR_custom_url_scheme"; //default

		string sOldIntent = PlayerPrefs.GetString("MergeVRIntentScheme");

		if (sOldIntent.Length>0)
			searchTag=sOldIntent;

		//get last scheme used

		if(File.Exists (xmlManifestPath))
			{

			string readText = File.ReadAllText(xmlManifestPath);

			Debug.Log ("MergeVRBridge " + readText);
			string sNewText = readText.Replace(searchTag, intentScheme);

			string sSecondPass = sNewText.Replace("mergeVR_custom_url_scheme", intentScheme); //second pass - just in case if developer changed back to original searchTag

			PlayerPrefs.SetString("MergeVRIntentScheme",intentScheme);
			PlayerPrefs.Save();

			File.WriteAllText(xmlManifestPath, sSecondPass);

			}

	}


	#endif



}
