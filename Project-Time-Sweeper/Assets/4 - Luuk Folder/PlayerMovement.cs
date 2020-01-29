using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class PlayerMovement : MonoBehaviour
    {
        public float ver;
        public float hor;

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
            GetInput();
            UpdateStates();
            delta = Time.fixedDeltaTime;
            states.FixedTick(delta);
        }

        public void Update()
        {
            cameraManager.Tick(delta);
            states.Tick(delta);
        }

        public void GetInput()
        {
            ver = Input.GetAxis("Vertical");
            hor = Input.GetAxis("Horizontal");
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
        }
    }
}
