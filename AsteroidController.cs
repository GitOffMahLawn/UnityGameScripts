using System.Collections;
// using System.Collections.Generic;
using UnityEngine;
public class AsteroidController : MonoBehaviour {

    // Public variables:
    /// <summary>
    ///     The speed multiplier that is applied after a certain
    ///     amount of waves.
    /// </summary>
	public float speedMultiplier;

	/// <summary>
	/// 	The chance (1 in bigAsteroidChance) that an asteroid will spawn as a big asteroid.
	/// </summary>
	public float bigAsteroidChance;

	/// <summary>
	/// 	The minimum number of pieces that big asteroids will break into.
	/// </summary>
	public int minAsteroidChunks;

	/// <summary>
	/// 	The maximum number of pieces that big asteroids will break into.
	/// </summary>
	public int maxAsteroidChunks;

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
	/// 	The asteroid chunks that big asteroids break into.
	/// </summary>
	public GameObject asteroidChunk;

    /// <summary>
    ///     The explosion of the asteroid.
    /// </summary>
	public GameObject explosion;

	/// <summary>
	/// 	The explosion of the big asteroid.
	/// </summary>
	public GameObject explosionBig;

    /// <summary>
    ///     The coin of the asteroid.
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
	/// 	The random number that decides whether or not an asteroid
	/// 	will spawn as a big asteroid.
	/// </summary>
	private float randBigAsteroidChance;

    /// <summary>
    ///     The multiplier for the <see cref="randRotation"/> value.
    /// </summary>
	private float randRotationMultiplier;

	/// <summary>
	/// 	True if the asteroid is a big asteroid.
	/// </summary>
	private bool isBigAsteroid;

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
		isBigAsteroid = false;

		randRotation = Random.value;
		randRotationMultiplier = 
            Random.Range(minAsteroidRotation, maxAsteroidRotation);

		randBigAsteroidChance = Random.value * bigAsteroidChance;
		if (randBigAsteroidChance <= 1f && transform.position.y > 5f) {
			transform.localScale = new Vector3 (0.5f, 0.5f, 0f);
			speed = 0.02f;
			health = 5f;
			randRotationMultiplier /= 3;
			isBigAsteroid = true;
		}
		else
		{
			transform.localScale = new Vector3 (0.1f, 0.1f, 0f);
		}

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
			Vector3 diff = new Vector3 (0.0f, -speed, 0.0f);
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

				if (isBigAsteroid)
				{
					DestroyBigAsteroid ();
				}
				else
				{
					DestroyAsteroid ();
				}

				Destroy(gameObject);
				Destroy(collision.gameObject);
			}
        }
		if (collision.tag == "Player")
        {
            Destroy(gameObject);

			if (isBigAsteroid)
			{
				DestroyBigAsteroid ();
			}
			else
			{
				DestroyAsteroid ();
			}
		}
		if (collision.tag == "Boundary")
        {
			if (collision.GetComponent<Transform> ().position.y != 5f)
			{
				Destroy (gameObject);
			}
        }
    }

    /// <summary>
    ///     Destorys the asteroid and instantiates an explosion.
    /// </summary>
	private void DestroyAsteroid()
	{
		Vector3 asteroidPosition = GetComponent<Rigidbody2D>().position;

		Instantiate(
			explosion,
			new Vector3(asteroidPosition.x, asteroidPosition.y, 1),
			Quaternion.identity);
	}

    /// <summary>
    ///     Instantiates a large explosion and instantiates a random number of
    ///     smaller asteroids.
    /// </summary>
	private void DestroyBigAsteroid()
	{
		Instantiate(
			explosionBig,
			GetComponent<Rigidbody2D>().position,
			Quaternion.identity);

		int randAsteroidChunk = Random.Range (minAsteroidChunks, maxAsteroidChunks + 1); // Accounts for an exclusive
																						 // maximum value

		for (int i = 0; i < randAsteroidChunk; i++)
		{
			Instantiate (asteroidChunk, GetComponent<Rigidbody2D>().position, Quaternion.identity);
		}
	}
}
