using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 spawnPos;
    private bool isGrnd;
    private bool canShoot;
    private bool gunReloading;
    private Transform fpsCam;
    private Transform gunProp;
    private string[] playerPerks;
    private string[] allPerks;
    private bool isInvincible;

    public GameObject explParticle;
    public Text playerStatusText;
    public int health;
    public int maxHealth;
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
        // initializing variables
        health = maxHealth;
        fpsCam = gameObject.transform.GetChild(0);
        gunProp = fpsCam.GetChild(0);
        canShoot = true;
        gunReloading = false;
        rb = GetComponent<Rigidbody>();
        spawnPos = transform.position;
        playerPerks = new string[0];
        isInvincible = false;
        allPerks = new string[] { "Overflow", "SpeedBoost", "QuickFire", "Health" };
        gunMagActive = gunMagSize;

        // set text to starting values
        TextUpdate();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        InputMove();

        // reloads if allowed
        if (Input.GetKey("r") && !gunReloading && gunMagActive < gunMagSize)
        {
            Reload();
        }

        // shoots if allowed
        if (Input.GetMouseButton(0) && canShoot && !gunReloading && gunMagActive > 0)
        {
            Shoot();
        }

        // universal death floor if the player jumps off the map
        if (transform.position.y < -4)
        {
            Respawn();
        }
    }

    private void InputMove()
    {
        // groundedness check
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, 1.5f))
        {
            isGrnd = true;
        }
        else
        {
            isGrnd = false;
        }
        
        // get player input and combine
        float xInput = Input.GetAxis("Horizontal");
        float zInput = Input.GetAxis("Vertical");
        Vector3 move = transform.right * xInput + transform.forward * zInput;
        
        // create tempVel with input data
        Vector3 tempVel = rb.velocity + (move * moveSpeed * Time.deltaTime);
        
        // add jump velocity on space press
        if (Input.GetKey("space") && isGrnd)
        {
            tempVel.y = Mathf.Sqrt(jumpHeight * -2 * Physics.gravity.y);
        }

        // applies temp vel to player vel
        rb.velocity = tempVel;
    }

    private void Shoot()
    {
        canShoot = false;
        gunMagActive--;

        // cast ray where the player is looking
        RaycastHit rayHit;
        Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out rayHit, Mathf.Infinity);

        // prevents error from shooting sky
        if (rayHit.collider != null)
        {
            Transform other = rayHit.transform;

            if (other.gameObject.CompareTag("Enemy"))
            {
                Destroy(other.gameObject);
            }
            else if (other.gameObject.CompareTag("Zombie"))
            {
                other.gameObject.GetComponent<EnemyBehavior>().TakeDamage(10);
            }
            
        }

        // explosion effect to show where the player shot
        Instantiate(explParticle, rayHit.point, Quaternion.identity);

        StartCoroutine("RecoilAnimation");
        TextUpdate();
        // renables shooting after timer determined by firerate
        Invoke("ResetShot", 60f / gunRPM);
    }

    private void ResetShot()
    {
        canShoot = true;
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
        TextUpdate();
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
        int bobs = gunMagSize;
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PU_Overflow"))
        {
            if (perkDupeCheck(0))
            {
                addPerk(0);
                other.gameObject.SetActive(false);
            }
            
        }
        else if (other.gameObject.CompareTag("PU_SpeedBoost"))
        {
            if (perkDupeCheck(1))
            {
                addPerk(1);
                other.gameObject.SetActive(false);
            }
        }
        else if (other.gameObject.CompareTag("PU_QuickFire"))
        {
            if (perkDupeCheck(2))
            {
                addPerk(2);
                other.gameObject.SetActive(false);
            }
        }
        else if (other.gameObject.CompareTag("PU_Health"))
        {
            if (perkDupeCheck(3))
            {
                addPerk(3);
                other.gameObject.SetActive(false);
            }
        }
        else if (other.gameObject.CompareTag("Bullet"))
        {
            TakeDamage(5);
        }
        else if (other.gameObject.CompareTag("Zombie"))
        {
            TakeDamage(10);
        }

        TextUpdate();
    }

    public void TakeDamage(int damage)
    {
        if (!isInvincible)
        {
            health -= damage;
            isInvincible = true;
            Invoke("InvinceReset", 2f);
        }
    }

    private void InvinceReset()
    {
        isInvincible = false;
    }

    public void addPerk(int perkId)
    {
        // adds new perk to end of string array
        string[] newArray = new string[playerPerks.Length + 1];
        for (int i = 0; i < playerPerks.Length; i++)
        {
            newArray[i] = playerPerks[i];
        }
        newArray[playerPerks.Length] = allPerks[perkId];
        playerPerks = newArray;

        // applies perk effect
        if (allPerks[perkId] == "Overflow")
        {
            gunMagActive = gunMagSize * 3;
            StartCoroutine(removePerk(perkId, 0));
        }
        else if (allPerks[perkId] == "SpeedBoost")
        {
            moveSpeed *= 2;
            StartCoroutine(removePerk(perkId, 10));
        }
        else if (allPerks[perkId] == "QuickFire")
        {
            gunRPM *= 2;
            reloadSpeed /= 2;
            StartCoroutine(removePerk(perkId, 10));
        }
        else if (allPerks[perkId] == "Health")
        {
            health = maxHealth;
        }


    }

    private bool perkDupeCheck(int perkId)
    {
        // checks if perk is already applied to the player
        bool perkExists = false;
        for (int i = 0; i < playerPerks.Length; i++)
        {
            if (playerPerks[i] == allPerks[perkId])
            {
                perkExists = true;
            }
        }
        return !perkExists;
    }

    IEnumerator removePerk(int perkId, float timer)
    {
        yield return new WaitForSeconds(timer);

        // get length of array minus perk being removed
        int newLength = 0;
        for (int i = 0; i < playerPerks.Length; i++)
        {
            if (playerPerks[i] != allPerks[perkId])
            {
                newLength++;
            }
        }

        // adds perks not being removed to new array
        string[] newArray = new string[newLength];
        int newArrProg = 0;
        for (int i = 0; i < playerPerks.Length; i++)
        {
            if (playerPerks[i] != allPerks[perkId])
            {
                newArray[newArrProg] = playerPerks[i];
                newArrProg++;
            }
        }

        // apply new array
        playerPerks = newArray;
        TextUpdate();

        // reverse effects
        if (allPerks[perkId] == "Overflow")
        {

        }
        else if (allPerks[perkId] == "SpeedBoost")
        {
            moveSpeed /= 2;
        }
        else if (allPerks[perkId] == "QuickFire")
        {
            gunRPM /= 2;
            reloadSpeed *= 2;
        }
        else if (allPerks[perkId] == "Health")
        {

        }

    }

    private void TextUpdate()
    {
        // updates player status text
        string comtxt = "HEALTH: " + health +
            "\nAMMO: " + gunMagActive + " / " + gunMagSize + "\n";

        for (int i = 0; i < playerPerks.Length; i++)
        {
            comtxt += playerPerks[i] + "\n";
        }
        
        playerStatusText.text = comtxt;
    }

}
