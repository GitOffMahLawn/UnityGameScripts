using System.Collections;
// using System.Collections.Generic;
using UnityEngine;

public class BarracudaMovement : MonoBehaviour {

    // Public variables:
	public float speed;
	public float health;
	public int scoreValue;
    private float speedVariation;

	public GameObject explosion;

    // Private variables:
    private Rigidbody2D rb2d;
	private bool inPosition;
	private float screenWidth;

	private GameController gameController;
	private GameObject playerShip;

	private float initial_y_destination;
    private float edgeBuffer;

    //shooting
    public int COOLDOWN = 10;
    public int bullets_per_cooldown = 10;
    private int shot_num;
    private int waitLoop;
    private Vector3 boltLocation;
    private float xOffset;
    private float yOffset;
    private float shipScale;
    public GameObject bolt;
    private AudioSource laserbolt; // Removing sound effects for enemy
    private bool fireLeft;
	private bool paused;

    public GameObject coin;

    private float playerDamage;

    // Use this for initialization
    void Start () {
        speedVariation = Random.Range(0f, speed * 1.0f);
        // If the random int is 0, an epsilon is added to the speed.
        // Otherwise, an epsilon is subtracted from speed.
        if (Random.Range(0, 1) == 0)
        {
            speed += speedVariation;
        }
        else
        {
            speed -= speedVariation;
        }

        rb2d = GetComponent<Rigidbody2D>();

        // Gets the camera for its size
        float cameraSize;
		cameraSize = Camera.main.orthographicSize;

        // Half of the screen width
        screenWidth = cameraSize * Camera.main.aspect;

        // Percentage of screenWidth to use as buffer
        edgeBuffer = screenWidth * 0.15f;

		initial_y_destination = Random.Range(1f, 3f);

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
			
        // laserbolt = GetComponent<AudioSource>();  Remove sound effect
        // Changes the offset of bullets depending on the scale of the ship
        shipScale = transform.localScale.x;
        xOffset = shipScale * 2.0f;
        yOffset = shipScale * 5.25f;

		waitLoop = COOLDOWN - 1;

        // Changes the scale of the bullets depending on the scale of the ship
        bolt.transform.localScale = new Vector3(
            shipScale * 0.375f, shipScale * 4.0625f, 1);
        fireLeft = true;
        shot_num = 0;
        waitLoop = 0;

        playerDamage = PlayerSaveLoad.playerSaver.GetDamage();
    }
	
	// Update is called once per frame
	void FixedUpdate () {

		paused = gameController.GetPaused ();

		if (!paused) {
			if (!inPosition) {
				MoveDown();
			} else {
				MoveSideways();
				Shoot();
			}

			if (rb2d.position.y <= initial_y_destination) {
				inPosition = true;
			}
		}

	}

    private void MoveDown()
    {
		gameObject.transform.position -= new Vector3(0.0f, speed, 0.0f);
    }

	private void MoveSideways()
	{
		gameObject.transform.position += new Vector3(-speed, 0.0f, 0.0f);
		if (rb2d.position.x < -screenWidth + edgeBuffer ||
            rb2d.position.x > screenWidth - edgeBuffer) {
			speed = -speed;
		}
	}


    IEnumerator OnTriggerEnter2D(Collider2D collision)
    {
		if (collision.tag == "Bolt" && inPosition)
        {
			// Destroy (collision.gameObject); Controlled in BoltController
			// Debug.Log ("Bolt hit");
			health -= playerDamage;
            if (health <= 0)
            {
				if (playerShip != null) {
					playerShip.GetComponent<PlayerController> ().IncreaseKills ();
				}
				gameController.SubtractShip();
				gameController.AddScore(scoreValue);
				Instantiate(coin, new Vector3(rb2d.position.x, rb2d.position.y, 1), Quaternion.identity);
                Destroy(gameObject);

				Instantiate (explosion, GetComponent<Rigidbody2D>().position, Quaternion.identity);
			} else {
				GetComponent<SpriteRenderer> ().color = new Color (1f, 0f, 0f);
				yield return new WaitForSeconds (0.1f);
				GetComponent<SpriteRenderer> ().color = new Color (1f, 1f, 1f);
			}
		}
        /*
        if (collision.tag == "Player")
        {
            Destroy(collision.gameObject);
			//gameController.GameOver();  Controlled in Player Controller
        }
        */
    }

    void Shoot()
    {
        // Bolt firing
		if (waitLoop == 0 && shot_num <= bullets_per_cooldown && !gameController.PlayerIsDead())
        {
            if (fireLeft)
            {
                boltLocation = new Vector3(
                    rb2d.position.x + xOffset,
                    rb2d.position.y - yOffset, 0);
            }
            else
            {
                boltLocation = new Vector3(
                    rb2d.position.x - xOffset,
                    rb2d.position.y - yOffset, 0);
            }
            Instantiate(bolt, boltLocation, Quaternion.identity);
            // laserbolt.Play();  Remove the sound effect for enemy bullets
            fireLeft = !fireLeft;
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
				shot_num++;
            }
        }
        shot_num++;

		// Debug.Log(waitLoop);
    }
}
