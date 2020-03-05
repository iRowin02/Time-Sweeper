using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerMove : MonoBehaviour
{

    [Header("Managers")]
    [SerializeField]
    private HUD_Manager HUD;
    [SerializeField]
    private TimeManager timeManager;

    [Header("Public Variables")]
    public Vector3 dir;
    public float movementSpeed, jumpHeight, speedMultiplier, yMovement;
    public Transform gunParent;

    [Header("Variables")]
    public float maxHealth, playerHealth;
    public int playerMana;

    public event Action<float> OnHealthPctChange = delegate { };

    [Header("Private Variables")]
    private float regSpeed, yMovement_,grenadeCharge;
    private bool canJump, isSprinting;
    private int _playerMana;
    private Rigidbody rb;
    private Animator anim;

    private float horInput, vertInput;
    

    private void Start()
    {
        canJump = true;
        Physics.IgnoreLayerCollision(8, 9);    
        rb = GetComponent<Rigidbody>();
        regSpeed = movementSpeed;
        yMovement_ = yMovement;
        playerHealth = maxHealth;
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        Move();
        PlayerInputs();
    }
    

    #region Move
    public void Move()
    {
        horInput = Input.GetAxis("Horizontal");
        vertInput = Input.GetAxis("Vertical");

        dir = new Vector3(horInput, 0, vertInput);

        transform.Translate(dir * movementSpeed * Time.deltaTime);
    }
    #endregion

    #region PlayerInputs
    public void PlayerInputs()
    {
        if(Input.GetButtonDown("Jump"))
        {
            if(canJump)
            {
                rb.AddForce(0,jumpHeight,0);
                canJump = false;
            }
        }
        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            isSprinting = true;
            PlayerSprinting();
        }
        if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            isSprinting = false;
            PlayerSprinting();
        }
        if(Input.GetKeyDown(KeyCode.Tab))
            {
                HealthUpdate(-10);
            }
            if(Input.GetKeyDown(KeyCode.T))
            {
                HUD.totalMana += 1;
            }
            if(Input.GetKeyDown(KeyCode.Z))
            {
                timeManager.SlowDown();
            }
            if(Input.GetKeyDown(KeyCode.X))
            {
                print("Time Restore");
            }
            // if(Input.GetKeyDown(KeyCode.Q))
            // {
            //     print("Grenade");
            // }
            if(Input.GetKey(KeyCode.Q))
            {
                grenadeCharge += Time.deltaTime;
            }
            if(Input.GetKeyUp(KeyCode.Q))
            {
                if(grenadeCharge >= 3)
                {
                    print("Grenade is charged");
                }
                else
                {
                    grenadeCharge = 0;
                    print("Grenade release");
                }
            }
    }
    #endregion

    #region Collision
    void OnCollisionEnter(Collision coll)
    {
        if(coll.transform.CompareTag("Floor"))
        {
            canJump = true;
        }
    }
    #endregion

    #region PlayerSprinting
    void PlayerSprinting()
    {
        if(isSprinting)
        {
            movementSpeed = movementSpeed * speedMultiplier;
            if(dir.x != 0 || dir.z != 0)
            {
                ViewBobbing();
            }
        }
        else
        {
            movementSpeed = regSpeed;
            anim.SetBool("isSprinting", false);
        }

    }
    #endregion

    #region ViewBobbing


    public void ViewBobbing()
    {
        anim.SetBool("isSprinting", true);
    }
    #endregion

    #region Health

    public void HealthUpdate(int damage)
    {
        playerHealth += damage;

        float currentHealthPct = playerHealth / maxHealth;

        OnHealthPctChange(currentHealthPct);
        if(playerHealth > 100)
        {
            playerHealth = maxHealth;
        }
    }      
    #endregion
}

