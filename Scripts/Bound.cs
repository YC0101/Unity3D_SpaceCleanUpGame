using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bound : MonoBehaviour
{
    public Vector3 SpawnPoint;
    public GameObject Player;

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Spaceship"))
        {
            other.gameObject.transform.position = SpawnPoint;
            other.attachedRigidbody.velocity = Vector3.zero;
            other.attachedRigidbody.angularVelocity = Vector3.zero;

            Player.GetComponent<PlayerMovement>().LoseLife();
        }

    }


}
