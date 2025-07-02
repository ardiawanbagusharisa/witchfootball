using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif
public class WitchController : MonoBehaviour
{
    private PlayerInput playerInput;
    public PlayerInput.ID playerID;
    public Witch.WitchClass baseClass;
    public Witch witch;
    public WitchController[] teamMatesWitches;
    public Team.TeamParty teamParty;
    private Rigidbody _rigidbody;
    public bool _possessingBall;
    public bool _isTackling; 
    public GameObject ball; 
    public GameObject ballPosition;
    public Sprite pinUpSprite;
    public Sprite HUDSprite;

    public WitchVoiceManager VoiceManager;
    public WitchSFXManager SFXManager;
    public bool IsFalling;

    public bool ControlAllowed;
    /* public bool ControlAllowed {
        get {
            bool allowed = false;
            Match match = GameObject.FindObjectOfType<Match>();
            if(witch.character.stunnedDuration.empty && match.gamestate == Match.GameState.MatchPlaying){
                allowed = true;
            }
            return allowed;
        }
    }*/
    // Any states
    private float moveInputHorizontal;
    private float moveInputVertical;
    private bool jumpPressed;
    private bool pauseOrResumePressed;
    private bool lightMagicPressed;
    private bool heavyMagicPressed;
    
    // Attacking 
    private bool shootPressed;
    private bool passPressed;
    
    // Defending 
    private bool tacklePressed;
    private bool followPressed;
    private bool followUnpressed;
    
    // Being acted 
    //private bool followActive;
    //private bool gettingTackled;
    //private bool stunned;

    void Start(){
        playerInput = PlayerInput.GetPlayer((int)playerID);
        Init();
    }

    void Init(){
        witch           = InitClass(baseClass); 
        _rigidbody      = GetComponent<Rigidbody>();
        _possessingBall = false;
        _isTackling     = false;
        ControlAllowed  = false;
        ball            = GameObject.FindGameObjectWithTag("Ball");
        ballPosition    = transform.Find("BallPosition").gameObject; 
        VoiceManager    = GetComponent<WitchVoiceManager>();
        SFXManager      = GetComponent<WitchSFXManager>(); 
        IsFalling       = false;
        //DribbleSFXDelay = new CharacterStat(DribbleSFXDelay, DribbleSFXDelay);

        if(teamMatesWitches == null || teamMatesWitches.Length < 1) {
            GameObject[] allWitches = GameObject.FindGameObjectsWithTag("Witch");
            List<WitchController> witchesTemp = new List<WitchController>();
            foreach (GameObject w in allWitches) {
                WitchController witchController = w.GetComponent<WitchController>();
                if(witchController.teamParty == this.teamParty && witchController != this) {
                    witchesTemp.Add(witchController);
                    //Debug.Log("Add " + w.name + " on " + teamParty.ToString() + "as team mate of " + gameObject.name); 
                }
            }
            teamMatesWitches = witchesTemp.ToArray();
        }
    }
    Witch InitClass(Witch.WitchClass baseClass){
        this.baseClass = baseClass;
        if(baseClass == Witch.WitchClass.Sorcerer){
            return WitchBase.Sorcerer;
        }else if(baseClass == Witch.WitchClass.Cleric){
            return WitchBase.Cleric;
        }else if(baseClass == Witch.WitchClass.Wizard){
            return WitchBase.Wizard;
        }else if(baseClass == Witch.WitchClass.Druid){
            return WitchBase.Druid;
        }
        // Class Base
        return WitchBase.Base;
        // Magic skills are included here
    }

    void Update(){
        if (witch.character.stunnedDuration.empty)
        {
            if (ControlAllowed)
            {
                HandleInput();
            }
        }
            CheckPauseOrResume();
        UpdateTimers();
    }

    private void FixedUpdate()
    {
        if (witch.character.stunnedDuration.empty) {
            if (ControlAllowed) {
                MovePhysics();      // Dribble Physics including guard, can be splitted. 
                ActionPhysics();
                MagicPhysics();
                MysteryBoxPhysics();

                ResetInput();
            }
        }
    }

