using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cannonAngleController : MonoBehaviour {

    private Quaternion quaternion;
    [SerializeField] private float Yangle;
    [SerializeField] bool Tutorial;
    // Use this for initialization
    void Start () {
       
    }
	
	// Update is called once per frame
	void Update () {
        if (Tutorial)
        {
            quaternion = Quaternion.Euler(-TutorialThrow.angle, Yangle, 0);
            gameObject.transform.rotation = quaternion;
            
        }
        else
        {
            quaternion = Quaternion.Euler(-RespownFruit.angle, Yangle, 0);
            gameObject.transform.rotation = quaternion;
        }
        //StartCoroutine(ChangeAngle());
    }
   IEnumerator ChangeAngle()
    {
        gameObject.transform.Rotate(RespownFruit.angle, 0, 0);
        yield return new WaitForSeconds(2);
    }
}
