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
    public float hitstun;
    public float hitstunStrength;
    public float gravMod;
    public float groundY;
    public float rallyTime;
    public float maxHealth;
    // handle player states
    private State state;
    private MoveState moveState;
    private MoveState bufferedState;
    private float bufferTimer;
    private float jumpHoldTimer;
    private bool canCombo;
    private Combo combo;
    private bool canBlock;
    private float stunTimer;
    private float rallyTimer;
    private bool isDead;
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
    public Boss boss;
    public Collider2D topCollider;
    public RectTransform[] healthbars;
    public GameObject[] attackPrefabs; // 0:light; 1:heavy; 2:air; 3: crouch
    public UltimateCharge[] ultimateCharges; 

    // handle other
    private float dir;
    private float health;
    private int ultCharges;
    private float oldHealth;
    private float horizVel;
    private float vertVel;

    void Start()
    {
        health = maxHealth;
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
            CheckGround();
        }

        // inputs
        if (BossManager.Instance.state == BossManager.State.PLAY)
        {
            if(!isDead) DoMoves();
            if (moveState != MoveState.STUNNED)
            {
                DoMovement();
                DoCrouch();
            } else
            {
                DoStun();
            }
        }

        // constrain position
        topCollider.enabled = state == State.CROUCHED;
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -10, boss.transform.position.x - 2), transform.position.y, transform.position.z);

        // timers
        DoRally();
        DoDeadSlow();
        bufferTimer -= Time.unscaledDeltaTime;
        jumpHoldTimer -= Time.deltaTime;
    }

    // handle movement/crouching
    void DoMovement(){
        // handle no movement allowed
        if (state != State.AIRBORNE && !CheckInput(moveStates))  return;

        // handle L/R
        float horizInput = Input.GetAxis("Horizontal");
        horizVel = horizInput * topSpeed * (state == State.CROUCHED ? crouchSpeedRatio : 1);
        // handle direction
        if (state != State.AIRBORNE)
        {
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
    public void DoStepSound()
    {
        BudioManager.Instance.PlayMove(0);
    }
    void DoCrouch()
    {
        // handle no movement allowed
        if (!CheckInput(crouchStates, true)) return;

        // handle input/states/anims
        if (Input.GetKey(KeyCode.S))
        {
            if (state != State.CROUCHED)
            {
                BudioManager.Instance.PlayMove(3);
                state = State.CROUCHED;
                anim.Play("crouch");
                anim.SetBool("crouched", true);
            }
        } else if (state == State.CROUCHED)
        {
            state = State.STANDING;
            BudioManager.Instance.PlayMove(3);
            anim.Play("uncrouch");
            anim.SetBool("crouched", false);
        }
    }
    void CheckGround()
    {
        // change state + more on grounded
        if(transform.position.y <= groundY)
        {
            transform.position = new Vector3(transform.position.x, groundY, transform.position.z);
            state = State.STANDING;
            vertVel = 0;
            anim.SetBool("airborne", false);
            if(moveState != MoveState.STUNNED) anim.Play("land");
            BudioManager.Instance.PlayMove(2);
            TryBuffer();
        }
    }

    // handle moves
    void DoMoves()
    {
        stunTimer = hitstun;
        // light
        if (Input.GetKeyDown(KeyCode.K) && TryMove(attackStates, MoveState.LIGHT))
        {
            DoLight();
        }
        // heavy
        else if(Input.GetKeyDown(KeyCode.L) && TryMove(attackStates, MoveState.HEAVY))
        {
            DoHeavy();
        }
        // block
        else if (Input.GetKeyDown(KeyCode.Space) && TryMove(blockStates, MoveState.BLOCK, true))
        {
            DoBlock();
        }
        // jump
        else if (Input.GetKeyDown(KeyCode.W) && TryMove(jumpStates, MoveState.JUMP, true))
        {
            DoJump();
        }

    }
    void DoStun()
    {
        stunTimer -= Time.deltaTime;
        if(state != State.AIRBORNE)
        {
            horizVel = Mathf.Clamp(horizVel + hitstunStrength / hitstun * Time.deltaTime * 2, -hitstunStrength, 0);
        }
        transform.Translate(Vector2.right * horizVel * Time.deltaTime);
        if (!isDead && stunTimer <= 0)
        {
            anim.Play("idle");
            ClearCombo();
        }
    }
    void DoRally()
    {
        if (rallyTimer < rallyTime)
        {
            rallyTimer += Time.deltaTime;
            Mathf.Lerp(oldHealth, healthbars[0].sizeDelta.x, Mathf.Clamp01(rallyTimer / rallyTime));
        }
    }
    void DoDeadSlow()
    {
        if (isDead)
        {
            Time.timeScale = Mathf.Clamp(Time.timeScale - Time.unscaledDeltaTime, .5f, 1);
        }
    }

    // moves
    void DoLight()
    {
        // convert to air/crouch attack
        if (ConvertAttack()) return;

        // do light
        moveState = MoveState.LIGHT;
        switch (combo)
        {
            default:
                BudioManager.Instance.PlayAttack(0);
                anim.Play("lightAttack");
                combo = Combo.LIGHT;
                break;
        }
    }
    void DoHeavy()
    {
        // convert to air/crouch attack
        if (ConvertAttack()) return;

        // do heavy
        moveState = MoveState.HEAVY;
        switch (combo)
        {
            default:
                BudioManager.Instance.PlayAttack(1);
                anim.Play("heavyAttack");
                combo = Combo.HEAVY;
                break;
        }
    }
    void DoAirAttack()
    {
        // do air
        moveState = MoveState.LIGHT;
        switch (combo)
        {
            default:
                BudioManager.Instance.PlayAttack(0);
                anim.Play("airAttack");
                combo = Combo.NONE;
                break;
        }
    }
    void DoCrouchAttack()
    {
        // do air
        moveState = MoveState.LIGHT;
        switch (combo)
        {
            default:
                BudioManager.Instance.PlayAttack(0);
                anim.Play("crouchAttack");
                combo = Combo.NONE;
                break;
        }
    }
    void DoBlock()
    {
        // only block on ground
        if (state == State.AIRBORNE || state == State.CROUCHED) return;
        // do block
        moveState = MoveState.BLOCK;
        switch (combo)
        {
            default:
                BudioManager.Instance.PlayAttack(2);
                canBlock = true;
                anim.Play("block");
                combo = Combo.NONE;
                break;
        }
    }
    void DoJump()
    {
        if (state != State.STANDING) return;
        vertVel = jumpSpeed;
        jumpHoldTimer = jumpHoldTime;
        BudioManager.Instance.PlayMove(1);
        anim.SetBool("airborne", true);
        anim.Play("jump");
        moveState = MoveState.JUMP;
        state = State.AIRBORNE;
    }
    // use buffer
    void DoBuffer()
    {
        // cancel crouch state if needed
        if (state == State.CROUCHED && !Input.GetKey(KeyCode.S))
        {
            state = State.STANDING;
            BudioManager.Instance.PlayMove(3);
            anim.Play("uncrouch");
            anim.SetBool("crouched", false);
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
                DoLight();
                break;
            case MoveState.HEAVY:
                DoHeavy();
                break;
            case MoveState.BLOCK:
                if (canCombo) DoBlock();
                return;
            case MoveState.JUMP:
                DoJump();
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
    void ClearCombo()
    {
        if (bufferedState == MoveState.BLOCK) DoBlock();
        canCombo = false;
        combo = Combo.NONE;
        moveState = MoveState.IDLE;
    }
    // buffer or clear combo
    void TryBuffer()
    {
        // do cancel
        canCombo = false;
        combo = Combo.NONE;
        moveState = MoveState.IDLE;
        // try buffer
        DoBuffer();
    }
    // failed to time block
    public void DisableBlock()
    {
        canBlock = false;
    }
    // create attack hurtbox
    public void SpawnAttack(int index)
    {
        Instantiate(attackPrefabs[index], transform);
    }
    // successful block
    public void DoSuccessfulBlock()
    {
        if(ultCharges < 3) ultimateCharges[ultCharges].Activate();
        ultCharges++;
        BudioManager.Instance.PlayAttack(3);
        DoBuffer();
    }
    // take/heal damage
    public void TakeDamage(float damage)
    {
        // exit if dead
        if (isDead) return;

        // handle block
        if (canBlock)
        {
            DoSuccessfulBlock();
            return;
        }

        // health changes
        health -= damage;
        healthbars[0].sizeDelta = new Vector2(health/maxHealth * 577, 78);
        oldHealth = healthbars[1].sizeDelta.x;
        rallyTimer = 0;
        moveState = MoveState.STUNNED;

        // handle death
        if(health <= 0)
        {
            vertVel = hitstunStrength * 2;
            horizVel = -hitstunStrength * 2;
            transform.Translate(Vector3.up * 0.05f);
            isDead = true;
            BossManager.Instance.ShowKO();
            BudioManager.Instance.PlayRoundStart(2);
            anim.Play("die");
            StartCoroutine(BossManager.Instance.ResetGame(1));
            return;
        }

        // sounds and anims
        BudioManager.Instance.PlayMove(4);
        anim.Play("hitstun");
        boss.HealRally();

        // reset state
        horizVel = -hitstunStrength;
        if(state == State.AIRBORNE)
        {
            vertVel = hitstunStrength;
        }
        if (state == State.CROUCHED)
        {
            state = State.STANDING;
        }
    }
    public void HealRally()
    {
        rallyTimer = rallyTime;
        health = healthbars[1].sizeDelta.x / 577f * maxHealth;
        oldHealth = health;
        healthbars[0].sizeDelta = new Vector2(health / maxHealth * 577, 78);
        healthbars[1].sizeDelta = new Vector2(health / maxHealth * 577, 78);
    }
    // reset game state
    public void ResetState()
    {
        Time.timeScale = 1f;
        anim.Play("idle");
        transform.position = Vector3.left * 4;
        state = State.STANDING;
        moveState = MoveState.IDLE;
        health = maxHealth;
        oldHealth = maxHealth;
        healthbars[0].sizeDelta = new Vector2(577, 78);
        healthbars[1].sizeDelta = new Vector2(577, 78);
        ultCharges = 0;
        isDead = false;
        foreach (UltimateCharge uc in ultimateCharges)
        {
            uc.Deactivate();
        }
    }

    /* 
     * helper methods
     * */
    // convert an attack to an air/crouch attack
    bool ConvertAttack()
    {
        if (state == State.AIRBORNE)
        {
            DoAirAttack();
            return true;
        }
        else if (state == State.CROUCHED)
        {
            DoCrouchAttack();
            return true;
        }
        return false;
    }
    // returns whether an input is acceptable
    bool CheckInput(MoveState[] valids, bool noAirborne = false)
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
    bool TryMove(MoveState[] valids, MoveState ms, bool noAirborne = false)
    {
        bool valid = CheckInput(valids, noAirborne);
        // buffer on fail
        if (!valid)
        {
            bufferedState = ms;
            bufferTimer = inputBufferTime;
            if (canCombo) DoBuffer();
        }
        return valid;
    }
}