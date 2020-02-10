using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class Helper : MonoBehaviour
    {
        [Range(-1, 1)]
        public float vertical;
        [Range(-1, 1)]
        public float horizontal;
        public float usingItemClamp = 0.5f;

        public string[] ohAttacks;
        public string[] thAttacks;

        public Animator animControl;

        public bool playAnim;
        public bool twohanded;
        public bool useItem;
        public bool lockOn;

        public bool interacting;
        public bool enableRM;

        void Start()
        {
            animControl = GetComponent<Animator>();
        }

        void Update()
        {
            enableRM = !animControl.GetBool("CanMove");
            animControl.applyRootMotion = enableRM;

            if (enableRM)
                return;


            if (lockOn == false)
            {
                horizontal = 0;
                vertical = Mathf.Clamp01(vertical);
            }

            UsingItem();
            AttackAnim();

            animControl.SetFloat("Horizontal", horizontal);
            animControl.SetFloat("Vertical", vertical);
        }

        public void UsingItem()
        {
            interacting = animControl.GetBool("Interacting");


            if (useItem)
            {
                animControl.Play("use_item");
                useItem = false;
            }

            if (interacting)
            {
                vertical = Mathf.Clamp(vertical, 0, usingItemClamp);
                playAnim = false;
            }
        }

        public void AttackAnim()
        {
            animControl.SetBool("TwoHandedWeapon", twohanded);

            if (playAnim)
            {
                string targetAnim;

                if (twohanded == false)
                {
                    int r = Random.Range(0, ohAttacks.Length);
                    targetAnim = ohAttacks[r];
                }
                else
                {
                    int r = Random.Range(0, thAttacks.Length);
                    targetAnim = thAttacks[r];
                }
                vertical = 0;
                animControl.CrossFade(targetAnim, 0.2f);
                playAnim = false;
            }
        }
    }
}
