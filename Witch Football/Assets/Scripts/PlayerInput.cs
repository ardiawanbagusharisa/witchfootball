using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput
{
    public enum ID{
        Player1 = 1,
        Player2 = 2,
        Player3 = 3,
        Player4 = 4
    }
    public ID IDPlayerInput;
    public string IDPlayer; // Delete later or above
    public string StartOrPause;
    public string HorizontalMove;
    public string VerticalMove;
    public string PassOrFollow;
    public string ShootOrTackle; // Also confirm
    public string Jump;
    public string LightMagic; // Also back
    public string HeavyMagic;

    public PlayerInput(int index){
        // Index start from 1
        IDPlayerInput   = (ID)index;
        IDPlayer        = index.ToString();
        StartOrPause    = "Start"+index;
        HorizontalMove  = "HorizontalMove"+index;
        VerticalMove    = "VerticalMove"+index;
        PassOrFollow    = "PassOrFollow"+index;
        ShootOrTackle   = "ShootOrTackle"+index;
        Jump            = "Jump"+index;
        LightMagic      = "LightMagic"+index;
        HeavyMagic      = "HeavyMagic"+index;
    }

    public static PlayerInput GetPlayer(int index){
        PlayerInput playerInput = new PlayerInput(index);
        return playerInput;
    }
}
