using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Student : MonoBehaviour
{
    public GameObject exclamationMark;
    public GameObject checkMark;
    public GameObject ePrompt;
    public bool hasCoffee;

    // Start is called before the first frame update
    void Start()
    {
        hasCoffee = false;

        ePrompt.SetActive(false);
        exclamationMark.SetActive(true);
        checkMark.SetActive(false);
    }

    public void ReceiveCoffee()
    {
        hasCoffee = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player" && !hasCoffee)
        {
            ePrompt.SetActive(true);
            exclamationMark.SetActive(false);
            checkMark.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            ePrompt.SetActive(false);
            exclamationMark.SetActive(!hasCoffee);
            checkMark.SetActive(hasCoffee);
        }
    }
}
