using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Walk : StateMachineBehaviour
{

    public float speed = 2f;
    public float attackRange = 1.0f;
    private float rageTimer;

    private bool rage = false;

    private int phase = 1;

    Ogre ogre;

    Transform groundY;
    Transform player;
    Rigidbody rb;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    	groundY = GameObject.FindGameObjectWithTag("GroundY").transform;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = animator.GetComponent<Rigidbody>();
        ogre = animator.GetComponent<Ogre>();
        animator.SetBool("Chase", true);

        rageTimer = Time.time + Random.Range(0f, 5f);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    	ogre.LookAtPlayer();

    	Vector2 target = new Vector2(player.position.x, rb.position.y);

    	if(Time.time >= rageTimer) {
    		rage = true;
    	}

    	if(rage) {
    		phase = 5;
    	}

    	if(phase < 3) {
	       	Vector2 newPos = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
	      	rb.MovePosition(newPos);
    	}
    	else if(phase < 5) {
    		speed = 3f;
   			Vector2 newPos = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
	      	rb.MovePosition(newPos);
    	}
    	// rage phase
    	else if(phase == 5) {
    		rage = false;
		    animator.SetTrigger("Attack");
		    animator.SetTrigger("JumpAttack");
		    animator.SetBool("JumpAttacks", true);
		    animator.SetBool("Chase", false);
    	}

      	if(ogre.GetHealth() <= 300f && !rage) {
      		phase = 4;
      	}
      	else if(ogre.GetHealth() <= 650f && !rage) {
      		phase = 3;
      	}
      	else if(ogre.GetHealth() <= 900f && !rage) {
      		phase = 2;
      	}
      	else if(ogre.GetHealth() <= 1000f && !rage) {
      		phase = 1;
      	}

      	switch(phase) {
      		case 1:
	      		if(Vector2.Distance(player.position, rb.position) <= attackRange) {
		      		animator.SetTrigger("Attack");
		      		animator.SetTrigger("Punch");
		      		animator.SetBool("Chase", false);
      			}
      			break;
      		case 2:
	      		if(Vector2.Distance(player.position, rb.position) <= attackRange+0.3f) {
		      		animator.SetTrigger("Attack");
		      		animator.SetTrigger("Overhead");
		      		animator.SetBool("Chase", false);
      			}
      			break;
      		case 3:
	      		if(Vector2.Distance(player.position, rb.position) <= attackRange) {
		      		animator.SetTrigger("Attack");
		      		animator.SetTrigger("Kick");
		      		animator.SetBool("Chase", false);
      			}
      			break;
      		case 4:
	      		if(Vector2.Distance(player.position, rb.position) <= attackRange) {
		      		animator.SetTrigger("Attack");
		      		animator.SetTrigger("Overhead");
		      		animator.SetBool("Chase", false);
      			}
      			break;
      	}

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Attack");
        animator.ResetTrigger("Punch");
        rage = false;
    }
}
