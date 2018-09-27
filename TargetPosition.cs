using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetPosition : MonoBehaviour {

    [SerializeField] private GameObject player_R;
    [SerializeField] private GameObject player_L;
    [SerializeField] private GameObject target;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
            target.gameObject.transform.position = player_R.transform.position;                        
            target.gameObject.transform.position = player_L.transform.position;
    }
}
