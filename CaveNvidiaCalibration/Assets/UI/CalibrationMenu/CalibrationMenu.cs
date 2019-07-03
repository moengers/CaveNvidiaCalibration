using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Htw.Cave.Joycons;
using Htw.Cave.Calibration.Configuration;

public class CalibrationMenu : MonoBehaviour
{
    //------------------------------------Sprites------------------------------------
    //Corner Arrow Sprits
    /*public Sprite spriteCornerW;
    public Sprite spriteCornerB;*/

    [SerializeField]
    private ConfigurationManager configurationManager;

    private const float MASHSTEPSIZEMULTIPLIKATOR = 0.01f;

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
    //How much x and y change (defualt 1)
    int stepSize = 1;

    //Corner Arrow
    private GameObject[] cornerRDSpriteRenderer;
    private GameObject[] cornerRUSpriteRenderer;
    private GameObject[] cornerLDSpriteRenderer;
    private GameObject[] cornerLUSpriteRenderer;
    //StepSize
    private GameObject stepSize1SpriteRenderer;
    private GameObject stepSize5SpriteRenderer;
    private GameObject stepSize10SpriteRenderer;
    private GameObject stepSize1Text;
    private GameObject stepSize5Text;
    private GameObject stepSize10Text;
    //Displays
    private GameObject displayUIL;
    private GameObject displayUIF;
    private GameObject displayUIR;
    private GameObject displayUIB;
    //XY Values
    private GameObject xValueText;
    private GameObject yValueText;

    //----------------------Start----------------------//
    void Start()
    {
        //Set the default R and Q Values (1 and 0)
        SetDefaultRQ(displayL);
        SetDefaultRQ(displayF);
        SetDefaultRQ(displayR);
        SetDefaultRQ(displayB);
        //Init/Find GameObjects
        InitUiElements();
        //Initial UI
        UpdateStepSizeUI();
        UpdateDisplayUI();
        UpdateCornerUI();

    }
    //----------------------Update----------------------//
    //Damit nur einmal ausgeführt wird:
    float m_lastPressed;
    void Update()
    {        
        //Press wird nur einmal ausgeführt (oder zumindest seltener)
        if (m_lastPressed != Time.time)
        {
            m_lastPressed = Time.time;
            
            //Update GUI by Input
            ChangeStepSizeInputManager();
            ChangeXYInputManager();
            ChangeDisplayInputManager();
            ChangeCornerInputManager();
        }

    }


    
    //----------------------Change Step Size----------------------//
    //q && Right Trigger
    private void ChangeStepSizeInputManager()
    {
        //Rechts Links vertauscht beim Trigger??
        if (JoyconInput.GetButtonUp("Trigger R") || Input.GetKeyDown(KeyCode.Q))
        {
            ChangeStepSize();
        }
    }
    //Change StepSize to 1,5,10 and back to 1
    private void ChangeStepSize()
    {
        if(stepSize == 1)
        {
            stepSize = 5;
            UpdateStepSizeUI();
                 
        }
        else if (stepSize == 5)
        {
            stepSize = 10;
            UpdateStepSizeUI();

        }
        else if (stepSize == 10)
        {
            stepSize = 1;
            UpdateStepSizeUI();

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
                stepSize1Text.GetComponent<Text>().color = Color.white;
                stepSize5Text.GetComponent<Text>().color = Color.black;
                stepSize10Text.GetComponent<Text>().color = Color.black;
                return;
            case 5:
                stepSize1SpriteRenderer.GetComponent<SpriteRenderer>().sprite = spriteDisplayW;
                stepSize5SpriteRenderer.GetComponent<SpriteRenderer>().sprite = spriteDisplayB;
                stepSize10SpriteRenderer.GetComponent<SpriteRenderer>().sprite = spriteDisplayW;
                stepSize1Text.GetComponent<Text>().color = Color.black;
                stepSize5Text.GetComponent<Text>().color = Color.white;
                stepSize10Text.GetComponent<Text>().color = Color.black;
                return;
            case 10:
                stepSize1SpriteRenderer.GetComponent<SpriteRenderer>().sprite = spriteDisplayW;
                stepSize5SpriteRenderer.GetComponent<SpriteRenderer>().sprite = spriteDisplayW;
                stepSize10SpriteRenderer.GetComponent<SpriteRenderer>().sprite = spriteDisplayB;
                stepSize1Text.GetComponent<Text>().color = Color.black;
                stepSize5Text.GetComponent<Text>().color = Color.black;
                stepSize10Text.GetComponent<Text>().color = Color.white;
                return;
        }
    }


