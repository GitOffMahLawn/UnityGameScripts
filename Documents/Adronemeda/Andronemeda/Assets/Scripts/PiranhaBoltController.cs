// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;

public class PiranhaBoltController : MonoBehaviour {

	public float speed;

	public GameObject explosion;

	private GameController gameController;
	private GameObject playerShip;

	private float speedMultiplier;

	private bool paused;

	private float angle;

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

		GameObject playerShipObject = GameObject.FindWithTag ("Player");
		if (playerShipObject != null)
		{
			playerShip = playerShipObject;
		}
		if (playerShipObject == null)
		{
			Debug.Log ("Cannot find playerShip object");
		}

		speedMultiplier = gameController.GetDifficultyMultiplier();
		speed *= speedMultiplier;

		// float xDistance = Mathf.Abs (transform.position.x - playerShip.transform.position.x);
		// float yDistance = Mathf.Abs (transform.position.y - playerShip.transform.position.y);

        float xDistance = playerShip.transform.position.x - transform.position.x;
        float yDistance = playerShip.transform.position.y - transform.position.y;
        angle = Mathf.Acos(xDistance / GetDistance(playerShip.transform.position));
        if (yDistance < 0)
            angle = -angle;

        Vector3 rotationVector = transform.rotation.eulerAngles;
		rotationVector.z = angle * Mathf.Rad2Deg + 90;
		transform.rotation = Quaternion.Euler (rotationVector);

        // Debug.Log("After: " + transform.rotation);
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		paused = gameController.GetPaused ();

		if (!paused) {
            // Vector3 diff = GetAngleVelocity(angle);

            Vector3 diff = GetAngleVelocity(angle);
			transform.position += diff;
		}
	}

    /*
	public void SetAngle(float angle)
	{
		this.angle = angle;
	}
    */

	Vector3 GetAngleVelocity(float angle)
	{
		return new Vector3(
			Mathf.Cos(angle) * speed,
			Mathf.Sin(angle) * speed,
			0f);
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Player")
		{
			Destroy(gameObject);
			Instantiate (explosion, GetComponent<Rigidbody2D>().position, Quaternion.identity);
		}
	}

    /// <summary>
	///     Gets the distance from this LeatherBack <see cref="GameObject"/>
	///         to the target.
	/// </summary>
	/// <param name="target">
	///     The <see cref="Vector3"/> that the method uses to find
	///         the distance between.
	/// </param>
	/// <returns>
	///     Returns the distance in the form of a float.
	/// </returns>
	float GetDistance(Vector3 target)
    {
        float x = target.x - transform.position.x;
        float y = target.y - transform.position.y;
        float distance = Mathf.Sqrt(Mathf.Pow(x, 2) + Mathf.Pow(y, 2));

        return distance;
    }
}
