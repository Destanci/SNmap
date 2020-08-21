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

    [SerializeField]
    public float DeltaTime = 0; // Wait time counter.

    [HideInInspector]
    public bool angleModified = false;
    [HideInInspector]
    private bool timeCalculated = false;
    [HideInInspector]
    private float targetAngle;
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
                        Debug.Log(targetAngle);
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
                transform.position = Vector3.MoveTowards(transform.position, MoveSpot.transform.position, Speed * Time.deltaTime);
                transform.LookAt(MoveSpot.transform);
                transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
                break;


            case KnightState.Turning:
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref TurnSmoothVelocity, 0.685f / 5);
                rigidbody.MoveRotation(Quaternion.Euler(0f, angle, 0f));

                //transform.rotation = Quaternion.Lerp(
                //    transform.rotation,
                //    Quaternion.Euler(0, targetAngle, 0), 
                //    Time.deltaTime * TurningSpeed);

                if(Quaternion.Angle(transform.rotation, Quaternion.Euler(0, targetAngle, 0)) < 0.5f) //go walking.
                {
                    state = KnightState.Walking;
                    MoveSpot.transform.position = transform.position + (transform.forward * Random.Range(minDistance, maxDistance));
                }
                break;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(state == KnightState.Walking && other.Equals(PatrolArea))
        {
            state = KnightState.Idle;
            angleModified = true;
            targetAngle = Quaternion.LookRotation(other.transform.position - transform.position, Vector3.up).eulerAngles.y;
            int num = Random.Range(-1, 2);
            Debug.Log(num);
            targetAngle += Random.Range(minTurnAngle, maxTurnAngle) / 2 * num;
            Debug.Log( "t " + targetAngle);
            //targetAngle += Random.Range(minTurnAngle, maxTurnAngle) / 2 * Random.Range(-1f, 1f);
        }
    }
    

    private void OnTriggerStay(Collider other)
    {
        if (state == KnightState.Walking) //go idle.
        {
            if(other.Equals(MoveSpot) || other.tag.Equals("Wall"))
            {
                state = KnightState.Idle;
            }
        }
    }

    private void OnCollisionEnter(Collision collision) // TODO : CALCULATE FUGIN TURN ANGLE AFTER HIT TO WALLS.
    {
        if(state == KnightState.Walking && collision.gameObject.tag.Equals("Wall"))
        {
            state = KnightState.Idle;
        }
    }

}
