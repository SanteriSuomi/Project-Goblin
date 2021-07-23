using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Jump : StateMachineBehaviour
{
    Ogre ogre;
    public float jumpSpeed;
    public float fallMultiplier;

    public Vector2 target;
    public Vector2 jumpTarget;

    float bounce;

    bool canJump = false;
    Vector2 leap;
    Vector2 leapL;
    Vector2 leapR;
    Transform jumpHeight;
    Transform groundY;
    Transform player;
    Rigidbody rb;

    bool falling = false;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        jumpHeight = GameObject.FindGameObjectWithTag("JumpHeight").transform;
        groundY = GameObject.FindGameObjectWithTag("GroundY").transform;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = animator.GetComponent<Rigidbody>();
        ogre = animator.GetComponent<Ogre>();
        leap.x = rb.position.x;
        bounce = Time.time + 0.7f;
        
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(Time.time >= bounce) {
            canJump = true;
        }
        if(ogre.facingLeft) {
            jumpTarget = new Vector2(leap.x-2, jumpHeight.position.y);
            target = new Vector2(leap.x-4, groundY.position.y);
        }
        else {
            jumpTarget = new Vector2(leap.x+2, jumpHeight.position.y);
            target = new Vector2(leap.x+4, groundY.position.y);
        }

        if(jumpHeight.position.y > rb.position.y && !falling && canJump) {
            Vector2 newPos = Vector2.MoveTowards(rb.position, jumpTarget, jumpSpeed * Time.fixedDeltaTime);
            rb.MovePosition(newPos);
        }
        else if(canJump) {
            falling = true;
            Vector2 newPos = Vector2.MoveTowards(rb.position, target, jumpSpeed * Time.fixedDeltaTime);
            rb.MovePosition(newPos);
        }
    }


    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        falling = false;
        canJump = false;
    }
}
