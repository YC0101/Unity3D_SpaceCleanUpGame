using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotateAround : MonoBehaviour
{
    public GameObject target;
    public float speed;
    private Vector3 randomVec;
    public float areaLimit = 10;

    // Start is called before the first frame update
    void Start()
    {
        float distance = Mathf.Sqrt(Mathf.Pow((target.transform.position.z - transform.position.z), 2) + Mathf.Pow((target.transform.position.x - transform.position.x), 2));
        speed = (areaLimit - distance)*5;
        Vector3 dir = transform.position - target.transform.position;
        randomVec = new Vector3(dir.z, 0, -dir.x);

    }

    // Update is called once per frame
    void Update()
    { 
        transform.RotateAround(target.transform.position, randomVec, speed * Time.deltaTime);
    }

}
