using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PinUp : MonoBehaviour
{
    public WitchController witchController;
    public Image image;
    public GameObject background;
    public Canvas canvas;
    public float duration = 2f;
    public float fadeSpeed = 5f;
    public float imgSpeed = 20f;
    public float bgSpeed = 30f;
    public RectTransform startPos;
    public RectTransform targetPos;
    public RectTransform endPos;
    private float currentDuration;
    private Vector3 imgStartPos;
    private Vector3 imgTargetPos;
    private Vector3 imgEndPos;
    private Vector3 bgStartPos;
    private Vector3 bgEndPos;
    private Team.TeamParty teamParty;
    private bool fadeIn = true;
    
    public void Init(WitchController witchController){
        this.witchController = witchController;
        image.sprite = witchController.pinUpSprite;
        // <Edit later> Assign the witchController here, so we can refer this like one in the PlayerHUD
    }
    void LateUpdate() {
        // Background & Image Moving
        // <Edit later> This will create error when the team is TeamB, but the player is Player1 and Player2.
         // we can actually check the direction and use it as either increment or decrement. 
        if(teamParty == Team.TeamParty.TeamA){   
            // Background 
            if(background.transform.position.x > bgEndPos.x){
                background.transform.position -= new Vector3(bgSpeed * canvas.scaleFactor, 0f, 0f);
            }
            // Image
            if(fadeIn){
                if(image.transform.position.x < imgTargetPos.x){
                    image.transform.position += new Vector3(imgSpeed * canvas.scaleFactor, 0f, 0f);
                } 
            } else {
                if(image.transform.position.x < imgEndPos.x){
                    image.transform.position += new Vector3(2f * imgSpeed * canvas.scaleFactor, 0f, 0f);
                }
            }
            
        } else {    
            // Background
            if(background.transform.position.x < bgEndPos.x){
                background.transform.position += new Vector3(bgSpeed * canvas.scaleFactor, 0f, 0f);
            }
            // Image
            if(fadeIn){
                if(image.transform.position.x > imgTargetPos.x){
                    image.transform.position -= new Vector3(imgSpeed * canvas.scaleFactor, 0f, 0f);
                } 
            } else {
                if(image.transform.position.x > imgEndPos.x){
                    image.transform.position -= new Vector3(2f * imgSpeed * canvas.scaleFactor, 0f, 0f);
                }
            }
        }

        // Image Fading
        if(fadeIn){
            if(image.color.a < 1){
                image.color += new Color(0f, 0f, 0f, 0.75f * fadeSpeed * Time.deltaTime);
            } else {
                if(currentDuration > 0){
                    currentDuration -= Time.deltaTime;
                } else {
                    fadeIn = false;
                }
            }
        } else {
            if(image.color.a > 0){
                image.color -= new Color(0f, 0f, 0f, fadeSpeed * Time.deltaTime);
            } else {
                // Revert and Disable
                //duration = 2f; 
                currentDuration = duration;
                fadeIn = true;
                // <edit later> Disable the object. Make Setup or Init. 
                //transform.position
                background.transform.position   = bgStartPos;
                image.transform.position        = new Vector3(imgStartPos.x, transform.position.y, transform.position.z);
                FindFirstObjectByType<PinUpController>().SetOff(this.gameObject);
            }
        }
    }
 
    public void Perform(WitchController witchController, Image pinUpimage){
        // Init
        teamParty   = witchController.teamParty;
        image       = pinUpimage;
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0f);
        fadeIn      = true;
        background.SetActive(true);
        currentDuration = duration;
        // Background
        if(teamParty == Team.TeamParty.TeamA){
            bgStartPos  = new Vector3(canvas.scaleFactor * canvas.GetComponent<RectTransform>().rect.width + canvas.scaleFactor * background.GetComponent<RectTransform>().rect.width / 2, 
                                    background.transform.position.y, background.transform.position.z);
            bgEndPos    = new Vector3 (-1 * (canvas.scaleFactor * background.GetComponent<RectTransform>().rect.width / 2),
                                    background.transform.position.y, background.transform.position.z);
        } else {
            bgStartPos  = new Vector3 (-1 * (canvas.scaleFactor * background.GetComponent<RectTransform>().rect.width / 2),
                                    background.transform.position.y, background.transform.position.z);
            bgEndPos    = new Vector3(canvas.scaleFactor * canvas.GetComponent<RectTransform>().rect.width + canvas.scaleFactor * background.GetComponent<RectTransform>().rect.width / 2, 
                                    background.transform.position.y, background.transform.position.z); 
        }
        background.transform.position = bgStartPos;

        // Image
        imgTargetPos    = new Vector3(targetPos.transform.position.x, image.transform.position.y, image.transform.position.z);
        imgStartPos     = new Vector3(startPos.transform.position.x, image.transform.position.y, image.transform.position.z);
        imgEndPos       = new Vector3(endPos.transform.position.x, image.transform.position.y, image.transform.position.z); 
        //Debug.Log("current, target, start, end" + transform.position.x + ", " + imgTargetPos.x + ", " + imgStartPos.x + ", " + imgEndPos.x); 
        image.transform.position = imgStartPos;
        //Debug.Log("current, target, start, end" + transform.position.x + ", " + imgTargetPos.x + ", " + imgStartPos.x + ", " + imgEndPos.x); 
        //Debug.Log("Duration: " + duration + ", Current: " + currentDuration + ", FadeIn: " + fadeIn);    
    }
    public void SetDuration(float duration){
        this.duration = duration;
        this.currentDuration = duration;
    }
}
