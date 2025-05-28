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
    public void changesScene(string name = null)
    {
        if (name == null || name == "")
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            SceneManager.LoadScene(name);
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown("1"))
        {
            changesScene(level_1);
        }
        if (Input.GetKeyDown("2"))
        {
            changesScene(level_2);
        }
        if (Input.GetKeyDown("3"))
        {
            changesScene(level_3);
        }
        if (Input.GetKeyDown("4"))
        {
            changesScene(level_4);
        }
    }
}
