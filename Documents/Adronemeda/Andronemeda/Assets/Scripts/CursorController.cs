// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour {

    private int height;
    private int width;
    private Vector2 mousePos;

    public float horizontalSpeed = 10f; // looks like "10" maps to the native speed
    public float verticalSpeed = 10f;

    //public GameObject cursorObj;

    public bool allowCursorMove = true;

    // Use this for initialization
    void Start ()
    {
        //Instantiate(cursorObj, new Vector3(0f, 0f, 0f), Quaternion.identity);
        CheckCursorLock();

        height = (int)Camera.main.orthographicSize;
        width = height > Screen.width ? Screen.width : Mathf.RoundToInt(height * 9f / 16f);
    }

    void CheckCursorLock()
    {
        if (Cursor.lockState == CursorLockMode.None)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void Update()
    {
        mousePos = new Vector2(transform.position.x, transform.position.x);
        if (Input.GetMouseButtonDown(0))
        {
            CheckCursorLock();
        }
        float h = horizontalSpeed * Input.GetAxis("Mouse X");
        float v = verticalSpeed * Input.GetAxis("Mouse Y");
        Vector3 delta = new Vector3(h, v, 0);

        if (allowCursorMove)
        {
            transform.position += delta; // moves the virtual cursor
            mousePos.x = Mathf.Clamp(transform.position.x, -width, width);
            mousePos.y = Mathf.Clamp(transform.position.y, -height, height);
            transform.position = mousePos;
        }
    }
}
