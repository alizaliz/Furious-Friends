using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [Header("Cone Raycasting")]
    public int m_ConeRays;
    public float m_ConeAngle;

	public float m_Speed = 1;
	public float m_TurnSpeed =  2;
	private string m_MovementAxisName;     
	private string m_TurnAxisName;         
	private Rigidbody m_Rigidbody;  
	private float m_MovementInputValue;    
	private float m_TurnInputValue;
       
    // Tag on Object to check for raycast collision, can change this to a more descriptive string
    public string friends = "Friend";
    public bool checkAllTags = false;
    public Transform playerPosition;
    // Wave range from player to AI
    public float raycastDist = 500f;

	// Use this for initialization
	void Start () {
		m_Rigidbody = GetComponent<Rigidbody>();
		m_MovementInputValue = 0f;
		m_MovementAxisName = "Vertical";
		m_TurnAxisName = "Horizontal";
		m_MovementInputValue = 0f;
		m_TurnInputValue = 0f;
	}
	
    private void Wave()
    {
        // Check for raycast hit
        HashSet<RaycastHit> coneHits = new HashSet<RaycastHit>();
        //Ray ray = new Ray (transform.position, transform.forward);

        float shortestDist;
        shortestDist = Mathf.Infinity;

        float forwardAngle = Mathf.Atan2(transform.forward.z, transform.forward.x);
        forwardAngle -= Mathf.Deg2Rad * 90.0f;
        float startAngleOffset = (Mathf.Deg2Rad * m_ConeAngle) / 2.0f;
        for (int i = 0; i < m_ConeRays; i++)
        {
            float newAngle = forwardAngle + startAngleOffset + (i * (Mathf.Deg2Rad * m_ConeAngle / m_ConeRays));
            Vector3 rayDir = new Vector3(Mathf.Cos(newAngle), 0.0f, Mathf.Sin(newAngle));
            RaycastHit[] hits = Physics.RaycastAll(transform.position, rayDir, 10.0f);
            Debug.DrawRay(transform.position, rayDir * 10.0f, Color.cyan);
            foreach (RaycastHit hit in hits)
            {
                if (hit.transform.CompareTag("Friend"))
                {
                    coneHits.Add(hit);
                }
            }
        }

        foreach (RaycastHit hit in coneHits)
        {
            Debug.DrawRay(hit.transform.position, hit.transform.forward * 10.0f, Color.red);
            Debug.Log("Hit: " + hit.collider.gameObject.name);

            float angle = Vector3.Dot(hit.transform.forward, transform.position - hit.transform.position);
            if (angle > 0.9f)
            {
                Debug.DrawRay(hit.transform.position, Vector3.up * 4.0f, Color.yellow);
            }
            else
            {
                Debug.DrawRay(hit.transform.position, Vector3.up * 4.0f, Color.red);
            }
        }
    }

	// Update is called once per frame
	private void Update ()
	{
        if (Input.GetKeyDown(KeyCode.F))
        {
            Wave();
        }
        //Debug.Log(hits);

        //for (int i = 0; i < hits.Length; i++)
        //{
        //    if (hits[i].transform.tag == friends &&((hits[i].distance) < shortestDist))
        //    {
        //        shortestDist = Vector3.Distance(transform.position, hits[i].point);
        //    }
        //}


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
