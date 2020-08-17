using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

public enum CheckPointType
{
    Checkpoint,
    Startpoint,
    Endpoint
};

public class CheckPointManager : MonoBehaviour
{
    [SerializeField]
    public CheckPointType type = CheckPointType.Checkpoint; 
    [SerializeField]
    private bool IsCheckpointPassed = false;

    private Vector3 CheckpointPos;

    //[SerializeField]
    //public List<SpawnOrc> spawnList = new List<SpawnOrc>();
    //[SerializeField]
    //public List<SpawnOrc> deSpawnList = new List<SpawnOrc>();

    //[SerializeField]
    //private List<GameObject> Lambs = new List<GameObject>();


    private void Awake()
    {
        //for (int i = 0; i < 4; i++)
        //{ 
        //    Lambs.Add(transform.GetChild(1).GetChild(i).gameObject);
        //} 
    }
    private void Start()
    {
        CheckpointPos = transform.position;
        IsCheckpointPassed = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (IsCheckpointPassed) return;
        if (collision.gameObject.CompareTag("Player"))
        {
            IsCheckpointPassed = true;

            //foreach(SpawnOrc spawn in spawnList)
            //{
            //    spawn.Spawn();
            //}
            //foreach(SpawnOrc deSpawn in deSpawnList)
            //{
            //    deSpawn.deSpawn();
            //}

            //foreach (var lamb in Lambs)
            //{
            //    lamb.GetComponentInChildren<Renderer>().materials[1].color = material.color;
            //}
            //switch (type)
            //{
            //    case CheckPointType.Checkpoint: 
            //        GameManager.current.ReachedCheckPoint++;
            //        GameManager.current.lastCheckpoint = this;
            //        GameManager.current.Text_ReachedCheckPoint.text = "CheckPoint Saved : " + GameManager.current.ReachedCheckPoint; 
            //        if (!IsCheckpointPassed)
            //        {
            //            IsCheckpointPassed = true;
            //            GameManager.current.ReachedCheckPoint++;
            //            GameManager.current.PlayerLastCheckPoint = CheckpointPos;
            //        }

            //        GameManager.current.Text_ReachedCheckPoint.text = "CheckPoint Saved : " + GameManager.current.ReachedCheckPoint; 
            //        switch (GameManager.current.ReachedCheckPoint)
            //        {
            //            case 1:
            //                GameManager.current.CheckPointInfo = "NICE";
            //                break;
            //            case 2:
            //                GameManager.current.CheckPointInfo = "GOOD";
            //                break;
            //            case 3:
            //                GameManager.current.CheckPointInfo = "YOU GOT THIS";
            //                break;
            //            case 4:
            //                GameManager.current.CheckPointInfo = "WOW";
            //                break;
            //            case 5:
            //                GameManager.current.CheckPointInfo = "PERFECT";
            //                break;
            //            case 6:
            //                GameManager.current.CheckPointInfo = "GOOD SLIDE";
            //                break;
            //            case 7:
            //                GameManager.current.CheckPointInfo = "AGILE";
            //                break;
            //            case 8:
            //                GameManager.current.CheckPointInfo = "WHAT A MOVEMENT";
            //                break;
            //            case 9:
            //                GameManager.current.CheckPointInfo = "PERFECT SLIDE";
            //                break;
            //            case 10:
            //                GameManager.current.CheckPointInfo = "ALMOST DONE";
            //                break;
            //            case 11:
            //                GameManager.current.CheckPointInfo = "UNSTOPPABLE";
            //                break; 
            //        }  
            //        break;
            //    case CheckPointType.Startpoint:
            //        Debug.Log("START");
            //        break;
            //    case CheckPointType.Endpoint:
            //        //GameMenu.current.Complete();
            //        GameManager.current.IsGameFinish = true; 
            //        GameManager.current.Timer.Stop();
            //        gameObject.transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
            //        break;
            //}
            //GameManager.current.Text_CheckPoint.enabled = true;
            //GameManager.current.Text_CheckPoint.text = GameManager.current.CheckPointInfo;
            //StartCoroutine(closeText(1.5f));
        }
    }

    private IEnumerator closeText(float time)
    {
        yield return new WaitForSeconds(time);
        GameManager.current.Text_CheckPoint.enabled = false;
    }
}
