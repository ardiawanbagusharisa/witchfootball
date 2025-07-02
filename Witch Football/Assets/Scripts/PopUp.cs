using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopUp : MonoBehaviour
{
    public enum PopUpType{
        Text, 
        Balloon, 
        Ability
    }
    public PopUpType popUpType;
    public float duration;
    public float fadeSpeed;
    public float moveSpeed;
    public TextMeshPro Text;
    public GameObject balloon;
    private TextMeshPro balloonTmpText;
    private SpriteRenderer balloonSpriteRenderer;
    public GameObject[] witchesPinUp;

    private bool fadeIn;
     

    void Start() {
        //Init(this.popUpType, "99", transform.position, Color.black);
    }
    // Call this when instantiate the PopUp
    public void Init(PopUpType type, string text, Vector3 position, Color color){
        Text.color = new Color(Text.color.r, Text.color.g, Text.color.b, 0f);

        balloonSpriteRenderer   = balloon.GetComponent<SpriteRenderer>();
        Color tmpBalloon        = balloonSpriteRenderer.color;
        balloonSpriteRenderer.color = new Color(tmpBalloon.r, tmpBalloon.g, tmpBalloon.b, 0f);
        balloonTmpText          = balloon.transform.Find("Text").GetComponent<TextMeshPro>();
        balloonTmpText.color    = new Color(balloonTmpText.color.r, balloonTmpText.color.g, balloonTmpText.color.b, 0f); 
        
        fadeIn = true;    
        this.Text.gameObject.SetActive(popUpType == PopUpType.Text ? true : false);
        this.balloon.gameObject.SetActive(popUpType == PopUpType.Balloon ? true : false);

        if(popUpType == PopUpType.Text){
            this.Text.text = text;
        } else if (popUpType == PopUpType.Balloon){
            this.balloonTmpText.text = text;
        }
        
        transform.position = position;
    }
    public void Perform(){
        if(popUpType == PopUpType.Text){
            if(fadeIn){
                if(Text.color.a < 1){
                    Text.color += new Color(0f, 0f, 0f, 2f * fadeSpeed * Time.deltaTime);
                    transform.position += new Vector3(0, 3f * moveSpeed * Time.deltaTime, 0);
                } else {
                    if(duration > 0){
                        duration -= Time.deltaTime;
                    } else {
                        fadeIn = false;
                    }
                }
            } else {
                if(Text.color.a > 0){
                    Text.color -= new Color(0f, 0f, 0f, fadeSpeed * Time.deltaTime);
                    transform.position -= new Vector3(0, 1f * moveSpeed * Time.deltaTime, 0);
                } else {
                    fadeIn = true;
                    //duration=
                }
            }
        }else if(popUpType == PopUpType.Balloon){
            if(fadeIn){
                if(balloonTmpText.color.a < 1){
                    balloonTmpText.color += new Color(0f, 0f, 0f, 2f * fadeSpeed * Time.deltaTime);
                    balloonSpriteRenderer.color += new Color(0f, 0f, 0f, 2f * fadeSpeed * Time.deltaTime);
                } else {
                    if(duration > 0){
                        duration -= Time.deltaTime;
                    } else {
                        fadeIn = false;
                    }
                }
            } else {
                if(balloonTmpText.color.a > 0){
                    balloonTmpText.color -= new Color(0f, 0f, 0f, fadeSpeed * Time.deltaTime);
                    balloonSpriteRenderer.color -= new Color(0f, 0f, 0f, fadeSpeed * Time.deltaTime);
                } else {
                    // Destroy
                    fadeIn = true;
                    duration = 1f; // edit later
                }
            }
        }else if(popUpType == PopUpType.Ability){
            // Structure: Background Env -> PinUp -> Text + Outline  
            // fadein and movein from left of the screen
            // add speedy flash effect from the opposite direction
            // add beautiful colored background
            // stay until the duration is over
            // fadeout to the opposite direction of the starting entry

        }
    }
    void Update(){
        Perform();
    }   
} 