using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BackCamera : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    // Start is called before the first frame update
    public Camera camera1;
    public Camera camera2;
    public bool isPressed = false;

    void Start()
    {
        camera1.enabled = true;
        camera2.enabled = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPressed)
        {
            camera1.enabled = false;
            camera2.enabled = true;
        }
        else
        {
            camera1.enabled = true;
            camera2.enabled = false;
        }
    }
}
