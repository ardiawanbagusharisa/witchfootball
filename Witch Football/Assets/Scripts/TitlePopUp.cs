using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TitlePopUp : MonoBehaviour
{
    public float duration;
    private TextMeshProUGUI TMPUGUI;

    void Start(){
        //duration = 3f;
        TMPUGUI = GetComponent<TextMeshProUGUI>();
        TMPUGUI.alignment = TextAlignmentOptions.Center; 
    }

    void Update(){
        if(duration <= 0){
            gameObject.SetActive(false);
            Destroy(this.gameObject);
        }
        duration -= Time.deltaTime;
    }
}
