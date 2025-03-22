using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelManger : MonoBehaviour
{
    public string sceneName;
    public void changesScene()
    {
        SceneManager.LoadScene(sceneName);
    }
}
