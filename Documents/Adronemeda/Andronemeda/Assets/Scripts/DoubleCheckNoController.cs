// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;

public class DoubleCheckNoController : MonoBehaviour {

	public GameObject pauseMenu;
	public GameObject doubleCheckBox;

	// Use this for initialization
	void OnMouseDown()
	{
		pauseMenu.SetActive (true);
		doubleCheckBox.SetActive (false);
	}
}
