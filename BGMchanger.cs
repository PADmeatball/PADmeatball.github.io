using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMchanger : MonoBehaviour {

    
    private bool pitchflag;
    private float timer;

    private AudioSource MainBGM;
    

    // Use this for initialization
    void Start () {
    
        timer = 45.0f;
        MainBGM = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
        timer -= Time.deltaTime;

       if(timer <=15.0f)
        {
            MainBGM.pitch = 1.2f;
        }
	}
}
