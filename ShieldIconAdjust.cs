// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;
//new comment
public class ShieldIconAdjust : MonoBehaviour {

	// Use this for initialization
	void Start () {
        float height = Camera.main.orthographicSize;
        float width = height * Screen.width / Screen.height;

        // Sets the position of the empty object that holds the shield icons
        // to the bottom right corner with an offset
        transform.position = new Vector3(
            (float)(-width + width * 0.12),
            (float)(-height + height * 0.1),
            0f);
        //Debug.Log("Width: " + width + " Height: " + height);
        //Debug.Log(transform.position);
	}
}
