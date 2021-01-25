using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class player_controller : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    public Collider2D foot_coll;
    public BoxCollider2D body_coll;
    public Transform hat_positiob;
    public Text cherry_text;
    public int num_cherry;
    public float speed;
    public float jumpforce;
    public LayerMask ground;
    private bool jump_pressed;
    private bool s_pressed;
    private bool w_pressed;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && foot_coll.IsTouchingLayers(ground))
        {
            jump_pressed = true;
        }
        if (Input.GetKeyDown(KeyCode.S) && foot_coll.IsTouchingLayers(ground))
        {
            s_pressed = true;
        }
        if (Input.GetKeyDown(KeyCode.W) && foot_coll.IsTouchingLayers(ground))
        {
            w_pressed = true;
        }

    }
	void FixedUpdate()
    {
        Movement();
        jump();
        crouch();
        switch_anim();
        updateUI();
    }
    void Movement()
	{
        // Horizontal move
        float Horizontal_movement = Input.GetAxis("Horizontal");
        if (Horizontal_movement != 0)
        {
            rb.velocity = new Vector2(Horizontal_movement * speed * Time.fixedDeltaTime, rb.velocity.y);
            anim.SetFloat("run_speed", Mathf.Abs(Horizontal_movement));
        }

        // change face
        float faced_direction = Input.GetAxisRaw("Horizontal");
        if(faced_direction != 0)
		{
            transform.localScale = new Vector3(faced_direction, 1, 1);
		}
 
    }
    void jump()
	{
        //jump
        if (jump_pressed && foot_coll.IsTouchingLayers(ground))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpforce * Time.fixedDeltaTime);
            anim.SetBool("is_jump_up", true);
            jump_pressed = false;
        }
    }
    void crouch()
	{
        if (s_pressed && foot_coll.IsTouchingLayers(ground))
		{
            anim.SetBool("is_crouch", true);
            anim.SetBool("is_idle", false);
            body_coll.enabled = false;
            s_pressed = false;

        }
        if (w_pressed && foot_coll.IsTouchingLayers(ground))
        {
			if (!Physics2D.OverlapCircle(hat_positiob.position,0.2f,ground))
			{
                anim.SetBool("is_crouch", false);
                anim.SetBool("is_idle", true);
                body_coll.enabled = true;
            }
            w_pressed = false;
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
        } else if (foot_coll.IsTouchingLayers(ground))
        {
            anim.SetBool("is_jump_down", false);
            anim.SetBool("is_idle", true);
        }

	}
	//collect items
    private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.tag == "cherry")
		{
            Destroy(collision.gameObject);
            num_cherry += 1;
		}
	}
	//destory forg 
	private void OnCollisionEnter2D(Collision2D collision)
	{
		if(collision.gameObject.tag == "forg")
		{
            if(anim.GetBool("is_jump_down"))
            {
                anim.SetBool("is_jump_down", false);
                anim.SetBool("is_jump_up", true);
                anim.SetBool("is_idle", false);
                rb.velocity = new Vector2(rb.velocity.x, jumpforce * Time.fixedDeltaTime);
                Destroy(collision.gameObject);

            }
		}
	}


	void updateUI()
	{
        cherry_text.text = num_cherry.ToString();
	}
}