    private void HandleInput() {
        if (Input.GetButtonDown(playerInput.StartOrPause))
            pauseOrResumePressed = true;

        moveInputHorizontal = Input.GetAxis(playerInput.HorizontalMove);
        moveInputVertical = Input.GetAxis(playerInput.VerticalMove);

        if (Input.GetButtonDown(playerInput.Jump))
            jumpPressed = true;
        if (Input.GetButtonDown(playerInput.LightMagic))
            lightMagicPressed = true;
        if (Input.GetButtonDown(playerInput.HeavyMagic))
            heavyMagicPressed = true;

        if (_possessingBall && Input.GetButtonDown(playerInput.ShootOrTackle)) 
            shootPressed = true;
        if (_possessingBall && Input.GetButtonDown(playerInput.PassOrFollow))
            passPressed = true;
        
        if (!_possessingBall && Input.GetButtonDown(playerInput.ShootOrTackle))
            tacklePressed = true;
        if (!_possessingBall && Input.GetButton(playerInput.PassOrFollow))
            followPressed = true;
        if (!_possessingBall && Input.GetButtonUp(playerInput.PassOrFollow))
            followUnpressed = true;
    }

    private void ResetInput() {
        // Reset inputs after each frame to avoid multiple inputs in one frame
        //moveInputHorizontal = 0f;
        //moveInputVertical = 0f;
        jumpPressed = false;
        pauseOrResumePressed = false;
        lightMagicPressed = false;
        heavyMagicPressed = false;
        shootPressed = false;
        passPressed = false;
        tacklePressed = false;
        followPressed = false;
        followUnpressed = false;
    }

    void CheckPauseOrResume(){
        if(pauseOrResumePressed)
        {
            Match match = FindFirstObjectByType<Match>();
            match.PauseOrResume();
            Debug.Log("Start Pressed" + playerInput.StartOrPause.ToString());
        }
    }

    void MovePhysics()
    {
        // Any states 
        //  Move 
        float moveThreshold = 0.2f;
        bool moveThresholdReached = Mathf.Abs(moveInputHorizontal) > moveThreshold || Mathf.Abs(moveInputVertical) > moveThreshold;
        //if (playerID == PlayerInput.ID.Player1)
        //    Debug.Log("Move Input: " + moveInputHorizontal + ", " + moveInputVertical + " | Threshold: " + moveThresholdReached);
        if (moveThresholdReached)
        {
            float angle = Mathf.Atan2(moveInputHorizontal, moveInputVertical);
            angle = Mathf.Rad2Deg * angle;
            Quaternion targetDir = Quaternion.Euler(0, angle, 0);
            transform.localRotation = Quaternion.Slerp(transform.rotation, targetDir, 10 * Time.fixedDeltaTime);
            //Debug.Log("Button pressed: movement");

            Vector3 moveDelta = new Vector3(
                moveInputHorizontal * witch.character.moveSpeed.current * Time.fixedDeltaTime,
                0f,
                moveInputVertical * witch.character.moveSpeed.current * Time.fixedDeltaTime
                );
            _rigidbody.MovePosition(transform.position + moveDelta);
        }

        //  Jump 
        float jumpTrheshold = 0.1f;
        bool jumpThresholdReached = _rigidbody.linearVelocity.y <= jumpTrheshold;
        if (jumpPressed && jumpThresholdReached && witch.character.jumpDelay.full)
        {
            witch.character.jumpDelay.current = 0f;
            SFXManager.Play(SFXManager.Jumping);
            Debug.Log("Button pressed: jump");
            _rigidbody.AddForce(witch.character.jumpForce.current * Vector3.up, ForceMode.Impulse);
        }

        // <edit later> move to updateTimer
        //witch.character.jumpDelay.current = AddTimerToMax(witch.character.jumpDelay.current, witch.character.jumpDelay.max);

        //  Dribble 
        if (_possessingBall) {
            ball.transform.position = ballPosition.transform.position;
            //ball.GetComponent<Rigidbody>().useGravity = false;
        }
            

        // Guard refreshed // <Edit later> move this in ball release instead 
        if (!_possessingBall && (witch.character.guard.available))
        {
            witch.character.guard.current = 0f;
        }

        // Defend
        //  Follow 
        if (followPressed && witch.character.followDelay.full)
        {
            Vector3 ballDir = ball.transform.position - transform.position;
            ballDir.y = 0f;
            transform.rotation = Quaternion.LookRotation(ballDir.normalized);
            Vector3 move = ballDir.normalized * witch.character.moveSpeed.current * Time.fixedDeltaTime;
            _rigidbody.MovePosition(transform.position + move);
            //Debug.Log("Delay " + witch.character.followDelay.current);
            //Debug.Log("Button pressed: follow");
            // <edit later> handle follow move here
        }
        else if (followUnpressed) {
            witch.character.followDelay.current = 0f;
        }
        //else {
        //    witch.character.followDelay.current = AddTimerToMax(witch.character.followDelay.current, witch.character.followDelay.max);
        //}
    }

