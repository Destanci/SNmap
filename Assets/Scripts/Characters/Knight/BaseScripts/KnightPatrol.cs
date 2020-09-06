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
    private Animator animator;

    [Header("Game Objects")]

    [SerializeField]
    private Collider PatrolArea;

    [SerializeField]
    private Collider MoveSpot;

    //[SerializeField]
    //private Transform MoveSpot;

    [Header("Settings")]

    [SerializeField]
    [Range(0, 20)]
    private float Speed = 1f;

    [SerializeField]
    [Range(0, 20)]
    private float TurningSpeed = 5f;

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
    [Range(0, 60)]
    private float minTurnAngle = 30f;

    [SerializeField]
    [Range(0, 90)]
    private float maxTurnAngle = 45f;

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
    private float TurnSmoothVelocity;

    [HideInInspector]
    private Rigidbody _rigidbody;
    public Rigidbody rigidbody
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
        }
    }

    private void FixedUpdate()
    {
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
                rigidbody.angularVelocity = Vector3.zero;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref TurnSmoothVelocity, 0.685f / 5);
                rigidbody.MoveRotation(Quaternion.Euler(0f, angle, 0f));

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
    }
}
