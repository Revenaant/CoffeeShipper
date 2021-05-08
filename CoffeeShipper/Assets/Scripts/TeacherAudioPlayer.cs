using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeacherAudioPlayer : MonoBehaviour
{
    [SerializeField]
    private AudioSource source;

    [SerializeField]
    private AudioClip playerDetected;

    [SerializeField]
    private AudioClip[] yvens;

    [SerializeField]
    private AudioClip[] hans;

    public void PlayerTeacherDetected()
    {
        source.PlayOneShot(playerDetected, 1);
    }

    public int PlayRandom()
    {
        int rnd = UnityEngine.Random.Range(0, 2);

        switch (rnd)
        {
            case 0: PlayYvens(); return 0;
            case 1: PlayHans(); return 1;
        }

        return 0;
    }

    private void PlayYvens()
    {
        int rnd = UnityEngine.Random.Range(0, yvens.Length);
        source.PlayOneShot(yvens[rnd], 1);
    }

    private void PlayHans()
    {
        int rnd = UnityEngine.Random.Range(0, hans.Length);
        source.PlayOneShot(hans[rnd], 1);
    }
}
