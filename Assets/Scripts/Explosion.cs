using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{


    [SerializeField]
    private AudioSource _explosionAudio = null;
    // Start is called before the first frame update
    void Start()
    {
        _explosionAudio.Play();
        Destroy(this.gameObject, 3.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
