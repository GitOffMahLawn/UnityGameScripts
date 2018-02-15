// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;

public class TreeRenderer : MonoBehaviour {

	public GameObject tree1;
	public GameObject tree2;

	// Use this for initialization
	void Start () {
		for (int i = 0; i < 150; i++) {
			int randMultiplierX;
			float randMultiplierXVal = Random.value;
			if (randMultiplierXVal <= 0.5) {
				randMultiplierX = 1;
			} else {
				randMultiplierX = -1;
			}

			int randMultiplierY;
			float randMultiplierYVal = Random.value;
			if (randMultiplierYVal <= 0.5) {
				randMultiplierY = 1;
			} else {
				randMultiplierY = -1;
			}

			float treeX = Random.value * 4f * randMultiplierX;
			float treeY = Random.value * 5.3f * randMultiplierY;

			Vector3 treePosition = new Vector3 (treeX, treeY, -1);

			int randTree = Random.Range (0, 2);
			if (randTree == 0) {
				Instantiate (tree1, treePosition, Quaternion.identity);
			}
			else
			{
				Instantiate (tree2, treePosition, Quaternion.identity);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
