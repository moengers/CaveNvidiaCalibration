using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Htw.Cave.Joycons;

public class CalibrationMenu : MonoBehaviour
{
    //display[_,] = {c0,c1,c2,c3}
    //c0-3 corner left-up, right-up, right-down, left-down
    //display[,_]: c0-3 values: x,y,,r,q (r,q default 1,0)
    int[,] displayL = new int[4, 4];
    int[,] displayR = new int[4, 4];
    int[,] displayF = new int[4, 4];
    int[,] displayB = new int[4, 4];

    int stepSize = 1;


    // Start is called before the first frame update
    void Start()
    {
        SetDefaultRQ(displayL);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (JoyconInput.GetButtonUp("Trigger R") || Input.GetKeyDown(KeyCode.Q))
        {
            ChangeStepSize();
            Debug.Log("Trigger R triggered & Stepsize: " + stepSize);
        }
    }

    private void SetDefaultRQ(int [,] display)
    {
        for(int i = 0; i <= 3; i++)
        {
            display[i, 2] = 1;
            display[i, 3] = 0;
        }
        Debug.Log(DisplayToString(display));
    }

    private string DisplayToString(int[,] display)
    {
        string s = "Display: ";
        for(int i = 0; i <= 3; i++)
        {
            s += "Corner" + i + ": ";
            s += "x: " + display[i, 0] + "; ";
            s += "y: " + display[i, 1] + "; ";
            s += "r: " + display[i, 2] + "; ";
            s += "q: " + display[i, 3] + "; ";
        }
        return s;
    }

    private void ChangeStepSize()
    {
        if(stepSize == 1)
        {
            stepSize = 5;
        }
        else if (stepSize == 5)
        {
            stepSize = 10;
        }
        else if (stepSize == 10)
        {
            stepSize = 1;
        }
    }

}
