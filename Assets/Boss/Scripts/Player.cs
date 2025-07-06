using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    // num modifiables
    public float inputBufferTime;
    public float topSpeed;
    public float crouchSpeedRatio;
    public float jumpSpeed;
    public float jumpHoldTime;
    public float gravMod;
    public float groundY;
    // handle player states
    private State state;
    private MoveState moveState;
    private MoveState bufferedState;
    private float bufferTimer;
    private float jumpHoldTimer;
    private bool canCombo;
    private Combo combo;
    private enum Combo
    {
        NONE,
        LIGHT,
        HEAVY
    }
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
        DOWNED,
        IDLE
    }
    public MoveState[] attackStates;
    public MoveState[] blockStates;
    public MoveState[] jumpStates;
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
        state = State.STANDING;
        moveState = MoveState.IDLE;
    }

    void Update()
    {
        // gravity/ground checks
        if (state == State.AIRBORNE)
        {
            if(jumpHoldTimer < 0 || !Input.GetKey(KeyCode.W)) vertVel -= Time.deltaTime * gravMod;
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
        jumpHoldTimer -= Time.deltaTime;
    }

    // handle movement/crouching
    void doMovement(){
        // handle no movement allowed
        if (state != State.AIRBORNE && !checkInput(moveStates))  return;

        //TODO block movement when too close to enemy
        // handle L/R
        float horizInput = Input.GetAxis("Horizontal");
        horizVel = horizInput * topSpeed * (state == State.CROUCHED ? crouchSpeedRatio : 1);
        // handle direction
        if (state != State.AIRBORNE)
        {
            //dir = Mathf.Sign(horizInput);
            sprite.flipX = dir == -1 ? true : false;
        }
        // do movement
        if (Mathf.Abs(horizVel) > 0.01f) {
            anim.SetInteger("moving", (int)Mathf.Sign(horizInput));
            transform.Translate(Vector2.right * horizVel * Time.deltaTime);
        } else
        {
            anim.SetInteger("moving", 0);
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
            tryBuffer();
            //TODO maybe clear air attack hitbox
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
        if (Input.GetKeyDown(KeyCode.W) && tryMove(jumpStates, MoveState.JUMP, true))
        {
            doJump();
        }

    }

    // moves
    void doLight()
    {
        // convert to air/crouch attack
        if (convertAttack()) return;

        // do light
        moveState = MoveState.LIGHT;
        switch (combo)
        {
            default:
                anim.Play("lightAttack");
                //TODO spawn hitbox
                combo = Combo.LIGHT;
                break;
        }
    }
    void doHeavy()
    {
        // convert to air/crouch attack
        if (convertAttack()) return;

        // do heavy
        moveState = MoveState.HEAVY;
        switch (combo)
        {
            default:
                anim.Play("heavyAttack");
                //TODO spawn hitbox
                combo = Combo.HEAVY;
                break;
        }
    }
    void doAirAttack()
    {
        // do air
        moveState = MoveState.LIGHT;
        switch (combo)
        {
            default:
                anim.Play("airAttack");
                //TODO spawn hitbox
                combo = Combo.NONE;
                break;
        }
    }
    void doCrouchAttack()
    {
        // do air
        moveState = MoveState.LIGHT;
        switch (combo)
        {
            default:
                anim.Play("crouchAttack");
                //TODO spawn hitbox
                combo = Combo.NONE;
                break;
        }
    }
    void doBlock()
    {
        // only block on ground
        if (state == State.AIRBORNE || state == State.CROUCHED) return;
        // do block
        moveState = MoveState.BLOCK;
        switch (combo)
        {
            default:
                anim.Play("block");
                combo = Combo.NONE;
                break;
        }
    }
    void doJump()
    {
        if (state != State.STANDING) return;
        vertVel = jumpSpeed;
        jumpHoldTimer = jumpHoldTime;
        anim.SetBool("airborne", true);
        anim.Play("jump");
        moveState = MoveState.JUMP;
        state = State.AIRBORNE;
    }
    // use buffer
    void doBuffer()
    {
        // cancel crouch state if needed
        if (state == State.CROUCHED && !Input.GetKey(KeyCode.S))
        {
            state = State.STANDING;
            anim.Play("uncrouch");
            anim.SetBool("crouched", false);
            // TODO hitbox manipulation
        }
        // use buffer
        MoveState heldState = bufferedState;
        bufferedState = MoveState.IDLE;
        if (bufferTimer <= 0)
        {
            canCombo = true;
            return;
        }
        bufferTimer = 0;
        switch (heldState)
        {
            case MoveState.LIGHT:
                doLight();
                break;
            case MoveState.HEAVY:
                doHeavy();
                break;
            case MoveState.BLOCK:
                if (canCombo) doBlock();
                return;
            case MoveState.JUMP:
                doJump();
                break;
            default:
                break;
        }
        if(moveState == MoveState.IDLE)
        {
            canCombo = true;
            bufferTimer = 0;
        }
    }
    // clear combo
    void clearCombo()
    {
        if (bufferedState == MoveState.BLOCK) doBlock();
        canCombo = false;
        combo = Combo.NONE;
        moveState = MoveState.IDLE;
    }
    // buffer or clear combo
    void tryBuffer()
    {
        // do cancel
        canCombo = false;
        combo = Combo.NONE;
        moveState = MoveState.IDLE;
        // try buffer
        doBuffer();
    }


    /* 
     * helper methods
     * */
    // convert an attack to an air/crouch attack
    bool convertAttack()
    {
        if (state == State.AIRBORNE)
        {
            doAirAttack();
            return true;
        }
        else if (state == State.CROUCHED)
        {
            doCrouchAttack();
            return true;
        }
        return false;
    }
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
            bufferedState = ms;
            bufferTimer = inputBufferTime;
            if (canCombo) doBuffer();
        }
        return valid;
    }
}

/**
 * TODO
 *      projectiles
 *      health script (hitstun/downed stuff)
 */