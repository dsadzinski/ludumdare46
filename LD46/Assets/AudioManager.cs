using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    [SerializeField] public AudioClip virus;
    [SerializeField] public AudioClip pest;
    [SerializeField] public AudioClip volcano;
    [SerializeField] public AudioClip earthquake;
    [SerializeField] public AudioClip tornado;



    public static AudioManager instance;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //transform.GetComponent<AudioSource>().
    }
}
