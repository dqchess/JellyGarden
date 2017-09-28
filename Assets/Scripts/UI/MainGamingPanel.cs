using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainGamingPanel : MonoBehaviour {

    public Slider scoreSlider;//分数条
    public GameObject[] scoreStars;//分数条的星星
    public Text scoreText;//分数显示
    public Text limitText;//剩余时间或者步数
    public Image limitImage;//剩余时间或者步数的图片
    public Text levelNumText;//关卡数文字

    [Space]
    public GameObject targetScore;
    public Text targetScoreText;

    [Space]
    public GameObject targetCollection;
    public Image[] jellyImage;
    public Text[] jellyImageText;
    

    [Space]
    public GameObject targetDestroy;
    public Text blockLeftText;






    //// Use this for initialization
    //void Start () {

    //}

    //// Update is called once per frame
    //void Update () {

    //}

   

    public void InitPanelElement(int levelNum)
    {
        //获得关卡数据 根据关卡数据来初始化UI界面
        LevelData levelData = ResManager.instance.GetLevelDataList().levelList[levelNum - 1];

        //摆放3个星星
        float sliderWidth = scoreSlider.gameObject.GetComponent<RectTransform>().sizeDelta.x;
        scoreSlider.value = 0;//分数条置0
        for (int i=0;i<3;i++)
        {
            //根据分数比例，摆放星星
            Vector2 pos = scoreStars[i].GetComponent<RectTransform>().anchoredPosition;
            float posX = (levelData.starScore[i] / (float)levelData.starScore[2])*sliderWidth;
            
            scoreStars[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(posX, pos.y);
            //把点亮星星设置为不可见
            GameObject fullStar = scoreStars[i].transform.GetChild(0).gameObject;
            fullStar.SetActive(false);
        }

        //分数
        scoreText.text = "0";

        //剩余时间或者步数 图片
        if (levelData.levelLimit==LevelLimit.MOVE)
        {
            limitText.text = levelData.moveLimit.ToString();
            limitImage.sprite = ResManager.instance.moveLimitImage;
        }
        else if(levelData.levelLimit == LevelLimit.TIME)
        {
            //把时间计算到 分：秒
            int minute = levelData.timeLimit / 60;
            int second = levelData.timeLimit % 60;
            limitText.text = "" + minute + ":" + second;
            limitImage.sprite = ResManager.instance.timeLimitImage;
        }

        //关卡数文字
        levelNumText.text = levelNum.ToString();

        //目标显示
        switch (levelData.levelTarget)
        {
            case LevelTarget.SCORE:
                targetScore.SetActive(true);
                targetCollection.SetActive(false);
                targetDestroy.SetActive(false);

                targetScoreText.text = levelData.starScore[0].ToString();
                break;
            case LevelTarget.ELIMINATE:
                targetScore.SetActive(false);
                targetCollection.SetActive(false);
                targetDestroy.SetActive(false);

                jellyImage[0].gameObject.SetActive(false);
                jellyImage[1].gameObject.SetActive(false);
                for (int i=0;i<levelData.eliminateTargetTypeList.Count;i++)
                {
                    jellyImage[i].gameObject.SetActive(true);
                    jellyImage[i].sprite = ResManager.instance.jellyRes[(int)JellyType.NORMAL].jellyColorRes[(int)levelData.eliminateTargetTypeList[i]];
                    jellyImageText[i].text = levelData.eliminateTargetCount[i].ToString();
                }

                break;
            case LevelTarget.COLLECTION:
                targetScore.SetActive(false);
                targetCollection.SetActive(true);
                targetDestroy.SetActive(false);

                jellyImage[0].gameObject.SetActive(false);
                jellyImage[1].gameObject.SetActive(false);
                for (int i = 0; i < levelData.collectionTargetTypeList.Count; i++)
                {
                    jellyImage[i].gameObject.SetActive(true);
                    jellyImage[i].sprite = ResManager.instance.ingredientRes[(int)levelData.collectionTargetTypeList[i]];
                    jellyImageText[i].text = levelData.collectionTargetTypeList[i].ToString();
                }
                break;
            case LevelTarget.DESTROY:
                targetScore.SetActive(false);
                targetCollection.SetActive(true);
                targetDestroy.SetActive(true);

                //遍历格子数据，得到有多少个block
                blockLeftText.text = levelData.GetBlockCount().ToString();
                break;
            default:
                break;
        }
    }
}
