using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorExplosions : MonoBehaviour
{
    public GameObject explosion;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject spawnedExplosion = Instantiate(explosion);
            spawnedExplosion.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
        }
    }
}
