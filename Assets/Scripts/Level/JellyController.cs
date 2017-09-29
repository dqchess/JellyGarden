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

    //当前的关卡数据
    LevelData levelData;

    //当前关卡数据
    List<List<Square>> gridData = new List<List<Square>>();

    void Awake()
    {
        instance = this;

        //获取关卡数据
        int levelNum = GameManager.instance.runningLevel;
        levelData = ResManager.instance.GetLevelDataList().levelList[levelNum - 1];

        //设置背景图片
        background.sprite = levelData.background;

        //生成初始的square和block
        GenerateSquareAndBlock();

        //随机生成糖果
        GenerateJelly();
        
    }

    //生成初始的square和block
    void GenerateSquareAndBlock()
    {
        float startPosX = 0;
        if (levelData.xCount % 2 == 1)
        {
            startPosX = -(levelData.xCount / 2) * GameData.tileWidth;
        }
        else
        {
            startPosX = -(levelData.xCount / 2) * GameData.tileWidth + GameData.tileWidth / 2.0f;
        }

        float startPosY = 0;
        if (levelData.yCount % 2 == 1)
        {
            startPosY = (levelData.yCount / 2) * GameData.tileHeight;
        }
        else
        {
            startPosY = ((levelData.yCount / 2) - 1) * GameData.tileHeight + GameData.tileHeight / 2.0f;
        }
        gridData.Clear();
        for (int i = 0; i < levelData.yCount; i++)
        {
            List<Square> lineData = new List<Square>();
            gridData.Add(lineData);
            for (int j = 0; j < levelData.xCount; j++)
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

                //加入数据链表
                lineData.Add(squareScript);

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

        //判断edge是否显示
        for (int i = 0; i < levelData.yCount; i++)
        {
            for (int j = 0; j < levelData.xCount; j++)
            {
                if (gridData[i][j].aboveBlock.blockType == BlockType.NONE)
                {
                    continue;
                }
                //上
                //如果是最上边的格子，并且block不是NONE
                if (i==0)
                {
                    gridData[i][j].edges[0].SetActive(true);
                }
                //如果不是最上边的格子，就要判断它的上一排是否是NONE
                else if (i!=0&& gridData[i-1][j].aboveBlock.blockType == BlockType.NONE)
                {
                    gridData[i][j].edges[0].SetActive(true);
                }

                //下
                //如果是最下边的格子，并且block不是NONE
                if (i == levelData.yCount-1)
                {
                    gridData[i][j].edges[1].SetActive(true);
                }
                //如果不是最下边的格子，就要判断它的下一排是否是NONE
                else if (i != (levelData.yCount - 1) && gridData[i + 1][j].aboveBlock.blockType == BlockType.NONE)
                {
                    gridData[i][j].edges[1].SetActive(true);
                }

                //左
                if (j == 0)
                {
                    gridData[i][j].edges[2].SetActive(true);
                }
                //如果不是最上边的格子，就要判断它的上一排是否是NONE
                else if (j != 0 && gridData[i][j-1].aboveBlock.blockType == BlockType.NONE)
                {
                    gridData[i][j].edges[2].SetActive(true);
                }

                //右
                if (j == levelData.xCount-1)
                {
                    gridData[i][j].edges[3].SetActive(true);
                }
                //如果不是最上边的格子，就要判断它的上一排是否是NONE
                else if (j != levelData.xCount - 1 && gridData[i][j + 1].aboveBlock.blockType == BlockType.NONE)
                {
                    gridData[i][j].edges[3].SetActive(true);
                }
            }
        }
    }

    void GenerateJelly()
    {
        for (int i=0; i< levelData.yCount;i++)
        {
            for (int j=0;j<levelData.xCount;j++)
            {
                switch (gridData[i][j].aboveBlock.blockType)
                {     
                    //可以生成糖果的block                                      
                    case BlockType.EMPTY:                     
                    case BlockType.SINGLEBLOCK:                       
                    case BlockType.DOUBLEBLOCK:                       
                    case BlockType.ICEBLOCK:
                        {
                            GameObject jelly = Instantiate<GameObject>(jellyPrefab);
                            Item jellyScript= jelly.GetComponent<Item>();
                            gridData[i][j].aboveBlock.itemAbove = jellyScript;
                            //
                            jellyScript.RandomJelly(levelData.jellyKindCount);
                            jelly.transform.SetParent(JellyRoot);
                            jelly.transform.position = new Vector3(gridData[i][j].aboveBlock.transform.position.x, gridData[i][j].aboveBlock.transform.position.y, GameData.jellyZOrderPos);


                        }
                        break;
                    //不能生成糖果的block
                    case BlockType.NONE:
                    case BlockType.SOLIDBLOCK:                       
                    case BlockType.UNDESTROYBLOCK:                       
                    case BlockType.GROWUPBLOCK:               
                        break;
                    default:
                        break;
                }
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
