using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    private GameObject player;
    private Vector3 lastPlayerPos;

    public int awareRange;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Vector3.Distance(player.transform.position, transform.position) < awareRange)
        {
            RaycastHit rayHit;
            Physics.Raycast(transform.position, (player.transform.position - transform.position), out rayHit, awareRange + 1);

            if (rayHit.collider != null)
            {
                if (rayHit.transform.gameObject.CompareTag("Player"))
                {
                    Vector3 playerPlane = player.transform.position;
                    playerPlane.y = transform.position.y;
                    transform.position = Vector3.MoveTowards(transform.position, playerPlane, speed * Time.deltaTime);
                    lastPlayerPos = player.transform.position;
                }
                if (lastPlayerPos != null)
                {
                    lastPlayerPos.y = transform.position.y;
                    transform.position = Vector3.MoveTowards(transform.position, lastPlayerPos, speed * Time.deltaTime);
                }
            }
            
        }
        else if (lastPlayerPos != null)
        {
            lastPlayerPos.y = transform.position.y;
            transform.position = Vector3.MoveTowards(transform.position, lastPlayerPos, speed * Time.deltaTime);
        }


    }
}
