using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadAsync : MonoBehaviour {
    
    public IEnumerator LoadAsyncAsset(string filepath)
    {
        //非同期ロード開始
        ResourceRequest resourceRequest = Resources.LoadAsync<Sprite>(filepath);

        while (!resourceRequest.isDone)
        {
          
            Debug.Log(resourceRequest.progress);
            yield return 0;
        }

        Sprite sprite = resourceRequest.asset as Sprite;
       


    }
}
