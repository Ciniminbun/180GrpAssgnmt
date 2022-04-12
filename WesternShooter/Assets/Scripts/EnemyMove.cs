using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Author:[Goldstein,Isys]
//Date:[04/12/2022]
//Script:[Making Enemy move in a certain area]
public class NewBehaviourScript : MonoBehaviour
{
    public GameObject leftPoint;
    public GameObject rightPoint;
    private Vector3 leftPos;
    private Vector3 rightPos;
    public int speed;
    public bool goingLeft;
    // Start is called before the first frame update
    void Start()
    {
        leftPos = leftPoint.transform.position;
        rightPos = rightPoint.transform.position
    }
private void Move()
{
    if (goingLeft)
    {
        if(transform.position.x <= leftPos.x)
        }
         else
        {
        transform.position += Vector.left * Time.deltaTime * speed;
        }
    }
    else
    {
        if(transform.position.x >=rightPos.x)
        {
            goingLeft = true;
        }
        else
        {
            transform.position += Vector3.right * Time.deltaTime* speed;
        }

    }
}

    // Update is called once per frame
    void Update()
    {
        Move();
    }
}
