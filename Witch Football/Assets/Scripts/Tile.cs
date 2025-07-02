using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public enum TileType{
        None, 
        Normal,
        Spiky, 
        Exploding,
        Shooting, 
        MysteryBox,
        FallingRock
    }
    public TileType tileType;
    public enum TileEffect{
        None, 
        RisingUP,
        Invisible 
    } 
    public TileEffect tileEffect;
    public enum TriggerType{
        None,
        Collide,
        Distance
    }
    public TriggerType triggerType;
    public enum SeasonType{
        None,
        Spring,
        Autumn,
        Winter,
        Summer
    }
    public SeasonType season;
    public GridPosition gridPos;
    // <Edit later> make a CharacterStat variable rather than these. Need a custom inspector or 
    // just simply pass this variable's value to the characterstat variable.
    public float triggerDistance;
    public float effectMaxDuration;
    public float effectMaxDelay;
    private float _effectDuration; 
    private float _effectDelay;
    public float typeMaxDuration;
    public float typeMaxDelay;
    public GameObject[] mysteryBoxes;
    public GameObject[] rocks;
    public GameObject[] smokes;
    public GameObject canon;
    private float _typeDuration;
    private float _typeDelay;
    private bool _effectPerformed; 
    private bool _typePerformed;
    private List<GameObject> _collidedWitches;
    private List<GameObject> _onRangeWitches;
    //private GameObject souroff;
    //private Transform trap;
    public TileSFXManager SFXManager;

    void Start() {
        //MaxDuration = 2f;
        //MaxDelay = 5f;
        _collidedWitches    = new List<GameObject>();
        _onRangeWitches     = new List<GameObject>();
        SFXManager          = GetComponent<TileSFXManager>();
        //souroff = transform.Find("Souroff").gameObject;
        //trap = transform.Find("Trap");
        OnRangeTrigger();
        //Transform trap = transform.Find("Trap");
        //Debug.Log(gameObject.name+" has children: "+transform.childCount);
    }
    public Tile(){
        tileType    = TileType.Normal;
        tileEffect  = TileEffect.None;
        triggerType = TriggerType.None;
        season      = SeasonType.None;
        gridPos     = new GridPosition(0,0,0);
        triggerDistance     = 0f;
        _effectDuration     = 0f;
        _effectDelay        = 0f;
        _effectPerformed    = false;
        _typePerformed      = false;
        effectMaxDelay      = 0f;
        effectMaxDuration   = 0f;
        _typeDuration       = 0f;
        _typeDelay          = 0f;
        typeMaxDelay        = 0f;
        typeMaxDuration     = 0f;
    }
    public Tile(TileType tileType, TileEffect tileEffect, TriggerType triggerType, GridPosition gridPos, 
                float triggerDistance, float maxDuration, float maxDelay, float typeDuration, float typeMaxDuration, float typeDelay, float typeMaxDelay){
        this.tileType    = tileType;
        this.tileEffect  = tileEffect;
        this.triggerType = triggerType;
        this.gridPos     = new GridPosition(gridPos);
        this.triggerDistance    = triggerDistance;
        this._effectDuration    = 0f;
        this._effectDelay       = 0f;
        this._effectPerformed   = false;
        this._typePerformed     = false;
        this.effectMaxDelay     = _effectDelay;
        this.effectMaxDuration  = _effectDuration;
        this._typeDuration      = typeDuration;
        this._typeDelay         = typeDelay;
        this.typeMaxDelay       = typeMaxDelay;
        this.typeMaxDuration    = typeMaxDuration;
    }
    //<Edit later>
    public void Randomized(){
        // Exclusive parameters. Ignore the None type.
        int r       = Random.Range(2,7);
        tileType    = (TileType) r; 
        // Adding the children here
    }
    //<Edit later> perform animation
    public void PerformType(){
        if(tileType == TileType.None){}
        else if(tileType == TileType.Normal){}
        else if(tileType == TileType.Exploding){
            if(_onRangeWitches.Count > 0 && triggerType == TriggerType.Distance) {
               foreach (GameObject cw in _onRangeWitches){
                    WitchController wc = cw.GetComponent<WitchController>();
                    // <Edit later>
                    if(wc.witch.character.getTackledDelay.full){
                    //if(wc.witch.character.getTackledDelay.current >= wc.witch.character.getTackledDelay.max){
                        // Do damage here. Move code below to here  
                        cw.GetComponent<WitchController>().Damaged(3f, 3f); // Make a variable to hold this
                        cw.transform.rotation = Quaternion.LookRotation(transform.position - cw.transform.position);
                        cw.GetComponent<Rigidbody>().AddExplosionForce(7f, transform.position, triggerDistance * 2f, 3f, ForceMode.Impulse);
                        Debug.Log("Collide with:" + cw.name + " direction:" + cw.transform.rotation);  
                        Camera.main.GetComponent<CameraShake>().ShakeCamera(0.5f, 2f);
                    }
                    
                    
                    // Need to check the ball possession 
                    // Uncomment this if we want to make the ball unpossessed by the player after explosion
                    //if(_collideWith.GetComponent<WitchController>()._possessingBall){}

                    // <Edit later> move to witch
                    GameObject ball = GameObject.Find("Ball");
                    if(ball.GetComponent<Ball>().ballState == Ball.BallState.Free){
                        ball.transform.rotation = Quaternion.LookRotation(transform.position - ball.transform.position);
                        ball.GetComponent<Rigidbody>().AddExplosionForce(7f, transform.position, triggerDistance * 2f, 3f, ForceMode.Impulse);
                    }
               } 
            }
            if(_collidedWitches.Count > 0 && triggerType == TriggerType.Collide){
                // <Edit later> move explosion damage to witch
                // <Doing> Change the single object with the all collided objects
                foreach (GameObject cw in _collidedWitches){
                    //<Edit later>
                    WitchController wc = cw.GetComponent<WitchController>();
                    if(wc.witch.character.getTackledDelay.full) {
                    //if(wc.witch.character.getTackledDelay.current >= wc.witch.character.getTackledDelay.max){
                        // Do damage here. Move code below to here  
                        cw.GetComponent<WitchController>().Damaged(3f, 3f); // Make a variable to hold this
                        cw.transform.rotation = Quaternion.LookRotation(transform.position - cw.transform.position);
                        cw.GetComponent<Rigidbody>().AddExplosionForce(7f, transform.position, 2f, 3f, ForceMode.Impulse);
                        Debug.Log("Collide with:" + cw.name + " direction:" + cw.transform.rotation);  
                        Camera.main.GetComponent<CameraShake>().ShakeCamera(0.5f, 2f);
                    }
                    
                
                    // Need to check the ball possession 
                    // Uncomment this if we want to make the ball unpossessed by the player after explosion
                    //if(_collideWith.GetComponent<WitchController>()._possessingBall){}

                    // <Edit later> move to witch
                    GameObject ball = GameObject.Find("Ball");
                    if(ball.GetComponent<Ball>().ballState == Ball.BallState.Free){
                        ball.transform.rotation = Quaternion.LookRotation(transform.position - ball.transform.position);
                        ball.GetComponent<Rigidbody>().AddExplosionForce(7f, transform.position, 2f, 3f, ForceMode.Impulse);
                    }
                }
            }
            
            if(smokes != null && smokes.Length > 0){
                GameObject smoke = GameObject.Instantiate(smokes[0]);
                smoke.transform.position = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
            }

            AudioSource.PlayClipAtPoint(SFXManager.Explode, transform.position);                
            //if(triggerType != TriggerType.None) {
                Destroy(this.gameObject);
            //}
        }
        else if(tileType == TileType.MysteryBox){
            if(_typeDelay <= 0f) {
                // Spawn mystery box prefabs based on random
                if(mysteryBoxes != null || mysteryBoxes.Length >= 0){
                    int r = Random.Range(0, mysteryBoxes.Length);
                    GameObject mysteryBox = GameObject.Instantiate(mysteryBoxes[r]);
                    Transform rootTiles = GameObject.Find("Tiles").transform;
                    mysteryBox.transform.position = new Vector3(transform.position.x, transform.position.y + 4, transform.position.z);
                    
                    _typeDelay = typeMaxDelay;
                    Debug.Log("Spawn " + mysteryBox.name);

                    SFXManager.Play(SFXManager.MysteryBox);
                }  
            }
            _typeDelay -= Time.deltaTime;
        }
        else if(tileType == TileType.Shooting){
            // Shoot random nearby witch; ignoring the trigger type (distance and collide can work)
            // <Edit later> Shooting to random witch but the closest witch get 50% higher chance of getting shot
            if(_typeDelay <= 0){
                if(_onRangeWitches.Count > 0) {
                    // Prepare shooter
                    GameObject projectile = GameObject.Instantiate(canon); 
                    projectile.transform.position = new Vector3(transform.position.x, transform.position.y + 2.5f, transform.position.z);
                    // <Edit later> moveup the shooter if the duration is available
                    
                    // Random nearby target
                    int r = Random.Range(0, _onRangeWitches.Count);
                    projectile.transform.LookAt(_onRangeWitches[r].transform);
                    //Debug.Log("Onrangedwitches: "+_onRangeWitches.Count);
                    // <Delete later> just to check the shooting
                    //canon.GetComponent<BoxCollider>().enabled = false;
                    projectile.GetComponent<Rigidbody>().AddForce(projectile.transform.forward * 10f, ForceMode.Impulse);
                    _typeDelay = typeMaxDelay;

                    SFXManager.Play(SFXManager.Shooter);
                }
            }
            _typeDelay -= Time.deltaTime;
        }
        else if(tileType == TileType.Spiky){
            // Only impact the CollidedWitches
            Transform trap = transform.Find("Trap");
            if(!_typePerformed){
                if(_typeDelay < typeMaxDelay) {
                    _typeDelay += Time.deltaTime;
                } else { // delay full
                    trap.gameObject.SetActive(true);
                    _typeDuration = typeMaxDuration;
                    _typePerformed = true;

                    SFXManager.Play(SFXManager.SpikySlash);
                }
            } else { //typePerformed
                if(_typeDuration > 0) {
                    _typeDuration -= Time.deltaTime;
                    //if(trap.gameObject.activeSelf) {
                      //  trap.gameObject.SetActive(true);
                    //}
                    foreach (GameObject w in _collidedWitches){
                    WitchController wc = w.GetComponent<WitchController>();
                    if(wc.witch.character.getTackledDelay.full){
                    //if(wc.witch.character.getTackledDelay.current >= wc.witch.character.getTackledDelay.max){
                        w.transform.rotation = Quaternion.LookRotation(w.transform.position - transform.position);
                        w.GetComponent<Rigidbody>().AddExplosionForce(5f,transform.position, 2f, 2f, ForceMode.Impulse);
                        wc.Damaged(1f, 1f);
                        Debug.Log(gameObject.name + " collide with " + w.name + " direction:" + w.transform.rotation);
                        GameObject b = GameObject.FindWithTag("Ball");
                        // <Edit later> Add force to the ball either before or after the witch rotation
                        if(wc.witch.character.guard.empty || wc.witch.character.healthPoint.empty){
                                //b.transform.position = w.transform.Find("BallPosition").position;
                                
                                // Release the ball <Edit later perform this via witchcontroller>
                                wc.BallReleasing(b.transform.position, b.transform.localRotation, b.transform.forward, Vector3.zero, Vector3.zero, 2 * transform.forward * wc.witch.character.passPower.current, false, false);

                                //SFXManager.Play(SFXManager.Passing);
                            }
                        }
                    }
                } 
                else { // duration is empty
                    trap.gameObject.SetActive(false);   
                    _typeDelay = 0f; 
                    _typePerformed = false;

                    SFXManager.Play(SFXManager.Invisible);
                }
            }
            Debug.Log("delay, duration" + _typeDelay + " " + _typeDuration);
            // If(collided witch is 0, immediatelly _typeperform false)
        }
        else if (tileType == TileType.FallingRock) {
            if(_typeDelay <= 0f) {
                // Spawn rock randomly, <1> Small rock, <2> Big rock
                int r = Random.Range(0, rocks.Length);
                GameObject rock = GameObject.Instantiate(rocks[r]);
                rock.transform.position = new Vector3(transform.position.x, transform.position.y + 4, transform.position.z);
                _typeDelay = typeMaxDelay;

                SFXManager.Play(SFXManager.FallingRock);
            }
            _typeDelay -= Time.deltaTime;
        }
    }
    
    public void PerformEffect(){
        if(tileEffect == TileEffect.None){}
        else if(tileEffect == TileEffect.RisingUP){
            //<Edit later>  Change the static Y value. Add effect speed. 
            if(_effectPerformed){
                if(transform.position.y >= 1){
                    transform.position = new Vector3 (transform.position.x, 1, transform.position.z);
                    _effectDuration = _effectDuration + 1 * Time.deltaTime;
                    if(_effectDuration > effectMaxDuration){
                        _effectPerformed = false;
                        _effectDuration = 0;
                    }
                } else {
                    transform.position = new Vector3(transform.position.x, 
                                                        transform.position.y + 1 * Time.deltaTime, 
                                                        transform.position.z);
                    
                    SFXManager.PlaySafe(SFXManager.Rise);
                }
            } 
            if(!_effectPerformed){
                if(transform.position.y <= 0){
                    transform.position = new Vector3 (transform.position.x, 0, transform.position.z);
                    _effectDelay = _effectDelay + 1 * Time.deltaTime;
                    if(_effectDelay > effectMaxDelay){
                        _effectPerformed = true;
                        _effectDelay    = 0f;
                    }
                } else {
                    transform.position = new Vector3(transform.position.x, 
                                                        transform.position.y - 1 * Time.deltaTime, 
                                                        transform.position.z);
                    SFXManager.PlaySafe(SFXManager.Fall);
                }
            }    
        }
        else if(tileEffect == TileEffect.Invisible){
            if(_effectPerformed){
                if(gameObject.GetComponent<MeshRenderer>().enabled){
                    gameObject.GetComponent<MeshRenderer>().enabled = false;  
                    gameObject.GetComponent<BoxCollider>().enabled  = false;

                    //SFXManager.Play(SFXManager.Invisible);   
                } else {
                    _effectDuration = _effectDuration + 1 * Time.deltaTime;
                    if(_effectDuration > effectMaxDuration){
                        _effectPerformed = false;
                        _effectDuration = 0;
                        gameObject.GetComponent<MeshRenderer>().enabled = true;
                        gameObject.GetComponent<BoxCollider>().enabled  = true;

                        SFXManager.Play(SFXManager.Reveal);
                    }
                }
            }
            if(!_effectPerformed){
                if(gameObject.GetComponent<MeshRenderer>().enabled){
                    _effectDelay = _effectDelay + 1 * Time.deltaTime;
                    if(_effectDelay > effectMaxDelay){
                        _effectPerformed = true;
                        _effectDelay    = 0f;
                        gameObject.GetComponent<MeshRenderer>().enabled = false;
                        gameObject.GetComponent<BoxCollider>().enabled  = false;

                        SFXManager.Play(SFXManager.Invisible);
                    }
                } else {
                    gameObject.GetComponent<MeshRenderer>().enabled = true;
                    gameObject.GetComponent<BoxCollider>().enabled  = true;

                    //SFXManager.Play(SFXManager.Reveal);
                }
            }       
        }
    }
    public bool ValidateTrigger(){
        if(triggerType == TriggerType.None) {
            return true;
        } else if (triggerType == TriggerType.Distance) {
            return (_onRangeWitches.Count >= 1);
        } else if (triggerType == TriggerType.Collide) {
            return (_collidedWitches.Count >= 1);
        }
        return false;
    }
    void Update(){
        if(ValidateTrigger()){
            PerformEffect();
            PerformType();
        }
        OnRangeTrigger();
        // <Edit later> Merge these methods. Then move it to the Fixed or LateUpdate. 
        UpdateShootingSprite();
        UpdateSpiky();
    }

    void OnCollisionEnter(Collision other) { 
        if(other.gameObject.tag == "Witch"){
            if(_collidedWitches == null || _collidedWitches.Count < 1) {
                _collidedWitches.Clear();
            }
            if(!_collidedWitches.Contains(other.gameObject)){ // Need to refine this, because the collision object might be different. 
                _collidedWitches.Add(other.gameObject);
            }

            // Play DribbleSFX. <Edit later> move this to oncollisionstay
            WitchController witchController = other.gameObject.GetComponent<WitchController>();
            Ball ball = witchController.ball.GetComponent<Ball>();
            if (witchController._possessingBall && ball.DribbleSFXDelay.full){
                ball.SFXManager.PlayDribbleSFX(season);
                ball.DribbleSFXDelay.current = 0f; 
            }
        }
        if(other.gameObject.tag == "MysteryBox"){
            SFXManager.Play(SFXManager.PotionGlass);
        }
        //<Edit later>
        //SFXManager.Play(SFXManager.ObjectLanded);
    }
    void OnCollisionExit(Collision other) {
        if(other.gameObject.tag == "Witch"){
            if(_collidedWitches.Contains(other.gameObject)){
                _collidedWitches.Remove(other.gameObject);
            }
        }
        if(_collidedWitches == null || _collidedWitches.Count < 1){
            _collidedWitches.Clear();
        }
    }

    void OnRangeTrigger() {
        List<GameObject> allWitches = GameObject.FindGameObjectsWithTag("Witch").ToList();
        foreach (GameObject w in allWitches)
        {
            float distance = Vector2.Distance(new Vector2(transform.position.x, transform.position.z), 
                                                new Vector2(w.transform.position.x, w.transform.position.z));
            
            // Enter 
            if(distance <= triggerDistance && !_onRangeWitches.Contains(w)){
                _onRangeWitches.Add(w);
            }

            // Exit 
            if(distance > triggerDistance && _onRangeWitches.Contains(w)){
                _onRangeWitches.Remove(w);
            }
        }
    }

    void UpdateShootingSprite(){
        if(tileType == TileType.Shooting){
            GameObject souroff = transform.Find("Souroff").gameObject;
            if(triggerType == TriggerType.None && !souroff.activeSelf){
                souroff.SetActive(true);
            }
            if(triggerType == TriggerType.Collide){
                if(_collidedWitches.Count > 0 && !souroff.activeSelf) {
                    souroff.SetActive(true);
                }else if(_collidedWitches.Count < 1) {
                    souroff.SetActive(false);
                }
            }
            if(triggerType == TriggerType.Distance){
                if(_onRangeWitches.Count > 0 && !souroff.activeSelf) {
                    souroff.SetActive(true);
                }else if(_onRangeWitches.Count < 1) {
                    souroff.SetActive(false);
                }
            }
        }
    }
    void UpdateSpiky(){
        if(tileType == TileType.Spiky){
            if(_collidedWitches == null || _collidedWitches.Count <= 0){
                /* if(_typePerformed) {
                    Transform trap = transform.Find("Trap");
                    trap.gameObject.SetActive(false);   
                    _typeDelay = 0f; 
                    _typePerformed = false;
                }*/
                if(_typeDuration > 0) {
                    _typeDuration -= Time.deltaTime;
                } else { // duration is empty
                    Transform trap = transform.Find("Trap");
                    trap.gameObject.SetActive(false);   
                    _typeDelay = 0f; 
                    _typePerformed = false;
                }

            }
        }

    }
}

public class GridPosition
{
        public int Row;
        public int Column;
        public int Height;

        public GridPosition(int row, int column, int height) {
            Row     = row;
            Column  = column;
            Height  = height;
        }
        public GridPosition(GridPosition source) {
            Row     = source.Row;
            Column  = source.Column;
            Height  = source.Height;
        }
}
