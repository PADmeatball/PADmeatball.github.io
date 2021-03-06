﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : MonoBehaviour
{
    private LoadAsync loadAsync = new LoadAsync();

    private void OnEnable()
    {
        OVRManager.tiledMultiResLevel = OVRManager.TiledMultiResLevel.LMSLow;
        StartCoroutine(loadAsync.LoadAsyncAsset("Title"));
    }

}
