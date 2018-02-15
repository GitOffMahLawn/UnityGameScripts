// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;

public class Explosion_Destroy : MonoBehaviour {
    
    void FixedUpdate()
    {
        Destroy(gameObject, 0.5f);
    }
}
