using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class powerupMove : MonoBehaviour
{
    public float speed;
    private Vector3 initPosition;
    private int bobDir;

    private void Start()
    {
        initPosition = transform.position;
        bobDir = 1;
    }
    void FixedUpdate()
    {
        transform.Rotate(0, speed, 0);
        float bobRange = 0.25f;
        if (transform.position.y > initPosition.y + bobRange || transform.position.y < initPosition.y - bobRange)
        {
            bobDir *= -1;
        }
        transform.position += Vector3.up * Time.deltaTime * bobDir * speed;
        
    }
}
