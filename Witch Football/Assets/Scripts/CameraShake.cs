using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public Transform camTransform;

    public float shakeSpeed = 1.0f;
    public float shakeDuration = 0f;
    public float shakeAmount = 0.7f;
    public float decreaseFactor = 1.0f;
    private bool isShake = false;
     Vector3 originalPos;
     float originalShakeDuration;

     void Awake() {
         if(camTransform == null) {
             camTransform = GetComponent<Transform>();
         }
     }

     void Start(){
         //ShakeCamera(5, 3);
     }

     void OnEnable(){
         originalPos = camTransform.localPosition;
         originalShakeDuration = shakeDuration;
     }

     void LateUpdate(){
         if(isShake){
             if(shakeDuration > 0){
                 camTransform.localPosition = Vector3.Lerp(camTransform.localPosition, 
                                                            originalPos + Random.insideUnitSphere * shakeAmount, 
                                                            Time.deltaTime * shakeSpeed);
                shakeDuration -= Time.deltaTime * decreaseFactor;
             }else {
                 shakeDuration = originalShakeDuration;
                 camTransform.localPosition = originalPos;
                 isShake = false;
             }
         }
     }
    
     public void ShakeCamera(float shakeDuration, float shakeAmount){
         isShake = true;
         this.shakeDuration = shakeDuration;
         this.shakeAmount = shakeAmount;
     }
}
