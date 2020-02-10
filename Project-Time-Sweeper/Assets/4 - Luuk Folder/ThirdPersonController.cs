using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThirdPersonMovement
{
    public class ThirdPersonController : MonoBehaviour
    {
        [HideInInspector]
        float animDamp = 0.4f;
        [HideInInspector]
        public float resetSpeed = 5;

        [Header("Inputs")]
        public Vector3 moveDir;
        public bool running;
        public bool onGround;
        public float walkSpeed = 5;
        public float runSpeed = 8;
        public float ver;
        public float hor;

        [Header("Handlers")]
        public Animator anim;
        public ThirdPersonCamera thirdPersonCamManager;
        public GameObject activeModel;

        public void Update()
        {
            BaseMovement();
            Inputs();
            thirdPersonCamManager.CamInit(transform);
            thirdPersonCamManager.Tick();
        }

        public void Inputs()
        {
            ver = Input.GetAxis("Vertical");
            hor = Input.GetAxis("Horizontal");
            running = Input.GetButton("Run");
        }
        public void Awake()
        {
            AnimatorSetup();
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

        public void BaseMovement()
        {
            moveDir = new Vector3(hor, 0, ver).normalized;
            float moveAmount = (running) ? runSpeed : walkSpeed;
            transform.Translate(moveDir * moveAmount * Time.deltaTime);
            MovementAnimHandler();
        }

        public void MovementAnimHandler()
        {
            anim.SetFloat("Vertical", ver, animDamp, Time.deltaTime);
            anim.SetBool("Running", running);
        }

        public void OnCollisionEnter(Collision collision)
        {

            if (collision.transform == GameObject.FindGameObjectWithTag("Ground"))
            {
                onGround = true;
            }
        }
    }
}