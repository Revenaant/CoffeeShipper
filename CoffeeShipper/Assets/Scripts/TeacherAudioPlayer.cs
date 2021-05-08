using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeacherAudioPlayer : MonoBehaviour
{
    [SerializeField]
    private AudioSource source;

    [SerializeField]
    private AudioClip playerDetected;

    public void PlayerTeacherDetected()
    {
        source.PlayOneShot(playerDetected, 1);
    }
}
