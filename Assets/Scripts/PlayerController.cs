﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [Header("Cone Raycasting")]
    public int m_ConeRays;
    public float m_ConeAngle;

	public float m_Speed = 1;
	public float m_TurnSpeed = 2;
	  
	[HideInInspector] public GameObject m_Instance; 
	[HideInInspector] public int m_PlayerNumber;            // This specifies which player this the manager for.

	private string m_MovementAxisName;     
	private string m_TurnAxisName;         
	private string m_FireButt;   
	private Rigidbody m_Rigidbody;  
	private float m_MovementInputValue;    
	private float m_TurnInputValue;
       
    // Tag on Object to check for raycast collision, can change this to a more descriptive string
    public string friends = "Friend";
    public bool checkAllTags = false;
    public Transform playerPosition;
    // Wave range from player to AI
    public float raycastDist = 500f;
    public Animator anim;

	private bool m_waved;

    // Use this for initialization
    void Start () {
		m_Rigidbody = GetComponent<Rigidbody>();
		m_MovementInputValue = 0f;
		m_MovementAxisName = "Vertical" + m_PlayerNumber;
		m_TurnAxisName = "Horizontal" + m_PlayerNumber;
		m_FireButt = "Fire" + + m_PlayerNumber;
		m_MovementInputValue = 0f;
		m_TurnInputValue = 0f;

		m_waved = false;	

	}
	
    private void Wave()
    {
        // Check for raycast hit
        HashSet<RaycastHit> coneHits = new HashSet<RaycastHit>();

        float forwardAngle = Mathf.Atan2(transform.forward.z, transform.forward.x);
        forwardAngle -= Mathf.Deg2Rad * 90.0f;
        float startAngleOffset = (Mathf.Deg2Rad * m_ConeAngle) / 2.0f;
        for (int i = 0; i < m_ConeRays; i++)
        {
            float newAngle = forwardAngle + startAngleOffset + (i * (Mathf.Deg2Rad * m_ConeAngle / m_ConeRays));
            Vector3 rayDir = new Vector3(Mathf.Cos(newAngle), 0.0f, Mathf.Sin(newAngle));
            RaycastHit[] hits = Physics.RaycastAll(transform.position, rayDir, 10.0f);
            
            foreach (RaycastHit hit in hits)
            {
                if (hit.transform.CompareTag("Friend"))
                {
                    coneHits.Add(hit);
                }
            }
        }

        if (m_waved == true && !anim.GetBool("isWaving"))
        {
            anim.SetBool("isWaving", true);
            Debug.Log("Waving");
        }
    }


	// Update is called once per frame
	private void Update()
	{
        // Store the player's input and make sure the audio for the engine is playing.
        m_MovementInputValue = Input.GetAxis(m_MovementAxisName);
		m_TurnInputValue = Input.GetAxis(m_TurnAxisName);
		m_waved = Input.GetButton (m_FireButt);
	
    }

	void FixedUpdate () {
		// Move and turn the player.
		Move();
		Turn ();
		Wave ();
	}

	private void Move()
	{
		// Adjust the position of the player based on the player's input.

		Vector3 movement = transform.forward * m_MovementInputValue * m_Speed * Time.deltaTime;

		m_Rigidbody.MovePosition (m_Rigidbody.position + movement);
	}


	private void Turn()
	{
		// Adjust the rotation of the player based on the player's input.

		float turn = m_TurnInputValue * m_TurnSpeed * Time.deltaTime;

		Quaternion turnRotation = Quaternion.Euler (0f, turn, 0f);

		m_Rigidbody.MoveRotation (m_Rigidbody.rotation * turnRotation);

	}

	void OnTriggerEnter(Collider other) 
	{
		if (other.gameObject.CompareTag ("Friend"))
		{
			AIController ai = other.GetComponent<AIController>();
			ai.m_target = m_Rigidbody.transform;
			ai.m_state = AIController.state.following;
			 
		}
	}
}
