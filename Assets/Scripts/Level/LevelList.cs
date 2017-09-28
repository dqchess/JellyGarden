using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum LevelLimit
{
    MOVE,
    TIME
}

[System.Serializable]
public enum LevelTarget
{
    SCORE,//分数
    ELIMINATE,//消除糖果（种类和数量）
    COLLECTION,//收集(水果掉落)
    DESTROY//摧毁block
}

[System.Serializable]
public class LevelData
{

    public int levelNum=0;//关卡关数
    public Sprite background;//背景图案
    public Transform levelButtonParent;//关卡按钮位置
    public int xCount=9;//x方向上有几列
    public int yCount=11;//y方向上有几行

    public LevelLimit levelLimit=LevelLimit.MOVE;//关卡结束方式 移动限制 或者 时间限制
    public int moveLimit=15;//移动次数限制
    public int timeLimit=100;//关卡时间限制

    public int jellyKindCount=4;//关卡糖果种类数量（多少种糖果出现在关卡）

    public int[] starScore = new int[3] { 100,200,300};//评级所需要的分数

    public LevelTarget levelTarget=LevelTarget.SCORE;//关卡目标

    public List<JellyColor> eliminateTargetTypeList = new List<JellyColor>();//消除糖果的目标种类
    public List<int> eliminateTargetCount = new List<int>();//消除糖果的目标数量

    public List<IngredientType> collectionTargetTypeList = new List<IngredientType>();//收集的目标种类
    public List<int> collectionTargetCount = new List<int>();//收集的目标数量


    //格子数据
    public BlockType[] block = new BlockType[GameData.maxRow*GameData.maxCol];

    public LevelData()
    {
        ResetAllGrid();
    }

    public void ResetAllGrid()
    {
        for (int i = 0; i < GameData.maxRow; i++)
        {
            for (int j = 0; j < GameData.maxCol; j++)
            {
                block[i*GameData.maxCol+j] = BlockType.EMPTY;
            }
        }
    }

    public int GetBlockCount()
    {
        int count = 0;
        for (int i=0;i<yCount;i++)
        {
            for (int j=0;j<xCount;j++)
            {
                if (block[i * GameData.maxCol + j] ==BlockType.SINGLEBLOCK)
                {
                    count += 2;
                }
                else if (block[i * GameData.maxCol + j] ==BlockType.DOUBLEBLOCK)
                {
                    count++;
                }
            }
        }
        return count;
    }
}


public class LevelList : ScriptableObject
{
    public List<LevelData> levelList = new List<LevelData>();
}