    void UpdateTimers()
    {   
        witch.character.jumpDelay.current = AddTimerToMax(witch.character.jumpDelay.current, witch.character.jumpDelay.max);

        if (!followPressed) 
            witch.character.followDelay.current = AddTimerToMax(witch.character.followDelay.current, witch.character.followDelay.max);

        witch.character.shootDelay.current = AddTimerToMax(witch.character.shootDelay.current, witch.character.shootDelay.max);
        witch.character.passDelay.current = AddTimerToMax(witch.character.passDelay.current, witch.character.passDelay.max);
        witch.character.tackleDelay.current = AddTimerToMax(witch.character.tackleDelay.current, witch.character.tackleDelay.max);

        // Update time duration and delay from magicskill!
        // When the delay is over, back to original stats 
        witch.character.lightMagicSkill.delay.current = AddTimerToMax(witch.character.lightMagicSkill.delay.current, witch.character.lightMagicSkill.delay.max);
        witch.character.heavyMagicSkill.delay.current = AddTimerToMax(witch.character.heavyMagicSkill.delay.current, witch.character.heavyMagicSkill.delay.max);
        witch.character.lightMagicSkill.duration.current = witch.character.UpdateDurationMagic(witch.character.lightMagicSkill);
        witch.character.heavyMagicSkill.duration.current = witch.character.UpdateDurationMagic(witch.character.heavyMagicSkill);

        if (witch.character.usedMysteryBox != null) {
            if (witch.character.usedMysteryBox.duration > 0 && witch.character.usedMysteryBox.casted)
                witch.character.usedMysteryBox.duration = ReduceDuration(witch.character.usedMysteryBox.duration);
        }

        witch.character.getTackledDelay.current = AddTimerToMax(witch.character.getTackledDelay.current, witch.character.getTackledDelay.max);

        if (!witch.character.stunnedDuration.empty)
            witch.character.stunnedDuration.current = ReduceDuration(witch.character.stunnedDuration.current);
    }


    // <Edit later> Change to static. Add the time to max. 
    // - Flow delay: from max to 0, because we have original value from max, use current to access current value  
    // - Flow duration: stun, mysterybox -> from 0 to max, because initial value is 0 
    public static float AddTimerToMax(float curVal, float maxVal){
        return curVal >= maxVal ? maxVal : (curVal += 1f * Time.deltaTime); 
    }
    //<Edit later> Better use void to avoid the bias. Reduce the time to 0.
    public static float ReduceDuration(float curVal){
        return curVal <= 0 ? 0 : (curVal -= 1f * Time.deltaTime);
    }

