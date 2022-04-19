using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EnemyBehavior : MonoBehaviour
{
    private GameObject player;
    private Vector3 lastPlayerPos;
    private bool canShoot;

    public GameObject bulletPrefab;
    public int awareRange;
    public float speed;
    public int health;
    public bool hasGun;
    public int RPM;
    [SerializeField, Range(0, 100)] private float powerupDropRate;

    // Start is called before the first frame update
    void Start()
    {
        // initializing variables
        player = GameObject.FindGameObjectWithTag("Player");
        lastPlayerPos = transform.position;
        canShoot = true;

    }

    private void Update()
    {
        // enemy death
        if (health <= 0)
        {
            // drop random power up on death
            if (Random.Range(0, 100) <= powerupDropRate)
            {
                Instantiate(GetRndPU(), transform.position, Quaternion.identity);
            }
            Destroy(this.gameObject);
        }
    }

    void FixedUpdate()
    {
        // check distance between player to see if the player is in the awareness bubble
        if (Vector3.Distance(player.transform.position, transform.position) < awareRange)
        {
            // cast ray between enemy and player to check for line of sight obstacles
            RaycastHit rayHit;
            Physics.Raycast(transform.position, (player.transform.position - transform.position), out rayHit, awareRange + 1);

            if (rayHit.collider != null)
            {
                // if the ray hits a player, then there's a clear line of sight and the enemy pursues
                if (rayHit.transform.gameObject.CompareTag("Player"))
                {
                    // keeping the enemy from flying off the ground
                    Vector3 playerPlane = player.transform.position;
                    playerPlane.y = transform.position.y;

                    // basic movetowards player
                    transform.position = Vector3.MoveTowards(transform.position, playerPlane, speed * Time.deltaTime);

                    // record last known player position in case the player leaves line of sight
                    lastPlayerPos = player.transform.position;
                    transform.LookAt(player.transform.position);

                    // fires bullets if the enemy is allowed to
                    if (hasGun && canShoot)
                    {
                        Shoot();
                    }

                }
                else
                {
                    // moves towards last known player position if there is no line of sight
                    lastPlayerPos.y = transform.position.y;
                    transform.LookAt(lastPlayerPos);
                    transform.position = Vector3.MoveTowards(transform.position, lastPlayerPos, speed * Time.deltaTime);
                }
            }
            
        }
        else
        {
            // moves towards last known player position if there is no line of sight
            lastPlayerPos.y = transform.position.y;
            transform.LookAt(lastPlayerPos);
            transform.position = Vector3.MoveTowards(transform.position, lastPlayerPos, speed * Time.deltaTime);
        }


    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        transform.position += transform.forward * -0.5f;
    }

    private void Shoot()
    {
        canShoot = false;

        // spawn bullet at end of gun prop and point it at the player
        Transform gunEnd = gameObject.transform.GetChild(0).GetChild(0);
        Instantiate(bulletPrefab, gunEnd.transform.position, Quaternion.LookRotation(player.transform.position - gunEnd.transform.position, Vector3.up));

        // allow shooting again after wait based on RPM
        Invoke("ResetShot", 60f / RPM);
    }

    private void ResetShot()
    {
        canShoot = true;
    }

    private Object GetRndPU()
    {
        Object[] powerUpList = Resources.LoadAll("Powerups", typeof(GameObject));
        int rndNum = Random.Range(0, powerUpList.Length);
        Object chosenPU = powerUpList[rndNum];

        return chosenPU;
    }

}
