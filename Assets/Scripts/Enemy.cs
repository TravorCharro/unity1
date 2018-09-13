using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	public Vector3[] points = new Vector3[2];

	private int currentPoint;

	private bool shouldMove;

	public float speed = 1;
	private Animator animator;
	public GameObject groundCheck;
	public GameObject killingObj;
	public bool killed;
	public float rollbackTime = 3f;
	public bool rollback;
	public float rollbackTimer;

// Use this for initialization
	void Start ()
	{
		points[0] = transform.position;
		points[1] = transform.position;
		
		animator = GetComponent<Animator>();
	}

	private void FixedUpdate()
	{
		Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.transform.position, 0.1f, 0);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
			{
				points[0].y = transform.position.y;
				points[1].y = transform.position.y;
				GetComponent<Rigidbody2D>().constraints =
					RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
			}
		}
	}

	// Update is called once per frame
	void Update ()
	{
		if(rollback)
			rollbackTimer += Time.deltaTime;
		if (rollbackTimer >= rollbackTime)
		{
			rollback = false;
			rollbackTimer = 0;
		}
		if (!rollback)
		{
			if (currentPoint == 0 && shouldMove)
			{
				transform.position = Vector3.MoveTowards(transform.position, points[1], speed * Time.deltaTime);
				if (transform.position.x == points[1].x)
				{
					currentPoint = 1;
				}
			}
			if (currentPoint == 1 && shouldMove)
			{
				transform.position = Vector3.MoveTowards(transform.position, points[0], speed * Time.deltaTime);
				if (transform.position.x == points[0].x)
				{
					currentPoint = 0;
					shouldMove = false;
					killingObj.GetComponent<CircleCollider2D>().enabled = true;
					animator.SetBool("trigger", false);
					rollback = true;
				}
			}
		}

		if (transform.position.y < -200)
		{
			Destroy(this.gameObject);
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player") && !rollback)
		{
			if (other.transform.position.x <= transform.position.x)
			{
				points[1].x = points[0].x-0.7f;
			}
			else
			{
				points[1].x = points[0].x+0.7f;
			}
			animator.SetBool("trigger", true);
			killingObj.GetComponent<CircleCollider2D>().enabled = false;
			shouldMove = true;
		}
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		if (other.transform.CompareTag("Player")&&!killed)
		{
			other.gameObject.GetComponent<Player>().Damage(1);
		}
	}
}
