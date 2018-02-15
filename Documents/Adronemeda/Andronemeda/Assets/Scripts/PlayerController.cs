using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//[System.Serializable]
//public class Boundary
//{
//    public float xMin, xMax, yMin, yMax;
//}

public class PlayerController : MonoBehaviour
{
    public int COOLDOWN;
    public int bullets_per_cooldown = 1;

    private float original_speed;
    public float speed;
    private float sensitivity;
    private Vector3 current_velocity;
    public float acceleration;
    public float decel_radius;
    public GameObject bolt;
    public Rigidbody2D rb2d;
    public GameObject explosion;
    public GameObject shieldIcon;
    public GameObject shieldIcon1;
    public GameObject shieldIcon2;
    public GameObject shieldIcon3;
	public Slider healthBar;
    public Slider sensitivity_slider;

    private GameController gameController;
    private Text coinsText;
    private Text killsText;
    private Text wavesClearedText;
    private Text scoreText;
    private Text highscoreText;

    private Text coinsTextTotal;
    private Text killsTextTotal;
    private Text playsTextTotal;
    private Text highscoreTextAllTime;
    private Text mostCoins;
    private Text mostKills;
    private Text mostWaves;

    private Text sensitivityValueText;
    //private Animator animator;  This variable is not used.
    private AudioSource laserbolt;

    private float xOffset;
    private float yOffset;
	private float xTripleOffset;
    private float shipScale;
    // private float iconXOffset;
    private bool fireLeft;
    private Vector3 boltLocation;
    private bool paused;
    private bool isDead;
	private int playerShields;

    private float moveHorizontal;
    private float moveVertical;

    private Camera cam;
    private float cam_size;
    private int screen_height;
    private int screen_width;

	// Upgrades
	private float damageMultiplier;
	private int health;
	private int shotType;
	private int shieldCapacity;
	private float magneticRange;
	private bool unlockedAbility;
	private float abilityMultiplier;

	private int num_shields;

    private int num_coins;
    private int num_kills;
    private int num_score;

    private bool isTouching;

    // private float shield_screen_offset;

	private Vector2 thePosition;
	public LayerMask touchInputMask;

    public float fireRate;
    private float nextFire;

    // Player Boundaries
    private float xMin, xMax, yMin, yMax;

    private void Start()
    {
        PlayerSaveLoad.playerSaver.Load();  // Loads the player information(coins)
        // gets the main camera
        cam = Camera.main;
        cam_size = cam.orthographicSize;

        // instantiates the height and width of the screen.
        screen_height = Screen.height;
        screen_width = Screen.width;

        // Sets the boundaries according to camera size:
        xMax = cam_size / ((float)screen_height / screen_width);
        xMin = -cam_size / ((float)screen_height / screen_width);
        yMax = cam_size;
        yMin = -cam_size;

        // shield_screen_offset = xMax * 2 * 0.07f;

        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
        }
        if (gameControllerObject == null)
        {
            Debug.Log("Cannot find 'GameController' script");
        }

        coinsText = GameObject.FindWithTag("CoinsText").GetComponent<Text>();
        killsText = GameObject.FindWithTag("KillsText").GetComponent<Text>();
        scoreText = GameObject.FindWithTag("ScoreText(GameOver)").GetComponent<Text>();
        wavesClearedText = GameObject.FindWithTag("Waves Cleared").GetComponent<Text>();
        highscoreText = GameObject.FindWithTag("HighscoreText").GetComponent<Text>();
        coinsTextTotal = GameObject.FindWithTag("CoinsTextTotal").GetComponent<Text>();
        killsTextTotal = GameObject.FindWithTag("KillsTextTotal").GetComponent<Text>();
        playsTextTotal = GameObject.FindWithTag("PlaysTextTotal").GetComponent<Text>();
        highscoreTextAllTime = GameObject.FindWithTag("HighscoreTextAllTime").GetComponent<Text>();
        mostCoins = GameObject.FindWithTag("Most Coins").GetComponent<Text>();
        mostKills = GameObject.FindWithTag("Most Kills").GetComponent<Text>();
        mostWaves = GameObject.FindWithTag("Most Waves").GetComponent<Text>();

