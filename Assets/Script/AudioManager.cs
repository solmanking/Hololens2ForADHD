using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic;


public class AudioManager : MonoBehaviour
{

    public AudioClip Correct;
    public AudioClip Wrong;
    public AudioClip thinking;
    public AudioClip claps;
    public AudioClip ding;

    public List<AudioSource> Audios = new List<AudioSource>();
    // Start is called before the first frame update
    void Start()
    {

       

    }

    private void Awake()
    {
        for (int i = 0; i < 5; i++)
        {
            var audio = gameObject.GetComponent<AudioSource>();
            Audios.Add(audio);

        }
    }
    // Update is called once per frame
    public void Play(int index, string name,bool isLoop)
    {
        var clip = getAudioClip(name);
        if (clip != null)
        {
            var audio = Audios[index];
            audio.clip = clip;
            audio.loop = isLoop;
            audio.Play();
            Debug.Log("播放音效");
        }
    
    }


    AudioClip getAudioClip(string name){
        switch (name) {

            case "Correct":
                return Correct;
            case "Wrong":
                return Wrong;
            case "thinking":
                return thinking;
            case "claps":
                return claps;
            case "ding":
                return ding;
        }

        return null;
    }
}
