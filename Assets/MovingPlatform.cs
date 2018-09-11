using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
	private Collider2D[] collideWith;
	public GameObject platformObj;
	public Transform[] points;
	public float speed = 1;
	public Transform currentPoint;
	public int pointID;
	// Use this for initialization
	void Start ()
	{
		currentPoint = points[Math.Abs(pointID)];
	}
	
	
	// Update is called once per frame
	void Update ()
	{
		
		platformObj.transform.position = Vector2.MoveTowards(platformObj.transform.position, currentPoint.position, speed*Time.deltaTime);
		if (platformObj.transform.position == currentPoint.position)
		{
			if (pointID != points.Length - 1)
			{
				pointID++;
			}
			else
			{
				pointID *= -1;
			}
			currentPoint = points[Math.Abs(pointID)];
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.transform.CompareTag("Player"))
		{
			other.transform.parent = GameObject.FindGameObjectWithTag("MovingPlatform").transform;
		}
	}


	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.transform.CompareTag("Player"))
		{
			other.transform.parent = null;
		}
	}
}
