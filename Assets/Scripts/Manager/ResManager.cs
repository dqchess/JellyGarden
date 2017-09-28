using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ResManager : MonoBehaviour {

    public static ResManager instance;

    [Tooltip("格子背景资源")]
    public Sprite[] squareBgRes;//背景资源

    [Space]
    [Tooltip("block资源")]
    public Sprite[] blockRes;//block资源

    [Space]
    [Tooltip("关卡背景资源")]
    public Sprite[] levelBackgroundRes;//block资源

    [Space]
    [Tooltip(" Jelly[种类][颜色]  资源")]
    public JellyColorRes[] jellyRes;// Jelly[种类][颜色]
    [Tooltip("樱桃 或者 开心果 资源")]
    public Sprite[] ingredientRes;
    [Tooltip("多彩糖果资源")]
    public Sprite colorfulJellyRes;

    [Space]
    public Sprite moveLimitImage;
    public Sprite timeLimitImage;

    [Space]
    //关卡是否打开的图标
    [Tooltip("关卡按钮开启 关闭 资源")]
    public Sprite[] buttonOverlayRes;
    //星星
    [Tooltip("关卡按钮星星资源")]
    public GameObject[] stars;

    //关卡目标
    [Tooltip("关卡目标资源")]
    public Sprite[] levelTargetsRes;

    
    private LevelList levelDataList=null;
    
    private PlayerData playerData=null;

    private void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //获取LevelList
    public LevelList GetLevelDataList()
    {
        if (levelDataList==null)
        {
            levelDataList= Resources.Load<LevelList>("LevelData/LevelAsset");
        }
        return levelDataList;
    }

    //获取PlayerData
    public PlayerData GetPlayerData()
    {
        if (playerData==null)
        {
            playerData = GameManager.instance.playerDataOperator.LoadPlayerData();
        }
        return playerData;
    }

    public string GetLevelTargetDescription(int levelNum)
    {
        GetLevelDataList();
        string discription = null;
        switch (levelDataList.levelList[levelNum - 1].levelTarget)
        {
            case LevelTarget.SCORE:
                discription = "Get " + levelDataList.levelList[levelNum - 1].starScore[0] + " score";
                break;
            case LevelTarget.ELIMINATE:
                discription = "Collect the items";
                break;
            case LevelTarget.COLLECTION:
                discription = "Collect all ingredients";
                break;
            case LevelTarget.DESTROY:
                discription = "Collect all blocks";
                break;
            default:
                break;
        }

        return discription;
    }

    public int GetLevelScore(int levelNum)
    {
        GetPlayerData();
        if (levelNum<= playerData.list_levelScore.Count)
        { 
            return playerData.list_levelScore[levelNum - 1].playerScore;
        }
        else
        {
            return 0;
        }
        
    }

    public int GetLevelStar(int levelNum)
    {
        GetPlayerData();
        if (levelNum <= playerData.list_levelScore.Count)
        {
            return playerData.list_levelScore[levelNum - 1].starCount;
        }
        else
        {
            return 0;
        }
            
    }
}
