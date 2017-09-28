using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectStagePanel : MonoBehaviour {

    public GameObject levelButtonPrefab;//关卡按钮预制体

    public Transform[] maps;

    public Transform levelPosRoot;

    //需要保存的数据
    private List<LevelButton> list_levelButton = new List<LevelButton>();

    // Use this for initialization
 //   void Start () {
	//	//动态生成level按钮


	//}
	
	//// Update is called once per frame
	//void Update () {
		
	//}

    //根据数据 生成关卡的按钮
    public void GenerateLevelButton()
    {
        //读取保存的玩家数据
        PlayerData playerData = ResManager.instance.GetPlayerData();

        //读取关卡数据
        LevelList levelData = ResManager.instance.GetLevelDataList();

        //根据数据生成LevelButton
        list_levelButton.Clear();
        for (int i = 0; i < levelData.levelList.Count; i++)
        {
            LevelButton button = GameObject.Instantiate<GameObject>(levelButtonPrefab).GetComponent<LevelButton>();
            list_levelButton.Add(button);

            //为LevelData中的levelButtonParent赋值
            levelData.levelList[i].levelButtonParent = levelPosRoot.Find("LevelPos (" + (i + 1) + ")");
            //设置位置
            button.transform.SetParent(levelData.levelList[i].levelButtonParent);
            button.parentPos = levelData.levelList[i].levelButtonParent;
            button.transform.localPosition = Vector3.zero;
            button.transform.localScale = Vector3.one;

            //设置关卡数和关卡文字
            button.SetLevelNum(levelData.levelList[i].levelNum);

            //设置target图标
            button.SetTargetImage(levelData.levelList[i]);

            //是否打开
            if (i < playerData.reachedLevel)
            {
                //已经开启                          
                //显示获得的星星
                button.SetActiveStar(playerData.list_levelScore[i].starCount);
                //变为可交互
                button.SetButtonState(true);
                             
            }
            else
            {
                //未开启               
                //隐藏所有的星星
                button.SetActiveStar(0);
                //变为不可交互
                button.SetButtonState(false);
                
            }
        }      
    }


    //一个关卡胜利 更新关卡按钮
    public void UpdateLevelButtonWhenWin(int winnedLevel, int starNum)
    {
        if (winnedLevel> list_levelButton.Count)
        {
            return;
        }
        //刚刚胜利的关卡按钮变化
        LevelButton winnedLevelButton = list_levelButton[winnedLevel - 1];
        winnedLevelButton.SetActiveStar(starNum);

        //下一个关卡按钮变化
        LevelButton nextLevelButton = null;
        if (winnedLevel<list_levelButton.Count)
        {
            nextLevelButton = list_levelButton[winnedLevel];
        }
        if (nextLevelButton)
        {
            //变为可交互
            nextLevelButton.SetButtonState(true);           
        }

    }
}