    void ActionPhysics()
    {
        // Any states
        // HP Regeneration
        if (witch.character.healthPoint.current <= 0f) // && witch.character.stunnedDuration.empty
        {
            witch.character.healthPoint.current += 5f;
        }

        // Falling <Edit later> all passive actions should be seperated
        if (transform.position.y <= -1 && IsFalling == false)
        {
            SFXManager.Play(SFXManager.Falling);
            VoiceManager.VoicePlayChance(VoiceManager.Falling);
            IsFalling = true;
        }

        //shootRequested = Input.GetButtonDown(playerInput.ShootOrTackle) && _possessingBall && witch.character.shootDelay.full;
        //passRequested = Input.GetButtonDown(playerInput.PassOrFollow) && _possessingBall && witch.character.passDelay.full;
        //tackleRequested = !_possessingBall && ((Input.GetButtonDown(playerInput.ShootOrTackle) && witch.character.tackleDelay.full) || (Input.GetKeyDown(KeyCode.Z) && playerID == PlayerInput.ID.Player1));

        // Attack
        //  Shoot 
        if (shootPressed && witch.character.shootDelay.full) //&& ball != null)
        {
            Debug.Log("Button pressed: shoot");
            ball.transform.localPosition += new Vector3(0f, 0f, 0.4f);
            BallReleasing(ball.transform.position, ball.transform.localRotation, ball.transform.forward, Vector3.zero, Vector3.zero, 2 * witch.character.shootPower.current * transform.forward, true, false);

            VoiceManager.VoicePlayChance(VoiceManager.Shooting);
            SFXManager.Play(SFXManager.Shooting);
            
            witch.character.shootDelay.current = 0f;
        }
        //witch.character.shootDelay.current = AddTimerToMax(witch.character.shootDelay.current, witch.character.shootDelay.max);
        
        //  Pass 
        if (passPressed && witch.character.shootDelay.full) // ball != null
        {
            Debug.Log("Button pressed: pass");
            GameObject closest = GetClosestTeamMate();
            if (closest != null)
            {
                Vector3 dir = closest.transform.position - transform.position;
                dir.y = 0;
                transform.rotation = Quaternion.LookRotation(dir);
            }
            ball.transform.localPosition += new Vector3(0f, 0f, 0.3f);
            BallReleasing(ball.transform.position, ball.transform.localRotation, ball.transform.forward, Vector3.zero, Vector3.zero, 2 * transform.forward * witch.character.passPower.current, false, true);
            
            VoiceManager.VoicePlayChance(VoiceManager.Passing);
            SFXManager.Play(SFXManager.Passing);
            
            witch.character.passDelay.current = 0f;
        }
        //witch.character.passDelay.current = AddTimerToMax(witch.character.passDelay.current, witch.character.passDelay.max);

        //tackleRequested =  witch.character.tackleDelay.full) || (Input.GetKeyDown(KeyCode.Z) && playerID == PlayerInput.ID.Player1));
        // Defend
        //  Tackle 
        if (tacklePressed && witch.character.tackleDelay.full) 
        {
            Debug.Log("Button pressed: tackle");
            witch.character.tackleDelay.current = 0f;
            _isTackling = true;
            VoiceManager.VoicePlayChance(VoiceManager.Tackling);
            SFXManager.Play(SFXManager.Tackling);
            // <Edit later> Increase manna in oncollision 
        }
        else // <Edit later in oncollisionstay> add tackling duration and if tackling succeed, refresh and damaging. use istackling and not tackle delay for tackling 
        {
            _isTackling = false;
        }
        //witch.character.tackleDelay.current = AddTimerToMax(witch.character.tackleDelay.current, witch.character.tackleDelay.max);
    }

    void MagicPhysics(){
        // Light Magic 
        if(lightMagicPressed && witch.character.lightMagicSkill.delay.full && !witch.character.lightMagicSkill.casted)
        {
            Debug.Log("Button pressed: light magic");
            if(witch.character.manna.current >= witch.character.lightMagicSkill.mannaNeed){
                witch.character.CastMagic(witch.character.lightMagicSkill);
                // <Debug>
                //witch.character.manna.current -= witch.character.lightMagicSkill.mannaNeed;
                witch.character.lightMagicSkill.delay.current = 0f;
                
                PinUpController pUController = FindFirstObjectByType<PinUpController>();
                pUController.Perform(this, 0.25f);
                    
                SFXManager.PlayMagicSFX(witch.character.lightMagicSkill);
                VoiceManager.PlayMagicVoice(witch.character.lightMagicSkill);
            } else {
                Debug.Log("Not enough Manna for light magic");
            }
        }

        // Heavy Magic
        if(heavyMagicPressed && witch.character.heavyMagicSkill.delay.full && !witch.character.heavyMagicSkill.casted)
        {
            Debug.Log("Button pressed: heavy magic");
            if(witch.character.manna.current >= witch.character.heavyMagicSkill.mannaNeed){
                witch.character.CastMagic(witch.character.heavyMagicSkill);
                // <Debug>
                //witch.character.manna.current -= witch.character.heavyMagicSkill.mannaNeed;
                witch.character.heavyMagicSkill.delay.current = 0f;
                
                PinUpController pUController = FindFirstObjectByType<PinUpController>();
                pUController.Perform(this, 0.5f);

                SFXManager.PlayMagicSFX(witch.character.heavyMagicSkill);
                VoiceManager.PlayMagicVoice(witch.character.heavyMagicSkill);
            } else {
                Debug.Log("Not enough Manna for heavy magic");
            }
        }

        // Revert when the buff duration is over
        if(witch.character.lightMagicSkill.duration.full && witch.character.lightMagicSkill.casted){
            witch.character.RevertMagic(witch.character.lightMagicSkill);
        }
        if(witch.character.heavyMagicSkill.duration.full && witch.character.heavyMagicSkill.casted){
            witch.character.RevertMagic(witch.character.heavyMagicSkill);
        }
    }

