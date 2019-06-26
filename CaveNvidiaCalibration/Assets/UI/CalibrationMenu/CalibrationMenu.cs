﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Htw.Cave.Joycons;

public class CalibrationMenu : MonoBehaviour
{
    //------------------------------------Sprites------------------------------------
    //Corner Arrow Sprits
    public Sprite spriteCornerW;
    public Sprite spriteCornerB;
    //Display Sprits
    public Sprite spriteDisplayW;
    public Sprite spriteDisplayB;


    //display[_,] = {c0,c1,c2,c3}
    //c0-3 corner left-up, right-up, right-down, left-down
    //display[,_]: c0-3 values: x,y,r,q (r,q default 1,0)
    int[,] displayL = new int[4, 4];
    int[,] displayF = new int[4, 4];
    int[,] displayR = new int[4, 4];
    int[,] displayB = new int[4, 4];
    //0,1,2,3 = L,F,R,B
    int selectedDisplay;
    //0,1,2,3 = left-up, right-up, right-down, left-down
    int selectedCorner;

    int stepSize = 1;

    //Corner Arrow
    private GameObject cornerRDSpriteRenderer;
    private GameObject cornerRUSpriteRenderer;
    private GameObject cornerLDSpriteRenderer;
    private GameObject cornerLUSpriteRenderer;
    //StepSize
    private GameObject stepSize1SpriteRenderer;
    private GameObject stepSize5SpriteRenderer;
    private GameObject stepSize10SpriteRenderer;
    private GameObject stepSize1Text;
    private GameObject stepSize5Text;
    private GameObject stepSize10Text;



    // Start is called before the first frame update
    void Start()
    {
        SetDefaultRQ(displayL);
        SetDefaultRQ(displayF);
        SetDefaultRQ(displayR);
        SetDefaultRQ(displayB);
        InitUiElements();

    }
    //Damit nur einmal ausgeführt wird:
    float m_lastPressed;
    // Update is called once per frame
    void Update()
    {
        
        //Press wird nur einmal ausgeführt
        if (m_lastPressed != Time.time)
        {
            m_lastPressed = Time.time;
            if (JoyconInput.GetButtonUp("Trigger R") || Input.GetKeyDown(KeyCode.Q))
            {
                ChangeStepSize();
                Debug.Log("Trigger R triggered & Stepsize: " + stepSize);
            }

            /*
            if (JoyconController.Left.GetButtonUp(JoyconLib.Joycon.Button.DPAD_DOWN))
            {
                cARDB.GetComponent<SpriteRenderer>().sprite = spriteCAW; 
            }*/

            ChangeXYInputManager();
        }

    }

    private void SetDefaultRQ(int [,] display)
    {
        for(int i = 0; i <= 3; i++)
        {
            display[i, 2] = 1;
            display[i, 3] = 0;
        }
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

    //Change StepSize to 1,5,10 and back to 1
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
    //Call it after the Change
    private void UpdateStepSizeUI()
    {
        switch (stepSize)
        {
            case 1:
                stepSize1SpriteRenderer.GetComponent<SpriteRenderer>().sprite = spriteDisplayB;
                stepSize5SpriteRenderer.GetComponent<SpriteRenderer>().sprite = spriteDisplayW;
                stepSize10SpriteRenderer.GetComponent<SpriteRenderer>().sprite = spriteDisplayW;
                stepSize1Text.GetComponent<Te>
                return;
            case 5:
                return;
            case 10:
                return;
        }
    }

    //IJKL && Left Joy Con Arrow Keys
    private void ChangeXYInputManager()
    {
        if (JoyconController.Left.GetButtonUp(JoyconLib.Joycon.Button.DPAD_UP) || Input.GetKeyDown(KeyCode.I))
        {
            ChangeY(1);
        }

        if (JoyconController.Left.GetButtonUp(JoyconLib.Joycon.Button.DPAD_RIGHT) || Input.GetKeyDown(KeyCode.L))
        {
            ChangeX(1);
        }

        if (JoyconController.Left.GetButtonUp(JoyconLib.Joycon.Button.DPAD_DOWN) || Input.GetKeyDown(KeyCode.K))
        {
            ChangeY(-1);
        }

        if (JoyconController.Left.GetButtonUp(JoyconLib.Joycon.Button.DPAD_LEFT) || Input.GetKeyDown(KeyCode.J))
        {
            ChangeX(-1);
        }
    }
    //sign -1 oder 1 
    private void ChangeX(int sign)
    {
        if(sign != -1 && sign != 1)
        {
            return;
        }
        switch (selectedDisplay)
        {
            case 0:
                displayL[selectedCorner,0] += stepSize*sign;
                return;
            case 1:
                displayF[selectedCorner, 0] += stepSize * sign;
                return;
            case 2:
                displayL[selectedCorner, 0] += stepSize * sign;
                return;
            case 3:
                displayB[selectedCorner, 0] += stepSize * sign;
                return;
        }
    }
    //sign -1 oder 1 
    private void ChangeY(int sign)
    {
        if (sign != -1 && sign != 1)
        {
            return;
        }
        switch (selectedDisplay)
        {
            case 0:
                displayL[selectedCorner, 1] += stepSize * sign;
                return;
            case 1:
                displayF[selectedCorner, 1] += stepSize * sign;
                return;
            case 2:
                displayL[selectedCorner, 1] += stepSize * sign;
                return;
            case 3:
                displayB[selectedCorner, 1] += stepSize * sign;
                return;
        }
    }

    private void InitUiElements()
    {
        //Corner Arrowrs
        cornerRDSpriteRenderer = GameObject.Find("Corner Arrow B/rd");
        cornerRUSpriteRenderer = GameObject.Find("Corner Arrow B/ru");
        cornerLDSpriteRenderer = GameObject.Find("Corner Arrow B/ld");
        cornerLUSpriteRenderer = GameObject.Find("Corner Arrow B/lu");
        //StepSize
        stepSize1SpriteRenderer = GameObject.Find("StepSize/1 b/stop");
        stepSize1SpriteRenderer = GameObject.Find("StepSize/5 b/stop");
        stepSize1SpriteRenderer = GameObject.Find("StepSize/10 b/stop");
        stepSize1Text = GameObject.Find("StepSize/1 b/Text");
        stepSize5Text = GameObject.Find("StepSize/5 b/Text");
        stepSize10Text = GameObject.Find("StepSize/10 b/Text");

    }




}
