using UnityEngine;
using System.Collections;

public class ReturnMenu : MonoBehaviour {

	void OnGUI () {
		GUILayout.Space(20);
		
		// メニューに戻る
		if (GUILayout.Button("Return Menu", GUILayout.Width(100)))
		{
			Application.LoadLevel("SampleMenu");
		}
	}
}
