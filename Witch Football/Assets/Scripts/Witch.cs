using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Witch {
    public enum WitchClass{
        None, 
        Sorcerer,
        Cleric, 
        Wizard,
        Druid
    }
    public enum WitchType{
        None, 
        Striker,
        Defender
    }
    public WitchClass witchClass;
    public WitchType witchType;
    public Character character; 
    public Witch(){
        witchClass  = WitchClass.None;
        witchType   = WitchType.None;
        character   = new Character();
    }
    public Witch(Witch witchBase){
        witchClass = witchBase.witchClass;
        witchType = witchBase.witchType;
        character = witchBase.character;
    }
}

public static class WitchBase {
    // When creating a witch, need to assign Class, Type, Character, and MagicSkills
    public static Witch Base{
        get {
            Witch witch         = new Witch();
            witch.witchClass    = Witch.WitchClass.None;
            witch.witchType     = Witch.WitchType.None;
            
            //witch.character     = new Character();
            witch.character.healthPoint     = new CharacterStat(10f, 10f);
            witch.character.jumpForce       = new CharacterStat(5f, 5f); 
            witch.character.jumpDelay       = new CharacterStat(5f, 5f);
            witch.character.moveSpeed       = new CharacterStat(2f, 2f); 
            witch.character.shootDelay      = new CharacterStat(2f, 2f);
            witch.character.passDelay       = new CharacterStat(0.5f, 0.5f);
            witch.character.shootPower      = new CharacterStat(5f, 5f);
            witch.character.passPower       = new CharacterStat(3f, 3f);
            witch.character.guard           = new CharacterStat(0f, 2f); 
            witch.character.tackleDelay     = new CharacterStat(1f, 1f); 
            witch.character.manna           = new CharacterStat(100f, 100f);
            witch.character.followDelay     = new CharacterStat(1f, 1f);
            witch.character.getTackledDelay = new CharacterStat(1.5f, 1.5f);
            witch.character.damageGuard     = new CharacterStat(1f, 1f);
            witch.character.damageHealth    = new CharacterStat(1f, 1f);
            witch.character.stunnedDuration = new CharacterStat(0f, 10f);

            witch.character.lightMagicSkill = new MagicSkill();
            witch.character.heavyMagicSkill = new MagicSkill();
            return witch;
        }
    }

    public static Witch Sorcerer{
        get {
            Witch witch         = new Witch();
            witch.witchClass    = Witch.WitchClass.Sorcerer;
            witch.witchType     = Witch.WitchType.Defender;
            
            //witch.character     = new Character();
            witch.character.healthPoint     = new CharacterStat(11f, 11f);
            witch.character.jumpForce       = new CharacterStat(5f, 5f); 
            witch.character.jumpDelay       = new CharacterStat(5f, 5f);
            witch.character.moveSpeed       = new CharacterStat(2f, 2f); 
            witch.character.shootDelay      = new CharacterStat(2f, 2f);
            witch.character.passDelay       = new CharacterStat(0.5f, 0.5f);
            witch.character.shootPower      = new CharacterStat(6f, 6f);
            witch.character.passPower       = new CharacterStat(3.5f, 3.5f);
            witch.character.guard           = new CharacterStat(0f, 4f); 
            witch.character.tackleDelay     = new CharacterStat(1f, 1f); 
            witch.character.manna           = new CharacterStat(100f, 100f);
            witch.character.followDelay     = new CharacterStat(1f, 1f);
            witch.character.getTackledDelay = new CharacterStat(1.5f, 1.5f);
            witch.character.damageGuard     = new CharacterStat(1f, 1f);
            witch.character.damageHealth    = new CharacterStat(1f, 1f);
            witch.character.stunnedDuration = new CharacterStat(0f, 10f);

            witch.character.lightMagicSkill = DefaultMagicSkill.LightHeal;
            witch.character.heavyMagicSkill = DefaultMagicSkill.HeavyHeal; 
            return witch;
        }
    }

