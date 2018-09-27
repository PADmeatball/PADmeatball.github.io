using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActive : MonoBehaviour {

    [SerializeField] private GameObject player_R;
    [SerializeField] private GameObject player_L;
    // Use this for initialization
    void Start () {

        var activecontroller = OVRInput.GetActiveController();
        //左右をとってきて現在のアクティブなコントローラ出ないほうを消す。
        if(activecontroller == OVRInput.Controller.RTrackedRemote)
        {
            player_L.gameObject.SetActive(false);
           
        }
        else
        {
            player_R.gameObject.SetActive(false);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