        sensitivityValueText = GameObject.FindWithTag("Sensitivity Value").GetComponent<Text>();

        gameController.ExecutePauseMenuShutdown();
        // Ensures that the pause menus are only shutdown when all of the GUIText components have been
        // located and assigned
        // shieldIcon1 = GameObject.FindWithTag("ShieldIcon1");
        // shieldIcon2 = GameObject.FindWithTag("ShieldIcon2");
        // shieldIcon3 = GameObject.FindWithTag("ShieldIcon3");

        rb2d = GetComponent<Rigidbody2D>();
        //animator = GetComponent<Animator>();  animator is never used in this script.
        laserbolt = GetComponent<AudioSource>();

        // Changes the offset of bullets depending on the scale of the ship   
        shipScale = transform.localScale.x;
        xOffset = shipScale * 1.25f;
        yOffset = shipScale * 4.7f;
		xTripleOffset = shipScale * 1f;

        // iconXOffset = 0.2f;

        shieldCapacity = PlayerSaveLoad.playerSaver.shieldUpgradeLevel;

        // Changes the scale of the bullets depending on the scale of the ship
        bolt.transform.localScale = new Vector3(
            shipScale * 0.375f, shipScale * 1.5f, 1);
        fireLeft = true;

        // Instantiates the number of shields the player has to 0
		playerShields = 0;

        decel_radius = 0.01f;
        if (SystemInfo.operatingSystem.Contains("Mac") || SystemInfo.operatingSystem.Contains("Windows"))
        {
            speed *= 10;
            sensitivity = 1.0f;
        }
        sensitivity = PlayerSaveLoad.playerSaver.player_sensitivity;
        sensitivityValueText.text = sensitivity.ToString();
        sensitivity_slider.value = sensitivity;

		damageMultiplier = PlayerSaveLoad.playerSaver.GetDamage();
		health = PlayerSaveLoad.playerSaver.healthUpgradeLevel + 1; // Accounts for base 0
		shotType = PlayerSaveLoad.playerSaver.shotUpgradeLevel;
		magneticRange = PlayerSaveLoad.playerSaver.magnetUpgradeLevel;

