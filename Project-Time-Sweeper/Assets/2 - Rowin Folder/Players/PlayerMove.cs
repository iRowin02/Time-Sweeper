using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
   public Rigidbody rb;

    public Vector3 dir;

    public float movementSpeed, jumpHeight;
    public bool canJump;
    

    private void Start()
    {
        canJump = true;
        Physics.IgnoreLayerCollision(8, 9);    
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Move();
    }

    public void Move()
    {
        float horiInput = Input.GetAxis("Horizontal");
        float vertInput = Input.GetAxis("Vertical");

        dir = new Vector3(horiInput, 0, vertInput);

        transform.Translate(dir * movementSpeed * Time.deltaTime);

        if(canJump)
        {
            if(Input.GetButtonDown("Jump"))
            {
                rb.AddForce(0,jumpHeight,0);
                canJump = false;
            }
        }
    }
    void OnCollisionEnter(Collision coll)
    {
        if(coll.transform.CompareTag("Floor"))
        {
            canJump = true;
        }
    }
}
