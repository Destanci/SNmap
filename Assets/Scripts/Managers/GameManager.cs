using Cinemachine;
using System;
using System.Collections;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class GameManager : MonoBehaviour
{
    public static GameManager current = null; 

    public CheckPointManager lastCheckpoint;
    public float CameraLastPathWay; 
     
    public Vector3 PlayerLastCheckPoint; 
    public static int TotalChance = 3;
    public int RemainingChance = TotalChance; 
    public int ReachedCheckPoint = 0;
    [Space] 
    public CinemachineVirtualCamera camera; 
    [Space]
    public GameObject Ninja;
    [Space]
    public TextMeshProUGUI Text_CheckPoint;
    public GameObject UX_RespawnNumber;
    public TextMeshProUGUI Text_ReachedCheckPoint;
    public TextMeshProUGUI Text_Timer;

    [HideInInspector]
    public bool IsRespawned = false;

    [HideInInspector]
    public string CheckPointInfo;

    [HideInInspector]
    public Stopwatch Timer = new Stopwatch();

    [HideInInspector]
    public bool IsPlayerStart = false; 

    [HideInInspector]
    public bool IsGameFinish = false;  


    private void Awake()
    {  
        if (current != null)
        { 
            Debug.Log("Already a GameManager Working.");
        }
        else
        {
            current = this;
        }
    }

    private void OnDestroy()
    {
        if (current == this)
            current = null;
    }

    private void Start()
    {  
        SpawnPlayer(Vector3.zero); 
    }

    private void Update()
    {
        if(IsPlayerStart && !Timer.IsRunning && !IsGameFinish)
        {
            Timer.Start(); 
        }
        if(Text_Timer && Timer.IsRunning)
        {
            TimeSpan ts = Timer.Elapsed; 
            Text_Timer.text = string.Format("{0:00}:{1:00}:{2:00}",ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
        }
    }

    public void Respawn()
    {
        IsRespawned = true;
        Destroy(GameObject.FindGameObjectWithTag("Player"));
        if (lastCheckpoint)
            SpawnPlayer(lastCheckpoint.transform.position);
        else
            SpawnPlayer(Vector3.zero);

        switch (RemainingChance)
        { 
            case 1:
                CheckPointInfo = "LAST CHANCE";
                break;
            case 2:
                CheckPointInfo = "WATCH OUT";
                break; 
            default:
                break;
        } 
        Text_CheckPoint.enabled = true;
        Text_CheckPoint.text = CheckPointInfo;
        StartCoroutine(closeText(1.5f));
    }

    private void SpawnPlayer(Vector3 pos)
    {
        pos.y += 0.25f;
        GameObject ninja = Instantiate(Ninja, pos, Quaternion.identity);
        ninja.transform.position = pos;
        SetCamera(ninja);
    }
    private void SetCamera(GameObject ninja)
    {
        camera.Follow = ninja.transform;
        camera.LookAt = ninja.transform; 
    } 
    private IEnumerator closeText(float time)
    {
        yield return new WaitForSeconds(time);
        GameManager.current.Text_CheckPoint.enabled = false;
    }
} 