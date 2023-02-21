using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class Timer : MonoBehaviour
{
    public float timeValue = 90;
    public TextMeshProUGUI timeText;

    

    void SetTimeText()
    {
        int textValue = (int)timeValue;
        timeText.text = "Time Left:  " + textValue.ToString() + "s";
        
    }

    // Update is called once per frame
    void Update()
    {
        timeValue -= Time.deltaTime;
        SetTimeText();
    }

}
