using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class AIController : MonoBehaviour {

	public Transform m_target;
	public float m_followDistance;
	public float m_pathDelay;

	public float m_idleRange = 2;
	public Animator anim;

    public ParticleSystem LoveEmitter;

	[HideInInspector]
	public state m_state;

	private NavMeshAgent m_agent;
	private Vector3 m_idleDest;
	private float m_idleTurn;
	private Rigidbody m_Rigidbody; 
	private float m_prevRestTime;

	//private bool m_waved;



    public AudioClip[] m_VoiceClips;
    public AudioClip m_Voice;
    public AudioSource m_VoiceSource;

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

        var RandNumber = (int)Mathf.Floor(Random.Range(0.0f, 5.0f));

        m_Voice = m_VoiceClips[RandNumber];

        m_VoiceSource = gameObject.AddComponent<AudioSource>();

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

		if (!anim.GetBool("isWalking"))
		{
			anim.SetBool("isWalking", true);

			Debug.Log("Walking");
		}
		else if ( anim.GetBool("isWaving"))
		{
			anim.SetBool("isWalking", false);
		}
	}

	void Chilling ()
	{
		// Check -> Last path is complete AND no new path AND pad time exceeded 
		if ((m_agent.pathStatus == NavMeshPathStatus.PathComplete ) && !m_agent.pathPending && (Time.time - m_prevRestTime > m_pathDelay) )
		{
			m_idleDest *= -1; // Switch direction

			m_agent.SetDestination (m_Rigidbody.position  + m_idleDest); // set destination

			m_prevRestTime = Time.time; // update time
		}
	}

	void Following (){
		// Check -> Last path is complete AND no new path AND pad time exceeded AND target has new destination
		if (((  m_agent.pathStatus == NavMeshPathStatus.PathComplete ) &&
               !m_agent.pathPending &&
               (Time.time - m_prevRestTime > m_pathDelay) &&
			(m_agent.destination != m_target.position) ) 
			|| (Time.time - m_prevRestTime > m_pathDelay && anim.GetBool("isWaving")) ){

			if(anim.GetBool("isWaving"))
				anim.SetBool("isWaving", false);
			m_agent.Resume();

			m_agent.SetDestination (m_target.position); // set destingnation

			m_prevRestTime = Time.time; // update time

		}


	}

	public void WaveTrigger()
	{
		Debug.Log("Waving Trigger!");
		if (!anim.GetBool("isWaving") )
		{
			m_agent.Stop ();
			anim.SetBool("isWaving", true);
			Debug.Log("Waving Trigger!");

		}

	}

    public void LoveTrigger()
    {
        LoveEmitter.GetComponent<ParticleSystem>().Play();

        if (!m_VoiceSource.isPlaying)
        {
            m_VoiceSource.PlayOneShot(m_Voice);
        }

        Debug.Log(LoveEmitter.IsAlive());
    }

    void OnTriggerEnter(Collider other) {
		if (other.attachedRigidbody) {
			other.attachedRigidbody.AddForce (Vector3.up * 10); 
		}
	}
}
