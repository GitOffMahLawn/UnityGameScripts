using System.Collections;
// using System.Collections.Generic;
using UnityEngine;

public class PlanetController : MonoBehaviour {
    /*
	public int planetID;

	public GameObject homePlanetBox;
	public GameObject planetABox;
	public GameObject planetBBox;
    */
    public float growFactor;

    private Vector3 scaleIncrease;

    private Vector3 original_scale;

    private float maxScale;
    private float waitTime;

    void Start()
    {
        original_scale = GetComponent<RectTransform>().localScale;
        maxScale = original_scale.x * 1.1f;
        scaleIncrease = new Vector3(
            original_scale.x * 0.1f * 2f,
            original_scale.y * 0.1f * 2f,
            0f);
        waitTime = 0.01f;
    }

    public void Test()
    {
        Debug.Log("in method");
    }

    public void ScaleUp(float growFactor)
	{
        // if(!homePlanetBox.activeSelf && !planetABox.activeSelf && !planetBBox.activeSelf)
        // {
        /*
        switch (planetID)
        {
            case 0:
                homePlanetBox.SetActive(true);
                planetABox.SetActive(false);
                planetBBox.SetActive(false);
                break;
            case 1:
                homePlanetBox.SetActive(false);
                planetABox.SetActive(true);
                planetBBox.SetActive(false);
                break;
            case 2:
                homePlanetBox.SetActive(false);
                planetABox.SetActive(false);
                planetBBox.SetActive(true);
                break;
        }
        */
        /*
			if (planetID == 0)
			{
				homePlanetBox.SetActive (true);
				planetABox.SetActive(false);
				planetBBox.SetActive(false);
			}
			if (planetID == 1) {
				homePlanetBox.SetActive(false);
				planetABox.SetActive(true);
				planetBBox.SetActive(false);
			}
			if (planetID == 2) {
				homePlanetBox.SetActive(false);
				planetABox.SetActive(false);
				planetBBox.SetActive(true);
			}
		// }
        */
        float timer = 0f;

        while (maxScale > GetComponent<RectTransform>().localScale.x)
        {
            timer += Time.deltaTime;
            GetComponent<RectTransform>().localScale += scaleIncrease * Time.deltaTime * growFactor;
        }
	}

	public IEnumerator ScaleDown()
	{
        float timer = 0;
        while (original_scale.x < transform.localScale.x)
        {
            timer += Time.deltaTime;
            transform.localScale -= scaleIncrease * Time.deltaTime * growFactor;
            yield return null;
        }

        timer = 0;
        yield return new WaitForSeconds(waitTime);
    }
    
}
