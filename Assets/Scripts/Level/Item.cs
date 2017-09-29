using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum JellyColor
{
    RED = 0,
    ORANGE,
    PURPLE,
    BLUE,
    GREEN,
    YELLOW,
    None
}

[System.Serializable]
public enum JellyType
{
    NORMAL=0,//普通
    PACKAGE,//包装
    HORIZONTAL,//水平条纹
    VERTICAL,//竖向条纹
    COLORFUL,//彩色糖果
    INGREDIENT//原料-樱桃 开心果
}

[System.Serializable]
public enum IngredientType
{
    CHERRY=0,//樱桃
    PISTACHIO//开心果
}

//糖果状态结构体，用于序列化二维数组
[System.Serializable]
public struct JellyColorRes
{
    public Sprite[] jellyColorRes;
}


public class Item : MonoBehaviour {

    ////资源
    //[Tooltip(" Jelly[种类][颜色]  资源")]
    //public JellyColorRes[] jellyRes;// Jelly[种类][颜色]
    //[Tooltip("樱桃 或者 开心果")]
    //public Sprite[] ingredientRes;
    //public Sprite colorfulJellyRes;
    //[Space]

    //自身属性
    public JellyType jellyType;
    public JellyColor jellyColor;   
    public IngredientType ingredientType;

    public SpriteRenderer spriteRenderer;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void RandomJelly(int typeLimit)
    {
        jellyType = JellyType.NORMAL;
        jellyColor = (JellyColor)Random.Range(0, typeLimit - 1);
        spriteRenderer.sprite = ResManager.instance.jellyRes[(int)jellyType].jellyColorRes[(int)jellyColor];

    }
}