    //void GuardControl(){
    //    //// This is after releasing the ball, so the guard is refreshed.
    //    //if (!_possessingBall && (witch.character.guard.available)){
    //    //    witch.character.guard.current = 0f;
    //    //}
    //    //witch.character.getTackledDelay.current = AddTimerToMax(witch.character.getTackledDelay.current, witch.character.getTackledDelay.max);
        
    //    //if(!witch.character.getTackledDelay.full)
    //    //Debug.Log("GetTackledDelay: " + witch.character.getTackledDelay.current);
    //    // Handle all the tackle and income damage here
    //    // if possessing && if(guard && health > 0)
    //}
    // <Edit Later> tackled(guard, hp, explosiontype, etc)
    void Tackled(float guardReduced, float healthReduced) {
        //Debug.Log("GetTackledDelayRemain: "+character.getTackledDelay.current);
        // Need to check first if its Still in tackled duration (invulnerable).
        // Code below is currently also implemented on tile.cs
        if(_possessingBall){
            // Guard
            if(witch.character.guard.available && witch.character.getTackledDelay.full){
                witch.character.guard.current -= guardReduced;
            }
            if(witch.character.guard.empty) {
                witch.character.guard.current = 0;
                // Do short stun
                witch.character.stunnedDuration.current = 5f;
                Debug.Log("Short stunned! Guard:" + witch.character.guard.current + ", HP:" + witch.character.healthPoint.current);
                //BallReleasing();
                Vector3 startPos = new Vector3(ballPosition.transform.position.x, ballPosition.transform.position.y + 1.5f, ballPosition.transform.position.z); 
                Vector3 vel = ball.transform.forward;
                vel += new Vector3(0,1,0);
                BallReleasing(startPos, ball.transform.localRotation, ball.transform.forward, vel, Vector3.zero, 1 * transform.up * witch.character.shootPower.current);   

                SFXManager.Play(SFXManager.Stunned);
                Ball b = ball.GetComponent<Ball>();
                b.SFXManager.Play(b.SFXManager.Release);
                VoiceManager.VoicePlayChance(VoiceManager.Sad);
            }
            Debug.Log("Tackled when possesses"+_possessingBall);
        } 
        // Move these codes into above block to not let player loss the HP when in guard. 
        // Health Point
        //Debug.Log(character.healthPoint.available + " " + (character.getTackledDelay.current >= character.getTackledDelay.max));
        if(witch.character.healthPoint.available && witch.character.getTackledDelay.full){
            witch.character.healthPoint.current -= healthReduced;
        }
        //Debug.Log("GetTackledDelay: "+character.getTackledDelay.current);
        if(witch.character.healthPoint.empty) {
            witch.character.healthPoint.current = 0;
            witch.character.guard.current       = 0;
            // Do long stun
            witch.character.stunnedDuration.current = 10f;
            Debug.Log("Long stunned! Guard:" + witch.character.guard.current + ", HP:" + witch.character.healthPoint.current);
            if(_possessingBall){
                Vector3 startPos = new Vector3(ballPosition.transform.position.x, ballPosition.transform.position.y + 1.5f, ballPosition.transform.position.z); 
                Vector3 vel = ball.transform.forward;
                vel += new Vector3(0,1,0);
                BallReleasing(startPos, ball.transform.localRotation, ball.transform.forward, vel, Vector3.zero, 1 * transform.up * witch.character.shootPower.current);
                
                //BallReleasing();
                Debug.Log("Ball Released because Long Stun.");
            }

            SFXManager.Play(SFXManager.Stunned);
            Ball b = ball.GetComponent<Ball>();
            b.SFXManager.Play(b.SFXManager.Kicked);
            VoiceManager.VoicePlayChance(VoiceManager.Cry);
        }
        Debug.Log("Guard: "+witch.character.guard.current +" .HP: "+witch.character.healthPoint.current);
        //Debug.Break();
        witch.character.getTackledDelay.current = 0f;
        Debug.Log("GetTackledDelay: "+witch.character.getTackledDelay.current);
        
        SFXManager.Play(SFXManager.Tackled);
        VoiceManager.VoicePlayChance(VoiceManager.Tackled);
    }

