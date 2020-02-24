using UnityEngine;

namespace ThirdPersonMovement
{
    public class ThirdPersonCamera : MonoBehaviour
    {
        [HideInInspector]
        Vector3 zoomPos = new Vector3(0.59f, -0.83f, -1.14f);

        [HideInInspector]
        Vector3 resetPos = new Vector3(0f, -0.83f, -1.9f);

        [Header("CamStats")]
        public float mouseSpeed = 2;
        public float followSpeed = 9;
        public float slerpSpeed = 9;
        public float zoomSpeed = 0.5f;
        public float turnSmoothing = 0.1f;

        [Header("Objects")]
        public Transform target;
        public Transform pivot;
        public Transform camTrans;
        public Transform aimTarget;

        [Header("Misc")]
        public float minAngle = -35;
        public float maxAngle = 35;
        public float lookAngle;
        public float tiltAngle;
        public bool aiming;

        float smoothX;
        float smoothY;
        float smoothXVel;
        float smoothYVel;



        public void CamInit(Transform t)
        {
            target = t;
            camTrans = Camera.main.transform;
            pivot = camTrans.parent;
            aimTarget = camTrans.GetChild(0);
        }

        public void Tick()
        {
            float h = Input.GetAxis("Mouse X");
            float v = Input.GetAxis("Mouse Y");

            float targetSpeed = mouseSpeed;
            aiming = target.GetComponent<ThirdPersonController>().aiming;

            if (aiming)
            {
                float aimSpeed = zoomSpeed * Time.deltaTime;
                Vector3 smoothZoom = Vector3.Lerp(camTrans.localPosition, zoomPos, aimSpeed);
                camTrans.localPosition = smoothZoom;
            }
            else
            {
                float unAimSpeed = zoomSpeed * Time.deltaTime;
                Vector3 smoothUnZoom = Vector3.Lerp(camTrans.localPosition, resetPos, unAimSpeed);
                camTrans.localPosition = smoothUnZoom;
            }

            Rotation(v, h, targetSpeed);

            FollowTarget();
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
