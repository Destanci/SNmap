 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class KeyboardInput : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            VirtualInputManager.Instance.W = true;
        }
        else
        {
            VirtualInputManager.Instance.W = false;
        }

        if (Input.GetKey(KeyCode.A))
        {
            VirtualInputManager.Instance.A = true;
        }
        else
        {
            VirtualInputManager.Instance.A = false;
        }

        if (Input.GetKey(KeyCode.S))
        {
            VirtualInputManager.Instance.S = true;
        }
        else
        {
            VirtualInputManager.Instance.S = false;
        }

        if (Input.GetKey(KeyCode.D))
        {
            VirtualInputManager.Instance.D = true;
        }
        else
        {
            VirtualInputManager.Instance.D = false;
        }
    }
}  
