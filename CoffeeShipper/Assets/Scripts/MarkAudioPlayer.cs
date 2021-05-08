using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEditor;
using UnityEngine;

public class MarkAudioPlayer : MonoBehaviour
{
    [SerializeField]
    private AudioSource source;
    
    [SerializeField]
    private AudioClip[] happyClips;
    
    [SerializeField]
    private AudioClip[] angryClips;

    [SerializeField]
    private AudioClip dontCryAudio;

    [SerializeField]
    private AudioClip coffeeAudio;

    public void PlayHappy()
    {
        source.pitch = Random.Range(0.95f, 1.05f);
        int index = Random.Range(0, happyClips.Length);
        source.PlayOneShot(happyClips[index]); 
    }

    public void PlayAngry()
    {
        source.pitch = Random.Range(0.95f, 1.05f);
        int index = Random.Range(0, angryClips.Length);
        source.PlayOneShot(angryClips[index]);
    }

    public void PlayDontCry()
    {
        source.pitch = Random.Range(0.95f, 1.05f);
        source.PlayOneShot(dontCryAudio, 1);
    }

    public void PlayCoffeePour()
    {
        source.PlayOneShot(coffeeAudio, 1);
    }
}
