using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerMove : MonoBehaviour
{

    public event Action<float> OnHealthPctChange = delegate { };

    [Header("Managers")]
    [SerializeField]
    private HUD_Manager HUD;
    [SerializeField]
    private TimeManager timeManager;

    [Header("Public Variables")]
    public GameObject grenade;

    public Transform grenadeSpot;

    public ParticleSystem dashParticle;

    public float movementSpeed;
    public float jumpHeight;
    public float speedMultiplier;
    public float playerHealth;
    public int playerMana;

    public float thrust;
    public float dodgeTime;
    private float _dodgeTime;
    

    [Header("Private Variables")]
    private Vector3 dir;
    private float regSpeed, yMovement_,grenadeCharge, maxHealth;
    private bool canJump, isSprinting;
    private int _playerMana;
    private Rigidbody rb;
    private Animator anim;
    private bool canDodge = true;

    private float horInput, vertInput;
    

    private void Start()
    {
        canJump = true;
        Physics.IgnoreLayerCollision(8, 9);    
        rb = GetComponent<Rigidbody>();
        regSpeed = movementSpeed;
        playerHealth = maxHealth;
        anim = GetComponentInChildren<Animator>();
        _dodgeTime = dodgeTime;
    }

    void Update()
    {
        Move();
        PlayerInputs();
        DodgeTimer();
    }
    

    #region Move
    public void Move()
    {
        horInput = Input.GetAxisRaw("Horizontal");
        vertInput = Input.GetAxisRaw("Vertical");

        dir = new Vector3(horInput, 0, vertInput);

        transform.Translate(dir * movementSpeed * Time.deltaTime);
    }
    #endregion

    #region PlayerInputs
    public void PlayerInputs()
    {
        if (HUD.totalMana != 0)
        {
            if (Input.GetButtonDown("Fire2") || HUD.totalMana <= 0)
            {
                Dodge();
            }

            if (Input.GetKey(KeyCode.Z))
            {
                timeManager.SlowDown();
                HUD.totalMana -= 3;
            }
        }
        if (Input.GetButtonDown("Jump"))
        {
            if (canJump)
            {
                rb.AddForce(0, jumpHeight, 0);
                canJump = false;
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isSprinting = true;
            PlayerSprinting();
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isSprinting = false;
            PlayerSprinting();
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            //HealthUpdate(-10);
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            HUD.totalMana += 1;
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            timeManager.SlowDown();
        }
        if(Input.GetKeyDown(KeyCode.X))
        {
            print("Time Restore");
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Instantiate(grenade, grenadeSpot.position, transform.rotation);
        }
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
        if(coll.transform.CompareTag("Ground"))
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

    /*#region Health
   
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
*/
    #region Dodge
    public void Dodge()
    {
        if(HUD.totalMana != 0)
        {
            if (canDodge == true)
            {
                dashParticle.Play();
                rb.AddForce(transform.forward * thrust, ForceMode.Impulse);
                canDodge = false;
                HUD.totalMana -= 1;
            }
        }
        else
        {
            print("no mana left");
        }
    }
    public void DodgeTimer()
    {
        if(canDodge == false)
        {
            dodgeTime -= Time.deltaTime;
            if(dodgeTime <= 0)
            {
                dodgeTime = _dodgeTime;
                canDodge = true;
            }
        }
    }
    #endregion
}

