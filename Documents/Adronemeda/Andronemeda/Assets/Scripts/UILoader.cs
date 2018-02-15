// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UILoader : MonoBehaviour {

	public void LoadLevel(string levelName)
	{
		SceneManager.LoadScene (levelName);
	}

    public void QuitApplication()
    {
        Application.Quit();
    }
}
