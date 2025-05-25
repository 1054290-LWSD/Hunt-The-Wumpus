using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public Movement moveScript;
    public GunHandler gunScript;
    public GameObject PausePanel;
    public bool isPaused = false;
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            if (isPaused) {
                Continue();    
            } else {
                Pause();
                
            }
            
        }
    }
    public void Pause()
    {
        moveScript.canMove = false;
        gunScript.isPaused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        isPaused = true;
        PausePanel.SetActive(true);
        Time.timeScale = 0;
        Debug.Log("Pause");
    }
    public void Continue()
    {
        gunScript.isPaused = false;
        moveScript.canMove = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isPaused = false;
        PausePanel.SetActive(false);
        Time.timeScale = 1;
        Debug.Log("Unpause");
    }
}
    