    public static Witch Cleric{
        get {
            Witch witch         = new Witch();
            witch.witchClass    = Witch.WitchClass.Cleric;
            witch.witchType     = Witch.WitchType.Defender;
            
            //witch.character     = new Character();
            witch.character.healthPoint     = new CharacterStat(12f, 12f);
            witch.character.jumpForce       = new CharacterStat(6f, 6f); 
            witch.character.jumpDelay       = new CharacterStat(3f, 3f);
            witch.character.moveSpeed       = new CharacterStat(2.5f, 2.5f); 
            witch.character.shootDelay      = new CharacterStat(1f, 1f);
            witch.character.passDelay       = new CharacterStat(0.3f, 0.3f);
            witch.character.shootPower      = new CharacterStat(6f, 6f);
            witch.character.passPower       = new CharacterStat(3.5f, 3.5f);
            witch.character.guard           = new CharacterStat(0f, 3f); 
            witch.character.tackleDelay     = new CharacterStat(1f, 1f); 
            witch.character.manna           = new CharacterStat(100f, 100f);
            witch.character.followDelay     = new CharacterStat(1f, 1f);
            witch.character.getTackledDelay = new CharacterStat(1.5f, 1.5f);
            witch.character.damageGuard     = new CharacterStat(1f, 1f);
            witch.character.damageHealth    = new CharacterStat(1f, 1f);
            witch.character.stunnedDuration = new CharacterStat(0f, 10f);

            witch.character.lightMagicSkill = DefaultMagicSkill.LightHeal;
            witch.character.heavyMagicSkill = DefaultMagicSkill.HeavyHeal; 
            return witch;
        }
    }

    public static Witch Wizard{
        get {
            Witch witch         = new Witch();
            witch.witchClass    = Witch.WitchClass.Wizard;
            witch.witchType     = Witch.WitchType.Striker;
            
            //witch.character     = new Character();
            witch.character.healthPoint     = new CharacterStat(10f, 10f);
            witch.character.jumpForce       = new CharacterStat(5f, 5f); 
            witch.character.jumpDelay       = new CharacterStat(3f, 3f);
            witch.character.moveSpeed       = new CharacterStat(2f, 2f); 
            witch.character.shootDelay      = new CharacterStat(2f, 2f);
            witch.character.passDelay       = new CharacterStat(0.5f, 0.5f);
            witch.character.shootPower      = new CharacterStat(7f, 7f);
            witch.character.passPower       = new CharacterStat(4f, 4f);
            witch.character.guard           = new CharacterStat(0f, 3f); 
            witch.character.tackleDelay     = new CharacterStat(0.75f, 0.75f); 
            witch.character.manna           = new CharacterStat(100f, 100f);
            witch.character.followDelay     = new CharacterStat(1f, 1f);
            witch.character.getTackledDelay = new CharacterStat(1.5f, 1.5f);
            witch.character.damageGuard     = new CharacterStat(1f, 1f);
            witch.character.damageHealth    = new CharacterStat(1f, 1f);
            witch.character.stunnedDuration = new CharacterStat(0f, 8f);

            witch.character.lightMagicSkill = DefaultMagicSkill.LightRage;
            witch.character.heavyMagicSkill = DefaultMagicSkill.HeavyRage; 
            return witch;
        }
    }

    public static Witch Druid{
        get {
            Witch witch         = new Witch();
            witch.witchClass    = Witch.WitchClass.Druid;
            witch.witchType     = Witch.WitchType.Striker;
            
            //witch.character     = new Character();
            witch.character.healthPoint     = new CharacterStat(10f, 10f);
            witch.character.jumpForce       = new CharacterStat(6f, 6f); 
            witch.character.jumpDelay       = new CharacterStat(2f, 2f);
            witch.character.moveSpeed       = new CharacterStat(2.5f, 2.5f); 
            witch.character.shootDelay      = new CharacterStat(2f, 2f);
            witch.character.passDelay       = new CharacterStat(0.5f, 0.5f);
            witch.character.shootPower      = new CharacterStat(5.5f, 5.5f);
            witch.character.passPower       = new CharacterStat(3.5f, 3.5f);
            witch.character.guard           = new CharacterStat(0f, 2f); 
            witch.character.tackleDelay     = new CharacterStat(0.75f, 0.75f); 
            witch.character.manna           = new CharacterStat(100f, 100f);
            witch.character.followDelay     = new CharacterStat(0.75f, 0.75f);
            witch.character.getTackledDelay = new CharacterStat(1.5f, 1.5f);
            witch.character.damageGuard     = new CharacterStat(1f, 1f);
            witch.character.damageHealth    = new CharacterStat(1f, 1f);
            witch.character.stunnedDuration = new CharacterStat(0f, 8f);

            witch.character.lightMagicSkill = DefaultMagicSkill.LightBoost; 
            witch.character.heavyMagicSkill = DefaultMagicSkill.HeavyBoost; 
            return witch;
        }
    }
}

