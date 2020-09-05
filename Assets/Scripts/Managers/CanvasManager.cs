using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasManager : MonoBehaviour
{ 
    public static CanvasManager Instance { get; private set; } 

    [SerializeField] private GameObject Doly; 
    [SerializeField] private GameObject GM;
    [SerializeField] private GameObject PlayButton;
    [SerializeField] private GameObject Ninja;


    private bool Sound = true;
    private bool Music = true;
     
    private void Awake()
    {
        if (Instance == null) { Instance = this; } else { Debug.Log("Warning: multiple " + this.name + " in scene!"); } 
    }

    private void Update()
    {
        if(Doly.GetComponentInChildren<CinemachineDollyCart>().m_Position > 62)
        {
            PlayButton.SetActive(true);
            Ninja.GetComponent<NinjaController>().animator.SetBool(TransitionParameters.RunFinished.ToString(), true);
        } 
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
        ResumeGame();
    }
    public void StartGame()
    {
        Doly.SetActive(false); 
        Instantiate(GM);  
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
    } 

    public void RestartGame()
    {
        ResumeGame();
    }
    
    public void SoundToggle()
    {
        if(Sound)
        { 
            AudioListener.volume = 0f;
            Sound = false;
        }
        else
        {
            AudioListener.volume = 1f;
            Sound = true;
        }
    }

    public void MusicToggle()
    {
        if (Music)
        {
            //Music
            Music = false;
        }
        else
        {
            //Music
            Music = true;
        }
    }
}