		healthBar.maxValue = health;
		healthBar.value = health;
    }

    void Update()
    {
        Vector3 diff;
        if (Input.touchCount > 0)
        {
            isTouching = true;
            foreach (Touch touch in Input.touches)
            {
                if (!paused)
                {
                    diff = touch.deltaPosition * (speed / 100.0f) * sensitivity;
                    transform.position += diff;
                    transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
                }

                /*
                Vector3 pos = Camera.main.ScreenToWorldPoint(touch.position);
                pos.z = 1;
                if (!paused)
                {
                    if (GetDistance(pos) > decel_radius)
                    {
                        diff = new Vector3(
                            (pos.x - transform.position.x) * speed,
                            (pos.y - transform.position.y) * speed,
                            0f);
                        current_velocity = diff;
                        speed += acceleration;
                    }
                    else
                    {
                        if (speed > original_speed)
                        {
                            diff = current_velocity;
                            speed -= acceleration;
                            current_velocity = new Vector3(
                                (pos.x - transform.position.x) * speed,
                                (pos.y - transform.position.y) * speed,
                                0f);
                        }
                        else
                        {
                            diff = new Vector3();
                        }
                    }
                    transform.position += diff;
                }
                */
                //transform.position = pos;
            }
        }
        else
        {
            isTouching = false;
        }
		if ((isTouching || Input.GetKey(KeyCode.Space)) && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
			if (shotType == 0) {
				ShootSingle ();
			} else if (shotType == 1) {
				ShootAlternate ();
			} else if (shotType == 2) {
				ShootTriple ();
			} else {
				ShootSingle ();
			}
        }
    }

    void FixedUpdate()
    {
        // Debug.Log("Player Shields: " + playerShields);
        paused = gameController.GetPaused();

        if (paused)
        {
            rb2d.velocity = new Vector3(0.0f, 0.0f, 0.0f);
            laserbolt.Pause();
        }

        if (!paused)
        {
            laserbolt.UnPause();

            // Movement with arrow keys or WASD
            moveHorizontal = Input.GetAxis("Horizontal") * sensitivity;
            moveVertical = Input.GetAxis("Vertical") * sensitivity;

            Vector3 movement = new Vector3(moveHorizontal, moveVertical, 0.0f);
            rb2d.velocity = movement * speed;

            Vector3 positionVector = new Vector3(
                Mathf.Clamp(rb2d.position.x, xMin, xMax),
                Mathf.Clamp(rb2d.position.y, yMin, yMax),
                0.0f);

            rb2d.position = positionVector;
            // Bolt firing
            //if (Input.GetKey (KeyCode.Space)) 
                /*
                if (waitLoop == 0 && shot_num <= bullets_per_cooldown)
                {
                    if (fireLeft)
                    {
                        boltLocation = new Vector3(
                            rb2d.position.x - xOffset,
                            rb2d.position.y + yOffset, 0);
                    }
                    else
                    {
                        boltLocation = new Vector3(
                            rb2d.position.x + xOffset,
                            rb2d.position.y + yOffset, 0);
                    }
                    Instantiate(bolt, boltLocation, Quaternion.identity);
                    laserbolt.Play();
                    fireLeft = !fireLeft;
                    //shot_num = 0;
                }
                else
                {
                    if (waitLoop != COOLDOWN)
                    {
                        waitLoop++;
                        shot_num++;
                    }
                    else
                    {
                        waitLoop = 0;
                        shot_num = 0;
                    }
                }
                shot_num++;
                */
        }

        isDead = gameController.PlayerIsDead();

        if (isDead)
        {
            Instantiate(explosion, rb2d.position, Quaternion.identity);
            Destroy(gameObject);  // Changed the location of ship destruction to here from AsteroidController.
        }
    }

    private void ShootSingle()
    {
        boltLocation = new Vector3(
            rb2d.position.x,
            rb2d.position.y + yOffset,
            0);
		
        Instantiate(bolt, boltLocation, Quaternion.identity);
        laserbolt.Play();
    }

	private void ShootAlternate()
	{
		if (fireLeft)
		{
			boltLocation = new Vector3(
				rb2d.position.x - xOffset,
				rb2d.position.y + yOffset,
				0);
		}
		else
		{
			boltLocation = new Vector3(
				rb2d.position.x + xOffset,
				rb2d.position.y + yOffset,
				0);
		}
		Instantiate(bolt, boltLocation, Quaternion.identity);
		laserbolt.Play();
		fireLeft = !fireLeft;
	}

	private void ShootTriple()
	{
		for (int i = 0; i < 3; i++) {
			float xTriplePos = rb2d.position.x;
			if (i == 0) {
				xTriplePos += xTripleOffset;
			} else if (i == 1) {
				xTriplePos -= xTripleOffset;
			}
			boltLocation = new Vector3 (
				xTriplePos,
				rb2d.position.y + yOffset,
				0);

			Instantiate (bolt, boltLocation, Quaternion.identity);
			laserbolt.Play ();
		}
	}

	public float GetDamageMultiplier()
	{
		return damageMultiplier;
	}

	public int GetShotType()
	{
		return shotType;
	}

	public int GetShieldCapacity()
	{
		return shieldCapacity;
	}

	public float GetMagneticRange()
	{
		return magneticRange;
	}

	public int GetShields()
	{
		return num_shields;
	}

    public int GetKills()
    {
        return num_kills;
    }

    public void IncreaseKills()
    {
        num_kills++;
    }

	void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Contains("Coin"))
        {
            switch (collision.tag)
            {
                case "Copper Coin":
                    num_coins += GameController.COPPER_VALUE;
                    break;
                case "Silver Coin":
                    num_coins += GameController.SILVER_VALUE;
                    break;
                case "Gold Coin":
                    num_coins += GameController.GOLD_VALUE;
                    break;
                case "Emerald Coin":
                    num_coins += GameController.EMERALD_VALUE;
                    break;
                case "Sapphire Coin":
                    num_coins += GameController.SAPPHIRE_VALUE;
                    break;
            }
            Destroy(collision.gameObject);
        }

        if (collision.tag == "ShieldPowerUp")
        {
            Destroy(collision.gameObject);

            // float shieldIconXPosition = xMin + shield_screen_offset + (playerShields * 0.5f) + (iconXOffset * playerShields);
            // Vector3 shieldIconPosition = new Vector3(shieldIconXPosition, -4.45f, -3.0f);
            // Instantiate(shieldIcon, shieldIconPosition, Quaternion.identity);

			if (GetShields() < GetShieldCapacity())
			{
				switch (playerShields)
				{
				case 0:
					shieldIcon1.SetActive (true);
					playerShields++;
					num_shields++;
					break;
				case 1:
					shieldIcon2.SetActive (true);
					playerShields++;
					num_shields++;
					break;
				case 2:
					shieldIcon3.SetActive (true);
					playerShields++;
					num_shields++;
					break;
				case 3:
					break;

				}
			}

            /*if (playerShields == 0) {
				shieldIcon1.SetActive (true);
			}

			if (playerShields == 1) {
				shieldIcon2.SetActive (true);
			}
            
			if (playerShields == 2) {
				shieldIcon3.SetActive (true);
			}

			if (playerShields < 3)
			{
				playerShields += 1;
			}*/

        }

        // If the player ship collides with an obstacle and the playerShields is greater
        // than 0, take a life. Otherwise, destroy the player ship.

        // Add to this list of harmful obstacles
        if (collision.tag == "Asteroid" ||
            collision.tag == "Barracuda" ||
            collision.tag == "EnemyBolt" ||
            collision.tag == "Leatherback" ||
            collision.tag == "Piranha" ||
			collision.tag == "Mine")
        {
			SubtractHealth();
        }
    }

	public void SubtractHealth()
	{
		if (playerShields > 0)
		{
			SubtractShields();
		}
		else
		{
			health--;
			healthBar.value--;
		}

		if (health > 0)
		{
			StartCoroutine(FlashRed());
		}
		else
		{
			EndGame();
		}
	}

	private void SubtractShields()
	{
		if (playerShields > 0)
		{
			switch (playerShields)
			{
			case 3:
				shieldIcon3.SetActive (false);
				playerShields--;
				num_shields--;
				break;
			case 2:
				shieldIcon2.SetActive (false);
				playerShields--;
				num_shields--;
				break;
			case 1:
				shieldIcon1.SetActive (false);
				playerShields--;
				num_shields--;
				break;
			}
		}
	}

	private IEnumerator FlashRed()
	{
		GetComponent<SpriteRenderer> ().color = new Color (1f, 0f, 0f); // Ship flashes red when hit
		yield return new WaitForSeconds (0.1f);
		GetComponent<SpriteRenderer> ().color = new Color (1f, 1f, 1f); // Ship returns to normal color
																		// after .1 seconds
	}

	private void EndGame()
	{
		PlayerSaveLoad.playerSaver.coins += num_coins;
		PlayerSaveLoad.playerSaver.kills += num_kills;
		PlayerSaveLoad.playerSaver.plays++;
		num_score = gameController.GetScore();
		if (num_score > PlayerSaveLoad.playerSaver.highscore)
		{
			PlayerSaveLoad.playerSaver.highscore = num_score;
		}
		if (num_kills > PlayerSaveLoad.playerSaver.most_kills_per_game)
		{
			PlayerSaveLoad.playerSaver.most_kills_per_game = num_kills;
		}
		if (num_coins > PlayerSaveLoad.playerSaver.most_coins_per_game)
		{
			PlayerSaveLoad.playerSaver.most_coins_per_game = num_coins;
		}
		if (GameController.waves_cleared > PlayerSaveLoad.playerSaver.most_waves_beaten)
		{
			PlayerSaveLoad.playerSaver.most_waves_beaten = GameController.waves_cleared;
		}
		PlayerSaveLoad.playerSaver.player_sensitivity = sensitivity;
		PlayerSaveLoad.playerSaver.Save();


		coinsText.text = "Coins: " + num_coins;
		if (num_coins >= PlayerSaveLoad.playerSaver.most_coins_per_game) {
			coinsText.color = new Color(219, 211, 0);
		}
		killsText.text = "Kills: " + num_kills;
		if (num_kills >= PlayerSaveLoad.playerSaver.most_kills_per_game) {
			killsText.color = new Color(219, 211, 0);
		}
		wavesClearedText.text = "Waves Cleared: " + GameController.waves_cleared;
		if (GameController.waves_cleared >= PlayerSaveLoad.playerSaver.most_waves_beaten) {
			wavesClearedText.color = new Color(219, 211, 0);
		}
		scoreText.text = "Score: " + num_score;
		highscoreText.text = "Highscore: " + PlayerSaveLoad.playerSaver.highscore;
		if (num_score >= PlayerSaveLoad.playerSaver.highscore) {
			scoreText.color = new Color(219, 211, 0);
			highscoreText.color = new Color(219, 211, 0);
		}

		coinsTextTotal.text = "Total Coins: " + PlayerSaveLoad.playerSaver.coins;
		killsTextTotal.text = "Total Kills: " + PlayerSaveLoad.playerSaver.kills;
		playsTextTotal.text = "Total Plays: " + PlayerSaveLoad.playerSaver.plays;
		highscoreTextAllTime.text = "Highscore: " + PlayerSaveLoad.playerSaver.highscore;
		mostCoins.text = "Most Coins Earned: " + PlayerSaveLoad.playerSaver.most_coins_per_game;
		mostKills.text = "Most Kills: " + PlayerSaveLoad.playerSaver.most_kills_per_game;
		mostWaves.text = "Most Waves Cleared: " + PlayerSaveLoad.playerSaver.most_waves_beaten;

		num_coins = 0;
		num_kills = 0;
		num_score = 0;
		Destroy(gameObject);
		Instantiate(explosion, GetComponent<Rigidbody2D>().position, Quaternion.identity);
		gameController.GameOver();
	}

    float GetDistance(Vector3 target)
    {
        float x = Mathf.Abs(target.x - gameObject.transform.position.x);
        float y = Mathf.Abs(target.y - gameObject.transform.position.y);
        float distance = Mathf.Sqrt((x * x) + (y * y));
        return distance;
    }

    void OnGUI()
    {
        // GUI.Label(new Rect(10, 10, 200, 60), "Coins: " + PlayerSaveLoad.playerSaver.coins);
        //GUI.Label(new Rect(10, 10, 200, 60), "x: " + thePosition.x + " y: " + thePosition.y);
    }

    /// <summary>
    ///     Changes the <see cref="sensitivity"/> value to the value of the
    ///     handle on the slider. Modifies the <see cref="sensitivity"/>
    ///     value.
    /// </summary>
    /// <param name="sensitivity_mult">
    ///     The float value of the handle on the slider.
    /// </param>
    public void AdjustSensitivity(float sensitivity_mult)
    {
        sensitivity = RoundtoInterval(sensitivity_mult, 1f / 20f);
        sensitivity_slider.value = sensitivity;
        sensitivityValueText.text = sensitivity.ToString();
		PlayerSaveLoad.playerSaver.player_sensitivity = sensitivity_slider.value;
    }

    /// <summary>
    ///     Rounds float value to the nearest multiple of the interval.
    ///     Does not modify the actual value.
    /// </summary>
    /// <param name="num">
    ///     The float value to round.
    /// </param>
    /// <param name="interval">
    ///     The float of the interval to round to.
    /// </param>
    /// <returns>
    ///     Returns a float that is rounded to the nearest multiple of the
    ///     interval.
    /// </returns>
    private float RoundtoInterval(float num, float interval)
    {
        float new_sensitivity;
        float closeness = num % interval;
        if (closeness < interval / 2)
        {
            new_sensitivity = num - closeness;
        }
        else
        {
            new_sensitivity = num - (closeness - interval);
        }
        return new_sensitivity;
    }
}
