using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Networking;

public class DynamicMusic : MonoBehaviour {
    public AudioMixer mixer;
    public AudioClip mainClip;

    private AudioSource mainMusicSource;
    private AudioSource[] sources;
    private bool introStarted;
    private bool inIntro;

    public float rampUpTime;
    public float rampDownTime;

    public float state2MinDistance;
    public float state3MinDistance;

    private List<AudioMixerSnapshot> states = new List<AudioMixerSnapshot>();

    private int currentStateID;

    void Start()
    {
        introStarted = false;
        inIntro = true;
        sources = GetComponents<AudioSource>();
        mainMusicSource = sources[0];

        states.Add(mixer.FindSnapshot("Base"));
        states.Add(mixer.FindSnapshot("State2"));
        states.Add(mixer.FindSnapshot("State3"));
        states.Add(mixer.FindSnapshot("Victory"));

        currentStateID = 0;
    }

	void Update () {
        if (!introStarted) {
            if (mainMusicSource.isPlaying) {
                introStarted = true;
            }
            return;
        }
        if (inIntro && !mainMusicSource.isPlaying)
        {
            inIntro = false;
            PlayMainLoop();
        } else {
            Transform localPlayer = ClientScene.localPlayers[0].gameObject.transform;
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            Transform nearestEnemy = null;
            float nearestEnemyDistance = Mathf.Infinity;
            foreach (GameObject player in players) {
                if (player.GetComponent<MonsterActions>() != null) {
                    Transform enemy = player.transform;
                    float distanceFromPlayer = Vector2.Distance(localPlayer.position, enemy.position);
                    if (distanceFromPlayer < nearestEnemyDistance) {
                        nearestEnemyDistance = distanceFromPlayer;
                        nearestEnemy = enemy;
                    }
                }
            }
            if (nearestEnemy != null) {
                if (nearestEnemyDistance > state2MinDistance) {
                    GoToState(0); // Base channel
                    Debug.Log("Enemy is further than min distance");
                } else {
                    if (nearestEnemyDistance < state3MinDistance) {
                        Debug.Log("Enemy is CLOSE");
                        GoToState(2); // State 3 channel
                    } else {
                        Debug.Log("Enemy is MEDIUM");
                        GoToState(1); // State 2 channel
                    }
                }
            } else
            {
                Debug.Log("No enemies found");
            }
        }
	}

    public void PlayMainLoop()
    {
        mainMusicSource.clip = mainClip;
        mainMusicSource.loop = true;
        foreach (AudioSource source in sources)
        {
            source.Play();
        }
    }

    public void GoToState(int newStateID)
    {
        if (newStateID == currentStateID || newStateID < 0 || newStateID > states.Count - 1) { return; }
        float rampTime = (newStateID > currentStateID ? rampUpTime : rampDownTime);
        states[newStateID].TransitionTo(rampTime);
        currentStateID = newStateID;
    }
}
