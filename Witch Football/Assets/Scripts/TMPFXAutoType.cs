using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TMPFXAutoType : MonoBehaviour
{
    public float speed = 0.05f;
        private TMP_Text _textMeshPro;

    void Awake(){
        _textMeshPro = GetComponent<TMP_Text>();
        _textMeshPro.textWrappingMode = TextWrappingModes.Normal;
        _textMeshPro.alignment = TextAlignmentOptions.Top;
    }

    IEnumerator Start(){
        _textMeshPro.ForceMeshUpdate();
        int totalVisibleCharacters = _textMeshPro.textInfo.characterCount;
        int counter = 0;
        int visibleCount = 0;

        while(true){
            visibleCount = counter % (totalVisibleCharacters + 1);
            _textMeshPro.maxVisibleCharacters = visibleCount;
            
            // Once the last character has been revealed, wait some seconds and start over. 
            if(visibleCount >= totalVisibleCharacters){
                yield return new WaitForSeconds(2f);
                //Destroy(this.gameObject);
                // Change Text here 
            }

            counter += 1;
            yield return new WaitForSeconds(speed);
        }

        // Done revealing the text. 
    }
}
