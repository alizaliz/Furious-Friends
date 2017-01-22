using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioScript : MonoBehaviour {
    public GameManager m_Game;

    public AudioClip[] m_VoiceAudio;
    public AudioClip[] m_MenuAudio;

    private AudioSource[] m_PlayerSource;
    private AudioSource[] m_AISource;
    

    /*// Use this for initialization
    void Start () {
        //m_Game = FindObjectOfType<GameManager>() as GameManager;
        //Debug.Log("Cannot find m_game: " + m_Game);

        // Random Player Voices
        for (int i = 0; i < m_Game.m_players.Length; i++)
        {
            //Debug.Log("Number of players: " + m_Game.m_players.Length);
            var RandNumber = (int)Mathf.Floor(Random.Range(0.0f, m_VoiceAudio.Length));

            //Debug.Log("Random Voice: " + RandNumber);
            m_Game.m_players[i].GetComponent<PlayerManager>().m_Voice = m_VoiceAudio[RandNumber];

            Debug.Log("Voice clip: " + m_Game.m_players[i].GetComponent<PlayerManager>().m_Voice);
        }
        
        // Random AI Voices
        for (int i = 0; i < m_Game.m_MaxAISpawnCount; i++)
        {
            var RandNumber = (int)Mathf.Floor(Random.Range(0.0f, m_VoiceAudio.Length));

            //m_Game.m_AIList[i].GetComponent<AIController>().m_Voice.clip = m_VoiceAudio[RandNumber];
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }*/
}
