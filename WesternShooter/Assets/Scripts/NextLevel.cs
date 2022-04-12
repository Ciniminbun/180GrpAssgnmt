using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
       DontDestroyOnLoad(this.GameObject);
       currentScene = SceneManager.GetActiveScene().buildIndex; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void switchScene(int sceneNumber)
    {
        SceneManager.LoadScene(sceneNumber);
        player.transform.position = new Vector3(0f,1f,0f);
    }
    public void loadNextScene()
    {
        currentScene += 1;
        SceneManager.LoadScene(currentScene);
    }
}
