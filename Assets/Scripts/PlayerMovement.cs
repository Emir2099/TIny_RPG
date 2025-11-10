using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class PlayerMovement : MonoBehaviour
{
    public float speed = 5;
    public int facingDirection = 1; // 1: right, -1: left
    public Rigidbody2D rb;
    public Animator animt;


    // FixedUpdate is called 50x per frame
    private void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (horizontal > 0 && transform.localScale.x < 0 || horizontal < 0 && transform.localScale.x > 0)
        {
            Flip();
        }


        animt.SetFloat("horizontal", Mathf.Abs(horizontal));
        animt.SetFloat("vertical", Mathf.Abs(vertical));

        rb.linearVelocity = new Vector2(horizontal * speed, vertical * speed);
    }
    
    void Flip()
    {
        facingDirection *= -1;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);   
    }
}
