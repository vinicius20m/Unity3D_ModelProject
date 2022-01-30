using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement ;
using UnityEngine.InputSystem ;

public class SceneKeys
{
    public const string MainScene = "MainScene" ;
    public const string MainMenu = "MainMenu" ;
}

public class GameController : MonoBehaviour
{
    
    PlayerInputActions pInput ;
    [SerializeField]GameObject pausePanel ;
    [SerializeField]GameObject hint ;

    void Awake()
    {
        
        pInput = new PlayerInputActions() ;
        if(SceneManager.GetActiveScene().buildIndex > 0){

            pInput.GeneralControls.Enable() ;
            pInput.GeneralControls.Pause.performed += PauseGame ;
        }
    }

    void PauseGame(InputAction.CallbackContext callback)
    {

        hint.SetActive(!hint.activeSelf) ;
        pausePanel.SetActive(!pausePanel.activeSelf) ;
        Time.timeScale = Time.timeScale > 0 ? 0 : 1 ;
    }

    public void LoadScene(string scene)
    {

        Time.timeScale = 1 ;
        SceneManager.LoadScene(scene) ;
    }

    public void ApplicationExit()
    {

        Application.Quit() ;
    }
}
