using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public Transform player;
    public float mouseSens;

    private float mouseX;
    private float mouseY;
    private float xRotation;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {        
        mouseX = Input.GetAxis("Mouse X") * mouseSens * Time.deltaTime;
        mouseY = Input.GetAxis("Mouse Y") * mouseSens * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90, 90);

        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        player.Rotate(Vector3.up * mouseX);

        Shoot();

    }
    private void Shoot()
    {
        if (Input.GetMouseButton(0))
        {
            RaycastHit hit;
            Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, 1.0f);
            Vector3 forward = transform.TransformDirection(Vector3.forward) * 50;
            Debug.DrawRay(transform.position, forward, Color.green);
            Debug.Log(hit.transform.gameObject.name);
        }
    }

}
