using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotLightMove : MonoBehaviour
{
    public float speed = 6;
    public float direction = 1;

    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (transform.localEulerAngles.y <= 350 && transform.localEulerAngles.y > 300)
        {
            direction = 1;
        }
        else if(transform.localEulerAngles.y >= 10 && transform.localEulerAngles.y < 300)
        {
            direction = -1;

        }
        transform.Rotate(new Vector3(0, speed * direction, 0) * Time.deltaTime);
    }
}
