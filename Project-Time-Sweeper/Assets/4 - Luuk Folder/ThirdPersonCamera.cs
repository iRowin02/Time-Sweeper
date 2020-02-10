using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThirdPersonMovement
{
    public class ThirdPersonCamera : MonoBehaviour
    {
        [Header("CamStats")]
        public float mouseSpeed = 2;
        public float followSpeed = 9;
        public float slerpSpeed = 9;
        public float turnSmoothing = 0.1f;

        [Header("Objects")]
        public Transform target;
        public Transform lockOnTarget;
        public Transform pivot;
        public Transform camTrans;

        [Header("Misc")]
        public float minAngle = -35;
        public float maxAngle = 35;
        float smoothX;
        float smoothY;
        float smoothXVel;
        float smoothYVel;

        public float lookAngle;
        public float tiltAngle;


        public void CamInit(Transform t)
        {
            target = t;
            camTrans = Camera.main.transform;
            pivot = camTrans.parent;
        }

        public void Tick()
        {
            float h = Input.GetAxis("Mouse X");
            float v = Input.GetAxis("Mouse Y");

            float targetSpeed = mouseSpeed;

            Vector3 targetDir = moveDir;
            targetDir.y = 0;
            if (targetDir == Vector3.zero)
            {
                targetDir = transform.forward;
            }
            Quaternion tr = Quaternion.LookRotation(targetDir);
            Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, Time.deltaTime * moveAmount * rotationSpeed);
            transform.rotation = targetRotation;

            FollowTarget();
            Rotation(v, h, targetSpeed);
        }

        public void Rotation(float v, float h, float targetSpeed)
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

            tiltAngle -= smoothY * targetSpeed;
            tiltAngle = Mathf.Clamp(tiltAngle, minAngle, maxAngle);
            pivot.localRotation = Quaternion.Euler(tiltAngle, 0, 0);

            lookAngle += smoothX * targetSpeed;
            transform.rotation = Quaternion.Euler(0, lookAngle, 0);
        }

        void FollowTarget()
        {
            float speed = Time.deltaTime * followSpeed;
            Vector3 targetPosition = Vector3.Lerp(transform.position, target.position, speed);
            target.position = targetPosition;
        }
    }
}
