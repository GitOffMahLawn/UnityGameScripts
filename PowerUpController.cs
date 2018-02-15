// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;

public class PowerUpController : MonoBehaviour {

	public float speed;

	private GameController gameController;
	private bool paused;

	void Start()
	{
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

	void FixedUpdate()
	{
		paused = gameController.GetPaused ();

		if (!paused) {
			Vector3 diff = new Vector3 (0.0f, -speed, 0.0f);
			GetComponent<Transform>().position += diff;
		}
	}
    /*  Not necessary for now
	void OnTriggerEnter2D(Collider2D collision)
	{

	}
    */
}
