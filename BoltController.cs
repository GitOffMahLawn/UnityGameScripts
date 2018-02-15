// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;

public class BoltController : MonoBehaviour
{
    public float speed;

	private float angle;

	private int shotType;

	private GameController gameController;
	private GameObject playerShip;
	private bool paused;

	void Start()
	{
		playerShip = GameObject.FindWithTag ("Player");

		GameObject gameControllerObject = GameObject.FindWithTag("GameController");
		if (gameControllerObject != null)
		{
			gameController = gameControllerObject.GetComponent<GameController>();
		}
		if (gameControllerObject == null)
		{
			Debug.Log("Cannot find 'GameController' script");
		}
        /*

		GameObject playerShipObject = GameObject.FindWithTag ("Player");
		if (playerShipObject != null)
		{
			playerShip = playerShipObject;
		}
		if (playerShipObject == null)
		{
			Debug.Log ("Cannot find Player Ship");
		}
        */
		// GetComponent<Transform> ().localScale = new Vector3 (0.075f, 0.3f, 0.0f);

		shotType = playerShip.GetComponent<PlayerController> ().GetShotType ();

		if (shotType == 2) {
			angle = gameController.ChangeAngle ();

			Vector3 rotationVector = transform.rotation.eulerAngles;
			rotationVector.z = angle * Mathf.Rad2Deg + 90;
			transform.rotation = Quaternion.Euler (rotationVector);
		}
	}

    void FixedUpdate()
    {
		paused = gameController.GetPaused ();

		if (!paused) {
			Vector3 diff;
			if (shotType == 2) {
				diff = GetAngleVelocity (angle);
				transform.position += diff;
			} else {
				diff = new Vector3 (0.0f, speed, 0.0f);
				transform.position += diff;
			}
		}
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
		if (collision.tag == "Asteroid" ||
		    collision.tag == "Barracuda" ||
		    collision.tag == "Leatherback" ||
		    collision.tag == "Piranha") {
			Destroy (gameObject);
		}
    }

	/// <summary>
	///     Calculates the component velocities of an angle with a speed constant.
	/// </summary>
	/// <param name="angle">
	///     The angle used to calculate the velocities.
	/// </param>
	/// <returns>
	///     A Vector3 with the component velocities of the angle
	///     with the speed constant.
	/// </returns>
	Vector3 GetAngleVelocity(float angle)
	{
		return new Vector3(
			Mathf.Cos(angle) * speed,
			Mathf.Sin(angle) * speed,
			0f);
	}
}
