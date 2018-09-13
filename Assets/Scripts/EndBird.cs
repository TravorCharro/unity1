using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndBird : MonoBehaviour {

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
			other.GetComponent<Player>().totalScore += (other.GetComponent<Player>().levelMaxTime - Convert.ToInt32(other.GetComponent<Player>().timer)) * 10;
			other.GetComponent<Player>().gameOver("You Win!");
		}
	}

}