    //----------------------Change XY Values----------------------//
    //IJKL && Left Joy Con Arrow Keys
    private void ChangeXYInputManager()
    {
        if(JoyconController.Left == null || JoyconController.Right == null)
        {
            //No Joy Cons detected
            //Only Keyboard Used:
            if (Input.GetKeyDown(KeyCode.I))
            {
                ChangeY(1);
                ChangeCalibrationManagerY(1);
                ChangeXYUI();
            }

            if (Input.GetKeyDown(KeyCode.L))
            {
                ChangeX(1);
                ChangeCalibrationManagerX(1);
                ChangeXYUI();
            }

            if (Input.GetKeyDown(KeyCode.K))
            {
                ChangeY(-1);
                ChangeCalibrationManagerY(-1);
                ChangeXYUI();
            }

            if (Input.GetKeyDown(KeyCode.J))
            {
                ChangeX(-1);
                ChangeCalibrationManagerX(-1);
                ChangeXYUI();
            }
            return;
        }
        //JoyCons && Keyboard: 
        if (JoyconController.Left.GetButtonUp(JoyconLib.Joycon.Button.DPAD_UP) || Input.GetKeyDown(KeyCode.I))
        {
            ChangeY(1);
            ChangeCalibrationManagerY(1);
            ChangeXYUI();
        }

        if (JoyconController.Left.GetButtonUp(JoyconLib.Joycon.Button.DPAD_RIGHT) || Input.GetKeyDown(KeyCode.L))
        {
            ChangeX(1);
            ChangeCalibrationManagerX(1);
            ChangeXYUI();
        }

        if (JoyconController.Left.GetButtonUp(JoyconLib.Joycon.Button.DPAD_DOWN) || Input.GetKeyDown(KeyCode.K))
        {
            ChangeY(-1);
            ChangeCalibrationManagerY(-1);
            ChangeXYUI();
        }

        if (JoyconController.Left.GetButtonUp(JoyconLib.Joycon.Button.DPAD_LEFT) || Input.GetKeyDown(KeyCode.J))
        {
            ChangeX(-1);
            ChangeCalibrationManagerX(-1);
            ChangeXYUI();
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
    private void ChangeCalibrationManagerX(int sign)
    {
        configurationManager
            .ConfigurationMeshes[MapperDisplayToMash(selectedDisplay)].
            ConfigurationVertices[MapperCornerToVertices(selectedCorner)].
            Move(new Vector3(stepSize * sign * MASHSTEPSIZEMULTIPLIKATOR, 0f, 0f));
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
    private void ChangeXYUI()
    {
        switch (selectedDisplay)
        {
            case 0:
                xValueText.GetComponent<Text>().text = displayL[selectedCorner, 0].ToString();
                yValueText.GetComponent<Text>().text = displayL[selectedCorner, 1].ToString();
                return;
            case 1:
                xValueText.GetComponent<Text>().text = displayF[selectedCorner, 0].ToString();
                yValueText.GetComponent<Text>().text = displayF[selectedCorner, 1].ToString();
                return;
            case 2:
                xValueText.GetComponent<Text>().text = displayR[selectedCorner, 0].ToString();
                yValueText.GetComponent<Text>().text = displayR[selectedCorner, 1].ToString();
                return;
            case 3:
                xValueText.GetComponent<Text>().text = displayB[selectedCorner, 0].ToString();
                yValueText.GetComponent<Text>().text = displayB[selectedCorner, 1].ToString();
                return;
        }
    }
    private void ChangeCalibrationManagerY(int sign)
    {
        configurationManager
            .ConfigurationMeshes[MapperDisplayToMash(selectedDisplay)].
            ConfigurationVertices[MapperCornerToVertices(selectedCorner)].
            Move(new Vector3(0f, stepSize*sign*MASHSTEPSIZEMULTIPLIKATOR, 0f));
    }
    //Mapped display and corner/vertices for the calibrationManager
    private int MapperDisplayToMash(int display)
    {
        //configManager use 0, 1, 2, 3 as R, L, B, F or random??
        switch (display)
        {
            case 0:
                return 1;
            case 1:
                return 3;
            case 2:
                return 0;
            case 3:
                return 2;
            default:
                return -1;
        }
    }
    private int MapperCornerToVertices(int corner)
    {
        switch (corner)
        {
            case 0:
                return 2;
            case 1:
                return 3;
            case 2:
                return 1;
            case 3:
                return 0;
            default:
                return -1;
        }
    }


    //----------------------Change Display----------------------//
    //1234 && Analog R
    private void ChangeDisplayInputManager()
    {        
        if (JoyconController.Left == null || JoyconController.Right == null)
        {
            //No Joy Cons detected
            //Only Keyboard used:
            //Left:
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                selectedDisplay = 0;
                UpdateDisplayUI();
            }
            //Front:
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                selectedDisplay = 1;
                UpdateDisplayUI();
            }
            //Right:
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                selectedDisplay = 2;
                UpdateDisplayUI();
            }
            //Bottom:
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                selectedDisplay = 3;
                UpdateDisplayUI();
            }
            ChangeXYUI();
            return;
        }

