using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class killEnemy : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			transform.parent.GetComponent<Animator>().Play("Idle");
			transform.parent.GetComponent<Animator>().enabled = false;
			transform.parent.GetComponent<Enemy>().killed = true;
			transform.parent.GetComponent<SpriteRenderer>().flipY = true;
			Destroy(transform.parent.GetComponent<BoxCollider2D>());
		}
	}
}
