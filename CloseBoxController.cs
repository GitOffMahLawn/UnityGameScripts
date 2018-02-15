// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;
// using UnityEngine.UI;

public class CloseBoxController : MonoBehaviour {
    public int menuID = 0;
    private GameObject[] menuPanels;
    private GameObject panel1;
    private GameObject panel2;
    private GameObject panel3;

    void Start()
    {
        panel1 = GameObject.FindGameObjectWithTag("Panel1");
        panel2 = GameObject.FindGameObjectWithTag("Panel2");
        panel3 = GameObject.FindGameObjectWithTag("Panel3");

        menuPanels = new GameObject[3];
        menuPanels[0] = panel1;
        menuPanels[1] = panel2;
        menuPanels[2] = panel3;

        SwitchToMenu(menuID);
    }
    
    public void SwitchToMenu(int menuID)
    {
        foreach(GameObject panel in menuPanels)
        {
            panel.SetActive(false);
        }

        switch(menuID)
        {
            case 1:
                panel1.SetActive(true);
                break;
            case 2:
                panel2.SetActive(true);
                break;
            case 3:
                panel3.SetActive(true);
                break;
        }
    }
}
