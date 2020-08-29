using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{ 
    public static CanvasManager Instance { get; private set; }
    [Header("Camera")]  
    [SerializeField] private Camera GameCam;
    [SerializeField] private GameObject Vcam;
    [SerializeField] private GameObject Doly;
    [Space]
    [SerializeField] private GameObject GM;
     
    private void Awake()
    {
        if (Instance == null) { Instance = this; } else { Debug.Log("Warning: multiple " + this.name + " in scene!"); } 
    } 

    public void StartGame()
    {
        Doly.SetActive(false);
        GameCam.gameObject.SetActive(true);
        Vcam.SetActive(true);
        Instantiate(GM);
        GameManager.current.camera = Vcam.GetComponent<CinemachineVirtualCamera>(); 
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

    }

    public void OpenMainMenu()
    {

    }
}
