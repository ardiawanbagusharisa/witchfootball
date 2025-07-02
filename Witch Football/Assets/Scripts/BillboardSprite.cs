using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardSprite : MonoBehaviour
{
    public bool FlipablePlayer;
    public bool RotatedBall;
    public float addRotation = 0f;
    private PlayerInput playerInput;
    private bool flip;
    private SpriteRenderer spriteRenderer;

    void Start(){
        if(FlipablePlayer){
            spriteRenderer = GetComponent<SpriteRenderer>();
            playerInput = PlayerInput.GetPlayer((int) transform.parent.GetComponent<WitchController>().playerID);
        }
    }

    void LateUpdate(){
        if(FlipablePlayer){
            ChangeDirection();
        } else {
            
        }
        //transform.forward = Camera.main.transform.forward;
 
        Transform mainCamTransform = Camera.main.transform;
        Vector3 newRotation = Vector3.zero;
        newRotation = new Vector3(mainCamTransform.eulerAngles.x, mainCamTransform.eulerAngles.y, mainCamTransform.eulerAngles.z);
        
        if(RotatedBall) {
            newRotation.z += addRotation;
            // <Edit later> change the constant
            float velocityX = -transform.parent.GetComponent<Rigidbody>().linearVelocity.x * 10; 
            addRotation += velocityX; 
            if(addRotation >= 360f || addRotation <= -360f) {
                addRotation = 0f;
            }
            //<Edit later> If use the method below, put it on the Update()
            //newRotation = new Vector3(newRotation.x, newRotation.y, transform.parent.rotation.eulerAngles.x); 
        }
        transform.eulerAngles = newRotation; 
        
    }

    void ChangeDirection(){
        float horizontal    = 0;
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
    }
}
