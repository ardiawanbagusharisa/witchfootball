using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PinUpController : MonoBehaviour
{
    // Index based on PlayerID
    public GameObject[] pinUpWitches; 
    public GameObject[] backgrounds;
    
    public GameObject topCinematicBorder;
    public GameObject botCinematicBorder;
    public Transform topTargetPos;
    public Transform topEndPos;
    public Transform botTargetPos;
    public Transform botEndPos; 
    private bool fadeIn = false;
    private bool fadeOut = false;
    private Canvas canvas;

    void Start(){
        Init();
    }
    void Init(){
        // Change Later
        /* topTargetPos    = transform.Find("Cinematic Positions").Find("TopTargetPos").position;
        botTargetPos    = transform.Find("Cinematic Positions").Find("BotTargetPos").position; 
        Canvas canvas = GameObject.Find("PinUp Canvas").GetComponent<Canvas>();
        GameObject topBorder = transform.Find("Bottom Cinematic Border").gameObject;
        topEndPos       = new Vector3(topEndPos.x, topEndPos.y + canvas.scaleFactor * topBorder.GetComponent<RectTransform>().rect.width, topEndPos.z);
        GameObject botBorder = transform.Find("Top Cinematic Border").gameObject;
        botEndPos       = new Vector3(botEndPos.x, botEndPos.y + canvas.scaleFactor * botBorder.GetComponent<RectTransform>().rect.width, botEndPos.z);
        */

        foreach(GameObject pinUp in pinUpWitches) {
            pinUp.GetComponent<PinUp>().Init(GetMatchedWitch(pinUp.GetComponent<PinUp>()));
            pinUp.SetActive(false);
        }
        foreach(GameObject background in backgrounds) {
            background.SetActive(false);
        }

        botCinematicBorder.transform.position = botEndPos.position;
        topCinematicBorder.transform.position = topEndPos.position;
        botCinematicBorder.SetActive(false);
        topCinematicBorder.SetActive(false);
        canvas = GetComponent<Canvas>();
    }
    public void Perform(WitchController witchController, float duration){
        GameObject pinUpWitch = GetMatchedPinUp(witchController).gameObject;
        pinUpWitch.SetActive(true);
        FadeIn();
        pinUpWitch.GetComponent<PinUp>().duration = duration; 
        pinUpWitch.GetComponent<PinUp>().Perform(witchController, pinUpWitch.GetComponent<Image>());
    }
    public void SetOff(GameObject pinUp){
        // Revert the position and color
        pinUp.GetComponent<PinUp>().background.SetActive(false);
        pinUp.SetActive(false);
        FadeOut();
    }
    // Get PinUp based on WitchController
    public GameObject GetMatchedPinUp(WitchController witchController){
        GameObject matchPinUp = null;
        foreach(GameObject pinUp in pinUpWitches) {
            if(pinUp.name.Contains(witchController.playerID.ToString())){
                // <Edit later> Might need to nulify it again in future
                //pinUp.GetComponent<Image>().sprite = witchController.pinUpSprite;
                matchPinUp = pinUp;
                return matchPinUp;
            }
        }
        return matchPinUp;
    }
    public WitchController GetMatchedWitch(PinUp pinUp){
        WitchController matchWitch = null;
        GameObject[] allWitches = GameObject.FindGameObjectsWithTag("Witch");
        foreach(GameObject w in allWitches) {
            if(pinUp.name.Contains(w.GetComponent<WitchController>().playerID.ToString())){
                matchWitch = w.GetComponent<WitchController>();
                return matchWitch;
            }
        }
        return matchWitch;
    }

    void FadeIn(){
        topCinematicBorder.SetActive(true);
        botCinematicBorder.SetActive(true);
        fadeIn = true;
        fadeOut = false;

        botCinematicBorder.transform.position = botEndPos.position;
        topCinematicBorder.transform.position = topEndPos.position;
    }
    void FadeOut(){
        //topCinematicBorder.SetActive(false);
        //botCinematicBorder.SetActive(false);
        fadeIn = false;
        fadeOut = true;
    }

    void LateUpdate(){
        if(fadeIn){
            // Top
            if(topCinematicBorder.transform.position.y > topTargetPos.position.y){
                topCinematicBorder.transform.position -= new Vector3(0f, 5f * canvas.scaleFactor, 0f);
            } else {
                topCinematicBorder.transform.position = topTargetPos.position;
                fadeIn = false;
            }
            // Bot
            if(botCinematicBorder.transform.position.y < botTargetPos.position.y){
                botCinematicBorder.transform.position += new Vector3(0f, 5f * canvas.scaleFactor, 0f);
            } else {
                botCinematicBorder.transform.position = botTargetPos.position;
                fadeIn = false;
            }
        }
        if(fadeOut){
            // Top
            if(topCinematicBorder.transform.position.y < topEndPos.position.y){
                topCinematicBorder.transform.position += new Vector3(0f, 5f * canvas.scaleFactor, 0f);
            } else {
                topCinematicBorder.transform.position = topEndPos.position;
                fadeOut = false;
                topCinematicBorder.SetActive(false);
            }
            // Bot
            if(botCinematicBorder.transform.position.y > botEndPos.position.y){
                botCinematicBorder.transform.position -= new Vector3(0f, 5f * canvas.scaleFactor, 0f);
            } else {
                botCinematicBorder.transform.position = botEndPos.position;
                fadeOut = false;
                botCinematicBorder.SetActive(false);
            }
        }
    }

}
