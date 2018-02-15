// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;

public class TitleScreenController : MonoBehaviour {
	
	private int height;
	private int width;

	private bool isFullscreen;

	// Use this for initialization
	void Start () {
        isFullscreen = true;
		height = Screen.height;
        width = height > Screen.width ? Screen.width : Mathf.RoundToInt(height * 9f / 16f);
		if (SystemInfo.operatingSystem.Contains ("Mac")) {
			isFullscreen = false;
		}
        Screen.SetResolution (width, height, isFullscreen, 60);
	}
}
