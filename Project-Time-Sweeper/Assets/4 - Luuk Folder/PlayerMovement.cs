using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class PlayerMovement : MonoBehaviour
    {
        public float ver;
        public float hor;

        public bool runInput;
        public bool cursorLockMode;

        float delta;

        StateManager states;
        CameraManager cameraManager;

        void Start()
        {
            states = GetComponent<StateManager>();
            states.Init();

            cameraManager = CameraManager.singleton;
            cameraManager.Init(transform);
        }

        public void FixedUpdate()
        {
            delta = Time.fixedDeltaTime;
            GetInput();
            UpdateStates();
            states.FixedTick(delta);
        }

        public void Update()
        {
            delta = Time.deltaTime;
            cameraManager.Tick(delta);
            states.Tick(delta);
        }

        public void GetInput()
        {
            ver = Input.GetAxis("Vertical");
            hor = Input.GetAxis("Horizontal");
            runInput = Input.GetButton("Run");
            cursorLockMode = Input.GetButton("Cancel");
            states.lockOn = Input.GetButton("RightMouse");
        }

        public void UpdateStates()
        {
            states.hor = hor;
            states.ver = ver;

            Vector3 v = ver * cameraManager.transform.forward;
            Vector3 h = hor * cameraManager.transform.right;
            states.moveDir = (v + h).normalized;
            float m = Mathf.Abs(hor) + Mathf.Abs(ver);
            states.moveAmount = Mathf.Clamp01(m);

            if (runInput)
            {
                states.run = states.moveAmount > 0;
            }
            else
            {
                states.run = false;
            }

            if (cursorLockMode)
            {
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
            }

            if(states.lockOn != cameraManager.lockOn)
            {
                cameraManager.lockOn = states.lockOn;

                if(states.lockOnTarget != null)
                {
                    cameraManager.lockOnTarget = states.lockOnTarget.transform;
                    states.lockOn = !states.lockOn;
                }
            }

        }
    }
}
