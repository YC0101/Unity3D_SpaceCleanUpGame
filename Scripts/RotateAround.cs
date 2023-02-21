using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAround : MonoBehaviour
{
    public GameObject target;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindWithTag("CenterPlanet");
        float distance = Mathf.Sqrt(Mathf.Pow((target.transform.position.z - transform.position.z), 2) + Mathf.Pow((target.transform.position.x - transform.position.x), 2));
        speed = 12 - (((distance / 8)- 4)* 7);

    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(target.transform.position, Vector3.up, speed * Time.deltaTime);
    }
}