    public void Damaged(float guardReduced, float healthReduced){
        Tackled(guardReduced, healthReduced);
    }

    void PlayerIndexControl(int index){
        
    }

    void BallPossessing(){
        // Need condition of When get the ball for the first time to avoid redundancy on guard = 3.  
        // Check if the ball is free
        // ball.transform.rotation = Quaternion.identity; 
        // ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
        
        // If in previous state ball is free, refresh the guard
        if(ball.GetComponent<Ball>().ballState != Ball.BallState.Possessed){// <Edit later> Check in oncollisionenter
            _possessingBall = true;
            witch.character.guard.current    = witch.character.guard.max;
            Debug.Log("Guard: "+witch.character.guard.current);
            ball.GetComponent<Ball>().Possessed(this);
        }
        // <Edit later> Refresh ball velocity and rotation or simply just make the ball as children 
        // <Edit this> in BallFollowing() 
        ball.transform.position = ballPosition.transform.position;
        ball.GetComponent<Rigidbody>().useGravity = false;

        // Change Team State
        Match match = FindFirstObjectByType<Match>();
        if(match != null){
            if(this.teamParty == match.TeamA.teamParty){
                match.TeamA.teamState = Team.TeamState.Offense;
                match.TeamB.teamState = Team.TeamState.Defense;
            } else if (this.teamParty == match.TeamB.teamParty){
                match.TeamA.teamState = Team.TeamState.Defense;
                match.TeamB.teamState = Team.TeamState.Offense;
            }
        }
    }
    //<Edit later> BallReleasing(direction, velocity, angularvelocity, force, startpos)
    public void BallReleasing(Vector3 startPosition, Quaternion rotation, Vector3 direction, Vector3 velocity, Vector3 angularVelocity, Vector3 force, bool shoot = false, bool pass = false){

        if (ball != null) {
            if (shoot || pass)
            {
                Ball ballComponent = ball.GetComponent<Ball>();

                if (witch.character.lightMagicSkill.casted || witch.character.heavyMagicSkill.casted)
                    ballComponent.SFXManager.Play(ballComponent.SFXManager.Raged);
                else
                    ballComponent.SFXManager.Play(ballComponent.SFXManager.Kicked);
            }
        }
        ball.GetComponent<Rigidbody>().useGravity = true;
        _possessingBall = false;
        // stuff before set off parent
        //ball.transform.localPosition += new Vector3(0f, 0f, 0.3f);
        ball.transform.position = startPosition;
        ball.transform.localRotation = rotation;
        //Vector3 targetDir = transform.forward; //-ball.transform.forward;
        //targetDir.y += 0.5f;
        
        ball.GetComponent<Ball>().Released(Ball.BallState.Free);
        //ball.transform.rotation = rotation;
        Rigidbody ballRigidBody = ball.GetComponent<Rigidbody>(); 
        ballRigidBody.linearVelocity  = velocity;
        ballRigidBody.angularVelocity = angularVelocity;
        ballRigidBody.AddForce(force, ForceMode.Impulse);
        //ball.GetComponent<Rigidbody>().velocity = direction;  
        //ball.GetComponent<Rigidbody>().AddForce(targetDir, ForceMode.Impulse);

        Match match = FindFirstObjectByType<Match>();
         if(match != null){
            if(this.teamParty == match.TeamA.teamParty){
                match.TeamA.teamState = Team.TeamState.Defense;
            } else if (this.teamParty == match.TeamB.teamParty){
                match.TeamB.teamState = Team.TeamState.Defense;
            }
        }
    }
    
    GameObject GetClosestTeamMate(){
        GameObject closestTeamMate = null;

        if(teamMatesWitches != null && teamMatesWitches.Length > 0){
            closestTeamMate = teamMatesWitches[0].gameObject;
            float distance  = Vector3.Distance(transform.position, closestTeamMate.transform.position); 
            // <Edit later> Check first if TeamMates > 1
            foreach (WitchController tmw in teamMatesWitches){
               float distanceNext = Vector3.Distance(transform.position, tmw.transform.position);
               if(distance > distanceNext) {
                   distance = distanceNext;
                   closestTeamMate = tmw.gameObject;
               } 
            }
        }
        if(closestTeamMate != null) {
            Debug.Log("Closest Team Mate: "+closestTeamMate.name);
        }
        return closestTeamMate;
    }

