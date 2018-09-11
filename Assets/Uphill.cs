using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Uphill : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	 private void OnCollisionEnter2D(Collision2D other)
	{
		if (other.transform.CompareTag("Player"))
		{
			other.gameObject.GetComponent<PlayerMovement>().disableJump = true;
		}
	}

	private void OnCollisionExit2D(Collision2D other)
	{
		if (other.transform.CompareTag("Player"))
		{
			other.gameObject.GetComponent<PlayerMovement>().disableJump = false;
		}
	}
}
