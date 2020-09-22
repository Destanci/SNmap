using Cinemachine;
using Lean.Gui;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasManager : MonoBehaviour
{ 
    public static CanvasManager Instance { get; private set; }

    private GameManager gameManager = null;

    [SerializeField] private GameObject Doly = null;
    [SerializeField] private GameObject GM = null;
    [SerializeField] private GameObject PlayButton = null;
    [SerializeField] private GameObject Ninja = null;
    [SerializeField] private GameObject Options = null;
    [SerializeField] private GameObject Game = null;
    [SerializeField] private GameObject Dead = null;

    [SerializeField] private TextMeshProUGUI Info = null;

    private bool Sound = true;
    private bool Music = true;
     
    private void Awake()
    {
        if (Instance == null) { Instance = this; } else { Debug.Log("Warning: multiple " + this.name + " in scene!"); } 
    } 

    private void Update()
    {
        if(Doly != null && Doly.GetComponentInChildren<CinemachineDollyCart>().m_Position > 62)
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
        Destroy(Doly);
        Doly = null;
        Instantiate(GM);
        setAudio(Options.transform.GetChild(0).gameObject, true);
        setAudio(Options.transform.GetChild(1).gameObject, false);
        gameManager = GM.GetComponent<GameManager>();

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
        gameManager.Restart();
    }

    public void RespawnNinja()
    {
        gameManager.Respawn();
    }

    public void Death()
    { 
        Game.SetActive(false);
        Dead.SetActive(true);
        gameManager.RemainingChance -=1;
        
        if (gameManager.RemainingChance == 0)
        {
            Info.text = "You passed"+ gameManager.ReachedCheckPoint +"checkpoint";
            Dead.transform.GetChild(1).GetChild(3).gameObject.SetActive(false);
        }
        else
        {
            Info.text = "LEFT "+ gameManager.RemainingChance +" TRY";
        }
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

    private void setAudio(GameObject Button, bool type)
    {
        if(type)
        { 
            Button.GetComponent<LeanToggle>().On = Music;
        }
        else
        { 
            Button.GetComponent<LeanToggle>().On = Sound;
        }
    }
     
}