    void OnCollisionStay(Collision other) {
        // Passive (GetTackled) <Edit later> should have been Active tackling 
        WitchController tacklingWitch = other.gameObject.GetComponent<WitchController>();
        if(other.gameObject.tag == "Witch" && tacklingWitch != null){
            if(tacklingWitch._isTackling){
                if(witch.character.getTackledDelay.full){
                    Debug.Log(gameObject.name + " tackled by" + other.gameObject.name);
                    Tackled(tacklingWitch.witch.character.damageGuard.current, tacklingWitch.witch.character.damageHealth.current);
                    tacklingWitch.witch.character.AddManna(10f);
                    //witch.character.AddManna(1f);
                    _rigidbody.AddForce(transform.forward + (transform.up * 2f), ForceMode.Impulse);
                    witch.character.getTackledDelay.current = 0f;
                }
            }
        }   
    }
    void OnCollisionEnter(Collision other) {
        // Add Spike here


        // MysteryBox
        if(other.gameObject.tag == "MysteryBox") { 
            if(witch.character.usedMysteryBox == null) {
                MysteryBox mysteryBox = other.gameObject.GetComponent<MysteryBox>();
                mysteryBox.UseEffect(this);
                VoiceManager.PlayMysteryBoxVoice(mysteryBox.type);
                Debug.Log("Taking MysteryBox: " + other.gameObject.name);
            }
        }
        // Rock
        if(other.gameObject.tag == "Rock"){
            if(witch.character.getTackledDelay.full){
                // <Edit later> Need to change the force when the ball is released
                Rock rock = other.gameObject.GetComponent<Rock>();
                Debug.Log(gameObject.name + " damaged by" + rock.gameObject.name);
                Damaged(rock.damageGuard, rock.damageHealth);
                Physics.IgnoreCollision(ball.GetComponent<SphereCollider>(), rock.gameObject.GetComponent<BoxCollider>(), true);
                //transform.rotation = Quaternion.LookRotation(rock.transform.position - transform.position);
                
                if(_possessingBall){
                    Vector3 startPos = new Vector3(ball.transform.position.x, ball.transform.position.y + 1.5f, ball.transform.position.z); 
                    Vector3 vel = ball.transform.forward;
                    vel += new Vector3(0,2,0);
                    BallReleasing(startPos, ball.transform.localRotation, ball.transform.forward, vel, Vector3.zero, 1 * transform.up * witch.character.shootPower.current);
                }                
                //gameObject.GetComponent<Rigidbody>().AddExplosionForce(5f, rock.transform.position, 2f, 2f, ForceMode.Impulse);
                witch.character.getTackledDelay.current = 0f;
                
                SFXManager.Play(SFXManager.Exploding);
                VoiceManager.VoicePlayChance(VoiceManager.Tackled);
            }
        }
        // Possess the ball when touching it, later it can possessed when the ball is Shot and Passed too. and when the velocity is low. 
        // Ball 
        if(other.gameObject == ball && witch.character.stunnedDuration.empty) {
            if(ball.GetComponent<Ball>().ballState == Ball.BallState.Free && witch.character.passDelay.full) {
                // <Edit later> Must be Ballpossessing()
                BallPossessing();
                ball.GetComponent<Ball>().Possessed(this);
                Debug.Log("Possessed by "+gameObject.name);
                // <Edit later> Refresh ball velocity and rotation 

                Ball b = ball.GetComponent<Ball>();
                b.SFXManager.Play(b.SFXManager.Controlled);  
                VoiceManager.VoicePlayChance(VoiceManager.Controlling);              
            }
        }
    }

    void MysteryBoxPhysics(){
        if(witch.character.usedMysteryBox != null){
            //Debug.Log(character.usedMysteryBox.duration);
            if (witch.character.usedMysteryBox.duration <= 0 && witch.character.usedMysteryBox.casted) {
                witch.character.RevertMysteryBox(witch.character.usedMysteryBox);
            }
            
        }
    }
} 