using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smoke : MonoBehaviour
{
    public float fadeInSpeed;
    public float fadeOutSpeed;
    public float scaleSpeed;
    private SpriteRenderer spriteRenderer;
    private bool _fadeIn = true;
    void Awake() {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.color = new Color(1f, 1f, 1f, 0f);
    }
    void Start()
    {
        
    }

    void LateUpdate() {
        if(_fadeIn){
            if(spriteRenderer.color.a < 1) {
                spriteRenderer.color += new Color(0f, 0f, 0f, fadeInSpeed * Time.deltaTime);
            }else {
                _fadeIn = false;
            }
        } else if (!_fadeIn){
            if(spriteRenderer.color.a > 0) {
                spriteRenderer.color -= new Color(0f, 0f, 0f, fadeOutSpeed * Time.deltaTime);
            }else {
                GameObject.Destroy(gameObject);
            } 
        }
        transform.localScale += (Vector3.one * scaleSpeed * Time.deltaTime); 
    }
}
