using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LevelEditor : EditorWindow
{
    static public LevelList data=null;

    int currentLevel = 1;//当前关卡
    BlockType choosenType = BlockType.NONE;
    Vector2 scrollPos=new Vector2(0,0);
    GUIStyle style=new GUIStyle();
    

    [MenuItem("GameEditor/LevelEditor")]
    static public void LevelEditorWindow()
    {
        //打开一个窗口
        EditorWindow.GetWindow<LevelEditor>("LevelEditor");

        //加载 LevelAsset 文件-如果没有 就创建一个新的文件
        Object taskAsset = null;
        taskAsset = Resources.Load("LevelData/LevelAsset");
        if (taskAsset == null)
        {
            data = ScriptableObject.CreateInstance<LevelList>();
            AssetDatabase.CreateAsset(data, "Assets/Resources/LevelData/LevelAsset.asset");
            AssetDatabase.SaveAssets();
        }
        else
            data = (LevelList)Resources.Load("LevelData/LevelAsset");

        if (data.levelList.Count==0)
        {
            EditorUtility.SetDirty(data);
            data.levelList.Add(new LevelData());
            data.levelList[0].levelNum = 1;
            EditorUtility.SetDirty(data);
        }

        ////自动获取按钮位置
        //GameObject uiRoot=GameObject.Find("UIRoot");
        //Transform selectStagePanel=uiRoot.transform.Find("SelectStagePanel");       
        //bool activeState = selectStagePanel.gameObject.activeInHierarchy;
        //if (!activeState)
        //{
        //    selectStagePanel.gameObject.SetActive(true);
        //}
        //for (int i=0;i<data.levelList.Count;i++)
        //{
        //    if (data.levelList[i].levelButtonParent==null)
        //    {
        //        EditorUtility.SetDirty(data);
        //        data.levelList[i].levelButtonParent = GameObject.Find("LevelPos (" + (i + 1) + ")");
        //        EditorUtility.SetDirty(data);
        //    }            
        //}
        //selectStagePanel.gameObject.SetActive(activeState);
    }

    //格子资源
    public Texture emptyBlock;
    public Texture singleBlock;
    public Texture doubleBlock;
    public Texture iceBlock;
    public Texture solidBlock;
    public Texture undestroyBlock;
    public Texture growupBlock;
    

    private void OnGUI()
    {
        //标题
        GUITitle();
        GUILayout.Space(10);

        //选择关卡
        GUIChooseLevel();
        GUILayout.Space(5);

        //关卡按钮位置
        //GUIButtonPos();
        //GUILayout.Space(5);

        //行列数量
        GUIRowColCount();       
        GUILayout.Space(5);

        //关卡结束方式
        GUILevelLimit();
        GUILayout.Space(5);

        //关卡糖果颜色限制数量
        GUIColorLimit();
        GUILayout.Space(5);

        //评级所需要的分数
        GUIStars();
        GUILayout.Space(5);

        //关卡目标
        GUILevelTarget();
        GUILayout.Space(5);

        /////////关卡编辑/////////
        GUILevelEditor();
    }

    //标题
    void GUITitle()
    {
        EditorGUILayout.BeginHorizontal("Box");
        {
            GUILayout.Label("关卡编辑器");
            if (GUILayout.Button("TestLevel"))
            {
                //开始测试当前关卡
            }
        }
        EditorGUILayout.EndHorizontal();
    }

    //选择 创建 删除关卡
    void GUIChooseLevel()
    {
        EditorGUILayout.BeginHorizontal("Box");
        {
            //显示总关卡数量
            GUILayout.Label("TotalLevelCount" + " " + data.levelList.Count);
            GUILayout.Space(50);

            //<<<按钮，选择前一个关卡
            if (GUILayout.Button("<<<"))
            {
                currentLevel--;
                if (currentLevel < 1)
                {
                    currentLevel = 1;
                }
            }
            //输入需要编辑的关卡
            currentLevel = (int)EditorGUILayout.IntField("CurrentLevel", currentLevel);
            //>>>按钮，选择后一个关卡
            if (GUILayout.Button(">>>"))
            {
                currentLevel++;
            }

            GUILayout.Space(50);
            //新建一个关卡按钮
            if (GUILayout.Button("NewLevel"))
            {
                EditorUtility.SetDirty(data);
                data.levelList.Add(new LevelData());
                currentLevel = data.levelList.Count;
                data.levelList[currentLevel-1].levelNum = currentLevel;
                EditorUtility.SetDirty(data);
            }

            //删除一个关卡按钮
            GUI.color = Color.red;
            if (GUILayout.Button("DeleteLevel"))
            {
                if (data.levelList.Count > 1)
                {
                    EditorUtility.SetDirty(data);
                    data.levelList.RemoveAt(currentLevel - 1);
                    EditorUtility.SetDirty(data);
                }
            }
            GUI.color = Color.white;

            if (currentLevel > data.levelList.Count)
            {
                currentLevel = data.levelList.Count;
            }

        }
        EditorGUILayout.EndHorizontal();
    }

    //关卡按钮位置
    void GUIButtonPos()
    {
        EditorGUILayout.BeginHorizontal("Box");
        {
            EditorUtility.SetDirty(data);
            data.levelList[currentLevel - 1].levelButtonParent = (Transform)EditorGUILayout.ObjectField("ButtonPos", data.levelList[currentLevel - 1].levelButtonParent, typeof(Transform), true);
            EditorUtility.SetDirty(data);
        }
        EditorGUILayout.EndHorizontal();
    }

    //行列数量
    void GUIRowColCount()
    {
        EditorGUILayout.BeginHorizontal("Box");
        {
            EditorUtility.SetDirty(data);
            data.levelList[currentLevel - 1].yCount = (int)EditorGUILayout.IntSlider("Row", data.levelList[currentLevel - 1].yCount, 3, 11);
            data.levelList[currentLevel - 1].xCount = (int)EditorGUILayout.IntSlider("Col", data.levelList[currentLevel - 1].xCount, 3, 9);
            EditorUtility.SetDirty(data);
        }
        EditorGUILayout.EndHorizontal();
    }

    //关卡结束方式
    void GUILevelLimit()
    {
        EditorGUILayout.BeginHorizontal("Box");
        {
            EditorUtility.SetDirty(data);
            data.levelList[currentLevel - 1].levelLimit = (LevelLimit)EditorGUILayout.EnumPopup("LevelLimit", data.levelList[currentLevel - 1].levelLimit);
            if (data.levelList[currentLevel - 1].levelLimit == LevelLimit.MOVE)
            {
                data.levelList[currentLevel - 1].moveLimit = (int)EditorGUILayout.IntField("MoveLimit", data.levelList[currentLevel - 1].moveLimit);
            }
            else if (data.levelList[currentLevel - 1].levelLimit == LevelLimit.TIME)
            {
                data.levelList[currentLevel - 1].timeLimit = (int)EditorGUILayout.IntField("TimeLimit", data.levelList[currentLevel - 1].timeLimit);
            }
            EditorUtility.SetDirty(data);
        }
        EditorGUILayout.EndHorizontal();
    }
    
    //糖果种类限制
    void GUIColorLimit()
    {
        EditorGUILayout.BeginHorizontal("Box");
        {
            EditorUtility.SetDirty(data);
            data.levelList[currentLevel - 1].jellyKindCount = (int)EditorGUILayout.IntSlider("JellyColorCount", data.levelList[currentLevel - 1].jellyKindCount, 3, 6);
            EditorUtility.SetDirty(data);
        }
        EditorGUILayout.EndHorizontal();
    }

    //星星需要的分数
    void GUIStars()
    {
        EditorGUILayout.BeginHorizontal("Box");
        {
            EditorUtility.SetDirty(data);
            data.levelList[currentLevel - 1].starScore[0] = (int)EditorGUILayout.IntField("Star1", data.levelList[currentLevel - 1].starScore[0]);
            data.levelList[currentLevel - 1].starScore[1] = (int)EditorGUILayout.IntField("Star2", data.levelList[currentLevel - 1].starScore[1]);
            data.levelList[currentLevel - 1].starScore[2] = (int)EditorGUILayout.IntField("Star3", data.levelList[currentLevel - 1].starScore[2]);
            EditorUtility.SetDirty(data);
        }
        EditorGUILayout.EndHorizontal();
    }

    //关卡的目标
    void GUILevelTarget()
    {
        EditorGUILayout.BeginVertical("Box");
        {
            EditorUtility.SetDirty(data);
            data.levelList[currentLevel - 1].levelTarget = (LevelTarget)EditorGUILayout.EnumPopup("LevelTarget", data.levelList[currentLevel - 1].levelTarget);
            //如果是收集任务
            if (data.levelList[currentLevel - 1].levelTarget == LevelTarget.COLLECTION)
            {
                if (data.levelList[currentLevel - 1].collectionTargetTypeList.Count == 0)
                {
                    data.levelList[currentLevel - 1].collectionTargetTypeList.Add(IngredientType.CHERRY);
                    data.levelList[currentLevel - 1].collectionTargetTypeList.Add(IngredientType.PISTACHIO);
                    data.levelList[currentLevel - 1].collectionTargetCount.Clear();
                    data.levelList[currentLevel - 1].collectionTargetCount.Add(0);
                    data.levelList[currentLevel - 1].collectionTargetCount.Add(0);
                }
                EditorGUILayout.BeginHorizontal();
                {
                    data.levelList[currentLevel - 1].collectionTargetTypeList[0] = (IngredientType)EditorGUILayout.EnumPopup("IngrdientType", data.levelList[currentLevel - 1].collectionTargetTypeList[0]);
                    data.levelList[currentLevel - 1].collectionTargetCount[0] = (int)EditorGUILayout.IntField(data.levelList[currentLevel - 1].collectionTargetCount[0]);
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                {
                    data.levelList[currentLevel - 1].collectionTargetTypeList[1] = (IngredientType)EditorGUILayout.EnumPopup("IngrdientType", data.levelList[currentLevel - 1].collectionTargetTypeList[1]);
                    data.levelList[currentLevel - 1].collectionTargetCount[1] = (int)EditorGUILayout.IntField(data.levelList[currentLevel - 1].collectionTargetCount[1]);
                }
                EditorGUILayout.EndHorizontal();
            }
            else
            {
                if (data.levelList[currentLevel - 1].collectionTargetTypeList.Count > 0)
                {
                    data.levelList[currentLevel - 1].collectionTargetTypeList.Clear();
                    data.levelList[currentLevel - 1].collectionTargetCount.Clear();
                }
            }

            //如果是消除任务
            if (data.levelList[currentLevel - 1].levelTarget == LevelTarget.ELIMINATE)
            {
                if (data.levelList[currentLevel - 1].eliminateTargetTypeList.Count == 0)
                {
                    data.levelList[currentLevel - 1].eliminateTargetTypeList.Add(JellyColor.RED);
                    data.levelList[currentLevel - 1].eliminateTargetTypeList.Add(JellyColor.ORANGE);
                    data.levelList[currentLevel - 1].eliminateTargetCount.Clear();
                    data.levelList[currentLevel - 1].eliminateTargetCount.Add(0);
                    data.levelList[currentLevel - 1].eliminateTargetCount.Add(0);
                }
                EditorGUILayout.BeginHorizontal();
                {
                    data.levelList[currentLevel - 1].eliminateTargetTypeList[0] = (JellyColor)EditorGUILayout.EnumPopup("IngrdientType", data.levelList[currentLevel - 1].eliminateTargetTypeList[0]);
                    data.levelList[currentLevel - 1].eliminateTargetCount[0] = (int)EditorGUILayout.IntField(data.levelList[currentLevel - 1].eliminateTargetCount[0]);
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                {
                    data.levelList[currentLevel - 1].eliminateTargetTypeList[1] = (JellyColor)EditorGUILayout.EnumPopup("IngrdientType", data.levelList[currentLevel - 1].eliminateTargetTypeList[1]);
                    data.levelList[currentLevel - 1].eliminateTargetCount[1] = (int)EditorGUILayout.IntField(data.levelList[currentLevel - 1].eliminateTargetCount[1]);
                }
                EditorGUILayout.EndHorizontal();
            }
            else
            {
                if (data.levelList[currentLevel - 1].eliminateTargetTypeList.Count > 0)
                {
                    data.levelList[currentLevel - 1].eliminateTargetTypeList.Clear();
                    data.levelList[currentLevel - 1].eliminateTargetCount.Clear();
                }
            }
            EditorUtility.SetDirty(data);
        }
        EditorGUILayout.EndVertical();
    }

    //关卡格子编辑
    void GUILevelEditor()
    {
        //清空所有的格子
        if (GUILayout.Button("ResetAllGrid"))
        {
            GUI.color = Color.yellow;
            data.levelList[currentLevel - 1].ResetAllGrid();
            GUI.color = Color.white;
        }

        //列出可选的格子选项
        EditorGUILayout.BeginHorizontal("Box");
        GUILayout.Space(125);
        for (int i=0;i<8;i++)
        {
            Texture blockTex = null;
            switch ((BlockType)i)
            {
                case BlockType.NONE:
                    break;
                case BlockType.EMPTY:
                    blockTex = emptyBlock;
                    break;
                case BlockType.SINGLEBLOCK:
                    blockTex = singleBlock;
                    break;
                case BlockType.DOUBLEBLOCK:
                    blockTex = doubleBlock;
                    break;
                case BlockType.ICEBLOCK:
                    blockTex = iceBlock;
                    break;
                case BlockType.SOLIDBLOCK:
                    blockTex = solidBlock;
                    break;
                case BlockType.UNDESTROYBLOCK:
                    blockTex = undestroyBlock;
                    break;
                case BlockType.GROWUPBLOCK:
                    blockTex = growupBlock;
                    break;
                default:
                    break;
            }
            if (GUILayout.Button(blockTex, new GUILayoutOption[] { GUILayout.Width(50), GUILayout.Height(50) }))
            {
                choosenType = (BlockType)i;
            }
        }
        GUILayout.Space(125);
        EditorGUILayout.EndHorizontal();

        //列出关卡格子
        
        EditorGUILayout.BeginVertical("Box");
        scrollPos=EditorGUILayout.BeginScrollView(scrollPos);
        GUILayout.Space(20);

        //列编号
        EditorGUILayout.BeginHorizontal();
        {
            GUILayout.Space(120);
            for (int i=0;i< data.levelList[currentLevel - 1].xCount;i++)
            {
                GUILayout.Label("" + (i + 1), new GUILayoutOption[] { GUILayout.Width(50)});                
            }
        }
        EditorGUILayout.EndHorizontal();
        
        for (int i = 0; i < data.levelList[currentLevel - 1].yCount; i++)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(80);

            //行编号   
            style.alignment = TextAnchor.MiddleRight;             
            GUILayout.Label("" + (i + 1), style, new GUILayoutOption[] { GUILayout.Width(20),GUILayout.Height(50) });

            for (int j = 0; j < data.levelList[currentLevel - 1].xCount; j++)
            {
                Texture blockTex=null;
                switch (data.levelList[currentLevel - 1].block[i * GameData.maxCol + j])
                {
                    case BlockType.NONE:                       
                        break;
                    case BlockType.EMPTY:
                        blockTex = emptyBlock;
                        break;
                    case BlockType.SINGLEBLOCK:
                        blockTex = singleBlock;
                        break;
                    case BlockType.DOUBLEBLOCK:
                        blockTex = doubleBlock;
                        break;
                    case BlockType.ICEBLOCK:
                        blockTex = iceBlock;
                        break;
                    case BlockType.SOLIDBLOCK:
                        blockTex = solidBlock;
                        break;
                    case BlockType.UNDESTROYBLOCK:
                        blockTex = undestroyBlock;
                        break;
                    case BlockType.GROWUPBLOCK:
                        blockTex = growupBlock;
                        break;
                    default:
                        break;
                }
                
                if (GUILayout.Button(blockTex, new GUILayoutOption[] { GUILayout.Width(50), GUILayout.Height(50) }))
                {
                    EditorUtility.SetDirty(data);
                    data.levelList[currentLevel - 1].block[i * GameData.maxCol + j] = choosenType;
                    EditorUtility.SetDirty(data);
                }
                
            }
            EditorGUILayout.EndHorizontal();
        }        
        //列编号
        EditorGUILayout.BeginHorizontal();
        {
            GUILayout.Space(120);
            for (int i = 0; i < data.levelList[currentLevel - 1].xCount; i++)
            {
                GUILayout.Label("" + (i + 1), new GUILayoutOption[] { GUILayout.Width(50) });
            }
        }
        EditorGUILayout.EndHorizontal();
        GUILayout.Space(20);
        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
       
    }
}
