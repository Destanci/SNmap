using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Look Sensitivity")]
    public float SensX;
    public float SensY;

    [Header("Clamping")]
    public float minY;
    public float maxY;

    [Header("Spectator")]
    public float spectatorMoveSpeed;

    private float rotX;
    private float rotY;

    [SerializeField]
    private bool IsSpectator = true;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void LateUpdate()
    {
        rotX += Input.GetAxis("Mouse X") * SensX;
        rotY += Input.GetAxis("Mouse Y") * SensY;


        rotY = Mathf.Clamp(rotY, minY, maxY);

        if (IsSpectator)
        {
            transform.rotation = Quaternion.Euler(-rotY, rotX, 0);

            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");
            float y = 0;

            if (Input.GetKey(KeyCode.E))
            {
                y = 1;
            }
            else if (Input.GetKey(KeyCode.Q))
            {
                y = -1;
            }

            Vector3 dir = transform.right * x + transform.up * y + transform.forward * z;
            transform.position += dir * spectatorMoveSpeed * Time.deltaTime;
        }
    }
}