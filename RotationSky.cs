using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationSky : MonoBehaviour {
    private float m_speed = 0.003f;

    private float m_rotation;
    [SerializeField] private Material sky;
    private void Update()
    {
        m_rotation += m_speed;
        if (360 <= m_rotation)
        {
            m_rotation -= 360;
        }
        

        sky.SetFloat("_Rotation", m_rotation);
    }
}
