using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public float m_Speed = 1;
	public float m_TurnSpeed = 2;
	private string m_MovementAxisName;     
	private string m_TurnAxisName;         
	private Rigidbody m_Rigidbody;  
	private float m_MovementInputValue;    
	private float m_TurnInputValue;    

	// Use this for initialization
	void Start () {
		m_Rigidbody = GetComponent<Rigidbody>();
		m_MovementInputValue = 0f;
		m_MovementAxisName = "Vertical";
		m_TurnAxisName = "Horizontal";
		m_MovementInputValue = 0f;
		m_TurnInputValue = 0f;
	}
	
	// Update is called once per frame
	private void Update()
	{
		// Store the player's input and make sure the audio for the engine is playing.
		m_MovementInputValue = Input.GetAxis(m_MovementAxisName);
		m_TurnInputValue = Input.GetAxis(m_TurnAxisName);

	}

	void FixedUpdate () {
		// Move and turn the tank.
		Move();
		Turn ();


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
			AIController ai = other.GetComponent<AIController>();
			ai.m_target = m_Rigidbody.transform;
			ai.m_state = AIController.state.following;
		}
	}
}
