using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public Movement moveScript;
    public GunHandler gunScript;
    public GameObject PausePanel;
    public GameObject CakePanel;
    public bool isPaused = false;
    public bool pausePanelOn = false;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                if (pausePanelOn)
                {
                    Continue();
                    ClosePauseMenu();
                }
                else
                {
                    CloseCakeMenu();
                    OpenPauseMenu();
                }
            }
            else
            {
                Pause();
                OpenPauseMenu();

            }
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isPaused && !pausePanelOn)
            {
                Continue();
                CloseCakeMenu();
            }
            else if (!isPaused)
            {
                Pause();
                OpenCakeMenu();
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
        Time.timeScale = 0;
    }
    public void Continue()
    {
        gunScript.isPaused = false;
        moveScript.canMove = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isPaused = false;
        Time.timeScale = 1;
    }
    public void OpenPauseMenu()
    {
        PausePanel.SetActive(true);
        pausePanelOn = true;
    }
    public void ClosePauseMenu()
    {
        PausePanel.SetActive(false);
        pausePanelOn = false;
    }
    public void OpenCakeMenu()
    {
        CakePanel.SetActive(true);
    }
    public void CloseCakeMenu()
    {
        CakePanel.SetActive(false);
    }
}
    