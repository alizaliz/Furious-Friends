﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [Header("Cone Raycasting")]
    public int m_ConeRays = 12;
    public float m_ConeAngle = 90;

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
    public Animator anim;

	private bool m_waved;
    public static bool m_looking;

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
        m_looking = false;
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
		// Move and turn the tank.
		Move();
		Turn ();
        Wave();

	}

	void Wave(){
        if (m_waved == true && !anim.GetBool("isWaving"))
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
                RaycastHit[] hits = Physics.RaycastAll(transform.position + Vector3.up, rayDir, 10.0f);
                //Debug.DrawLine(transform.position + Vector3.up, transform.position + Vector3.up + rayDir * 10.0f);

                foreach (RaycastHit hit in hits)
                {
                    //Debug.DrawLine(transform.position, hit.transform.position, Color.blue);

                    if (hit.transform.CompareTag("Friend"))
                    {
                        coneHits.Add(hit);
                    }
                }
            }

            foreach (RaycastHit hit in coneHits)
            {
                Debug.Log("here");
                Debug.DrawLine(hit.transform.position, transform.position, Color.red);
                float dot = Vector2.Dot(new Vector2(hit.transform.forward.x, hit.transform.forward.z), new Vector2(transform.position.x - hit.transform.position.x, transform.position.z - hit.transform.position.z));
                if (dot > 0.9f)
                {
                    //Debug.DrawLine(hit.transform.position, hit.transform.position + Vector3.up, Color.green, 5.0f);

                    AIController ai = hit.collider.GetComponent<AIController>();
                    ai.m_target = m_Rigidbody.transform;
                    ai.m_state = AIController.state.following;
                }
                else
                {
                    //Debug.DrawLine(hit.transform.position, hit.transform.position + Vector3.up, Color.red, 5.0f);
                }
            }

        
            anim.SetBool("isWaving", true);
        }
    }

	private void Move()
	{
		// Adjust the position of the tank based on the player's input.

		Vector3 movement = transform.forward * m_MovementInputValue * m_Speed * Time.deltaTime;

		m_Rigidbody.MovePosition (m_Rigidbody.position + movement);
	}


	private void Turn()
	{
		// Adjust the rotation of the tank based on the player's input.

		float turn = m_TurnInputValue * m_TurnSpeed * Time.deltaTime;

		Quaternion turnRotation = Quaternion.Euler (0f, turn, 0f);

		m_Rigidbody.MoveRotation (m_Rigidbody.rotation * turnRotation);

	}

	void OnTriggerEnter(Collider other) 
	{
		if (other.gameObject.CompareTag ("Friend"))
		{

           // other.attachedRigidbody.AddForce(Vector3.up * 10);

            /*
     
			ai.m_target = m_Rigidbody.transform;
			ai.m_state = AIController.state.following;

            */

        }
    }
}
