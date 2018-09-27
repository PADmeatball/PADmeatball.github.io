using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PushBackBottan : MonoBehaviour {

    private void Start()
    {
       
    }


    // Update is called once per frame
    void Update () {
		if(Input.GetKeyDown(KeyCode.Escape))
        {

            OVRManager.PlatformUIConfirmQuit();
        }
      
	}
   
}
