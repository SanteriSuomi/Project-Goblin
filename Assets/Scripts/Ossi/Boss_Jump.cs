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

    float distant = 2;

    float bounce;

    Transform edgeR;
    Transform edgeL;

    bool canJump = false;
    Vector2 leap;
    Vector2 leapL;
    Vector2 leapR;
    Transform jumpHeight;
    Transform introPoint;
    Transform groundY;
    Transform player;
    Rigidbody rb;

    bool falling = false;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        edgeL = GameObject.FindGameObjectWithTag("EdgeL").transform;
        edgeR = GameObject.FindGameObjectWithTag("EdgeR").transform;
        introPoint = GameObject.FindGameObjectWithTag("Intro").transform;
        jumpHeight = GameObject.FindGameObjectWithTag("JumpHeight").transform;
        groundY = GameObject.FindGameObjectWithTag("GroundY").transform;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = animator.GetComponent<Rigidbody>();
        ogre = animator.GetComponent<Ogre>();
        leap.x = rb.position.x;
        if(rb.position.x < player.position.x - 5 || rb.position.x > player.position.x + 5) {
            bounce = Time.time + 0.8f;
            distant = 4;
            animator.SetFloat("JumpAnimationSpeed", 1.2f);
        }
        else {
            bounce = Time.time + 0.6f;
            distant = 2;
            animator.SetFloat("JumpAnimationSpeed", 1.4f);
        }
        
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {   
        if(Time.time >= bounce) {
            canJump = true;
        }
        if(ogre.intro == true) {
            if(ogre.facingLeft) {
                jumpTarget = new Vector2(leap.x-distant, jumpHeight.position.y);
                target = new Vector2(leap.x-distant*2, groundY.position.y);
            }
            else {
                jumpTarget = new Vector2(leap.x+distant, jumpHeight.position.y);
                target = new Vector2(leap.x+distant*2, groundY.position.y);
            }

            if(rb.position.x <= edgeL.position.x) {
                jumpTarget = new Vector2(edgeL.position.x, jumpHeight.position.y);
                target = new Vector2(edgeL.position.x, groundY.position.y);
            }
            else if(rb.position.x >= edgeR.position.x) {
                jumpTarget = new Vector2(edgeR.position.x, jumpHeight.position.y);
                target = new Vector2(edgeR.position.x, groundY.position.y);
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
        else {
            jumpTarget = new Vector2(leap.x - 4, 12);
            target = new Vector2(introPoint.position.x +10, groundY.position.y);
            animator.SetFloat("JumpAnimationSpeed", 0.8f);

            if(12 > rb.position.y && !falling && canJump) {
                Vector2 newPos = Vector2.MoveTowards(rb.position, jumpTarget, 15 * Time.fixedDeltaTime);
                rb.MovePosition(newPos);
                animator.SetFloat("JumpAnimationSpeed", 0.6f);
            }
            else if(canJump) {
                falling = true;
                Vector2 newPos = Vector2.MoveTowards(rb.position, target, 15 * Time.fixedDeltaTime);
                rb.MovePosition(newPos);
                animator.SetFloat("JumpAnimationSpeed", 1f);
            }

        }
    }


    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        falling = false;
        canJump = false;
        ogre.intro = true;

    }
}
