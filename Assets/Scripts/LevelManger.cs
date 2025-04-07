using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelManger : MonoBehaviour
{
    public string sceneName;
    public string level_1;
    public string level_2;
    public string level_3;
    public string level_4;
    public void changesScene()
    {
        SceneManager.LoadScene(sceneName);
    }
    private void Update()
    {
        if (Input.GetKeyDown("1"))
        {
            SceneManager.LoadScene(level_1);
        }
        if (Input.GetKeyDown("2"))
        {
            SceneManager.LoadScene(level_2);
        }
        if (Input.GetKeyDown("3"))
        {
            SceneManager.LoadScene(level_3);
        }
        if (Input.GetKeyDown("4"))
        {
            SceneManager.LoadScene(level_4);
        }
    }
}
