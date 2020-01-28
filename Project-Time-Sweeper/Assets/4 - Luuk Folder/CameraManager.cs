using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class CameraManager : MonoBehaviour
    {
        public bool lockOn;

        public float mouseSpeed = 2;
        public float followSpeed = 9;

        public float turnSmoothing = 0.1f;
        public float minAngle = -35;
        public float maxAngle = 35;

        float smoothX;
        float smoothY;
        float smoothXVel;
        float smoothYVel;
        public float lookAngle;
        public float tiltAngle;

        public Transform target;
        public Transform pivot;
        public Transform camTrans;

        public void Init(Transform t)
        {
            target = t;
            camTrans = Camera.main.transform;
            pivot = camTrans.parent;
        }

        public void Tick(float d)
        {
            float h = Input.GetAxis("Mouse X");
            float v = Input.GetAxis("Mouse Y");

            float targetSpeed = mouseSpeed;

            FollowTarget(d);
            Rotation(d, v, h, targetSpeed);
        }

        public void Rotation(float d, float v, float h, float targetSpeed)
        {
            if (turnSmoothing > 0)
            {
                smoothX = Mathf.SmoothDamp(smoothX, h, ref smoothXVel, turnSmoothing);
                smoothY = Mathf.SmoothDamp(smoothY, v, ref smoothYVel, turnSmoothing);
            }
            else
            {
                smoothX = h;
                smoothY = v;
            }

            if (lockOn)
            {

            }

            lookAngle += smoothX * targetSpeed;
            transform.rotation = Quaternion.Euler(0, lookAngle, 0);

            tiltAngle -= smoothY * targetSpeed;
            tiltAngle = Mathf.Clamp(tiltAngle, minAngle, maxAngle);
            pivot.localRotation = Quaternion.Euler(tiltAngle, 0, 0);

        }

        public static CameraManager singleton;

        public void Awake()
        {
            singleton = this;
        }

        void FollowTarget(float d)
        {
            float speed = d * followSpeed;
            Vector3 targetPosition = Vector3.Lerp(transform.position, target.position, speed);
            target.position = targetPosition;
        }

    }
}
