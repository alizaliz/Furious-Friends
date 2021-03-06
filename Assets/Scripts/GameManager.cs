﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public CameraController m_cameraController;
	public PlayerManager[] m_players;
	public GameObject m_playerPrefab; 
	public GameObject m_aiPrefab;

	public int m_MaxAISpawnCount;
	public GameObject m_groundArea;

    public AudioSource m_MainMusic;
    public AudioScript m_AudioSystem;
    public AudioClip[] m_Voices;
    public List<GameObject> m_AIList;
	private float spawnPadValue = 0.0f;


	// Use this for initialization
	void Start () {

		SpawnAllPlayers ();
		SpawnAllAI ();
		SetCameraTargets ();
        m_MainMusic.Play();
    }

	private void SpawnAllAI()
	{
		
		float x, z;
		Vector3 spawnPosition;
		Quaternion spawnRotation;
		GameObject AI;
		float spawnAreaAbs = (m_groundArea.transform.lossyScale.x * 10) / 2;
		m_AIList = new List<GameObject> ();
		for (int i = 0; i < m_MaxAISpawnCount; i++) {
			x = Random.Range (-spawnAreaAbs + spawnPadValue ,spawnAreaAbs - spawnPadValue);
			z = Random.Range (-spawnAreaAbs  + spawnPadValue, spawnAreaAbs- spawnPadValue);
			spawnPosition = new Vector3 (x,0,z); 
			spawnRotation =  Quaternion.Euler (0f, Random.Range (0, 90), 0f);
			AI = Instantiate(m_aiPrefab, spawnPosition,spawnRotation) as GameObject;

            m_AIList.Add (AI);
		}
	}

	private void SpawnAllPlayers()
	{
		// For all the players...
		for (int i =0 ; i < m_players.Length; i++)
		{
			// ... create them, set their player number and references needed for control.
			m_players[i].m_Instance =
				Instantiate(m_playerPrefab, m_players[i].m_SpawnPoint.position, m_players[i].m_SpawnPoint.rotation) as GameObject;
			m_players[i].m_PlayerNumber = i + 1;
            m_players[i].m_Voice = m_Voices[i];
            m_players[i].Setup();
		}
	}

	private void SetCameraTargets()
	{
		// Create a collection of transforms the same size as the number of players.
		Transform[] targets = new Transform[m_players.Length];

		// For each of these transforms...
		for (int i = 0 ; i < targets.Length; i++)
		{
			// ... set it to the appropriate tank transform.
			targets[i] = m_players[i].m_Instance.transform;
		}

		// These are the targets the camera should follow.
		m_cameraController.m_Targets = targets;
	}
}