        //JoyconController.Right.GetStick()[0] : Horizontal
        //JoyconController.Right.GetStick()[1] : Vertical
        //Left:
        if (JoyconController.Right.GetStick()[0] < -0.2 || Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectedDisplay = 0;
            UpdateDisplayUI();
        }
        //Front:
        if (JoyconController.Right.GetStick()[1] > 0.2 || Input.GetKeyDown(KeyCode.Alpha2))
        {
            selectedDisplay = 1;
            UpdateDisplayUI();
        }
        //Right:
        if (JoyconController.Right.GetStick()[0] > 0.2 || Input.GetKeyDown(KeyCode.Alpha3))
        {
            selectedDisplay = 2;
            UpdateDisplayUI();
        }  
        //Bottom:
        if (JoyconController.Right.GetStick()[1] < -0.2 || Input.GetKeyDown(KeyCode.Alpha4))
        {
            selectedDisplay = 3;
            UpdateDisplayUI();
        }
        ChangeXYUI();
    }
    //Call it after the Change
    private void UpdateDisplayUI()
    {
        switch (selectedDisplay)
        {
            case 0:
                displayUIL.GetComponent<SpriteRenderer>().sprite = spriteDisplayB;
                displayUIF.GetComponent<SpriteRenderer>().sprite = spriteDisplayW;
                displayUIR.GetComponent<SpriteRenderer>().sprite = spriteDisplayW;
                displayUIB.GetComponent<SpriteRenderer>().sprite = spriteDisplayW;
                return;
            case 1:
                displayUIL.GetComponent<SpriteRenderer>().sprite = spriteDisplayW;
                displayUIF.GetComponent<SpriteRenderer>().sprite = spriteDisplayB;
                displayUIR.GetComponent<SpriteRenderer>().sprite = spriteDisplayW;
                displayUIB.GetComponent<SpriteRenderer>().sprite = spriteDisplayW;
                return;
            case 2:
                displayUIL.GetComponent<SpriteRenderer>().sprite = spriteDisplayW;
                displayUIF.GetComponent<SpriteRenderer>().sprite = spriteDisplayW;
                displayUIR.GetComponent<SpriteRenderer>().sprite = spriteDisplayB;
                displayUIB.GetComponent<SpriteRenderer>().sprite = spriteDisplayW;
                return;
            case 3:
                displayUIL.GetComponent<SpriteRenderer>().sprite = spriteDisplayW;
                displayUIF.GetComponent<SpriteRenderer>().sprite = spriteDisplayW;
                displayUIR.GetComponent<SpriteRenderer>().sprite = spriteDisplayW;
                displayUIB.GetComponent<SpriteRenderer>().sprite = spriteDisplayB;
                return;
        }
    }


    //----------------------Change Corner----------------------//
    //1234 && Analog R
    private void ChangeCornerInputManager()
    {
        if (JoyconController.Left == null || JoyconController.Right == null)
        {
            //No Joy Cons detected
            //Only Keyboard used:
            //Left:
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                selectedCorner = 0;
                UpdateCornerUI();
            }
            //Front:
            if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                selectedCorner = 1;
                UpdateCornerUI();
            }
            //Right:
            if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                selectedCorner = 2;
                UpdateCornerUI();
            }
            //Bottom:
            if (Input.GetKeyDown(KeyCode.Alpha8))
            {
                selectedCorner = 3;
                UpdateCornerUI();
            }
            ChangeXYUI();
            return;
        }

        //JoyconController.Right.GetStick()[0] : Horizontal
        //JoyconController.Right.GetStick()[1] : Vertical
        //Left:
        if (JoyconController.Left.GetStick()[0] < -0.2 || Input.GetKeyDown(KeyCode.Alpha5))
        {
            selectedCorner = 0;
            UpdateCornerUI();
        }
        //Front:
        if (JoyconController.Left.GetStick()[1] > 0.2 || Input.GetKeyDown(KeyCode.Alpha6))
        {
            selectedCorner = 1;
            UpdateCornerUI();
        }
        //Right:
        if (JoyconController.Left.GetStick()[0] > 0.2 || Input.GetKeyDown(KeyCode.Alpha7))
        {
            selectedCorner = 2;
            UpdateCornerUI();
        }
        //Bottom:
        if (JoyconController.Left.GetStick()[1] < -0.2 || Input.GetKeyDown(KeyCode.Alpha8))
        {
            selectedCorner = 3;
            UpdateCornerUI();
        }
        ChangeXYUI();
    }
    //Call it after the Change
    private void UpdateCornerUI()
    {
        switch (selectedCorner)
        {
            case 0:
                CornerSpriteRendererChangeVisablity(cornerLUSpriteRenderer, true);
                CornerSpriteRendererChangeVisablity(cornerRUSpriteRenderer, false);
                CornerSpriteRendererChangeVisablity(cornerRDSpriteRenderer, false);
                CornerSpriteRendererChangeVisablity(cornerLDSpriteRenderer, false);
                return;
            case 1:
                CornerSpriteRendererChangeVisablity(cornerLUSpriteRenderer, false);
                CornerSpriteRendererChangeVisablity(cornerRUSpriteRenderer, true);
                CornerSpriteRendererChangeVisablity(cornerRDSpriteRenderer, false);
                CornerSpriteRendererChangeVisablity(cornerLDSpriteRenderer, false);
                return;
            case 2:
                CornerSpriteRendererChangeVisablity(cornerLUSpriteRenderer, false);
                CornerSpriteRendererChangeVisablity(cornerRUSpriteRenderer, false);
                CornerSpriteRendererChangeVisablity(cornerRDSpriteRenderer, true);
                CornerSpriteRendererChangeVisablity(cornerLDSpriteRenderer, false);
                return;
            case 3:
                CornerSpriteRendererChangeVisablity(cornerLUSpriteRenderer, false);
                CornerSpriteRendererChangeVisablity(cornerRUSpriteRenderer, false);
                CornerSpriteRendererChangeVisablity(cornerRDSpriteRenderer, false);
                CornerSpriteRendererChangeVisablity(cornerLDSpriteRenderer, true);
                return;
        }
    }
    //Switch the visabilty of a corner arrow
    private void CornerSpriteRendererChangeVisablity(GameObject[] cornerSpriteRenderer, bool visible)
    {
        foreach(GameObject arrow in cornerSpriteRenderer)
        {
            arrow.GetComponent<Renderer>().enabled = visible;
        }
    }


    //----------------------Init/Find all Elements----------------------//
    private void InitUiElements()
    {
        //Corner Arrowrs
        cornerRDSpriteRenderer = GameObject.FindGameObjectsWithTag("Arrowrd");
        cornerRUSpriteRenderer = GameObject.FindGameObjectsWithTag("Arrowru");
        cornerLDSpriteRenderer = GameObject.FindGameObjectsWithTag("Arrowld");
        cornerLUSpriteRenderer = GameObject.FindGameObjectsWithTag("Arrowlu");
        //StepSize
        stepSize1SpriteRenderer = GameObject.Find("StepSize/1 b/stop");
        stepSize5SpriteRenderer = GameObject.Find("StepSize/5 b/stop");
        stepSize10SpriteRenderer = GameObject.Find("StepSize/10 b/stop");
        stepSize1Text = GameObject.Find("StepSize/1 b/Text");
        stepSize5Text = GameObject.Find("StepSize/5 b/Text");
        stepSize10Text = GameObject.Find("StepSize/10 b/Text");
        //Display
        displayUIL = GameObject.Find("Displays/D Left");
        displayUIF = GameObject.Find("Displays/D Front");
        displayUIR = GameObject.Find("Displays/D Right");
        displayUIB = GameObject.Find("Displays/D Bot");
        //XY Values
        xValueText = GameObject.Find("XY/X/Value");
        yValueText = GameObject.Find("XY/Y/Value");

    }
    //----------------------Set the default RQ Values----------------------//
    private void SetDefaultRQ(int[,] display)
    {
        for (int i = 0; i <= 3; i++)
        {
            display[i, 2] = 1;
            display[i, 3] = 0;
        }
    }
    //----------------------ForDebug: Format the Display Values to String----------------------//
    private string DisplayToString(int[,] display)
    {
        string s = "Display: ";
        for (int i = 0; i <= 3; i++)
        {
            s += "Corner" + i + ": ";
            s += "x: " + display[i, 0] + "; ";
            s += "y: " + display[i, 1] + "; ";
            s += "r: " + display[i, 2] + "; ";
            s += "q: " + display[i, 3] + "; ";
        }
        return s;
    }



}
