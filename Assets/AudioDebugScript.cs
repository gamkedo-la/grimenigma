using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioDebugScript : MonoBehaviour
{
    private AudioSource myAudioSource;
    // Start is called before the first frame update
    void Start()
    {
        myAudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.P))
        {
            myAudioSource.PlayOneShot(myAudioSource.clip);
        }
    }

    public void PlayDeathSound()
    {
        myAudioSource.PlayOneShot(myAudioSource.clip);
    }
}
