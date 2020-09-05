using Cinemachine;
using System;
using System.Collections;
using System.Diagnostics;
using TMPro;
using UnityEngine; 
using Debug = UnityEngine.Debug;

public class GameManager : MonoBehaviour
{ 
    public static GameManager current { get; private set; }

    public CheckPointManager lastCheckpoint;  
    public Vector3 PlayerLastCheckPoint; 
    public static int TotalChance = 3;
    public int RemainingChance = TotalChance; 
    public int ReachedCheckPoint = 0;
    [Space] 
    public GameObject VirtualCamera;
    public CinemachinePathBase DollyTrack;
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

    GameObject ninja;


    private void Awake()
    {
        if (current == null) { current = this; } else { Debug.Log("Warning: multiple " + this.name + " in scene!"); }
    } 

    private void Start()
    { 
        SpawnPlayer(Vector3.zero, 0.002146877f); 
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
    public void SpawnPlayer(Vector3 pos, float path)
    {
        pos.y += 0.25f;
        ninja = Instantiate(Ninja, pos, Quaternion.identity);
        ninja.transform.position = pos;
        SetCamera(ninja, path);  
    }

    public void Restart()
    {
        IsRespawned = true;
        Destroy(ninja);
        SpawnPlayer(Vector3.zero, 0.002146877f);
    }
    public void Respawn()
    {
        IsRespawned = true;
        Destroy(ninja);
        if (lastCheckpoint)
        { 
            SpawnPlayer(lastCheckpoint.transform.position, lastCheckpoint.pathPosition); 
        }
        else
        { 
            SpawnPlayer(Vector3.zero, 0.002146877f); 
        }

        //switch (RemainingChance)
        //{ 
        //    case 1:
        //        CheckPointInfo = "LAST CHANCE";
        //        break;
        //    case 2:
        //        CheckPointInfo = "WATCH OUT";
        //        break; 
        //    default:
        //        break;
        //} 
        //Text_CheckPoint.enabled = true;
        //Text_CheckPoint.text = CheckPointInfo;
        //StartCoroutine(closeText(1.5f));
    } 
    public void SetCamera(GameObject ninja, float path)
    {
        Destroy(GameObject.FindGameObjectWithTag("VirtualCamera"));
        CinemachineVirtualCamera camera = Instantiate(VirtualCamera).GetComponent<CinemachineVirtualCamera>();
        camera.GetCinemachineComponent<CinemachineTrackedDolly>().m_Path = DollyTrack;
        camera.GetCinemachineComponent<CinemachineTrackedDolly>().m_PathPosition = path;
        camera.Follow = ninja.transform;
        camera.LookAt = ninja.transform;
    }
     
    private IEnumerator closeText(float time)
    {
        yield return new WaitForSeconds(time);
        GameManager.current.Text_CheckPoint.enabled = false;
    } 
} 