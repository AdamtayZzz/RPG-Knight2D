using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioVolume : MonoBehaviour
{
    private float m_audioVolume = 0;

    //音乐组件
    private AudioSource m_audioSource;

    // Use this for initialization
    void Start()
    {
        Debug.Log("Start to audio music.");

        m_audioVolume = PlayerPrefs.GetFloat("AudioVolume", 1);
        m_audioSource = GetComponent<AudioSource>();
        m_audioSource.volume = m_audioVolume;
        m_audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        //更新音量
        m_audioVolume = PlayerPrefs.GetFloat("AudioVolume", 1);
        m_audioSource.volume = m_audioVolume;
    }
}
