using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class timedDestroy : MonoBehaviour
{
    public float duration;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("deathTimer");
    }

    IEnumerator deathTimer()
    {
        yield return new WaitForSeconds(duration);
        Destroy(this.gameObject);
    }
}
