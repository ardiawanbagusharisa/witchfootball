using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MysteryBox : MonoBehaviour
{
    public MagicSkill.AffectTo affectTo;
    public MagicSkill.Type type;
    public StatModifier statModifier;
    public float duration;
    public bool casted;

    // <Edit later> Change this with StatsModifier
    public float modifierDamage;
    public float modifierHealthPoint;
    public float modifierPower;
    public float modifierMoveSpeed;

    public void Init(MagicSkill.Type type, MagicSkill.AffectTo affectTo, StatModifier modifier, float duration){
        this.affectTo    = affectTo;
        this.type        = type;
        this.statModifier = modifier;
        this.duration   = duration;
        this.casted     = false;
    }
    
    public void Init(float duration, float damage, float healthPoint, float power, float moveSpeed){
        // Doesn't need to init affectTo and type.
        this.duration = duration;
        this.casted = false;
        statModifier = new StatModifier(damage, healthPoint, power, moveSpeed);
    }
    public void Start(){
        // <Edit later> assign the static variables here from the default class for statmodifier

        // <Edit later> Assign statmodifier here. 
        Init(this.duration, this.modifierDamage, this.modifierHealthPoint, this.modifierPower, this.modifierMoveSpeed);
    }

    public void UseEffect(WitchController casterWitch){
        gameObject.GetComponent<BoxCollider>().enabled = false;
        Destroy(gameObject.GetComponent<Rigidbody>());
        transform.SetParent(casterWitch.transform, false);
        transform.localRotation = Quaternion.identity;
        transform.localPosition = new Vector3(0f, 0.5f, 0f);

        // Every affecto need to check the null first
        // <Edit later>Assign all witches and team here is better

        // Need to check if the player's box is null or not, and if its casting still or not
        if(affectTo == MagicSkill.AffectTo.Self){
            if(casterWitch.witch.character.usedMysteryBox == null) {
                casterWitch.witch.character.CastMysteryBox(this);
                Debug.Log("Casting MysteryBox" + this.gameObject.name + " on " + "" + casterWitch.gameObject.name);
            } else {
                Debug.Log("Casting MysteryBox Failed.");
            }
        } else if (affectTo == MagicSkill.AffectTo.OneTeamMate){
            // Randomly cast on one team mate, if not casting, the apply
            if(casterWitch.teamMatesWitches.Length >= 1){
                int r = Random.Range(0, casterWitch.teamMatesWitches.Length);
                GameObject teamMate = casterWitch.teamMatesWitches[r].gameObject;
                if(teamMate.GetComponent<WitchController>().witch.character.usedMysteryBox == null){ // casted?
                    teamMate.GetComponent<WitchController>().witch.character.CastMysteryBox(this);
                    Debug.Log("Casting MysteryBox" + this.gameObject.name + " on " + "" + teamMate.name);
                } else {
                    Debug.Log("Casting MysteryBox Failed.");
                }
            } else {
                Debug.Log("Casting MysteryBox Failed. Has no team mates.");
            }
        } else if (affectTo == MagicSkill.AffectTo.AllTeamMates){
            
        } else if (affectTo == MagicSkill.AffectTo.Team){
            GameObject[] allWitches = GameObject.FindGameObjectsWithTag("Witch");
            foreach (GameObject w in allWitches)
            {
                if(w.GetComponent<WitchController>().teamParty == casterWitch.teamParty){
                    if(w.GetComponent<WitchController>().witch.character.usedMysteryBox == null){ //casted?
                        w.GetComponent<WitchController>().witch.character.CastMysteryBox(this);
                        Debug.Log("Casting MysteryBox" + this.gameObject.name + " on " + "" + w.name);
                    } else {
                        Debug.Log("Casting MysteryBox Failed.");
                    }
                }
            }
        } else if (affectTo == MagicSkill.AffectTo.OneOpponent){
            GameObject[] allWitches = GameObject.FindGameObjectsWithTag("Witch");
            List<GameObject> allOpponents = new List<GameObject>();
            foreach (GameObject w in allWitches)
            {
                if(w.GetComponent<WitchController>().teamParty != casterWitch.teamParty){
                    allOpponents.Add(w);
                }    
            }
            if(allOpponents.Count >= 1){
                int r = Random.Range(0, allOpponents.Count);
                if(allOpponents[r].GetComponent<WitchController>().witch.character.usedMysteryBox == null){ //casted?
                    allOpponents[r].GetComponent<WitchController>().witch.character.CastMysteryBox(this);
                    Debug.Log("Casting MysteryBox" + this.gameObject.name + " on " + "" + allOpponents[r].name);
                } else {
                    Debug.Log("Casting MysteryBox Failed.");
                }
            } else {
                Debug.Log("Casting MysteryBox Failed. Has no team mates.");
            }
        } else if (affectTo == MagicSkill.AffectTo.AllOpponents){
            
        } else if (affectTo == MagicSkill.AffectTo.AllOtherCharacters){
            GameObject[] allWitches = GameObject.FindGameObjectsWithTag("Witch");
            List<GameObject> allOpponents = new List<GameObject>();
            foreach (GameObject w in allWitches)
            {
                if(w.GetComponent<WitchController>().teamParty != casterWitch.teamParty){
                    allOpponents.Add(w);
                }    
            }
            if(allOpponents.Count >= 1){
                foreach (GameObject w in allOpponents)
                {
                    if(w.GetComponent<WitchController>().teamParty != casterWitch.teamParty){
                        if(w.GetComponent<WitchController>().witch.character.usedMysteryBox == null){ //casted?
                            w.GetComponent<WitchController>().witch.character.CastMysteryBox(this);
                            Debug.Log("Casting MysteryBox" + this.gameObject.name + " on " + "" + w.name);
                        } else {
                            Debug.Log("Casting MysteryBox Failed.");
                        }
                    }
                }
            } else {
                Debug.Log("Casting MysteryBox Failed. Has no team mates.");
            }
        }
    }
}

// <Edit later> Delete this or Change Mysterybox class into non monobehaviour class
public static class MysteryBoxDefault{
    
    private static MagicSkill.Type type; 
    private static MagicSkill.AffectTo affectTo;
    private static float duration;
    private static float damage;
    private static float healthPoint;
    private static float power;
    private static float moveSpeed;
    static StatModifier statModifier;
    
    
    public static MysteryBox HealthPotion{
        get {
            MysteryBox m = new MysteryBox();
            m.type = MagicSkill.Type.HealthPoint;
            m.affectTo = MagicSkill.AffectTo.Self;
            m.duration = 5f;
            m.statModifier = new StatModifier(0f, 5f, 0f, 0f);
            return m;
        }
    }
    public static MysteryBox GetPotion(MagicSkill.Type type, MagicSkill.AffectTo affectTo, float duration){
        MysteryBox m = new MysteryBox();
        m.type = type;
        m.affectTo = affectTo;
        m.duration = duration;
        m.statModifier = new StatModifier(0f, 5f, 0f, 0f);
        return m;
    }
}
