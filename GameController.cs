using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
	// Constant Variables

	/// <summary>
	/// 	The value of a copper coin.
	/// </summary>
	public const int COPPER_VALUE = 1;

	/// <summary>
	/// 	The value of a silver coin.
	/// </summary>
	public const int SILVER_VALUE = 5;

	/// <summary>
	/// 	The value of a gold coin.
	/// </summary>
	public const int GOLD_VALUE = 10;

	/// <summary>
	/// 	The value of an emerald coin.
	/// </summary>
	public const int EMERALD_VALUE = 20;

	/// <summary>
	/// 	The value of a sapphire coin.
	/// </summary>
	public const int SAPPHIRE_VALUE = 50;

	/// <summary>
	/// 	The value of an amythest coin.
	/// </summary>
	public const int AMYTHEST_VALUE = 100;

	// Static Objects

	/// <summary>
	/// 	The stats panel of the canvas.
	/// </summary>
	public static GameObject STATS_MENU;

	// Static Variables

	/// <summary>
	///     The amount of waves cleared in one round of arcade mode.
	/// </summary>
	public static int waves_cleared;

	// Public Objects

	/// <summary>
	/// 	The larger explosion model used for the player ship death.
	/// </summary>
	public GameObject explosion1;

	/// <summary>
	/// 	The asteroid model.
	/// </summary>
    public GameObject asteroid;

	/// <summary>
	/// 	The mine model.
	/// </summary>
	public GameObject mine;

	/// <summary>
	/// 	The shield power up model.
	/// </summary>
	public GameObject shieldPowerUp;

	/// <summary>
	/// 	The third and final shield icon in the player's shieldIcons bar, used to
	/// 	detect if the player has a full set of shield powerups.
	/// </summary>
	public GameObject lastShieldIcon;

	/// <summary>
	/// 	The barracuda spaceship model.
	/// </summary>
	public GameObject barracuda;

	/// <summary>
	/// 	The leatherback spaceship model.
	/// </summary>
	public GameObject leatherback;

	/// <summary>
	/// 	The piranha spaceship model.
	/// </summary>
	public GameObject piranha;

	/// <summary>
	/// 	The text that displays the player's score.
	/// </summary>
	public Text scoreText;

    /// <summary>
    ///     The <see cref="Slider"/> <see cref="GameObject"/> used to control
    ///     the bg volume.
    /// </summary>
    public Slider bgVolSlider;

    // Public Variables

    /// <summary>
    /// 	The minimum number of asteroids that will spawn per wave.
    /// </summary>
    public int minAsteroidsPerWave;

	/// <summary>
	/// 	The maximum number of asteroids that will spawn per wave.
	/// </summary>
	public int maxAsteroidsPerWave;

	/// <summary>
	/// 	The minimum number of barracuda spaceships that will spawn per wave.
	/// </summary>
	public int minBarracudasPerWave;

	/// <summary>
	/// 	The maximum number of barracuda spaceships that will spawn per wave.
	/// </summary>
	public int maxBarracudasPerWave;

	/// <summary>
	/// 	The minimum number of piranha spaceships that will spawn per wave.
	/// </summary>
	public int minPiranhasPerWave;

	/// <summary>
	/// 	The maximum number of piranha spaceships that will spawn per wave.
	/// </summary>
	public int maxPiranhasPerWave;

	/// <summary>
	/// 	The maximum number of digits in the score.
	/// </summary>
	public int maxDigits = 6;

	// Private Objects

	/// <summary>
	/// 	The player's ship.
	/// </summary>
	private GameObject playerShip;

	/// <summary>
	/// 	The pause panel of the canvas.
	/// </summary>
	private GameObject pauseMenu;

	/// <summary>
	/// 	The options panel of the canvas.
	/// </summary>
	private GameObject optionsPanel;

	/// <summary>
	///     The gameover panel of the canvas.
	/// </summary>
	private GameObject gameOverMenu;

    /// <summary>
	/// 	The player controller.
	/// </summary>
	private PlayerController playerController;

    /// <summary>
    /// 	The game camera.
    /// </summary>
    private Camera cam;

    /// <summary>
    ///     The ArrayList of the enemy ships on the screen. Keeps track of the
    ///     ships that are still alive.
    /// </summary>
	private ArrayList spaceships;

    /// <summary>
    ///     The text of the bg music volume.
    /// </summary>
	private Text bgMusicVolText;

    /// <summary>
    ///     The bg music audio file.
    /// </summary>
	private AudioSource bgmusic;

	// Private Variables

    /// <summary>
    ///     The height of the screen in pixels.
    /// </summary>
	private int height;

    /// <summary>
    ///     The width of the screen in pixels.
    /// </summary>
	private int width;

	// private int shieldCapacity;

    /// <summary>
    ///     
    /// </summary>
	private int slope;
	private int angle_index;
	private int leatherback_angle_index;

	private int score;

    private float maxWidth;
    private float asteroidRadius;
	private float asteroidYPos;
	private float shieldRadius;
	private float mineRadius;
	/// <summary>
	/// 	The difficulty multiplier. Used for increasing the speed of the bullets
	/// 	and making the level more difficult to survive.
	/// </summary>
	private float difficultyMultiplier;
	private float minShieldPowerUpChance;
	private float shieldSpawnDenominator = 2000f;
	private float minMineChance;
	private float mineSpawnDenominator = 1000f;
	private float angle;
	private float leatherback_angle;

	private bool isFullscreen;

	/// <summary>
	/// 	The variable that displays whether or not the game is paused.
	/// </summary>
	private bool paused;
	private bool inWave;
	private bool justMultiplied;
	private bool gameOver;

	private float[] angles;
	private float[] leatherback_angles;

    /// <summary>
    /// 	The height of the screen and the width of the screen are used to
    /// 	set the	aspect ratio.
    /// 	<para/>
    ///     The resolution is set to the correct figures.
    /// 	<para/>
    /// 	<see cref="gameOverText"/> and <see cref="restartText"/>
    /// 	are initialized.
    /// 	<para/>
    /// 	<see cref="cam"/> is initalized to <see cref="Camera.main"/>.
    /// 	A new <see cref="ArrayList"/> of spaceships is created.
    /// 	<para/>The background music is initialized. The
    /// 	<see cref="asteroidRadius"/> is set based on the bounds of the
    /// 	screen.
    /// 	<para/>
    /// 	The target width is set as a reference for spawning asteroids
    /// 	within the screen.
    /// 	<para/>
    /// 	The starting <see cref="slope"/> (obsolete) is initialized to -2.
    /// 	<para/>
    /// 	The <see cref="angles"/> array is initialized with nine radian
    /// 	angles.
    /// 	<para/>
    /// 	<see cref="minShieldPowerUpChance"/> is initialized.
    /// 	<c>UpdateScore</c> and <c>StartCoroutine</c> are called.
    /// </summary>
    void Start()
	{
        PlayerSaveLoad.playerSaver.Load();
        GameObject playerShipObject = GameObject.FindWithTag("Player");
		if (playerShipObject != null)
		{
			playerShip = playerShipObject;
		}
		if (playerShip == null)
		{
			Debug.Log("Cannot find PlayerShip object");
		}

		playerController = playerShip.GetComponent<PlayerController>();

        height = Screen.height;
        width = height > Screen.width ? 
            Screen.width : Mathf.RoundToInt(height * cam.aspect);
		if (!SystemInfo.operatingSystem.Contains ("Mac")) {
			isFullscreen = true;
		}
        Screen.SetResolution(width, height, isFullscreen, 60);

        if (cam == null)
            cam = Camera.main;

		spaceships = new ArrayList();

		waves_cleared = 0;

		bgmusic = GetComponent<AudioSource>();
        bgMusicVolText = 
            GameObject.FindWithTag("Music Vol Text").GetComponent<Text>();
        bgMusicVolText.text = 
            ConvertFloatToPercent(PlayerSaveLoad.playerSaver.music_volume);
        bgVolSlider.value = PlayerSaveLoad.playerSaver.music_volume;
        // Gets the bounds of the screen
        asteroidRadius = asteroid.GetComponent<Renderer>().bounds.extents.x;
		shieldRadius = shieldPowerUp.GetComponent<Renderer>().bounds.extents.x;
		mineRadius = mine.GetComponent<Renderer>().bounds.extents.x;
		// Rewritten to account for the new bounds
		// dictated by the build resolution
        Vector3 targetWidth = cam.ScreenToWorldPoint(
            new Vector3(Screen.width - 5.0f, 5.2f, 0.0f));
        float asteroidWidth = asteroidRadius;
        maxWidth = targetWidth.x - asteroidWidth;
        asteroidYPos = 7.2f;

		// shieldCapacity = playerController.GetShieldCapacity();

		angles = new float[] {
			Mathf.PI / 2,
			Mathf.PI / 3,
			2 * Mathf.PI / 3
        };

		leatherback_angles = new float[] {
			7 * Mathf.PI / 6,
			5 * Mathf.PI / 4,
			4 * Mathf.PI / 3,
			17 * Mathf.PI / 12,
			3 * Mathf.PI / 2,
			19 * Mathf.PI / 12,
			5 * Mathf.PI / 3,
			7 * Mathf.PI / 4,
			11 * Mathf.PI / 6
		};

        scoreText.text = ExtraZeros(score) + "0";

		difficultyMultiplier = 1;
		justMultiplied = true;
		
		minShieldPowerUpChance = 0.0f;
		minMineChance = 0.0f;
        // Coroutine is like threads in a CPU, running processes in parallel.
        StartCoroutine(Spawn());
    }

	/// <summary>
	/// 	Called once per frame. Checks whether the game is over.
    /// 	Checks whether the game is restarted. If it is, reloads
	/// 	the scene. Checks whether the "P" button is pressed. If it is, pauses the
	/// 	game.
	/// </summary>
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.P))
        {
            if (paused)
            {
                ResumeGame();
                pauseMenu.SetActive(false);
				optionsPanel.SetActive (false);
            }
            else
            {
                if (!gameOver)
                {
                    PauseGame();
                    pauseMenu.SetActive(true);
                }
            }
        }

		if (playerController.GetShields () >= playerController.GetShieldCapacity())
        {
			// Deactivates shield spawning
            minShieldPowerUpChance = shieldSpawnDenominator;
        }
        else
        {
			// Activates shield spawning
            minShieldPowerUpChance = 0.0f;
        }
        if (!paused && !gameOver)
        {
            float randShieldChance = Random.Range(minShieldPowerUpChance, shieldSpawnDenominator);

            if (randShieldChance <= 1.0f)
            {
                float randShieldPos = Random.Range(-maxWidth + shieldRadius, maxWidth - shieldRadius);
                Vector3 shieldLocation = new Vector3(randShieldPos, asteroidYPos, 0.0f); // Previously 3.0f
                Instantiate(shieldPowerUp, shieldLocation, Quaternion.identity);
            }

			float randMineChance = Random.Range(minMineChance, mineSpawnDenominator);

			if (randMineChance <= 1.0f)
			{
				float randMinePos = Random.Range(-maxWidth + mineRadius, maxWidth - mineRadius);
				Vector3 mineLocation = new Vector3(randMinePos, asteroidYPos, 0.0f); // Previously 3.0f
				Instantiate(mine, mineLocation, Quaternion.identity);
			}
        }
        if (gameOver && pauseMenu.activeSelf)
        {
            pauseMenu.SetActive(false);
        }
    }

	/// <summary>
	/// 	Called once per physics frame. If the last shield is active, it sets the
	/// 	minShieldPowerUpChance to 1000.0f. If the last shield is not active, it
	/// 	sets the minShieldPowerUpChance to 0.0f so shield power ups cannot spawn.
	/// 	If the game is not paused and the player has not been killed, there is a
	/// 	1 in 1000 chance that a shield power up will spawn every frame. If the
	/// 	chance is good, the shield will be instantiated in a random x position
	/// 	above the camera's line of sight.
	/// </summary>
	void FixedUpdate()
	{
		if (!paused && !gameOver)
		{
			if (playerController.GetKills () % 10 == 0)
			{
				if (!justMultiplied)
				{
					difficultyMultiplier += 0.05f;
					justMultiplied = true;
				}
			}
			else
			{
				justMultiplied = false;
			}
		}
	}

    /// <summary>
    ///     Finds the panels with tags and then sets then sets them as inactive.
    /// </summary>
	public void ExecutePauseMenuShutdown()
	{
        pauseMenu = GameObject.FindGameObjectWithTag("Pause Menu");
        pauseMenu.SetActive(false);
        optionsPanel = GameObject.FindGameObjectWithTag ("Options Panel");
        optionsPanel.SetActive (false);
        gameOverMenu = GameObject.FindGameObjectWithTag("Gameover Menu");
        gameOverMenu.SetActive(false);
        STATS_MENU = GameObject.FindGameObjectWithTag("Stats Menu");
        STATS_MENU.SetActive (false);
	}
    
	/// <summary>
	/// 	Returns true if the game is paused.
	/// </summary>
	/// <returns> true, if game is paused, false otherwise.</returns>
	public bool GetPaused()
	{
		return paused;
	}

	/// <summary>
	/// 	Returns true if the player is dead.
	/// </summary>
	/// <returns><c>true</c>, if player is dead, <c>false</c> otherwise.</returns>
	public bool PlayerIsDead()
	{
		return gameOver;
	}

	/// <summary>
	/// 	Returns the difficulty multiplier. Used for increasing bullet speeds.
	/// </summary>
	/// <returns>The difficulty multiplier.</returns>
	public float GetDifficultyMultiplier()
	{
		return difficultyMultiplier;
	}

    // TODO Double check what the save and load are doing 
    /// <summary>
    ///     Saves the game and pauses the game if game is not over. Also
    ///     pauses the music.
    /// </summary>
    public void PauseGame()
    {
        //PlayerSaveLoad.playerSaver.Load();
        PlayerSaveLoad.playerSaver.Save();
        if (!gameOver)
        {
            paused = true;
            bgmusic.Pause();
        }
    }

    /// <summary>
    ///     Resumes the game and music.
    /// </summary>
    public void ResumeGame()
    {
        paused = false;
        if (!gameOver)
            bgmusic.Play();
        //PlayerSaveLoad.playerSaver.Save();
        PlayerSaveLoad.playerSaver.Load();
    }

	/// <summary>
	/// 	Returns the current slope (obsolete).
	/// </summary>
	/// <returns>The current slope.</returns>
	public int GetNextSlope()
	{
		return slope;
	}

	/// <summary>
	/// 	If slope is equal to 2, slope is changed to -2. Otherwise, slope is
	/// 	incremented.
	/// </summary>
	public void IncrementSlope()
	{
		if (slope == 2) {
			slope = -2;
		} else {
			slope++;
		}
	}
    
	/// <summary>
	/// 
	/// ChangeAngle indexes the array angles, which contains
	/// the different angles the leatherback fires at in radians.
	/// 
	/// </summary>
	public float ChangeAngle()
	{
		angle_index++;
		if (angle_index == angles.Length)
		{
			angle_index = 0;
		}
		return angles[angle_index];
	}

    /// <summary>
    /// 
    /// ChangeLeatherbackAngle indexes the array angles, which contains
    /// the different angles the leatherback fires at in radians.
    /// 
    /// </summary>
    public float ChangeLeatherbackAngle()
    {
        leatherback_angle_index++;
        if (leatherback_angle_index == leatherback_angles.Length)
        {
            leatherback_angle_index = 0;
        }
        return leatherback_angles[leatherback_angle_index];
    }
    /*
     * No longer necessary.
	/// <summary>
	/// 	Sets the pause menu to active.
	/// </summary>
	void TurnOnPauseMenu()
	{
		pauseMenu.SetActive(true);
	}

	/// <summary>
	/// 	Sets the pause menu to inactive.
	/// </summary>
	void TurnOffPauseMenu()
	{
		pauseMenu.SetActive(false);
	}
    */

    // TODO: Update the summary of Spawn().
	/// <summary>
	/// 	Waits for some time, then calculates a random wave. If randWaveType is
	/// 	less than or equal to 0.33, between minAsteroidsPerWave and
	/// 	maxAsteroidsPerWave will spawn, with a pause between each. Otherwise if
	/// 	randWaveType is less than or equal to 0.66, between minBarracudasPerWave
	/// 	and maxBarracudasPerWave will spawn simultaneously. Otherwise, one
	/// 	leatherback will spawn. If the length of spaceships is greater than 0,
	/// 	inWave is set to true. Otherwise, inWave is set to false. If the game
	/// 	is over, breaks out of this loop. Pauses after each wave spawned.
	/// </summary>
    IEnumerator Spawn()
    {
        yield return new WaitForSeconds(0.75f);
        while (true)
        {
			float randWaveType = Random.Range(0.0f, 0.1f);
			if (randWaveType <= 0.20f) {
				int numAsteroidsThisWave = Random.Range (minAsteroidsPerWave, maxAsteroidsPerWave + 1);
				for (int i = 0; i < numAsteroidsThisWave; i++) {
					if (!paused && !inWave) {
						float randPos = Random.Range (-maxWidth + asteroidRadius,
                            maxWidth - asteroidRadius);
							
						Instantiate (asteroid,
							new Vector3 (randPos, asteroidYPos, 0.0f),
							Quaternion.identity);
							
						yield return new WaitForSeconds (Random.Range (0.25f, 0.5f));
					}
				}
			} else if (randWaveType <= 0.40f) {
				int numSpaceships = Random.Range (minBarracudasPerWave, maxBarracudasPerWave + 1);
				for (int j = 0; j < numSpaceships; j++) {
					if (!paused && !inWave) {
						float randPos = Random.Range (-maxWidth + asteroidRadius,
							                maxWidth - asteroidRadius);

						Instantiate (barracuda, new Vector3 (randPos, 6.0f, 0.0f), Quaternion.identity);
						spaceships.Add (barracuda);
					}
				}
			} else if (randWaveType <= 0.60f) {
				if (!paused && !inWave)
				{
					Instantiate (leatherback, new Vector3 (0.0f, 6.0f, 0.0f), Quaternion.identity);
					spaceships.Add (leatherback);
				}
			} else if (randWaveType <= 0.80f) {
				int numSpaceships = Random.Range (minPiranhasPerWave, maxPiranhasPerWave + 1);
				for (int k = 0; k < numSpaceships; k++) {
					if (!paused && !inWave) {
						float randPos = Random.Range (-maxWidth + asteroidRadius,
							                maxWidth - asteroidRadius);
						
						Instantiate (piranha, new Vector3 (randPos, 6.0f, 0.0f), Quaternion.identity);
						spaceships.Add (piranha);
					}
				}
			} else {
				if (!paused && !inWave) {
					Instantiate (leatherback, new Vector3 (0.0f, 6.0f, 0.0f), Quaternion.identity);
					spaceships.Add (leatherback);
				}
				int numSpaceships = Random.Range (minPiranhasPerWave, maxPiranhasPerWave + 3);
				for (int k = 0; k < numSpaceships; k++) {
					if (!paused && !inWave) {
						float randPos = Random.Range (-maxWidth + asteroidRadius,
							                maxWidth - asteroidRadius);

						Instantiate (piranha, new Vector3 (randPos, 6.0f, 0.0f), Quaternion.identity);
						spaceships.Add (piranha);
					}
				}
			}
            if (spaceships.Count > 0)
			{
				inWave = true;
			}
			else
			{
				inWave = false;
                waves_cleared++;
			}

            if (gameOver)
            {
                // Subtracts one from wave_cleared if the player
                // dies before clearing the wave.
                break;
            }
            yield return new WaitForSeconds(Random.Range(2.0f, 3.0f));
        }
    }

	/// <summary>
	/// 	Removes a spaceship from the spaceships ArrayList.
	/// </summary>
	public void SubtractShip()
	{
		spaceships.RemoveAt(0);
	}

	/// <summary>
	/// 	Adds a score value to the score variable and updates the score.
	/// </summary>
	/// <param name="value">Score value.</param>
    public void AddScore(int value)
    {
        score += value;
        if (score >= Mathf.Pow(10, maxDigits - 1))
            maxDigits++;
        UpdateScore();
    }

	public int GetScore()
	{
		return score;
	}
	/// <summary>
	/// 	Updates the score text.
	/// </summary>
    void UpdateScore()
    {
        scoreText.text = ExtraZeros(score) + score.ToString();
    }

	/// <summary>
	/// 	Creates extra zeros to add to the score string.
	/// </summary>
	/// <returns>Extra zeros.</returns>
	/// <param name="score">Score.</param>
    private string ExtraZeros(int score)
    {
        string result = "";
        for (int i = 0; i < maxDigits - score.ToString().Length; i++)
            result += "0";
        return result;
    }

	/// <summary>
	/// 	Sets gameOver to true. Stops the background music.
	/// </summary>
    public void GameOver()
    {
        gameOver = true;
        gameOverMenu.SetActive(true);
		bgmusic.Stop();
    }

    /// <summary>
    ///     Adjusts the background music volume.
    /// </summary>
    /// <param name="value">The percentage of the volume to adjust.</param>
    public void AdjustMusicVolume(float value)
    {
        bgmusic.volume = value;
        PlayerSaveLoad.playerSaver.music_volume =
            (float)System.Math.Round(value, 2);
        bgMusicVolText.text = ConvertFloatToPercent(value);
    }

    /// <summary>
    ///     Convert a float value to a percentage string.
    /// </summary>
    /// <param name="value">The float value to convert.</param>
    /// <returns>A whole number with a percent sign behind it.</returns>
    private string ConvertFloatToPercent(float value)
    {
        return (System.Math.Round(value, 2) * 100).ToString() + "%";
    }

	/// <summary>
	/// 	Displays the width and height of the screen in a GUI label.
	/// </summary>
	void OnGUI()
	{
		// GUI.Label(new Rect(100, 100, 200, 30), "Width: " + width + " Height: " + height);
	}
}
