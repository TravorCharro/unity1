using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	private Animator animator;

	public Vector2[] movePath = new Vector2[2];

	public bool moved = true;
	// Use this for initialization
	void Start ()
	{
		animator = this.GetComponent<Animator>();
		movePath[0] = this.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if (!moved)
		{
			animator.Play("Attack");
			
			//this.transform.position = Vector2.MoveTowards(this.transform.position, , 1 * Time.deltaTime); TODO: ДОДЕЛАТЬ
		}
//		if (this.transform.position.Equals(MoveTo))
//		{
//			moved = true;
//		} 
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			animator.Play("Idle");
//			MoveTo = (this.transform.position.x < other.transform.position.x)
//				? new Vector2(this.transform.position.x + 1, this.transform.position.y)
//				: new Vector2(this.transform.position.x - 1, this.transform.position.y);
			moved = false;
		}
	}
}
