using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    static public UIManager instance;

    //Panels
    public Image fadeMask;
    public GameObject loadingPanel;
    public StartPanel startPanel;
    public SelectStagePanel selectStagePanel;
    public LevelInfoPanel levelInfoPanel;
    public MainGamingPanel mainGamingPanel;
    

    void Awake()
    {
        instance = this;
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void FadeOutMask(float fadeTime=1.0f)
    {
        fadeMask.color = Color.black;
        fadeMask.DOColor(Color.clear, fadeTime);
    }

    public void FadeInMask(float fadeTime=1.0f)
    {
        fadeMask.color = Color.clear;
        fadeMask.DOColor(Color.black, fadeTime);      
    }

    public void FadeInOutMask(float fadeInTime = 1.0f, float fadeOutTime = 1.0f, TweenCallback midCall = null, TweenCallback finishCall = null)
    {
        fadeMask.color = Color.clear;
        Sequence fade = DOTween.Sequence();
        fade.Append(fadeMask.DOColor(Color.black, fadeInTime));
        fade.AppendCallback(midCall);
        fade.Append(fadeMask.DOColor(Color.clear, fadeOutTime));
        fade.OnComplete(finishCall);  
    }

    
}
