using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionScript : MonoBehaviour
{
    private void Start()
    {
        Destroy(gameObject, 0.5f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("OW");
            other.gameObject.GetComponent<Rigidbody2D>().AddForce((other.transform.position - transform.position) * 50f);
        }
    }
}
