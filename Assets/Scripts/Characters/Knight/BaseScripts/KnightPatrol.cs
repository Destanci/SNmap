using UnityEngine;

public enum KnightState
{
    Idle,
    Walking,
    Turning
}

public class KnightPatrol : MonoBehaviour
{
    #region Inspector Variables

    [SerializeField]
    private Animator animator = null;

    [Header("Game Objects")]

    [SerializeField]
    private Collider PatrolArea = null;

    [SerializeField]
    private Collider MoveSpot = null;
    
    [SerializeField]
    private GameObject Exclamation = null;

    //[SerializeField]
    //private Transform MoveSpot;

    [Header("Settings")]

    [SerializeField]
    [Range(0, 20)]
    private float Speed = 1f;

    //[SerializeField]
    //[Range(0, 20)]
    //private float TurningSpeed = 5f;

    [SerializeField]
    [Range(0, 5)]
    private float minWaitTime = 1f;

    [SerializeField]
    [Range(0, 10)]
    private float maxWaitTime = 2f;

    [SerializeField]
    [Range(0, 10)]
    private float minDistance = 1f;

    [SerializeField]
    [Range(0, 15)]
    private float maxDistance = 2f;

    [SerializeField]
    [Range(0, 90)]
    private float minTurnAngle = 90f;

    [SerializeField]
    [Range(0, 120)]
    private float maxTurnAngle = 120f;

    #endregion
    #region nonInspector Variables
    [Header("Debug")]
    [SerializeField]
    public bool paused = false;

    [SerializeField]
    public KnightState state = KnightState.Idle;

    [HideInInspector]
    public float DeltaTime = 0; // Wait time counter.

    [SerializeField]
    private float targetAngle;

    [HideInInspector]
    public bool angleModified = false;
    [HideInInspector]
    private bool timeCalculated = false;
    [HideInInspector]
    private bool stopPatrol = false; 
    [HideInInspector]
    private float TurnSmoothVelocity;  
    [HideInInspector]
    private Rigidbody _rigidbody;
    public Rigidbody Rigidbody
    {
        get
        {
            if (_rigidbody == null)
            {
                _rigidbody = GetComponent<Rigidbody>();
            }
            return _rigidbody;
        }
    } 
    #endregion

    private void Start()
    { 
        if(maxWaitTime < minWaitTime)
        {
            float tmp = maxWaitTime;
            maxWaitTime = minWaitTime;
            minWaitTime = tmp;
        } ;
    }

    private void FixedUpdate()
    { 
        if (NinjaController.current == null || (NinjaController.current.IsSpotted && stopPatrol))
        { 
            return;
        } 
        else if(NinjaController.current.IsSpotted && !stopPatrol)
        { 
            stopPatrol = true;
            state = KnightState.Idle;
            Exclamation.SetActive(true);
            setRigidbody();
            gameObject.transform.LookAt(NinjaController.current.gameObject.transform.position);
        }
        else if(!NinjaController.current.IsSpotted && !NinjaController.current.IsSlideArea)
        { 
            Exclamation.SetActive(false);
            stopPatrol = false;
        }
        switch (state)
        {
            case KnightState.Idle:
                animator.SetBool("Patrol", false);
                if (!timeCalculated)
                {
                    timeCalculated = true;
                    if (!angleModified)
                    {
                        angleModified = true;
                        float turningAngle = Random.Range(minTurnAngle, maxTurnAngle);
                        int i = Random.Range(0, 2);
                        if (i == 1) turningAngle *= -1;

                        targetAngle = transform.eulerAngles.y + turningAngle;
                        //Debug.Log(targetAngle);
                    }
                    DeltaTime = Random.Range(minWaitTime, maxWaitTime);
                }
                DeltaTime -= Time.deltaTime;
                if(DeltaTime <= 0)
                {
                    state = KnightState.Turning;
                    timeCalculated = false;
                    angleModified = false;
                }
                break;
                 
            case KnightState.Walking: 
                animator.SetBool("Patrol", true);
                transform.position = Vector3.MoveTowards(transform.position, MoveSpot.transform.position, Speed * Time.deltaTime);
                transform.LookAt(MoveSpot.transform);
                transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
                break;
                 
            case KnightState.Turning: 
                animator.SetBool("Patrol", false); 
                Rigidbody.angularVelocity = Vector3.zero;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref TurnSmoothVelocity, 0.685f / 5);
                Rigidbody.MoveRotation(Quaternion.Euler(0f, angle, 0f));

                if(Quaternion.Angle(Quaternion.Euler(0f, angle, 0f), Quaternion.Euler(0, targetAngle, 0)) < 0.5f)
                {
                    MoveSpot.transform.position = transform.position + (transform.forward * Random.Range(minDistance, maxDistance));
                    state = KnightState.Walking;
                }
                break;
        }
    }

    private void AngleToMiddle()
    {
        angleModified = true;
        targetAngle = Quaternion.LookRotation(PatrolArea.transform.position - transform.position, Vector3.up).eulerAngles.y;
        float dif = Random.Range(minTurnAngle, maxTurnAngle) / 4;

        int num = Random.Range(-1, 2);
        dif *= num; // dif will randomly effected or not effected.

        targetAngle += dif;

        //Debug.Log("t " + targetAngle);
    }
    private void setRigidbody()
    {
        gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
    }

    private void OnTriggerExit(Collider other)
    {
        if(state == KnightState.Walking && other.Equals(PatrolArea))
        {
            state = KnightState.Idle;
            AngleToMiddle();
        } 
    }
    

    private void OnTriggerEnter(Collider other)
    {
        if (state == KnightState.Walking)
        {
            if(other.tag.Equals("Wall"))
            { 
                state = KnightState.Idle;
                AngleToMiddle();
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (state == KnightState.Walking)
        {
            if (other.Equals(MoveSpot))
            {
                state = KnightState.Idle;
            }
        }
    } 

    private void OnCollisionEnter(Collision collision)
    {
        if(state == KnightState.Walking && (collision.gameObject.tag.Equals("Wall") || collision.gameObject.tag.Equals("Knight")))
        { 
            state = KnightState.Idle;
            AngleToMiddle();
        }

        if(collision.gameObject.CompareTag("Player"))
        {
            NinjaController.current.IsSpotted = true;
        }
    } 
}
