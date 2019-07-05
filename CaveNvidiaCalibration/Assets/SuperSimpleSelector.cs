using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Htw.Cave.Calibration.Configuration;
using Htw.Cave.Joycons;

public class SuperSimpleSelector : MonoBehaviour
{
    private ConfigurationManager configurationManager;

    private int selectedDisplay;
    private int selectedVertex;
    private int mappedVertex;

    public void Awake()
    {
        this.configurationManager = base.GetComponent<ConfigurationManager>();
        this.selectedDisplay = 0;
        this.selectedVertex = 2; // I'm so fucking dumb, jesus! This is more than just a hack, this is art!
        this.mappedVertex = 0;
    }

    public void Update()
    {
        UpdateDisplaySelection();

        UpdateVertexSelection();
        MapVertexIndex();

        Vector3 v = new Vector3(JoyconInput.GetAxis("Horizontal R"), JoyconInput.GetAxis("Vertical R"), 0f);
        this.configurationManager.ConfigurationMeshes[this.selectedDisplay].ConfigurationVertices[mappedVertex].Move(v);
    }

    public void UpdateDisplaySelection()
    {
        if (JoyconInput.GetButtonDown("Trigger L"))
            this.selectedDisplay -= this.selectedDisplay > 0 ? 1 : 0;
        else if (JoyconInput.GetButtonDown("Trigger R")) { }
            //this.selectedDisplay += this.selectedDisplay < this.configurationManager.ConfigurationMeshes.Count - 1 ? 1 : 0;
    }

    private void UpdateVertexSelection()
    {
        if (JoyconInput.GetAxis("Horizontal L") < 0)
            this.selectedVertex &= ~(1 << 0);
        else if (JoyconInput.GetAxis("Horizontal L") > 0)
            this.selectedVertex |= 1 << 0;
        if (JoyconInput.GetAxis("Vertical L") > 0)
            this.selectedVertex &= ~(1 << 1);
        else if (JoyconInput.GetAxis("Vertical L") < 0)
            this.selectedVertex |= 1 << 1;
    }

    // NSA or BND hire me, best hack euw
    private void MapVertexIndex()
    {
        switch (this.selectedVertex)
        {
            case 2:
                this.mappedVertex = 0;
                break;
            case 3:
                this.mappedVertex = 1;
                break;
            case 0:
                this.mappedVertex = 2;
                break;
            case 1:
                this.mappedVertex = 3;
                break;
        }
    }

}
