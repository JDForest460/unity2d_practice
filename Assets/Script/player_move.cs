using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_move : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    public Collider2D coll;
    public float speed;
    public float jumpforce;
    public LayerMask ground;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Movement();
        switch_anim();
    }

    void Movement()
	{
        // Horizontal move
        float Horizontal_movement = Input.GetAxis("Horizontal");
        if (Horizontal_movement != 0)
        {
            rb.velocity = new Vector2(Horizontal_movement * speed * Time.deltaTime, rb.velocity.y);
            anim.SetFloat("run_speed", Mathf.Abs(Horizontal_movement));
        }

        // change face
        float faced_direction = Input.GetAxisRaw("Horizontal");
        if(faced_direction != 0)
		{
            transform.localScale = new Vector3(faced_direction, 1, 1);
		}
        //jump

		if (Input.GetKeyDown(KeyCode.Space) && coll.IsTouchingLayers(ground))
		{
            rb.velocity = new Vector2(rb.velocity.x, jumpforce * Time.deltaTime);
            anim.SetBool("is_jump_up", true);
        }
    }

    void switch_anim()
	{
        anim.SetBool("is_idle", true);
        if (anim.GetBool("is_jump_up"))
        {
            if (rb.velocity.y < 0)
            {
                anim.SetBool("is_jump_up", false);
                anim.SetBool("is_jump_down", true);
            }
        } else if (coll.IsTouchingLayers(ground))
        {
            anim.SetBool("is_jump_down", false);
            anim.SetBool("is_idle", true);
        }

	}
}
