// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;

public class DestroyOnContact : MonoBehaviour {

	// private GameController gameController;

	void Start()
	{
		/*GameObject gameControllerObject = GameObject.FindWithTag("GameController");
		if (gameControllerObject != null)
		{
			gameController = gameControllerObject.GetComponent<GameController>();
		}
		if (gameControllerObject == null)
		{
			Debug.Log("Cannot find 'GameController' script");
		}*/
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
		if (collision.tag == "Bolt" || collision.tag == "EnemyBolt")
		{
			Destroy (collision.gameObject);
		}
    }
}
