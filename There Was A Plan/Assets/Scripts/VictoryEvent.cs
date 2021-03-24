using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryEvent : MonoBehaviour
{
    public AudioClip victoryMusic;

    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            GameObject.Find("MusicNight").GetComponent<AudioSource>().Stop();
            audioSource.Play();
            audioSource.PlayOneShot(victoryMusic);
            foreach (ParticleSystem particle in GetComponentsInChildren<ParticleSystem>())
            {
                particle.Play();
            }
            GetComponentInChildren<Canvas>().enabled = true;
        }
    }

}
