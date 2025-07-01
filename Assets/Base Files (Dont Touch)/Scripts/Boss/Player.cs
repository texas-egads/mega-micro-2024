using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    // num modifiables
    public float inputBufferTime;
    public float comboTime;
    public float topSpeed;
    public float crouchSpeedRatio;
    public float jumpSpeed;
    public float gravMod;
    public float groundY;
    // handle player states
    private State state;
    private MoveState moveState;
    private MoveState bufferedState;
    private float bufferTimer;
    private bool canCombo;
    public enum State
    {
        STANDING,
        AIRBORNE,
        CROUCHED
    }
    public enum MoveState
    {
        LIGHT,
        HEAVY,
        BLOCK,
        JUMP,
        STUNNED,
        IDLE
    }
    public MoveState[] attackStates;
    public MoveState[] blockStates;
    public MoveState[] jumpStates;
    public MoveState[] jabStates;
    public MoveState[] crouchStates;
    public MoveState[] moveStates;

    // handle player visuals
    public Animator anim;
    public SpriteRenderer sprite;

    // handle other
    private float dir;
    private float horizVel;
    private float vertVel;

    void Start()
    {

    }

    void Update()
    {
        // gravity/ground checks
        if (state == State.AIRBORNE)
        {
            if(moveState != MoveState.JUMP || !Input.GetKey(KeyCode.Space)) vertVel -= Time.deltaTime * gravMod;
            transform.Translate(new Vector2(0, vertVel * Time.deltaTime));
            checkGround();
        }

        // inputs
        if (BossManager.Instance.state == BossManager.State.PLAY)
        {
            doMovement();
            doCrouch();
            doMoves();
        }


        // timers
        bufferTimer -= Time.unscaledDeltaTime;
    }

    // handle movement/crouching
    void doMovement(){
        // handle no movement allowed
        if (!checkInput(moveStates))  return;

        // handle L/R
        float horizInput = Input.GetAxis("Horizontal");
        horizVel = horizInput * topSpeed * (state == State.CROUCHED ? crouchSpeedRatio : 1);
        // handle direction
        if (state != State.AIRBORNE)
        {
            dir = Mathf.Sign(horizInput);
            sprite.flipX = dir == -1 ? true : false;
        }
        // do movement
        if (horizVel > 0.01f) {
            anim.SetBool("moving", true);
            transform.Translate(Vector2.right * horizVel * Time.deltaTime);
        } else
        {
            anim.SetBool("moving", false);
        }
    }
    void doCrouch()
    {
        // handle no movement allowed
        if (!checkInput(crouchStates, true)) return;

        // handle input/states/anims
        if (Input.GetKey(KeyCode.S))
        {
            if (state != State.CROUCHED)
            {
                state = State.CROUCHED;
                anim.Play("crouch");
                anim.SetBool("crouched", true);
            }
        } else if (state == State.CROUCHED)
        {
            state = State.STANDING;
            anim.Play("uncrouch");
            anim.SetBool("crouched", false);
        }

        // TODO hitbox manipulation
    }
    void checkGround()
    {
        // change state + more on grounded
        if(transform.position.y <= groundY)
        {
            transform.position = new Vector3(transform.position.x, groundY, transform.position.z);
            state = State.STANDING;
            vertVel = 0;
            anim.SetBool("airborne", false);
            anim.Play("land");
        }
    }

    // handle moves
    void doMoves()
    {
        // light
        if(Input.GetMouseButtonDown(0) && tryMove(attackStates, MoveState.LIGHT))
        {
            doLight();
        }
        // heavy
        if(Input.GetMouseButtonDown(1) && tryMove(attackStates, MoveState.HEAVY))
        {
            doHeavy();
        }
        // block
        if (Input.GetKeyDown(KeyCode.Space) && tryMove(blockStates, MoveState.BLOCK, true))
        {
            doBlock();
        }
        // jump
        if (Input.GetMouseButtonDown(1) && tryMove(jumpStates, MoveState.JUMP, true))
        {
            doJump();
        }

    }

    // moves
    void doLight()
    {
        //TODO
    }
    void doHeavy()
    {
        //TODO
    }
    void doAirAttack()
    {
        //TODO
    }
    void doCrouchAttack()
    {
        //TODO
    }
    void doBlock()
    {
        //TODO
        //TODO also deny block if canCombo
    }
    void doJump()
    {
        vertVel = jumpSpeed;
        anim.SetBool("airborne", true);
        anim.Play("land");
        moveState = MoveState.JUMP;
        state = State.AIRBORNE;
    }
    // use buffer/combo
    void doBuffer()
    {
        canCombo = true;
        //TODO
    }
    // clear jump
    void clearJump()
    {
        moveState = MoveState.IDLE;
    }
    // reset to idle
    void makeIdle()
    {
        canCombo = false;
        moveState = MoveState.IDLE;
    }


    /* 
     * helper methods
     * */
    // returns whether an input is acceptable
    bool checkInput(MoveState[] valids, bool noAirborne = false)
    {
        if (noAirborne && state == State.AIRBORNE) return false;
        bool valid = false;
        // check if interruptable
        foreach (MoveState v in valids)
        {
            if (moveState == v) valid = true;
        }
        return valid;

    }
    // returns true if a move is immediately executable, and buffers it otherwise
    bool tryMove(MoveState[] valids, MoveState ms, bool noAirborne = false)
    {
        bool valid = checkInput(valids, noAirborne);
        // buffer on fail
        if (!valid)
        {
            if (canCombo)
            {
                bufferedState = ms;
                doBuffer();
            }
            else
            {
                bufferedState = ms;
                bufferTimer = inputBufferTime;
            }
        }
        return valid;
    }
}

/**
 * TODO
 *      l/r movement
 *      crouch
 *      projectiles
 *      health script
 */