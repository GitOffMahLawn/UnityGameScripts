// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UpgradeController : MonoBehaviour {

    private int[] DAMAGE_UPGRADE_PRICES = {
		/*200,*/ 800, 3200, 12800, 51200 };
	// 100, 200, 400, 800, 1600, 3200, 6400, 12800, 25600, 51200
    public Slider damageSlider;
    public Button damageButton;
    public Text damageLevelText;
    private Text damageText;

    private int[] HEALTH_UPGRADE_PRICES = {
		/*200,*/ 1000, 2500, 6500, 20000 };
	// 100, 200, 400, 1000, 1500, 2500, 4000, 6500, 10000, 20000
    public Slider healthSlider;
    public Button healthButton;
    public Text healthLevelText;
    private Text healthText;

    private int[] SHOT_UPGRADE_PRICES = {
		/*1000,*/ 3000, 10000, 20000, 50000 };
	// 500, 1000, 2000, 3000, 5000, 10000, 15000, 20000, 35000, 50000
    public Slider shotSlider;
    public Button shotButton;
    public Text shotLevelText;
    private Text shotText;

    private int[] SHIELD_UPGRADE_PRICES = {
        1000, 10000, 20000 };
    public Slider shieldSlider;
    public Button shieldButton;
    public Text shieldLevelText;
    private Text shieldText;

    private int[] MAGNET_UPGRADE_PRICES = {
		/*1000,*/ 3000, 5000, 10000 };
	// 500, 1000, 2000, 3000, 4000, 5000, 10000, 15000, 20000, 50000
    public Slider magnetSlider;
    public Button magnetButton;
    public Text magnetLevelText;
    private Text magnetText;

    private int[] ABILITY_UPGRADE_PRICES = {
		250000 }; // (Binary)
	// 100000, 2000, 4000, 8000, 10000 (Incremental)
	// 1500, 3000, 9000, 20000, 45000, 100000, 150000, 200000, 300000, 500000
    public Slider abilitySlider;
    public Button abilityButton;
    public Text abilityLevelText;
    private Text abilityText;

    public Text coinsText;

    private void Start()
    {
        PlayerSaveLoad.playerSaver.Load();

		PlayerSaveLoad.playerSaver.SetCoins (1000000);

        damageText = damageButton.GetComponentInChildren<Text>();
        healthText = healthButton.GetComponentInChildren<Text>();
        shotText = shotButton.GetComponentInChildren<Text>();
        shieldText = shieldButton.GetComponentInChildren<Text>();
        magnetText = magnetButton.GetComponentInChildren<Text>();
        abilityText = abilityButton.GetComponentInChildren<Text>();

        coinsText.text = PlayerSaveLoad.playerSaver.coins.ToString();

		// Delete these assignments after testing.
		// PlayerSaveLoad.playerSaver.damageUpgradeLevel = 0;
		// PlayerSaveLoad.playerSaver.healthUpgradeLevel = 0;
		// PlayerSaveLoad.playerSaver.shotUpgradeLevel = 0;
		// PlayerSaveLoad.playerSaver.shieldUpgradeLevel = 0;
		// PlayerSaveLoad.playerSaver.magnetUpgradeLevel = 0;
		// PlayerSaveLoad.playerSaver.abilityUpgradeLevel = 0;

        damageSlider.value = PlayerSaveLoad.playerSaver.damageUpgradeLevel;
        healthSlider.value = PlayerSaveLoad.playerSaver.healthUpgradeLevel;
        shotSlider.value = PlayerSaveLoad.playerSaver.shotUpgradeLevel;
        shieldSlider.value = PlayerSaveLoad.playerSaver.shieldUpgradeLevel;
        magnetSlider.value = PlayerSaveLoad.playerSaver.magnetUpgradeLevel;
        abilitySlider.value = PlayerSaveLoad.playerSaver.abilityUpgradeLevel;

        damageLevelText.text = "x" + 
            (PlayerSaveLoad.playerSaver.damageUpgradeLevel + 1).ToString();
        healthLevelText.text = 
            (PlayerSaveLoad.playerSaver.healthUpgradeLevel + 1).ToString();
        shotLevelText.text = "Type " + 
            (PlayerSaveLoad.playerSaver.shotUpgradeLevel + 1).ToString();
        shieldLevelText.text = 
            PlayerSaveLoad.playerSaver.shieldUpgradeLevel.ToString();
        magnetLevelText.text = "x" + 
            (PlayerSaveLoad.playerSaver.magnetUpgradeLevel + 1).ToString();

		if (PlayerSaveLoad.playerSaver.abilityUpgradeLevel == 0) {
			abilityLevelText.text = "Locked";
		} else {
			abilityLevelText.text = "Unlocked";
		}

        if (PlayerSaveLoad.playerSaver.damageUpgradeLevel < DAMAGE_UPGRADE_PRICES.Length)
        {
            damageText.text = DAMAGE_UPGRADE_PRICES[PlayerSaveLoad.playerSaver.damageUpgradeLevel].ToString();
        }
        else
        {
            damageText.text = "MAX";
            damageButton.interactable = false;
        }
        if (PlayerSaveLoad.playerSaver.healthUpgradeLevel < HEALTH_UPGRADE_PRICES.Length)
        {
            healthText.text = HEALTH_UPGRADE_PRICES[PlayerSaveLoad.playerSaver.healthUpgradeLevel].ToString();
        }
        else
        {
            healthText.text = "MAX";
            healthButton.interactable = false;
        }
        if (PlayerSaveLoad.playerSaver.shotUpgradeLevel < SHOT_UPGRADE_PRICES.Length)
        {
            shotText.text = SHOT_UPGRADE_PRICES[PlayerSaveLoad.playerSaver.shotUpgradeLevel].ToString();
        }
        else
        {
            shotText.text = "MAX";
            shotButton.interactable = false;
        }
        if (PlayerSaveLoad.playerSaver.shieldUpgradeLevel < SHIELD_UPGRADE_PRICES.Length)
        {
            shieldText.text = SHIELD_UPGRADE_PRICES[PlayerSaveLoad.playerSaver.shieldUpgradeLevel].ToString();
        }
        else
        {
            shieldText.text = "MAX";
            shieldButton.interactable = false;
        }
        if (PlayerSaveLoad.playerSaver.magnetUpgradeLevel < MAGNET_UPGRADE_PRICES.Length)
        {
            magnetText.text = MAGNET_UPGRADE_PRICES[PlayerSaveLoad.playerSaver.magnetUpgradeLevel].ToString();
        }
        else
        {
            magnetText.text = "MAX";
            magnetButton.interactable = false;
        }
		if (PlayerSaveLoad.playerSaver.abilityUpgradeLevel < ABILITY_UPGRADE_PRICES.Length)
        {
			abilityText.text = ABILITY_UPGRADE_PRICES [PlayerSaveLoad.playerSaver.abilityUpgradeLevel].ToString ();
		}
        else {
			abilityText.text = "MAX";
			abilityButton.interactable = false;
		}
        
        PlayerSaveLoad.playerSaver.Save();
    }

    public void UpgradeDamage()
    {
        PlayerSaveLoad.playerSaver.Load();
        if (PlayerSaveLoad.playerSaver.damageUpgradeLevel <
            DAMAGE_UPGRADE_PRICES.Length &&
            PlayerSaveLoad.playerSaver.coins >=
            DAMAGE_UPGRADE_PRICES[PlayerSaveLoad.playerSaver.damageUpgradeLevel])
        {
            PlayerSaveLoad.playerSaver.coins -=
                DAMAGE_UPGRADE_PRICES[PlayerSaveLoad.playerSaver.damageUpgradeLevel];
            PlayerSaveLoad.playerSaver.damageUpgradeLevel++;
            PlayerSaveLoad.playerSaver.Save();
            damageSlider.value = PlayerSaveLoad.playerSaver.damageUpgradeLevel;
            damageLevelText.text = "x" + 
                (PlayerSaveLoad.playerSaver.damageUpgradeLevel + 1).ToString();
            if (PlayerSaveLoad.playerSaver.damageUpgradeLevel < DAMAGE_UPGRADE_PRICES.Length)
            {
                damageText.text = DAMAGE_UPGRADE_PRICES[PlayerSaveLoad.playerSaver.damageUpgradeLevel].ToString();
            }
            else
            {
                damageText.text = "MAX";
                damageButton.interactable = false;
            }
        }
        coinsText.text = PlayerSaveLoad.playerSaver.coins.ToString();
    }

    public void UpgradeHealth()
    {
        PlayerSaveLoad.playerSaver.Load();
        if (PlayerSaveLoad.playerSaver.healthUpgradeLevel <
            HEALTH_UPGRADE_PRICES.Length &&
            PlayerSaveLoad.playerSaver.coins >=
            HEALTH_UPGRADE_PRICES[PlayerSaveLoad.playerSaver.healthUpgradeLevel])
        {
            PlayerSaveLoad.playerSaver.coins -=
                HEALTH_UPGRADE_PRICES[PlayerSaveLoad.playerSaver.healthUpgradeLevel];
            PlayerSaveLoad.playerSaver.healthUpgradeLevel++;
            PlayerSaveLoad.playerSaver.Save();
            healthSlider.value = PlayerSaveLoad.playerSaver.healthUpgradeLevel;
            healthLevelText.text = 
                (PlayerSaveLoad.playerSaver.healthUpgradeLevel + 1).ToString();
            if (PlayerSaveLoad.playerSaver.healthUpgradeLevel < HEALTH_UPGRADE_PRICES.Length)
            {
                healthText.text = HEALTH_UPGRADE_PRICES[PlayerSaveLoad.playerSaver.healthUpgradeLevel].ToString();
            }
            else
            {
                healthText.text = "MAX";
                healthButton.interactable = false;
            }
        }
        coinsText.text = PlayerSaveLoad.playerSaver.coins.ToString();
    }

	// Shot Types
	// 1. Main Guns
	// 2. Spread Guns
	// 3. Missiles

    public void UpgradeShot()
    {
        PlayerSaveLoad.playerSaver.Load();
        if (PlayerSaveLoad.playerSaver.shotUpgradeLevel <
            SHOT_UPGRADE_PRICES.Length &&
            PlayerSaveLoad.playerSaver.coins >=
            SHOT_UPGRADE_PRICES[PlayerSaveLoad.playerSaver.shotUpgradeLevel])
        {
            PlayerSaveLoad.playerSaver.coins -=
                SHOT_UPGRADE_PRICES[PlayerSaveLoad.playerSaver.shotUpgradeLevel];
            PlayerSaveLoad.playerSaver.shotUpgradeLevel++;
            PlayerSaveLoad.playerSaver.Save();
            shotSlider.value = PlayerSaveLoad.playerSaver.shotUpgradeLevel;

            shotLevelText.text = "Type " +
                (PlayerSaveLoad.playerSaver.shotUpgradeLevel + 1).ToString();
			
            if (PlayerSaveLoad.playerSaver.shotUpgradeLevel < SHOT_UPGRADE_PRICES.Length)
            {
                shotText.text = SHOT_UPGRADE_PRICES[PlayerSaveLoad.playerSaver.shotUpgradeLevel].ToString();
            }
            else
            {
                shotText.text = "MAX";
                shotButton.interactable = false;
            }
        }
        coinsText.text = PlayerSaveLoad.playerSaver.coins.ToString();
    }

    public void UpgradeShieldCap()
    {
        PlayerSaveLoad.playerSaver.Load();
        if (PlayerSaveLoad.playerSaver.shieldUpgradeLevel <
            SHIELD_UPGRADE_PRICES.Length &&
            PlayerSaveLoad.playerSaver.coins >=
            SHIELD_UPGRADE_PRICES[PlayerSaveLoad.playerSaver.shieldUpgradeLevel])
        {
            PlayerSaveLoad.playerSaver.coins -=
                SHIELD_UPGRADE_PRICES[PlayerSaveLoad.playerSaver.shieldUpgradeLevel];
            PlayerSaveLoad.playerSaver.shieldUpgradeLevel++;
            PlayerSaveLoad.playerSaver.Save();
            shieldSlider.value = PlayerSaveLoad.playerSaver.shieldUpgradeLevel;
            shieldLevelText.text = 
                PlayerSaveLoad.playerSaver.shieldUpgradeLevel.ToString();
            if (PlayerSaveLoad.playerSaver.shieldUpgradeLevel < SHIELD_UPGRADE_PRICES.Length)
            {
                shieldText.text = SHIELD_UPGRADE_PRICES[PlayerSaveLoad.playerSaver.shieldUpgradeLevel].ToString();
            }
            else
            {
                shieldText.text = "MAX";
                shieldButton.interactable = false;
            }
        }
        coinsText.text = PlayerSaveLoad.playerSaver.coins.ToString();
    }

    public void UpgradeMagnetic()
    {
        PlayerSaveLoad.playerSaver.Load();
        if (PlayerSaveLoad.playerSaver.magnetUpgradeLevel <
            MAGNET_UPGRADE_PRICES.Length &&
            PlayerSaveLoad.playerSaver.coins >=
            MAGNET_UPGRADE_PRICES[PlayerSaveLoad.playerSaver.magnetUpgradeLevel])
        {
            PlayerSaveLoad.playerSaver.coins -=
                MAGNET_UPGRADE_PRICES[PlayerSaveLoad.playerSaver.magnetUpgradeLevel];
            PlayerSaveLoad.playerSaver.magnetUpgradeLevel++;
            PlayerSaveLoad.playerSaver.Save();
            magnetSlider.value = PlayerSaveLoad.playerSaver.magnetUpgradeLevel;
            magnetLevelText.text = "x" + 
                (PlayerSaveLoad.playerSaver.magnetUpgradeLevel + 1).ToString();
            if (PlayerSaveLoad.playerSaver.magnetUpgradeLevel < MAGNET_UPGRADE_PRICES.Length)
            {
                magnetText.text = MAGNET_UPGRADE_PRICES[PlayerSaveLoad.playerSaver.magnetUpgradeLevel].ToString();
            }
            else
            {
                magnetText.text = "MAX";
                magnetButton.interactable = false;
            }
        }
        coinsText.text = PlayerSaveLoad.playerSaver.coins.ToString();
    }

    public void UpgradeAbility()
    {
        PlayerSaveLoad.playerSaver.Load();
        if (PlayerSaveLoad.playerSaver.abilityUpgradeLevel < 
            ABILITY_UPGRADE_PRICES.Length && 
            PlayerSaveLoad.playerSaver.coins >= 
            ABILITY_UPGRADE_PRICES[PlayerSaveLoad.playerSaver.abilityUpgradeLevel])
        {
            PlayerSaveLoad.playerSaver.coins -=
                ABILITY_UPGRADE_PRICES[PlayerSaveLoad.playerSaver.abilityUpgradeLevel];
            PlayerSaveLoad.playerSaver.abilityUpgradeLevel++;
            PlayerSaveLoad.playerSaver.Save();
            abilitySlider.value = PlayerSaveLoad.playerSaver.abilityUpgradeLevel;
			if (PlayerSaveLoad.playerSaver.abilityUpgradeLevel == 0)
			{
				abilityLevelText.text = "Locked";
			}
			else
			{
				abilityLevelText.text = "Unlocked";
			}

            if (PlayerSaveLoad.playerSaver.abilityUpgradeLevel < ABILITY_UPGRADE_PRICES.Length)
            {
                abilityText.text = ABILITY_UPGRADE_PRICES[PlayerSaveLoad.playerSaver.abilityUpgradeLevel].ToString();
            }
            else
            {
                abilityText.text = "MAX";
                abilityButton.interactable = false;
            }
        }
        coinsText.text = PlayerSaveLoad.playerSaver.coins.ToString();
    }

    public void ResetUpgrades()
    {
		PlayerSaveLoad.playerSaver.damageUpgradeLevel = 0;
        PlayerSaveLoad.playerSaver.healthUpgradeLevel = 0;
        PlayerSaveLoad.playerSaver.shotUpgradeLevel = 0;
        PlayerSaveLoad.playerSaver.shieldUpgradeLevel = 0;
		PlayerSaveLoad.playerSaver.magnetUpgradeLevel = 0;
        PlayerSaveLoad.playerSaver.abilityUpgradeLevel = 0;
        PlayerSaveLoad.playerSaver.Save();
        SceneManager.LoadScene("Upgrades");
    }
}
