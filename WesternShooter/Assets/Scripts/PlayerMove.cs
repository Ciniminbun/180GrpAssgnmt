using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 spawnPos;
    private bool isGrnd;
    private bool canShoot;
    private bool gunReloading;
    private Transform fpsCam;
    private Transform gunProp;

    public int health;
    public float moveSpeed;
    public float jumpHeight;
    public int gunAmmo;
    public int gunRPM;
    public int gunMagSize;
    public int gunMagActive;
    public float reloadSpeed;

    // Start is called before the first frame update
    void Start()
    {
        fpsCam = gameObject.transform.GetChild(0);
        gunProp = fpsCam.GetChild(0);
        canShoot = true;
        gunReloading = false;
        rb = GetComponent<Rigidbody>();
        spawnPos = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        InputMove();

        if (Input.GetKey("r") && !gunReloading && gunMagActive < gunMagSize)
        {
            print("reloading...");
            Reload();
        }

        if (Input.GetMouseButton(0) && canShoot && !gunReloading && gunMagActive > 0)
        {
            print("bang!");
            Shoot();
        }

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

        float xInput = Input.GetAxis("Horizontal");
        float zInput = Input.GetAxis("Vertical");

        Vector3 move = transform.right * xInput + transform.forward * zInput;
        
        Vector3 tempVel = rb.velocity + (move * moveSpeed * Time.deltaTime);
        if (Input.GetKey("space") && isGrnd)
        {
            tempVel.y = Mathf.Sqrt(jumpHeight * -2 * Physics.gravity.y);
        }

        rb.velocity = tempVel;
    }

    private void Shoot()
    {
        canShoot = false;
        gunMagActive--;

        RaycastHit rayHit;
        
        Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out rayHit, Mathf.Infinity);

        Transform other = rayHit.transform;

        if (other.gameObject.CompareTag("Enemy"))
        {
            Destroy(other.gameObject);
        }
        StartCoroutine("RecoilAnimation");
        Invoke("ResetShot", 60f / gunRPM);
    }

    private void ResetShot()
    {
        canShoot = true;
        print("ioajhf");
    }

    private void Respawn()
    {
        transform.position = spawnPos;
        rb.velocity = new Vector3(0, 0, 0);
    }

    private void Reload()
    {
        gunReloading = true;
        StartCoroutine("ReloadAnimation");
        Invoke("ReloadFinished", reloadSpeed);
    }

    private void ReloadFinished()
    {
        gunReloading = false;
        gunMagActive = gunMagSize;
        print("reloaded");
    }

    IEnumerator RecoilAnimation()
    {
        float recoilDist = 0.1f;
        gunProp.position += transform.forward * -recoilDist;
        yield return new WaitForSeconds(0.15f);
        gunProp.position += transform.forward * recoilDist;
    }

    IEnumerator ReloadAnimation()
    {
        float recoilDist = 0.05f;
        int bobs = 3;
        float delay = (reloadSpeed / bobs) / 2;
        
        gunProp.Rotate(-5f, 0, 0, Space.Self);

        for (int i = 0; i < bobs; i++)
        {
            gunProp.position += gunProp.forward * -recoilDist;
            yield return new WaitForSeconds(delay);
            gunProp.position += gunProp.forward * recoilDist;
            yield return new WaitForSeconds(delay);
        }

        gunProp.Rotate(5f, 0, 0, Space.Self);

    }

}
