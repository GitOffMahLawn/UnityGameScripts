// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;
public class BarracudaBoltController : MonoBehaviour
{

	public float speed;

	public GameObject explosion;

	private GameController gameController;
	private float speedMultiplier;
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

		speedMultiplier = gameController.GetDifficultyMultiplier();
		speed *= speedMultiplier;
	}

	void FixedUpdate()
	{
		paused = gameController.GetPaused ();

		if (!paused) {
			Vector3 diff = new Vector3 (0.0f, -speed, 0.0f);
			GetComponent<Transform> ().position += diff;
		}
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
        /*if (collision.tag == "Boundary")
		{
			Destroy(gameObject);
		}

		if (collision.tag == "Player")
		{
            // Destroy(collision.gameObject);
            // gameController.GameOver();  Controlled in playerController

            Destroy(gameObject);
		}*/
		if (collision.tag == "Player")
		{
			Destroy(gameObject);
			Instantiate (explosion, GetComponent<Rigidbody2D>().position, Quaternion.identity);
		}
	}
}
