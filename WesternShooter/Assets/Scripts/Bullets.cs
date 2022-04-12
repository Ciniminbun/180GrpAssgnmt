using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Author:[Goldstein, Isys];
//Date:[April 12, 2022];
//Script:[Making bullet to harm player];
public class Bullets : MonoBehaviour
{
    [Header("Projectile Variables")]
    public float speed;
    public bool goingLeft;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(goingLeft == true)
        {
            transform.position += speed * Vector3.left * Time.deltaTime;
        }
        else
        {
            transform.position += speed * Vector3.right * Time.deltaTime;
        }
    }
}