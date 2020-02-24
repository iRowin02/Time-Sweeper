using UnityEngine;
using System;

namespace ThirdPersonMovement
{
    public class ThirdPersonController : MonoBehaviour
    {
        [HideInInspector]
        float animDamp = 0.4f;
        [HideInInspector]
        public float resetSpeed = 5;

        [Header("Managers")]
        [SerializeField]
        private HUD_Manager HUD;

        [Header("States")]
        public bool running;
        public bool jumping;
        public bool onGround;
        public bool aiming;
        public bool cursorLockMode;

        [Header("Inputs")]
        public Vector3 moveDir;

        public int rigAngDrag = 999;
        public int rigDrag = 4;

        public float walkSpeed = 5;
        public float runSpeed = 8;
        public float jumpForce = 7;
        public float rotationSpeed = 5;
        public float moveVar;
        public float ver;
        public float hor;

        [Header("Variables")]
        public float maxHealth;
        [ReadOnly]
        public float playerHealth;

        public int playerMana;
        private int _playerMana;

        public event Action<float> OnHealthPctChange = delegate { };

        [SerializeField]
        private float manaRate;

        [Header("Handlers")]
        public Animator anim;
        public Rigidbody rig;
        public ThirdPersonCamera thirdPersonCamManager;
        public GameObject activeModel;

        public void Start()
        {
            playerHealth = maxHealth;
        }

        public void Update()
        {
            BaseMovement();
            IntermediateMovement();
            Inputs();
            thirdPersonCamManager.Tick();
        }

        public void Awake()
        {
            thirdPersonCamManager.CamInit(transform);
            AnimatorSetup();

            rig = gameObject.GetComponent<Rigidbody>();
            rig = GetComponent<Rigidbody>();
            rig.angularDrag = rigAngDrag;
            rig.drag = rigDrag;
            rig.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY;
        }

        public void Inputs()
        {
            ver = Input.GetAxis("Vertical");
            hor = Input.GetAxis("Horizontal");
            running = Input.GetButton("Run");
            jumping = Input.GetButton("Jump");
            aiming = Input.GetButton("RightMouse");
            cursorLockMode = Input.GetButton("Cancel");
        }
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

        public void BaseMovement()
        {
            Vector3 v = ver * thirdPersonCamManager.transform.forward;
            Vector3 h = hor * thirdPersonCamManager.transform.right;

            moveDir = (v + h).normalized;
            float m = Mathf.Abs(hor) + Mathf.Abs(ver);
            moveVar = Mathf.Clamp01(m);

            float moveAmount = (running) ? runSpeed : walkSpeed;
            if (onGround)
                rig.velocity = moveDir * (moveAmount * moveVar);

            rig.drag = (moveAmount > 0 || onGround == false) ? 0 : rigDrag;

            if (onGround)
                if (jumping)
                    rig.AddForce(Vector3.up.normalized * jumpForce);

            if (cursorLockMode)
            {
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
            }

        }

        public void IntermediateMovement()
        {
            if (!aiming)
            {
                Vector3 targetDir = moveDir;
                targetDir.y = 0;
                if (targetDir == Vector3.zero)
                {
                    targetDir = transform.forward;
                }
                Quaternion tr = Quaternion.LookRotation(targetDir);
                Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, Time.deltaTime * moveVar * rotationSpeed);
                transform.rotation = targetRotation;
            }
            else
            {
                transform.LookAt(thirdPersonCamManager.aimTarget);
            }

            MovementAnimHandler();
        }

        #region AnimHandlers
        public void MovementAnimHandler()
        {
            anim.SetFloat("Vertical", moveVar, animDamp, Time.deltaTime);
            anim.SetBool("Running", running);
        }

        public void AnimatorSetup()
        {
            if (activeModel == null)
            {
                anim = GetComponentInChildren<Animator>();
                if (anim == null)
                {
                    Debug.Log("No model found");
                }
                else
                {
                    activeModel = anim.gameObject;
                }
            }
            if (anim == null)
            {
                anim = activeModel.GetComponent<Animator>();
            }
        }

        #endregion

        #region ongroundOn/Off
        void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.tag == "Ground")
            {
                onGround = true;
                anim.SetBool("OnGround", onGround);
            }
        }

        private void OnCollisionExit(Collision other)
        {
            if (other.gameObject.tag == "Ground")
            {
                onGround = false;
                anim.SetBool("OnGround", onGround);
            }
        }
        
    }
    #endregion
    
}