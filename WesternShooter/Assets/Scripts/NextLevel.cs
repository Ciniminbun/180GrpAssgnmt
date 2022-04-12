using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    public GameObject player;

    public static NextLevel instance;

    private int currentScene;

    // Start is called before the first frame update
    void Start()
    {
       instance = this;
       DontDestroyOnLoad(player);
       DontDestroyOnLoad(this.gameObject);
    }

    public void LoadScene(int sceneNumber)
    {
        SceneManager.LoadScene(sceneNumber);
        player.transform.position = new Vector3(0f, 1f, 0f);
    }
}
