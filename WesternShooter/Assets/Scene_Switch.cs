using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene_Switch : MonoBehaviour
{
    public GameObject player;
    public GameObject userInterface;

    public static Scene_Switch instance;

    public int currentScene;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        DontDestroyOnLoad(player);
        DontDestroyOnLoad(userInterface);
        DontDestroyOnLoad(this.gameObject);
        currentScene = SceneManager.GetActiveScene().buildIndex;


    }

    public void switchScene(int sceneNumber)
    {
        SceneManager.LoadScene(sceneNumber);
    }
    public void loadNextScene()
    {
        currentScene += 1;
        switchScene(currentScene);
    }

}
