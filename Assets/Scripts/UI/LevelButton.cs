using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour {
     
    //需要用到的资源
    //关卡是否打开的图标
    public Sprite[] buttonOverlayRes;   
    [Space]    

    //关卡目标
    public Sprite[] levelTargetsRes;    
    [Space]
 

    //星星
    public GameObject[] stars;
    //对应的按钮
    public Image buttonImage;
    //关卡数据
    //关卡的目标
    public Image targetImage;
    //关卡数
    public int levelNum;
    public Text levelNumText;
    //关卡位置（父节点）
    public Transform parentPos;

 //   // Use this for initialization
 //   void Start () {
       
	//}
	
	//// Update is called once per frame
	//void Update () {
		
	//}

    public void SetActiveStar(int starCount)
    {
        for (int i=0;i< starCount;i++)
        {
            stars[i].SetActive(true);
        }
        for (int i= starCount; i<3;i++)
        {
            stars[i].SetActive(false);
        }
    }

    public void SetLevelNum(int num)
    {
        levelNum = num;        
        levelNumText.text = num.ToString();
    }

    public void SetTargetImage(LevelData date)
    {
        switch (date.levelTarget)
        {
            case LevelTarget.SCORE:
                if (date.levelLimit == LevelLimit.MOVE)
                {
                    targetImage.sprite = ResManager.instance.levelTargetsRes[0];
                }
                else if (date.levelLimit == LevelLimit.TIME)
                {
                    targetImage.sprite = ResManager.instance.levelTargetsRes[2];
                }
                break;
            case LevelTarget.ELIMINATE:
                targetImage.sprite = ResManager.instance.levelTargetsRes[4];
                break;
            case LevelTarget.COLLECTION:
                targetImage.sprite = ResManager.instance.levelTargetsRes[1];
                break;
            case LevelTarget.DESTROY:
                targetImage.sprite = ResManager.instance.levelTargetsRes[3];
                break;
            default:
                break;
        }
    }

    public void SetButtonState(bool isOn)
    {
        if (isOn)
        {
            buttonImage.sprite = ResManager.instance.buttonOverlayRes[0];
            buttonImage.gameObject.GetComponent<Button>().enabled = true;
            levelNumText.gameObject.SetActive(true);
            targetImage.gameObject.SetActive(true);
        }
        else
        {
            buttonImage.sprite = ResManager.instance.buttonOverlayRes[1];
            buttonImage.gameObject.GetComponent<Button>().enabled = false;
            levelNumText.gameObject.SetActive(false);
            targetImage.gameObject.SetActive(false);
        }
    }

    public void ShowLevelInfo()
    {
        int starCount = ResManager.instance.GetLevelStar(levelNum);
        string des = ResManager.instance.GetLevelTargetDescription(levelNum);
        UIManager.instance.levelInfoPanel.SetLevelInfoPanelElement(starCount, des, levelNum);
        UIManager.instance.levelInfoPanel.gameObject.SetActive(true);
    }
}
