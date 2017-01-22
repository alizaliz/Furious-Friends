using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    [Header("Cone Raycasting")]
    public int m_ConeRays;
    public float m_ConeAngle;

    public float m_Speed = 1;
	public float m_TurnSpeed = 2;
	public Transform m_SpawnPoint;     
	[HideInInspector] public GameObject m_Instance; 
	[HideInInspector] public int m_PlayerNumber;            // This specifies which player this the manager for.

	private string m_MovementAxisName;     
	private string m_TurnAxisName;
    private string m_FireButt;

    private Rigidbody m_Rigidbody;  
	private float m_MovementInputValue;    
	private float m_TurnInputValue;
    private float m_DeadHeight = -5.0f;

    public Animator anim;
    public ParticleSystem WaveEmitter;

    private bool m_waved;
    public static bool m_looking;


    // Use this for initialization
    void Start () {
		m_Rigidbody = GetComponent<Rigidbody>();

		m_MovementAxisName = "Vertical" + m_PlayerNumber;
		m_TurnAxisName = "Horizontal" + m_PlayerNumber;
        m_FireButt = "Fire" + m_PlayerNumber;
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

        if (transform.position.y <= m_DeadHeight)
        {
            RespawnPlayer();
        }
	}

    void Wave() {
        // Check for raycast hit
        if (m_waved) {
            WaveEmitter.GetComponent<ParticleSystem>().Play();

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
                //Debug.Log("here");
                //Debug.DrawLine(hit.transform.position, transform.position, Color.red);
                float dot = Vector2.Dot(new Vector2(hit.transform.forward.x, hit.transform.forward.z), new Vector2(transform.position.x - hit.transform.position.x, transform.position.z - hit.transform.position.z));
                if (dot > 0.9f)
                {
                    //Debug.DrawLine(hit.transform.position, hit.transform.position + Vector3.up, Color.green, 5.0f);

                    AIController ai = hit.collider.GetComponent<AIController>();
                    ai.m_target = m_Rigidbody.transform;
                    ai.m_state = AIController.state.following;

                    ai.LoveTrigger();
					ai.WaveTrigger ();
                }
                else
                {
                    //Debug.DrawLine(hit.transform.position, hit.transform.position + Vector3.up, Color.red, 5.0f);
                }
            }
        }
        if (m_waved == true && !anim.GetBool("isWaving") )
		{
			anim.SetBool("isWaving", true);
			//Debug.Log("Waving");
		}
        else if (m_waved == false && anim.GetBool("isWaving"))
        {
            anim.SetBool("isWaving", false);
        }
	}

	private void Move()
	{
		// Adjust the position of the tank based on the player's input. Can't move while waving.
        if (!m_waved)
        {
		    Vector3 movement = transform.forward * m_MovementInputValue * m_Speed * Time.deltaTime;

		    m_Rigidbody.MovePosition (m_Rigidbody.position + movement);

            if ((m_MovementInputValue > 0.1f || m_MovementInputValue < -0.1f) && !anim.GetBool("isWalking"))
            {
                anim.SetBool("isWalking", true);

                //Debug.Log("Walking");
            }
            else if (m_MovementInputValue == 0.0f && anim.GetBool("isWalking"))
            {
                anim.SetBool("isWalking", false);
            }
        }
        // Making sure we disable walking and switch to waving if trying to wave while walking
        else if (!anim.GetBool("isWaving") && m_waved)
        {
            anim.SetBool("isWalking", false);

            anim.SetBool("isWaving", false);
        }
    }

    private void RespawnPlayer()
    {
        transform.position = m_SpawnPoint.position;
        transform.rotation = m_SpawnPoint.rotation;
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
			/*AIController ai = other.GetComponent<AIController>();
			ai.m_target = m_Rigidbody.transform;
			ai.m_state = AIController.state.following;
            ai.LoveTrigger();*/

            //Debug.Log("Triggered");
		}
	}
}
