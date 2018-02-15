using System.Collections;
// using System.Collections.Generic;
using UnityEngine;
//some random comment
public class AsteroidChunkController : MonoBehaviour {

	// Public variables:
	/// <summary>
	///     The speed multiplier that is applied after a certain
	///     amount of waves.
	/// </summary>
	public float speedMultiplier;

	/// <summary>
	///     The score given to the player when this <see cref="GameObject"/>
	///     is destoryed.
	/// </summary>
	public int scoreValue;

	/// <summary>
	///     The lower bound of the asteroid's <see cref="randRotation"/> value.
	/// </summary>
	public float minAsteroidRotation;

	/// <summary>
	///     The upper bound of the asteroid's <see cref="randRotation"/> value.
	/// </summary>
	public float maxAsteroidRotation;

	/// <summary>
	///     The explosion of the asteroid.
	/// </summary>
	public GameObject explosion;

	/// <summary>
	///     The coin that the asteroid drops upon death.
	/// </summary>
	public GameObject coin;

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
	/// 	The health of the asteroid.
	/// </summary>
	private float health;

	/// <summary>
	/// 	The damage that the player does to the asteroid.
	/// </summary>
	private float playerDamage;

	/// <summary>
	///     The rotation value of the asteroid.
	/// </summary>
	private float randRotation;

	/// <summary>
	///     The multiplier for the <see cref="randRotation"/> value.
	/// </summary>
	private float randRotationMultiplier;

	/// <summary>
	/// 	The angle at which this chunk moves.
	/// </summary>
	private float randAngle;

	/// <summary>
	///     Whether or not the game is paused.
	/// </summary>
	private bool paused;

	/// <summary>
	///     Initializes <see cref="gameController"/> and
	///     <see cref="playerShip"/>. A random value is assigned to
	///     <see cref="randRotation"/>.
	///     <para/>    
	///     A random value between <see cref="minAsteroidRotation"/> and
	///     <see cref="maxAsteroidRotation"/> is assigned to
	///     <see cref="randRotationMultiplier"/>.
	///     <para/>
	///     <see cref="speedMultiplier"/> is updated and <see cref="speed"/>
	///     is multiplied by <see cref="speedMultiplier"/>.
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

		GameObject playerShipObject = GameObject.FindWithTag ("Player");
		if (playerShipObject != null)
		{
			playerShip = playerShipObject;
		}

		speed = 0.1f;
		health = 1f;

		randRotation = Random.value;
		randRotationMultiplier = 
			Random.Range(minAsteroidRotation, maxAsteroidRotation);

		randAngle = Random.Range ((Mathf.PI * 3) / 4, (Mathf.PI * 5) / 4);
		randAngle += (Mathf.PI / 2); // Accounts for a different starting position of the radian unit circle

		speedMultiplier = gameController.GetDifficultyMultiplier ();
		speed *= speedMultiplier;

		playerDamage = PlayerSaveLoad.playerSaver.GetDamage ();
	}

	/// <summary>
	///     The value of <see cref="paused"/> is updated.
	///     <para/>
	///     If the game is not paused, the asteroid moves down at a value
	///     of <see cref="speed"/>.
	///     <para/>
	///     Then, the asteroid rotates by <see cref="randRotation"/>
	///     multiplied by <see cref="randRotationMultiplier"/>
	///     <para/>
	///     if <see cref="randRotation"/> is greater than 0.5; otherwise,
	///     it is multiplied by negative <see cref="randRotationMultiplier"/>.
	/// </summary>
	void Update () {
		paused = gameController.GetPaused();
		if (!paused)
		{
			Vector3 diff = GetAngleVelocity(randAngle);
			transform.position += diff;
			if (randRotation > 0.5) {
				transform.Rotate (0f, 0f, randRotationMultiplier);
			} else {
				transform.Rotate (0f, 0f, -randRotationMultiplier);
			}
		}
	}

	/// <summary>
	///     Detects the collision between this <see cref="GameObject"/>
	///         and other another <see cref="GameObject"/>.
	/// </summary>
	/// <param name="collision">
	///     The <see cref="Collider2D"></see> of the object
	///         that collided with this <see cref="GameObject"/>.
	/// </param>
	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Bolt")
		{
			health -= playerDamage;
			if (health <= 0)
			{
				if (playerShip != null)
				{
					playerShip.GetComponent<PlayerController> ().IncreaseKills ();
				}
				gameController.AddScore(scoreValue);

				Instantiate(
					coin,
					GetComponent<Rigidbody2D>().position,
					Quaternion.identity);
				
				DestroyAsteroid ();

				Destroy(gameObject);
				Destroy(collision.gameObject);
			}
		}
		if (collision.tag == "Player")
		{
			Destroy(gameObject);

			DestroyAsteroid();
		}
		if (collision.tag == "Boundary")
		{
			if (collision.GetComponent<Transform> ().position.y != 5f)
			{
				Destroy(gameObject);
			}
		}
	}

    /// <summary>
    ///     Calculates the speed depending on the angle.
    /// </summary>
    /// <param name="angle">The angle in radians to move in</param>
    /// <returns>A Vector3 that is adjested according to trig.</returns>
	Vector3 GetAngleVelocity(float angle)
	{
		return new Vector3(
			Mathf.Cos(angle) * speed,
			Mathf.Sin(angle) * speed,
			0f);
	}

    /// <summary>
    ///     Destroys the asteroid and instantiate an explosion.
    /// </summary>
	private void DestroyAsteroid()
	{
		Instantiate(
			explosion,
			GetComponent<Rigidbody2D>().position,
			Quaternion.identity);
	}
}