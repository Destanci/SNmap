using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualInput : MonoBehaviour
{ 
    private NinjaController ninjaController;

    private void Awake()
    {
        ninjaController = gameObject.GetComponent<NinjaController>();
    }

    private void Update()
    { 

    }
}
