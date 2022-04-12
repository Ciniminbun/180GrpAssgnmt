using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 spawnPos;
    private bool isGrnd;

    public int health;
    public float moveSpeed;
    public float jumpHeight;
    public float gravity = -16f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        spawnPos = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        InputMove();

        if (transform.position.y < -4)
        {
            Respawn();
        }
    }

    private void InputMove()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, 1.0f))
        {
            isGrnd = true;
        }
        else
        {
            isGrnd = false;
        }

        Vector3 tempVel = rb.velocity;

        if (Input.GetKey("space") && isGrnd)
        {
            tempVel.y = Mathf.Sqrt(jumpHeight * -2 * Physics.gravity.y);
        }

        rb.velocity = tempVel; // note

        float xInput = Input.GetAxis("Horizontal");
        float zInput = Input.GetAxis("Vertical");
        Vector3 move = transform.right * xInput + transform.forward * zInput;
        transform.position += move * moveSpeed * Time.deltaTime;
    }

    private void Respawn()
    {
        transform.position = spawnPos;
        rb.velocity = new Vector3(0, 0, 0);
    }

}
