using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class DynamicMusic : MonoBehaviour {
    public AudioMixer mixer;
    public AudioClip mainClip;

    private AudioSource mainMusicSource;
    private AudioSource[] sources;
    private bool introStarted;
    private bool inIntro;

    public float rampUpTime;
    public float rampDownTime;

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
