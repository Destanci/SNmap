using UnityEngine; 

public enum TransitionParameters
{
    Sprint,
    Slide,
    Death,  
    RunFinished,
}
public class NinjaController : MonoBehaviour
{ 
    public Animator animator; 
    [Space]
    [Range(1f, 50f)]
    public float Speed = 10f;
    [Range(0.01f, 1f)]
    public float TurnSmoothTime = 0.685f; 
    private float TurnSmoothVelocity = 0;  
    [HideInInspector]
    public float vertical;
    [HideInInspector]
    public float horizontal;  
    [HideInInspector]
    public bool IsSlideArea;
    [HideInInspector]
    public bool IsRespawnDone;
    [HideInInspector]
    public bool touchingWall = false; 

    private Rigidbody _rigidbody;
    public Rigidbody rigid_body
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
    private void Start()
    {
        IsSlideArea = false;
        IsRespawnDone = false;  
    }

    private void OnCollisionEnter(Collision collision)
    {
        //if (!animator.GetBool(TransitionParameters.Death.ToString()) && collision.gameObject.CompareTag("Knight"))
        //{ 
        //    animator.SetBool(TransitionParameters.Death.ToString(), true); 
        //    return;
        //}
         
        if (collision.gameObject.CompareTag("Wall"))
        {
            touchingWall = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            touchingWall = false;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        CheckPointManager cb = collision.gameObject.GetComponent<CheckPointManager>();
        if (IsSlideArea && cb && cb.enabled )
        {  
            IsSlideArea = false;
            return;
        }
        else if (!IsSlideArea && collision.gameObject.CompareTag("SlideArea"))
        { 
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName(TransitionParameters.Slide.ToString()))
            {
                animator.SetBool(TransitionParameters.Slide.ToString(), true);
            } 
            IsSlideArea = true;
            return;
        }
    }

    private void FixedUpdate()
    {
        if (animator.GetBool(TransitionParameters.Death.ToString()) || !IsRespawnDone) return;

        vertical = SimpleInput.GetAxisRaw("Vertical");
        horizontal = SimpleInput.GetAxisRaw("Horizontal");


        Camera cam = Camera.main;
        Vector3 forward = cam.transform.forward;
        Vector3 right = cam.transform.right;

        forward.y = 0;
        forward.Normalize();

        right.y = 0;
        right.Normalize();

        Vector3 direction = forward * vertical + right * horizontal;

        if (!IsSlideArea)
        {
            if (horizontal == 0 && vertical == 0) return;

            if (direction.magnitude >= 0.1f)
            {
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                float angle = Mathf.SmoothDampAngle(gameObject.transform.eulerAngles.y, targetAngle, ref TurnSmoothVelocity, TurnSmoothTime / 5);

                rigid_body.angularVelocity = Vector3.zero;
                rigid_body.MoveRotation(Quaternion.Euler(0f, angle, 0f)); 

                Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

                rigid_body.MovePosition(rigid_body.position + moveDir.normalized * (Speed / 1.5f) * Time.fixedDeltaTime);
                gameObject.transform.position += gameObject.transform.forward * Time.fixedDeltaTime * (Speed / 1.5f); 
            }
        }
        else
        {
            if (direction.magnitude >= 0.1f)
            {
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                float angle = Mathf.SmoothDampAngle(gameObject.transform.eulerAngles.y, targetAngle, ref TurnSmoothVelocity, TurnSmoothTime);

                rigid_body.angularVelocity = Vector3.zero;
                rigid_body.MoveRotation(Quaternion.Euler(0f, angle, 0f)); 
            }
            //gameObject.transform.Translate(Vector3.forward * Speed * Time.deltaTime);
            gameObject.transform.position += gameObject.transform.forward * Time.fixedDeltaTime * Speed;
        }
    } 
}