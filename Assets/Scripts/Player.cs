using System;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
	[SerializeField] private float m_JumpForce = 400f;							// Amount of force added when the player jumps.
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;	// How much to smooth out the movement
	[SerializeField] private bool m_AirControl = false;							// Whether or not a player can steer while jumping;
	[SerializeField] private LayerMask m_WhatIsGround;							// A mask determining what is ground to the character
	[SerializeField] private Transform m_GroundCheck;							// A position marking where to check if the player is grounded.

	public int health = 2;
	public int carrots = 0;
	private Vector2 spawnPos;
	const float k_GroundedRadius = .02f; // Radius of the overlap circle to determine if grounded
	private bool m_Grounded;            // Whether or not the player is grounded.
	private Rigidbody2D m_Rigidbody2D;
	private bool m_FacingRight = true;  // For determining which way the player is currently facing.
	private Vector3 m_Velocity = Vector3.zero;
	private bool noDamage, startBlinking;
	private float spriteBlinkingTotalTimer, spriteBlinkingTimer;
	public float noDamageTime = 3f;
	public Sprite[] healthSprites = new Sprite[4];
	public float timer;
	public int levelMaxTime = 120;
	public int totalScore;
	public GameObject gameOverObj;
	private bool gameEnded;
	public GameObject timerObj;

	public UnityEngine.UI.Text carrotsText;
	public UnityEngine.UI.Image healthImage;
	

	[Header("Events")]
	[Space]

	public UnityEvent OnLandEvent;

	[System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }

	private void Awake()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D>();
		if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();
	}

	private void Start()
	{
		spawnPos = this.transform.position;
		timerObj.GetComponent<Text>().text = "Time: "+levelMaxTime;
		healthImage.sprite = healthSprites[health];
		carrotsText.text = carrots.ToString();
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.DrawSphere(m_GroundCheck.position, k_GroundedRadius);
	}

	private void FixedUpdate()
	{
		if (timer < levelMaxTime)
		{
			timer += Time.deltaTime;
			timerObj.GetComponent<Text>().text = "Time: "+(levelMaxTime-Convert.ToInt32(timer)).ToString();
		}
		if (timer >= levelMaxTime && !gameEnded)
		{

			gameOver();
		}

		if (health <= 0 && !gameEnded)
		{
			gameOver();
		}

		healthImage.sprite = healthSprites[health];
		carrotsText.text = carrots.ToString();
		bool wasGrounded = m_Grounded;
		m_Grounded = false;
		if (startBlinking)
		{
			SpriteBlinkingEffect();
		}
		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		// This can be done using layers instead but Sample Assets will not overwrite your project settings.
		Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
			{
				m_Grounded = true;
				if (!wasGrounded)
					OnLandEvent.Invoke();
			}
		}
	}


	public void Move(float move, bool jump)
	{
		//only control the player if grounded or airControl is turned on
		if (m_Grounded || m_AirControl)
		{
			// Move the character by finding the target velocity
			Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
			// And then smoothing it out and applying it to the character
			m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

			// If the input is moving the player right and the player is facing left...
			if (move > 0 && !m_FacingRight)
			{
				// ... flip the player.
				Flip();
			}
			// Otherwise if the input is moving the player left and the player is facing right...
			else if (move < 0 && m_FacingRight)
			{
				// ... flip the player.
				Flip();
			}
		}
		// If the player should jump...
		if (m_Grounded && jump)
		{
			// Add a vertical force to the player.
			m_Grounded = false;
			m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
		}
	}


	public void Damage(int value)
	{
		if (value > 0)
		{
			if (!noDamage)
			{
				if (health >= 0)
				{
					healthImage.sprite = healthSprites[health];
					this.gameObject.GetComponent<Animator>().Play("Damage");
				}
				if (health > 0)
				{
					health = health - value;
					Invoke("DisableNoDamage", noDamageTime);
					startBlinking = true;
					noDamage = true;
				}
			}
		}
	}

	public void Heal(int value)
	{
		if (value > 0)
		{
			this.Health = health + value;	
		}
	}

	public void gameOver(string setText = "")
	{
		gameEnded = true;
		this.GetComponent<PlayerMovement>().enabled = false;
		totalScore += carrots * 1000;
		totalScore += (levelMaxTime - Convert.ToInt32(timer)) * 10;
		if (setText.Length == 0)
		{
			if (timer < levelMaxTime)
			{
				gameOverObj.transform.GetComponentInChildren<Text>().text = "Game Over!\nYou Died!";
			}
			else
			{
				gameOverObj.transform.GetComponentInChildren<Text>().text = "Game Over!\nTime is Over!";
			}
		}
		else
		{
			gameOverObj.transform.GetComponentInChildren<Text>().text = setText;
		}

		gameOverObj.transform.GetComponentInChildren<Text>().text += "\nScore: "+totalScore;
		timerObj.SetActive(false);
		gameOverObj.gameObject.SetActive(true);
	}
	
	void DisableNoDamage()
	{
		noDamage = false;
	}

	private void Flip()
	{
		// Switch the way the player is labelled as facing.
		m_FacingRight = !m_FacingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	private void SpriteBlinkingEffect()
	{
		spriteBlinkingTotalTimer += Time.deltaTime;
		if (spriteBlinkingTotalTimer >= noDamageTime)
		{
			startBlinking = false;
			spriteBlinkingTotalTimer = 0.0f;
			this.gameObject.GetComponent<SpriteRenderer>().enabled = true; // according to 
			//your sprite
			return;
		}

		spriteBlinkingTimer += Time.deltaTime;
		if (spriteBlinkingTimer >= 0.2f)
		{
			spriteBlinkingTimer = 0.0f;
			if (this.gameObject.GetComponent<SpriteRenderer>().enabled == true)
			{
				this.gameObject.GetComponent<SpriteRenderer>().enabled = false; //make changes
			}
			else
			{
				this.gameObject.GetComponent<SpriteRenderer>().enabled = true; //make changes
			}
		}
	}

	public int Health
	{
		get { return health; }
		set
		{
			if (health < 3 && health >0)
			{
				health = value;
				healthImage.sprite = healthSprites[health - 1];
			}
		}
	}
	public int Carrots
	{
		get { return carrots; }
		set
		{
			carrotsText.text = value.ToString();
		}
	}

}