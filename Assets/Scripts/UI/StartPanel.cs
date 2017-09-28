using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class StartPanel : MonoBehaviour {

    public Image logoImage;
    public Image lightImage;
    public Button startButton;
    
   
    Tweener logoTweener;
    Tweener lightScaleTweener;
    Tweener lightRotateTweener;
    Tweener startButtonScaleTweener;
    Tweener startButtonMoveTweener;
   
    // Use this for initialization
    void Start () {
        //logo
        logoTweener = logoImage.transform.DOLocalMoveY(480f, 2.5f);       

        //lightImage
        lightScaleTweener=lightImage.transform.DOScale(1f, 2.5f);
               
        lightRotateTweener = lightImage.transform.DORotate(new Vector3(0, 0, -360f), 3f, RotateMode.FastBeyond360);
        lightRotateTweener.SetLoops(-1, LoopType.Restart);
        lightRotateTweener.SetEase(Ease.Linear);
        

        //startButton
        startButton.transform.localScale = new Vector3(1f, 0.8f, 1f);
        startButtonScaleTweener = startButton.transform.DOScale(new Vector3(0.9f,1f,1f), 1);
        startButtonScaleTweener.SetLoops(-1, LoopType.Yoyo);       

        startButtonMoveTweener = startButton.transform.DOLocalMoveX(0f, 2.5f);

        //mask
        UIManager.instance.FadeOutMask();
	}

    

    private void OnDisable()
    {
        lightRotateTweener.Kill();
        startButtonScaleTweener.Kill();
    }

    // Update is called once per frame
    //void Update () {		
    //}

    public void StartGame()
    {
        //显示Loading界面
        UIManager.instance.loadingPanel.SetActive(true);

        //开启异步加载场景携程
        StartCoroutine(LoadChooseLevel());  
    }

    IEnumerator LoadChooseLevel()
    {
        //生成按钮
        UIManager.instance.selectStagePanel.GenerateLevelButton();
        //异步加载下一个场景
        yield return SceneManager.LoadSceneAsync("ChooseLevel");       

        //加载完成后 不显示startPanel(直接摧毁)       
        GameObject.Destroy(this.gameObject);
     
        //mask      
        UIManager.instance.FadeInOutMask(1.0f, 1.0f,
            delegate() 
            {
                //不显示Loading
                UIManager.instance.loadingPanel.SetActive(false);
                //显示ChooseLevel场景的初始UI
                UIManager.instance.selectStagePanel.gameObject.SetActive(true);
            }
            );

        GameManager.instance.playerDataOperator.SavePlayerData();
    }

    
}
