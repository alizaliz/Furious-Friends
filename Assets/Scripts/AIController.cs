using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class AIController : MonoBehaviour {

	public Transform m_target;
	public float m_followDistance;
	public float m_pathDelay;

	public float m_idleRange = 2;

	[HideInInspector]
	public state m_state;

	private NavMeshAgent m_agent;
	private Vector3 m_idleDest;
	private Rigidbody m_Rigidbody; 
	private float m_prevRestTime;

	public enum state
	{
		chilling,
		following
	}



	// Use this for initialization
	void Start () {
		m_state = state.chilling;
		m_idleRange = Random.Range (0, m_idleRange);
		m_idleDest = transform.forward * m_idleRange;
		m_agent.SetDestination (m_Rigidbody.position  + m_idleDest);
		m_prevRestTime = Time.time;

	}

	void Awake(){

		m_agent = GetComponent<NavMeshAgent>();
		m_Rigidbody = GetComponent<Rigidbody>();

	}

	// Update is called once per frame
	void Update () {
		switch(m_state){
		case state.chilling:	
			Chilling ();
			break;
		case state.following:
			Following ();
			break;
		}
	}

	void Chilling ()
	{
		// Check -> Last path is complete AND no new path AND pad time exceeded 
		if ((m_agent.pathStatus == NavMeshPathStatus.PathComplete ) && !m_agent.pathPending && (Time.time - m_prevRestTime > m_pathDelay) )
		{
				m_idleDest *= -1; // Switch direction
				m_agent.SetDestination (m_Rigidbody.position  + m_idleDest); // set destingnation



				m_prevRestTime = Time.time; // update time

		}

	}

	void Following (){
		// Check -> Last path is complete AND no new path AND pad time exceeded AND target has new destination
		if ((  m_agent.pathStatus == NavMeshPathStatus.PathComplete ) && !m_agent.pathPending && (Time.time - m_prevRestTime > m_pathDelay) && (m_agent.destination != m_target.position)) {

			m_agent.SetDestination (m_target.position); // set destingnation

			m_prevRestTime = Time.time; // update time

		}
	}

	void OnTriggerEnter(Collider other) {
		if (other.attachedRigidbody)
			other.attachedRigidbody.AddForce(Vector3.up * 10);        
	}
}
