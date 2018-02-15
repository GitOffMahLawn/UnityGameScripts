// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;

public class Background_Adjuster : MonoBehaviour {

	// Use this for initialization
    /// <summary>
    ///     Sets the background to the aspect ratio.
    /// </summary>
	void Start () {
        float bgAspectRatio =
            GetComponent<MeshRenderer>().bounds.size.x /
            GetComponent<MeshRenderer>().bounds.size.y;
        float x_scale = Camera.main.orthographicSize * 4.0f;
        float y_scale = x_scale * bgAspectRatio;
        transform.localScale = (new Vector3(x_scale, y_scale, 0f));
	}
}
