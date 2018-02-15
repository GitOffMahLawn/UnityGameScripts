// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;
//Some comment
public class BackgroundScroller : MonoBehaviour {

    public float speed;

	private GameController gameController;
	private bool paused;

	// Use this for initialization
	void Start () {
		GameObject gameControllerObject = GameObject.FindWithTag("GameController");
		if (gameControllerObject != null)
		{
			gameController = gameControllerObject.GetComponent<GameController>();
		}
		if (gameControllerObject == null)
		{
			Debug.Log("Cannot find 'GameController' script");
		}
	}
	
	// Update is called once per frame
	void Update () {
		paused = gameController.GetPaused();
		if (!paused) {
			Vector2 offset = new Vector2 (0, Time.time * (speed /* * gameController.GetDifficultyMultiplier() * 2.5f */ ));
			GetComponent<Renderer> ().material.mainTextureOffset = offset;
		} else {
			//Vector2 offset = new Vector2 (0f, 0f);
		}
	}
}
