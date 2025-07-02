using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardRotator : MonoBehaviour
{
    /* private SpriteRenderer spriteRenderer;
    private Rigidbody rigidBody;

    void Start(){
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidBody = transform.parent.GetComponent<Rigidbody>();
    }
    void LateUpdate() {
        float angularVelocityX = rigidBody.angularVelocity.x;
        //if(angularVelocityX < 0){
            Quaternion rotation = transform.localRotation;
            rotation = new Quaternion(rotation.x, rotation.y*angularVelocityX, rotation.z, 1);
            transform.localRotation = rotation;
            //transform.Rotate(transform.eulerAngles.x, transform.eulerAngles.y * angularVelocityX, transform.eulerAngles.z);
        //}
    }
    */

    //public bool Flipable;
    //private PlayerInput playerInput;
    //private bool flip;
    private SpriteRenderer spriteRenderer;
    private Rigidbody rigidBody;

    void Start(){
        rigidBody = transform.parent.GetComponent<Rigidbody>();
        //if(Flipable){
          //  spriteRenderer = GetComponent<SpriteRenderer>();
         //   playerInput = PlayerInput.GetPlayer((int) transform.parent.GetComponent<WitchController>().playerID);
       // }
       transform.forward = Camera.main.transform.forward;
    }

    void LateUpdate(){
        //if(Flipable){
          //  ChangeDirection();
       // } else {
            
       // }
        
        float angularVelocityZ = rigidBody.angularVelocity.z;
        Transform newTransform = new GameObject().transform;
        //newTransform.forward = Camera.main.
        //transform.forward = Camera.main.transform.forward;
        //transform.localRotation = Quaternion.Euler(transform.localRotation)
        transform.Rotate(transform.localRotation.x, 
                                                    transform.transform.localRotation.y, 
                                                    transform.localRotation.z * angularVelocityZ);
        //transform.rotation = Quaternion.Euler(transform.localRotation.x, 
                                                   // transform.transform.localRotation.y, 
                                                   // transform.localRotation.z * angularVelocityZ);

    }

    void ChangeDirection(){
        /* float horizontal    = 0;
        float vertical      = 0;

        horizontal = Input.GetAxis(playerInput.HorizontalMove);
        vertical = Input.GetAxis(playerInput.VerticalMove);

        if(horizontal > 0.1){
            flip = false;
        }
        if(horizontal < -0.1){
            flip = true;
        }
        spriteRenderer.flipX = flip;

        //transform.forward = Camera.main.transform.forward;
        //transform.forward = new Vector3(Camera.main.transform.forward.x, 
          //                              lastDir, 
            //                            Camera.main.transform.forward.z);
    */
    }
    

}
