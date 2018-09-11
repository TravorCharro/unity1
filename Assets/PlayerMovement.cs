using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

	public Player playerController;
	
	public float runSpeed = 40f;
	private float horizontalMove = 0f;

	public bool isJumping = false;

	public Animator animator;
	public bool disableJump = false;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
		animator.SetFloat("speed", Math.Abs(horizontalMove)*Time.fixedDeltaTime);
		if (Input.GetButtonDown("Jump") && !disableJump)
		{
			isJumping = true;
			animator.SetBool("jumping", true);
		}

		animator.SetFloat("verticalSpeed", playerController.GetComponent<Rigidbody2D>().velocity.y);
		
	}

	public void onLanding()
	{
		animator.SetBool("jumping", false);
	}
	
	private void FixedUpdate()
	{
		playerController.Move(horizontalMove * Time.fixedDeltaTime, isJumping);
		isJumping = false;
	}
}
