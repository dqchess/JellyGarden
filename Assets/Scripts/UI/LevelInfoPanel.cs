using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelInfoPanel : MonoBehaviour {

    //Frame
    public GameObject frame;

    //关卡星星
    public GameObject[] stars;

    //关卡主要任务
    public Text levelTargetText;

    //关卡数
    public Text levelNumText;
    private int levelNum;

    //按钮
    public GameObject closeButton;
    public GameObject playButton;


    //Tweener
    Tweener closeButtonTweener;
    Tweener playButtonTweener;

	// Use this for initialization
	void Awake () {
        closeButton.gameObject.transform.localScale = new Vector3(0.8f, 1f, 1f);
        playButton.gameObject.transform.localScale = new Vector3(0.8f, 1f, 1f);
        closeButtonTweener = closeButton.gameObject.transform.DOScale(new Vector3(1f, 0.8f, 1f), 1f).SetLoops(-1, LoopType.Yoyo);
        playButtonTweener = playButton.gameObject.transform.DOScale(new Vector3(1f, 0.8f, 1f), 1f).SetLoops(-1, LoopType.Yoyo);

        closeButtonTweener.Pause();
        playButtonTweener.Pause();
	}

    private void OnEnable()
    {
        closeButtonTweener.Restart();
        playButtonTweener.Restart();
        ResetFrame();
    }

    private void OnDisable()
    {
        closeButtonTweener.Pause();
        playButtonTweener.Pause();
    }
    // Update is called once per frame
    //void Update () {		
    //}

    public void SetLevelInfoPanelElement(int starCount,string targetText,int levelNum)
    {
        //设置星星
        for (int i=0;i<starCount;i++)
        {
            stars[i].SetActive(true);
        }
        for (int i=starCount;i<3;i++)
        {
            stars[i].SetActive(false);
        }

        levelTargetText.text = targetText;

        this.levelNum = levelNum;
        levelNumText.text = levelNum.ToString();
    }

    public void ResetFrame()
    {
        frame.transform.localScale = new Vector3(1.5f, 1.5f, 1f);
        Sequence seq = DOTween.Sequence();
        seq.Append(frame.transform.DOScale(new Vector3(0.8f,1.2f,1f), 0.1f));
        seq.Append(frame.transform.DOScale(new Vector3(1.2f, 0.8f, 1f), 0.1f));
        //seq.Append(frame.transform.DOScale(new Vector3(0.9f, 1.1f, 1f), 0.1f));
        //seq.Append(frame.transform.DOScale(new Vector3(1.1f, 0.9f, 1f), 0.06f));
        seq.Append(frame.transform.DOScale(Vector3.one, 0.06f));
    }

    public void StartMainLevel()
    {
        //显示Loading界面
        UIManager.instance.loadingPanel.SetActive(true);

        //初始化主游戏界面中的元素
        UIManager.instance.mainGamingPanel.InitPanelElement(levelNum);

        //GameManager中关卡数赋值
        GameManager.instance.runningLevel = levelNum;

        //开启异步加载场景携程
        StartCoroutine(LoadMainLevel());
    }

    IEnumerator LoadMainLevel()
    {
        //异步加载下一个场景
        yield return SceneManager.LoadSceneAsync("Main");

        Debug.Log("MainScene load finish");

        //mask      
        UIManager.instance.FadeInOutMask(1.0f, 1.0f,
            delegate ()
            {
                //不显示的几个panel
                UIManager.instance.loadingPanel.SetActive(false);
                UIManager.instance.selectStagePanel.gameObject.SetActive(false);
                UIManager.instance.levelInfoPanel.gameObject.SetActive(false);
                //显示Main场景的初始UI
                UIManager.instance.mainGamingPanel.gameObject.SetActive(true);              
            }
            );

        
        
    }
}
