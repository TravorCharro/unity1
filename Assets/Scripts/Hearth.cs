using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hearth : MonoBehaviour {

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
			if (other.GetComponent<Player>().Health < 3)
			{
				other.GetComponent<Player>().Health++;
				Destroy(this.gameObject);
			}
		}
	}
}
