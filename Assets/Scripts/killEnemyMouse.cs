using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class killEnemyMouse : MonoBehaviour
{


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			transform.parent.GetComponent<Enemy_Mouse>().killed = true;
			transform.parent.GetComponent<SpriteRenderer>().flipY = true;
			Destroy(transform.parent.GetComponent<CapsuleCollider2D>());
			other.GetComponent<Player>().totalScore += 500;
		}
	}
}
