using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public UISFXManager SFXManager;

    void Start(){
        SFXManager = GetComponent<UISFXManager>();
    }

    void Update(){
        // Check the inputs
    }
}
