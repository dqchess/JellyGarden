using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellyController : MonoBehaviour {

    public static JellyController instance;

    public SpriteRenderer background;
    public Transform GridRoot;
    public Transform JellyRoot;

    public GameObject squarePrefab;
    public GameObject jellyPrefab;

    void Awake()
    {
        instance = this;

        //获取关卡数据
        int levelNum = GameManager.instance.runningLevel;
        LevelData levelData = ResManager.instance.GetLevelDataList().levelList[levelNum - 1];

        //设置背景图片
        background.sprite = levelData.background;

        //生成square和block
        float startPosX = 0;
        if (levelData.xCount%2==1)
        {
            startPosX = -(levelData.xCount / 2) * GameData.tileWidth;
        }
        else
        {
            startPosX = -(levelData.xCount / 2) * GameData.tileWidth + GameData.tileWidth / 2.0f;
        }
        
        float startPosY =0;
        if (levelData.yCount % 2 == 1)
        {
            startPosY = (levelData.yCount / 2) * GameData.tileHeight;
        }
        else
        {
            startPosY = ((levelData.yCount / 2)-1) * GameData.tileHeight + GameData.tileHeight / 2.0f;
        }
        for (int i=0;i<levelData.yCount;i++)
        {
            for (int j=0;j<levelData.xCount;j++)
            {
                //生成square
                Vector3 pos = new Vector3(startPosX + j * GameData.tileWidth, startPosY - i * GameData.tileHeight, 0);
                GameObject square = GameObject.Instantiate<GameObject>(squarePrefab);
                square.transform.SetParent(GridRoot);               
                square.transform.localPosition = pos;
                
                GameObject block = square.transform.GetChild(0).gameObject;
                //block 和 square的脚本
                Square squareScript = square.GetComponent<Square>();
                Block blockScript = block.GetComponent<Block>();

                //设置图片和状态
                squareScript.aboveBlock = blockScript;
                squareScript.xPos = j;
                squareScript.yPos = i;

                blockScript.belongToSquare = squareScript;                
                blockScript.xPos = j;
                blockScript.yPos = i;
                blockScript.SetBlockType(levelData.block[i * GameData.maxCol + j]);

                //设置square背景（深色或者浅色）
                //if (j%2==1)
                //{                   
                //    square.GetComponent<SpriteRenderer>().sprite = ResManager.instance.squareBgRes[i%2];
                //}
                //else if (j%2==0)
                //{
                //    square.GetComponent<SpriteRenderer>().sprite = ResManager.instance.squareBgRes[(i+1)%2];
                //}
                squareScript.spriteRnderer.sprite = ResManager.instance.squareBgRes[(i + j % 2) % 2];                              
            }
        }
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
