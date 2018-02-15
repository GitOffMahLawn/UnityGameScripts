// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour {

    public float speed;
    public float acceleration;

    private GameObject playerShip;

	private Vector3 coinMove;

    private GameController gameController;
	private float originalSpeed;
	private float magneticRange;
    private bool paused;

	// Use this for initialization
	void Start () {
        playerShip =
            GameObject.FindWithTag("Player");

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

        originalSpeed = 0.00625f;

        // Added by 1 to account for the base 0 upgrade system
		if (!gameController.PlayerIsDead ())
		{
			magneticRange = playerShip.GetComponent<PlayerController> ().GetMagneticRange () + 1;
		}
    }
	
	// Update is called once per frame
	void Update () {
        paused = gameController.GetPaused();
        if (!paused)
        {
            Vector3 diff;
			if (!gameController.PlayerIsDead ())
			{
				float distance = GetDistance (playerShip.transform.position);
				float angle = Mathf.Asin ((playerShip.transform.position.y -
				                          gameObject.transform.position.y) / distance);
				if (distance < magneticRange)
				{
					diff = new Vector3 (
						playerShip.transform.position.x > transform.position.x ?
                        Mathf.Cos (angle) : -Mathf.Cos (angle),
						Mathf.Sin (angle),
						0f);
					diff *= speed;
					coinMove = diff;
					speed += acceleration;
				}
				else
				{
					// speed = originalSpeed;
					if (speed > originalSpeed)
					{
						diff = coinMove;
						speed -= acceleration;
						coinMove = new Vector3 (
							(playerShip.transform.position.x -
							gameObject.transform.position.x) * speed,

							(playerShip.transform.position.y -
							gameObject.transform.position.y) * speed,
							0f);
					}
					else
					{
						diff = new Vector3 (0f, -speed, 0f);
					}
				}
				transform.position += diff;
			}
			else
			{
				diff = new Vector3 (0f, -originalSpeed, 0f);
				transform.position += diff;
			}
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Boundary")
		{
			Destroy (gameObject);
		}
	}

    float GetDistance(Vector3 target)
    {
        float x = Mathf.Abs(target.x - gameObject.transform.position.x);
        float y = Mathf.Abs(target.y - gameObject.transform.position.y);
        float distance = Mathf.Sqrt((x * x) + (y * y));
        return distance;
    }
}
