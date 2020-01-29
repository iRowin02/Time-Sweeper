﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class StateManager : MonoBehaviour
    {
        public GameObject activeModel;

        int currentLayer = 8;
        int layerNine = 9;

        [Header("Inputs")]
        public float ver;
        public float hor;
        public float animOffset = 0.4f;
        public float groundOffset = 0.3f;
        public float moveAmount;
        public Vector3 moveDir;

        [Header("Stats")]
        public float moveSpeed = 2;
        public float runSpeed = 3.5f;
        public float toGround = 0.5f;

        [Header("States")]
        public bool onGround;
        public bool run;

        [Header("RigStats")]
        public int rigAngDrag = 999;
        public int rigDrag = 4;

        [HideInInspector]
        public float delta;

        [HideInInspector]
        public Animator anim;
        [HideInInspector]
        public Rigidbody rig;
        [HideInInspector]
        public LayerMask ignoreLayer;

        public void Init()
        {
            SetupAnimtor();
            rig = GetComponent<Rigidbody>();
            rig.angularDrag = rigAngDrag;
            rig.drag = rigDrag;
            rig.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

            gameObject.layer = currentLayer;
            ignoreLayer = ~(1 << layerNine);
        }

        public void SetupAnimtor()
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

            anim.applyRootMotion = false;
        }

        public void Tick(float d)
        {
            d = delta;
            onGround = OnGround();
        }

        public void FixedTick(float d)
        {
            delta = d;

            rig.drag = (moveAmount > 0) ? 0 : rigDrag;

            float targetSpeed = moveSpeed;
            if (run)
            {
                targetSpeed = runSpeed;
            }
            rig.velocity = moveDir * (targetSpeed * moveAmount);
            MovementAnimationHandler();
        }

        public void MovementAnimationHandler()
        {
            anim.SetFloat("Vertical", moveAmount, animOffset, delta);
        }

        public bool OnGround()
        {
            bool r = false;

            Vector3 origin = transform.position + (Vector3.up * toGround);
            Vector3 dir = -Vector3.up;
            float dis = toGround - groundOffset;
            RaycastHit hit;
            if(Physics.Raycast(origin, dir, out hit, dis, ignoreLayer))
            {
                r = true;
                Vector3 targetPosition = hit.point;
                transform.position = targetPosition;
            }

            return r;
        }
    }
}