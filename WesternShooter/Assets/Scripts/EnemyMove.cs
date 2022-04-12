using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author:[Goldstein, Isys];
//Date:[April 12, 2022];
//Script:[Enemies serving as an obstacle for player];

public class EnemyMove : MonoBehaviour
{
    public float speed;
    public GameObject leftPoint;
    public GameObject rightPoint;
    private Vector3 leftPos;
    private Vector3 rightPos;
    private bool goingLeft = true;
    // Start is called before the first frame update
    void Start()
    {
        leftPos = leftPoint.transform.position;
        rightPos = rightPoint.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }
    private void Move()
    {
        //if moving left check the boundry not reach boundary and left vector
        if(goingLeft)
        {
            if(transform.position.x <= leftPos.x)
            {
                goingLeft = false;
            }
            else
            {
                transform.position += Vector3.left * Time.deltaTime * speed;
            }
        }
        else
        {
            if(transform.position.x >= rightPos.x)
            {
                goingLeft = true;
            }
            else
            {
                transform.position += Vector3.right * Time.deltaTime * speed;
            }
        }
        
    }
}
