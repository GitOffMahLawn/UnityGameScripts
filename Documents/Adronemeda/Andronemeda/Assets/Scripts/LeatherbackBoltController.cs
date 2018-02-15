// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;

public class LeatherbackBoltController : MonoBehaviour {

    /// <summary>
    ///     The speed constant of the bolt.
    /// </summary>
	public float speed;


    /// <summary>
    ///     The explosion instantiated when the bolt collides with something.
    /// </summary>
	public GameObject explosion;

	// private int slope; // Between -2 and 2

    /// <summary>
    ///     The angle in radians of the current bolt to fire.
    /// </summary>
    private float angle;

	private float speedMultiplier;

	private GameController gameController;

    /// <summary>
    ///     Whether or not the game is paused.
    /// </summary>
	private bool paused;

    /// <summary>
    ///     Finds the GameController object with FindWithTag.
    ///     Changes the slope and angle variables using functions
    ///         in GameController.
    /// </summary>
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

		speedMultiplier = gameController.GetDifficultyMultiplier();
		speed *= speedMultiplier;

        angle = gameController.ChangeLeatherbackAngle();
    }
	
    /// <summary>
    ///     Checks if the game is paused.
    ///     Changes the position of the bolt if the game isn't paused.
    /// </summary>
	void FixedUpdate () {
		paused = gameController.GetPaused ();

		if (!paused) {
			Vector3 diff;

            diff = GetAngleVelocity(angle);

			transform.position += diff;
		}
	}

    /// <summary>
    ///     Checks if the bolt collides with the player.
    /// </summary>
    /// <param name="collision">
    ///     The <see cref="Collider2D"></see> of the object
    ///         that collided with the bolt.
    /// </param>
	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Player")
		{
			Destroy(gameObject);
			Instantiate (
                explosion,
                GetComponent<Rigidbody2D>().position,
                Quaternion.identity);
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
