using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class codeBtnSound : MonoBehaviour
{
    public AudioSource g_audioSource { get { return GetComponent<AudioSource>(); } }
    public Button g_btn { get { return GetComponent<Button>(); } }
    public AudioClip g_audioClip;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.AddComponent<AudioSource>();

        g_btn.onClick.AddListener(PlaySound);
    }

    void PlaySound()
    {
        g_audioSource.PlayOneShot(g_audioClip);
    }
}
