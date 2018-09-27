using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class UIController : MonoBehaviour
{
    private AudioSource se_Button;
    public bool onlineranking = false;
    public bool rankingflag = true;

    private void Start()
    {
        se_Button = GetComponent<AudioSource>();
    }
   
    public void OnTutorialClick()
    {
        se_Button.PlayOneShot(se_Button.clip);

        SceneManager.LoadScene("Tutorial");        


    }
    public void OnGameStartClick()
    {
        se_Button.PlayOneShot(se_Button.clip);
        
        SceneManager.LoadScene("Main");

        
    }
    public void OnTitleClick()
    {
        se_Button.PlayOneShot(se_Button.clip);
        SceneManager.LoadScene("Title");
        

    }
    public void OnOnlineClick()
    {
        se_Button.PlayOneShot(se_Button.clip);
        if (onlineranking)
        {
            onlineranking = false;
            rankingflag = true;
        }
        else
        {
            onlineranking = true;
            rankingflag = true;
        }
    }
   
    
}


