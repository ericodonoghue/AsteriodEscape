﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PauseControl : MonoBehaviour
{
    public bool isPaused = false;
    public bool isListening = true;

    private GameObject pauseMenu;
    private YouDiedControl youDied;
    private YouWinControl youWin;
    private GameObject youWonMenu;
    private PlayerInputManager playerInputManager;


    // Start is called before the first frame update
    void Start()
    {
        youDied = Camera.main.GetComponent<YouDiedControl>();
        youWin = Camera.main.GetComponent<YouWinControl>();
        SetPauseMenuDeactive();
    }

    private void Awake()
    {
        youWonMenu = GameObject.FindGameObjectWithTag("YouWonMenu");
        pauseMenu = GameObject.FindGameObjectWithTag("PauseMenu");
        this.playerInputManager = Camera.main.GetComponent<PlayerInputManager>();


        if (isPaused)
        {
            SetPauseMenuActive();
        }
        else
        {
            SetPauseMenuDeactive();
        }


        // Set up event handlers for Player Input Manager to monitor for menu commands
        this.playerInputManager.AssignPlayerInputEventHandler(PlayerInput.PauseGame, PauseGame_Pressed, PauseGame_Released);
        // TODO: Implement a separate Pause Screen and Game Menu? this.playerInputManager.AssignPlayerInputEventHandler(PlayerInput.GameMenu, GameMenu_Pressed, GameMenu_Released);
    }

    private void OnDisable()
    {
        // Tear down event handlers for Player Input Manager to monitor for menu commands
        this.playerInputManager.UnassignPlayerInputEventHandler(PlayerInput.PauseGame, PauseGame_Pressed, PauseGame_Released);
        // TODO: if we ever implement at game menu, will need this: this.playerInputManager.UnassignPlayerInputEventHandler(PlayerInput.GameMenu, SystemMenu_Pressed, SystemMenu_Released);
    }

    public void SetPauseMenuActive()
    {
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
        Cursor.lockState = CursorLockMode.Confined;
        this.playerInputManager.ActivatePlayerInputMonitoring = false;
    }

    public void SetPauseMenuDeactive()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        this.playerInputManager.ActivatePlayerInputMonitoring = true;
    }


    #region PlayerInput Events

    private void PauseGame_Pressed() {}
    private void PauseGame_Released()
    {
        // If the player uses the currently assigned "pause" key on the settings menu
        // to reassign it, this code will resume gameplay, but not close the settings screen
        // This flag allows the settings screen to tell the pause system to stop listening
        // while it is busy assigning keys
        if (this.isListening)
        {
            if (!youWin.won && !youDied.isDead)
            {
                isPaused = !isPaused;
                if (isPaused)
                {
                    SetPauseMenuActive();
                }
                else
                {
                    SetPauseMenuDeactive();
                }
            }
        }
    }

    private void GameMenu_Pressed() {}
    private void GameMenu_Released() {}

    #endregion PlayerInput Events
}


// Decprecated (actaully just relocated) code
//// Update is called once per frame
//void Update()
//{
//    if (!youWin.won && !youDied.isDead)
//    {
//        if (Input.GetKeyDown(KeyCode.BackQuote))
//        {
//            isPaused = !isPaused;
//            if (isPaused)
//            {
//                SetPauseMenuActive();
//            }
//            else
//            {
//                SetPauseMenuDeactive();
//            }
//        }          
//    }
//}
