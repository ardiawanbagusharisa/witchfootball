using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public Team.TeamParty teamParty;
    public bool move;
    public float moveSpeed;
    public float minPosZ; // 0.75f
    public float maxPosZ; // 3f
    public Vector3 targetPos;

    void OnCollisionEnter(Collision other) {
        if(other.gameObject.GetComponent<Ball>() != null){
            Match match = FindFirstObjectByType<Match>();
            if(match.gamestate == Match.GameState.MatchPlaying){
                Debug.Log("Goal! OLE!");
                Ball ball = other.gameObject.GetComponent<Ball>();
                if(ball.lastToucher != null) {
                    Match matchController = FindFirstObjectByType<Match>();
                    matchController.GoalScored(ball.lastToucher, this);
                }
            }
        }
    }

    void Move(Vector3 targetPos, float speed){
        transform.position = Vector3.Lerp(transform.position, targetPos, speed * Time.deltaTime);
    }
    Vector3 RandomTarget(){
        float targetPosZ = Random.Range(minPosZ, maxPosZ);
        return new Vector3(transform.position.x, transform.position.y, targetPosZ);
    }

    void LateUpdate(){
        if(move) {
            Move(targetPos, moveSpeed);
            if(transform.position == targetPos){
                targetPos = RandomTarget();
            }
        }
    }
}
