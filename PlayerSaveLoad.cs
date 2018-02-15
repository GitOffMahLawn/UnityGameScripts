// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
public class PlayerSaveLoad : MonoBehaviour {

    public static PlayerSaveLoad playerSaver;

	/// <summary>
	/// 	The amount of shields the player can have at one time. Available to upgrade.
	/// </summary>
	//public int shieldCapacity;
    
    /// <summary>
    ///     The total amount of coins the user has ever earned.
    /// </summary>
    public int coins;

    /// <summary>
    ///     The total amount of kills the user has ever had.
    /// </summary>
	public int kills;

    /// <summary>
    ///     The amount of times the user has played arcade mode.
    /// </summary>
	public int plays;

    /// <summary>
    ///     The user's highscore in arcade mode.
    /// </summary>
	public int highscore;

    /// <summary>
    ///     The greatest amount of coins the user
    ///     has earnedin one arcade game.
    /// </summary>
    public int most_coins_per_game;

    /// <summary>
    ///     The greatest amount of enemies the user has killed
    ///     in one arcade game.
    /// </summary>
    public int most_kills_per_game;

    /// <summary>
    ///     The most waves the user has beaten in one arcade game.
    /// </summary>
    public int most_waves_beaten;

    public float player_sensitivity;

    public float music_volume;

    public int damageUpgradeLevel;
    public int healthUpgradeLevel;
    public int shotUpgradeLevel;
    public int shieldUpgradeLevel;
    public int magnetUpgradeLevel;
    public int abilityUpgradeLevel;

    // Use this for initialization
    void Awake () {
		if (playerSaver == null)
        {
            DontDestroyOnLoad(gameObject);
            playerSaver = this;
        }
        else if (playerSaver != this)
        {
            Destroy(gameObject);
        }
	}
	
    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/playerInfo.dat");

        PlayerData data_to_save = new PlayerData
        {
			//shieldCapacity = shieldCapacity,
            coins = coins,
            kills = kills,
            plays = plays,
            highscore = highscore,
            most_coins_per_game = most_coins_per_game,
            most_kills_per_game = most_kills_per_game,
            most_waves_beaten = most_waves_beaten,
            player_sensitivity = player_sensitivity,
            music_volume = music_volume,
            damageUpgradeLevel = damageUpgradeLevel,
            healthUpgradeLevel = healthUpgradeLevel,
            shotUpgradeLevel = shotUpgradeLevel,
            shieldUpgradeLevel = shieldUpgradeLevel,
            magnetUpgradeLevel = magnetUpgradeLevel,
            abilityUpgradeLevel = abilityUpgradeLevel
    };
		/*PlayerData killData = new PlayerData
		{
			kills = kills
		};
		PlayerData highscoreData = new PlayerData
		{
			highscore = highscore
		};*/

		bf.Serialize(file, data_to_save);
		/*bf.Serialize(file, killData);
		bf.Serialize(file, highscoreData);*/
        file.Close();
    }

    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);

            PlayerData data = (PlayerData)bf.Deserialize(file);
            file.Close();

			//shieldCapacity = data.shieldCapacity;
            coins = data.coins;
			kills = data.kills;
			plays = data.plays;
			highscore = data.highscore;
            most_coins_per_game = data.most_coins_per_game;
            most_kills_per_game = data.most_kills_per_game;
            most_waves_beaten = data.most_waves_beaten;
            player_sensitivity = data.player_sensitivity;
            music_volume = data.music_volume;
            damageUpgradeLevel = data.damageUpgradeLevel;
            healthUpgradeLevel = data.healthUpgradeLevel;
            shotUpgradeLevel = data.shotUpgradeLevel;
            shieldUpgradeLevel = data.shieldUpgradeLevel;
            magnetUpgradeLevel = data.magnetUpgradeLevel;
            abilityUpgradeLevel = data.abilityUpgradeLevel;
        }
    }

	public void AddCoins(int val)
	{
		coins += val;
	}

	public void SetCoins(int val)
	{
		coins = val;
	}

    public float GetDamage()
    {
        return 1 + (float)damageUpgradeLevel * 3 / 5;
    }

    [System.Serializable]
    class PlayerData
    {
		//public int shieldCapacity;
        public int coins;
		public int kills;
		public int plays;
		public int highscore;
        public int most_coins_per_game;
        public int most_kills_per_game;
        public int most_waves_beaten;
        public float player_sensitivity;
        public float music_volume;
        public int damageUpgradeLevel;
        public int healthUpgradeLevel;
        public int shotUpgradeLevel;
        public int shieldUpgradeLevel;
        public int magnetUpgradeLevel;
        public int abilityUpgradeLevel;
    }
}
