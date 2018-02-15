using System.Collections;
// using System.Collections.Generic;
using UnityEngine;

public class PiranhaMovement : MonoBehaviour {
	/// <summary>
	/// The speed at which <see cref="GameObject"/> moves.
	/// </summary>
	public float speed;

	/// <summary>
	///     The health points of the <see cref="GameObject"/>.
	/// </summary>
	public float health;

	/// <summary>
	/// The number of points given when this <see cref="GameObject"/> is
	///     destroyed.
	/// </summary>
	public int scoreValue;

	/// <summary>
	///     The <see cref="GameObject"/> that this leatherback
	///         <see cref="GameObject"/> shoots.
	/// </summary>
	public GameObject bolt;

	private Rigidbody2D rb2d;

	private GameController gameController;

	/// <summary>
	/// 	The player ship.
	/// </summary>
	private GameObject playerShip;

	private Vector3 boltLocation;

	/// <summary>
	///     The amount of times <c>FixedUpdate</c> is called before 
	///     this <see cref="GameObject"/> can <c>Shoot</c> again.
	/// </summary>
	public int COOLDOWN;
	public int bullets_per_cooldown;
	private int shot_num;
	private int waitLoop;
	private int shotsPerAttack;
	private float xOffset;
	private float yOffset;

	/// <summary>
	///     The random y value the <see cref="GameObject"/> needs to reach
	///         before it starts moving around, shooting, and able to be
	///         shot at.
	/// </summary>
	private float randYPos;

	/// <summary>
	///     Whether or not this <see cref="GameObject"/> has reached the
	///         <see cref="randYPos"/> when it is first instantiated.
	/// </summary>
	private bool inPosition;

	/// <summary>
	///     Whether or not the game is paused.
	/// </summary>
	private bool paused;

	/// <summary>
	///     The width of the camera.
	/// </summary>
	private float width;

	/// <summary>
	///     The height of the camera.
	/// </summary>
	private float height;

	/// <summary>
	///     The random <see cref="Vector3"/> that determines where this
	///         <see cref="GameObject"/> will move to.
	/// </summary>
	private Vector3 randVector3;

	/// <summary>
	///     The angle at which this <see cref="GameObject"/> needs to move
	///         at in order to reach <see cref="randVector3"/>.
	/// </summary>
	private float angle;

	/// <summary>
	///     Whether or not this <see cref="GameObject"/> has reached the
	///         random <see cref="Vector3"/>.
	/// </summary>
	private bool inRandPosition;

	/// <summary>
	///     The coin that is instantiated when this <see cref="GameObject"/>
	///         is destroyed.
	/// </summary>
	public GameObject coin;

    private float playerDamage;

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

		rb2d = GetComponent<Rigidbody2D> ();

		height = Camera.main.orthographicSize;
		width = height * Camera.main.aspect;
		randYPos = Random.Range(2f, 5f);

		// Might change logic later
		inRandPosition = true;

		shotsPerAttack = 1;

		xOffset = 0;
		yOffset = -GetComponent<SpriteRenderer>().sprite.rect.height *
			transform.localScale.y / 2 * 0.01f;

		waitLoop = COOLDOWN - 1;

        playerDamage = PlayerSaveLoad.playerSaver.GetDamage();
	}

	/// <summary>
	///     Runs every physics frame. Checks if the game is paused.
	///         If it is not paused and has not moved to 
	///         <see cref="randYPos"/>, it moves down.
	///         <para/>
	///         If the game isn't paused and has reached 
	///         <see cref="randYPos"/>, it starts to <see cref="MoveAround"/>
	///         and <see cref="Shoot>"/>.
	/// </summary>
	// Update is called once per frame
	void Update () {
		paused = gameController.GetPaused ();

		if (!paused) {
			TurnTowardsPlayer();
			if (!inPosition) {
				MoveDown();
				if (rb2d.position.y <= randYPos) {
					inPosition = true;
				}
			} else {
				MoveAround();
				Shoot();
			}
		}
	}

	void TurnTowardsPlayer()
	{
		// float xDistance = Mathf.Abs (transform.position.x - playerShip.transform.position.x);
		// float yDistance = Mathf.Abs (transform.position.y - playerShip.transform.position.y);

		// Vector3 targetDir = transform.position - playerShip.transform.position;
		// float angle = Vector3.Angle (targetDir, transform.position);

		if (playerShip != null)
		{
			Vector3 dir = playerShip.transform.position - transform.position;
			dir = playerShip.transform.TransformDirection (dir);
			float angle = Mathf.Atan2 (dir.y, dir.x) * Mathf.Rad2Deg + 90;

			Vector3 rotationVector = transform.rotation.eulerAngles;
			rotationVector.z = angle;
			transform.rotation = Quaternion.Euler (rotationVector);
		}
	}

	/// <summary>
	///     Moves this <see cref="GameObject"/> down at a rate of
	///         <see cref="speed"/>.
	/// </summary>
	void MoveDown()
	{
		transform.position -= new Vector3 (0.0f, speed, 0.0f);
	}

	/// <summary>
	///     Chooses a random Vector3 away from the center,
	///         left and right edge, and above the x axis.
	///         <para/>
	///         Then moves <see cref="GameObject"/> towards that Vector3 at 
	///         <see cref="speed"/> until it is within a certain distance.
	/// </summary>
	void MoveAround()
	{
		if (inRandPosition)
		{
			randVector3 = new Vector3(
				Random.Range(-width + width * 0.25f, width - width * 0.25f),  // Change values later
				Random.Range(1f, 5f),
				0f);
			angle = Mathf.Acos((randVector3.x -
				transform.position.x) / GetDistance(randVector3));
			if (randVector3.y - transform.position.y < 0)
				angle = -angle;
			inRandPosition = false;
		}
		else
		{
			gameObject.transform.position +=
				new Vector3(
					Mathf.Cos(angle) * speed,
					Mathf.Sin(angle) * speed,
					0f);
		}
		if (GetDistance (randVector3) < 0.5f) // Change value to a variable
		{
			inRandPosition = true;
		}
	}

	void Shoot()
	{
		if (waitLoop == 0 && shot_num <= bullets_per_cooldown 
			&& !gameController.PlayerIsDead ())
		{
			for (int i = 0; i < shotsPerAttack; i++) {
				boltLocation = new Vector3 (
					rb2d.position.x + xOffset,
					rb2d.position.y + yOffset,
					0.0f);

				Instantiate (bolt, boltLocation, Quaternion.identity);
			}

			// laserbolt.Play();  Remove the sound effect for enemy bullets
			//shot_num = 0;
		}
		else
		{
			if (waitLoop == COOLDOWN)
			{
				waitLoop = 0;
				shot_num = 0;
			}
			else
			{
				waitLoop++;
			}
		}
		shot_num++;
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

	IEnumerator OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Bolt" && inPosition) {
            health -= playerDamage;

			if (health <= 0) {
				if (playerShip != null) {
					playerShip.GetComponent<PlayerController> ().IncreaseKills ();
				}
				gameController.SubtractShip ();
				gameController.AddScore (scoreValue);
				Instantiate (coin, new Vector3(rb2d.position.x, rb2d.position.y, 1), Quaternion.identity);
				Destroy (gameObject);
			} else {
				GetComponent<SpriteRenderer> ().color = new Color (1f, 0.0f, 0.0f); // Original Value: 0.388f
				yield return new WaitForSeconds (0.1f);
				GetComponent<SpriteRenderer> ().color = new Color (1f, 1f, 1f);
			}
		}
	}
}
