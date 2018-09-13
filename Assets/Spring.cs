using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : MonoBehaviour
{
	public float force = 500;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.transform.CompareTag("Player"))
		{
			this.GetComponent<Animator>().Play("Release");
			other.GetComponent<Animator>().SetBool("jumping", true);
			other.GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);
			other.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, force));
			Debug.Log(other.GetComponent<Rigidbody2D>().velocity.y);
		}
	}
}
