using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalibrationMenu : MonoBehaviour
{
    public GameObject CanvasPrefab;
    private Canvas MenuCanvas;

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(CanvasPrefab, FindObjectOfType<Canvas>().transform);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
