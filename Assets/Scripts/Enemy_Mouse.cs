using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Mouse : MonoBehaviour {
	private bool flipped;

	public bool killed;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		GameObject player = GameObject.FindGameObjectWithTag("Player");
		if (player.transform.position.x > transform.position.x)
		{
			flipped = false;
		}
		else
		{
			flipped = true;
		}

		GetComponent<SpriteRenderer>().flipX = flipped;
		
		if (transform.position.y < -200)
		{
			Destroy(this.gameObject);
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
