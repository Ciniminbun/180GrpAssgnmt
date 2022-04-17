using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class simpleBullet : MonoBehaviour
{
    public float speed;

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }
}
