using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

	public Transform m_SpawnPoint;                          // The position and direction the tank will have when it spawns.
	[HideInInspector] public int m_PlayerNumber;    
	[HideInInspector] public GameObject m_Instance;


	private PlayerController m_Player;
	private GameObject m_CanvasGameObject;  

	// Use this for initialization
	public void Setup () {
		// Get references to the components.
		m_Player = m_Instance.GetComponent<PlayerController> ();
        //m_CanvasGameObject = m_Instance.GetComponentInChildren<Canvas> ().gameObject;

		// Set the player numbers to be consistent across the scripts.
		m_Player.m_PlayerNumber = m_PlayerNumber;
        m_Player.m_SpawnPoint = m_SpawnPoint;

	}
	
	// Used during the phases of the game where the player shouldn't be able to control their tank.
	public void DisableControl ()
	{
		m_Player.enabled = false;
		m_CanvasGameObject.SetActive (false);

	}


	// Used during the phases of the game where the player should be able to control their tank.
	public void EnableControl ()
	{
		m_Player.enabled = true;
		m_CanvasGameObject.SetActive (true);
	}


	// Used at the start of each round to put the tank into it's default state.
	public void Reset ()
	{
		m_Instance.transform.position = m_SpawnPoint.position;
		m_Instance.transform.rotation = m_SpawnPoint.rotation;

		m_Instance.SetActive (false);
		m_Instance.SetActive (true);
	}
}
