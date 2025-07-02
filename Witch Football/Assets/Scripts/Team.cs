using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Team 
{
    public enum TeamParty {
        Unassigned, 
        TeamA, 
        TeamB
    }
    public TeamParty teamParty;
    public enum TeamState { 
        Offense,
        Defense 
    }
    public TeamState teamState;

    public int Score;
    public WitchController[] witches;
    public Team(TeamParty teamParty){
        this.teamState  = TeamState.Defense;
        this.teamParty  = teamParty;
        Score = 0;
        WitchController[] allWitches = GameObject.FindObjectsByType<WitchController>(FindObjectsSortMode.None); //GameObject.FindObjectsOfType<WitchController>();
        List<WitchController> witchesTemp = new List<WitchController>();
        foreach (WitchController w in allWitches) {
            if(w.teamParty == this.teamParty) {
                witchesTemp.Add(w);
                //Debug.Log("Add " + w.name + " on " + teamParty.ToString()); 
            }
        }
        witches = witchesTemp.ToArray();
    }
}
