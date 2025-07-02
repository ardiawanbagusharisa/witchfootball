using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotator : MonoBehaviour
{
    public float initialRotationX = -45f;
    public float targetRotationX = 35f;
    
    private Quaternion targetRot;

    void Start()
    {
        transform.rotation = Quaternion.Euler(initialRotationX, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        targetRot = Quaternion.Euler(targetRotationX, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);    
    }

    void Update()
    {   
        transform.rotation =  Quaternion.Slerp(transform.rotation, targetRot, 0.5f *  Time.deltaTime);
        if(Quaternion.Angle(transform.rotation, targetRot) == 0){
            transform.rotation = Quaternion.Euler(targetRotationX, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
            Destroy(this);
        }
    }
}
