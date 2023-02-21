using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BoostControll : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool isPressed = false;
    public GameObject Player;

    public float speed = 3;
    public float movespeed = 18.0f;
    public float originalspeed = 1.0f;

    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = Player.GetComponent<Rigidbody>();
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
            rb.velocity = Player.transform.forward * originalspeed + Player.transform.forward * movespeed;
        }

    }
}
