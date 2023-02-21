using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfRotation : MonoBehaviour
{
    public float speedY;
    public float speedZ;
    private Rigidbody rb;
    public float torque;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.AddTorque(transform.up * torque * speedY);
    }

    void FixedUpdate()
    {
        rb.AddTorque(transform.forward * torque * speedZ);
    }
}
