using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    public float lifeTime;
    public float damageGuard;
    public float damageHealth;
    public GameObject smokeEffect;

    void Update(){
        if(lifeTime <= 0f){
            Destroy(gameObject);
        }
        lifeTime -= Time.deltaTime;
    }

    void OnCollisionEnter(Collision other) {
        if(other.gameObject.tag == "Witch"){
            GameObject smoke = GameObject.Instantiate(smokeEffect);
            smoke.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        }
    }
}
