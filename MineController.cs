using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineController : MonoBehaviour {

	// Public variables:
	/// <summary>
	///     The speed multiplier that is applied after a certain
	///     amount of waves.
	/// </summary>
	public float speedMultiplier;

	/// <summary>
	///     The lower bound of the mine's <see cref="randRotation"/> value.
	/// </summary>
	public float minMineRotation;

	/// <summary>
	///     The upper bound of the mine's <see cref="randRotation"/> value.
	/// </summary>
	public float maxMineRotation;

	/// <summary>
	///     The explosion of the mine.
	/// </summary>
	public GameObject explosion;

	/// <summary>
	///     The <see cref="GameController"/> object used to
	///     control the asteroid.
	/// </summary>
	private GameController gameController;

	/// <summary>
	///     The player ship <see cref="GameObject"/>.
	/// </summary>
	private GameObject playerShip;

	/// <summary>
	///     The speed at which this <see cref="GameObject"/> moves.
	/// </summary>
	private float speed;

	/// <summary>
	///     The rotation value of the mine.
	/// </summary>
	private float randRotation;

	/// <summary>
	///     The multiplier for the <see cref="randRotation"/> value.
	/// </summary>
	private float randRotationMultiplier;

	/// <summary>
	///     Whether or not the game is paused.
	/// </summary>
	private bool paused;

	// Use this for initialization
	void Start () {
		GameObject gameControllerObject = 
			GameObject.FindWithTag("GameController");
		if (gameControllerObject != null)
		{
			gameController = 
				gameControllerObject.GetComponent<GameController>();
		}
		if (gameControllerObject == null)
		{
			Debug.Log("Cannot find 'GameController' script");
		}

		GameObject playerShipObject = GameObject.FindWithTag ("Player");
		if (playerShipObject != null)
		{
			playerShip = playerShipObject;
		}

		speed = 0.08f;

		randRotation = Random.value;
		randRotationMultiplier = Random.Range (minMineRotation, maxMineRotation);

		speedMultiplier = gameController.GetDifficultyMultiplier ();
		speed *= speedMultiplier;
	}
	
	// Update is called once per frame
	void Update () {
		paused = gameController.GetPaused();
		if (!paused)
		{
			Vector3 diff = new Vector3 (0.0f, -speed, 0.0f);
			transform.position += diff;
			if (randRotation > 0.5) {
				transform.Rotate (0f, 0f, randRotationMultiplier);
			} else {
				transform.Rotate (0f, 0f, -randRotationMultiplier);
			}
		}
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Bolt")
		{
			DestroyMine();
			Destroy (collision.gameObject);
			playerShip.GetComponent<PlayerController>().SubtractHealth();
		}
		if (collision.tag == "Player")
		{
			DestroyMine();
		}
		if (collision.tag == "Boundary")
		{
			if (collision.GetComponent<Transform>().position.y != 5f)
			{
				Destroy(gameObject);
			}
		}
	}

	private void DestroyMine()
	{
		Instantiate(
			explosion,
			GetComponent<Rigidbody2D>().position,
			Quaternion.identity);

		Destroy (gameObject);
	}
}
