 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour {
    [SerializeField] private GameObject loadobject;
    private LoadAsync loadAsync;

    private void OnEnable()
    {
        loadAsync = loadobject.GetComponent<LoadAsync>();
        Debug.Log(loadAsync);
        OVRManager.tiledMultiResLevel = OVRManager.TiledMultiResLevel.LMSLow;
        StartCoroutine(loadAsync.LoadAsyncAsset("Resources"));
    }
    
}
