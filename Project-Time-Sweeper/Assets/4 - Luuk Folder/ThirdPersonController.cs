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
        public float rotationSpeed = 5;
        public float moveVar;
        public float ver;
        public float hor;

        [Header("Handlers")]
        public Animator anim;
        public Rigidbody rig;
        public ThirdPersonCamera thirdPersonCamManager;
        public GameObject activeModel;

        public void Update()
        {
            BaseMovement();
            IntermediateMovement();
            Inputs();
            thirdPersonCamManager.Tick();
        }

        public void Awake()
        {
            rig = gameObject.GetComponent<Rigidbody>();
            thirdPersonCamManager.CamInit(transform);
            AnimatorSetup();
        }

        public void Inputs()
        {
            ver = Input.GetAxis("Vertical");
            hor = Input.GetAxis("Horizontal");
            running = Input.GetButton("Run");
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
            Vector3 v = ver * thirdPersonCamManager.transform.forward;
            Vector3 h = hor * thirdPersonCamManager.transform.right;
            moveDir = (v + h).normalized;
            float m = Mathf.Abs(hor) + Mathf.Abs(ver);
            moveVar = Mathf.Clamp01(m);
            float moveAmount = (running) ? runSpeed : walkSpeed;
            rig.velocity = moveDir * (moveAmount * moveVar);
            MovementAnimHandler();
        }

        public void IntermediateMovement()
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

        public void MovementAnimHandler()
        {
            anim.SetFloat("Vertical", moveVar, animDamp, Time.deltaTime);
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