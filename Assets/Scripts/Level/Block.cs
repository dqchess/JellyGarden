using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum BlockType
{
    NONE=0,//没有格子
    EMPTY,
    SINGLEBLOCK,//单个block 可以打破 作为关卡目标
    DOUBLEBLOCK,//双层block 打破后变成sigleBlcok

    ICEBLOCK,//冰块-可以放糖果，被打破前不能移动
    SOLIDBLOCK,//木板-不能放糖果，可以被打破
    UNDESTROYBLOCK,//石头-不能放糖果不可破坏
    GROWUPBLOCK,//巧克力 不能放糖果会自动生长替代掉原来的糖果，可以打破

}
    


public class Block : MonoBehaviour {

    ////资源
    //public Sprite[] blockRes;//block资源
    //[Space]

    //属性
    public BlockType blockType;//block类型
    public SpriteRenderer spriteRenderer;
    public int xPos;//x位置
    public int yPos;//y位置
    public Square belongToSquare;//隶属于哪个Square
    public Item itemAbove;//block上的糖果

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetBlockType(BlockType type)
    {
        blockType = type;
        switch (type)
        {
            case BlockType.NONE:
                belongToSquare.spriteRnderer.enabled = false;
                //this.gameObject.SetActive(false);
                spriteRenderer.enabled = false;
                break;
            case BlockType.EMPTY:
                //this.gameObject.SetActive(false);
                spriteRenderer.enabled = false;
                break;
            case BlockType.SINGLEBLOCK:
                spriteRenderer.sprite = ResManager.instance.blockRes[(int)BlockType.SINGLEBLOCK];
                this.transform.localPosition = new Vector3(this.transform.localPosition.x, this.transform.localPosition.y, GameData.belowBlockZOrderPos);
                break;
            case BlockType.DOUBLEBLOCK:
                spriteRenderer.sprite = ResManager.instance.blockRes[(int)BlockType.DOUBLEBLOCK];
                this.transform.localPosition = new Vector3(this.transform.localPosition.x, this.transform.localPosition.y, GameData.belowBlockZOrderPos);
                break;
            case BlockType.ICEBLOCK:
                spriteRenderer.sprite = ResManager.instance.blockRes[(int)BlockType.ICEBLOCK];
                this.transform.localPosition = new Vector3(this.transform.localPosition.x, this.transform.localPosition.y, GameData.upperBlockZOrderPos);
                break;
            case BlockType.SOLIDBLOCK:
                spriteRenderer.sprite = ResManager.instance.blockRes[(int)BlockType.SOLIDBLOCK];
                this.transform.localPosition = new Vector3(this.transform.localPosition.x, this.transform.localPosition.y, GameData.upperBlockZOrderPos);
                break;
            case BlockType.UNDESTROYBLOCK:
                spriteRenderer.sprite = ResManager.instance.blockRes[(int)BlockType.UNDESTROYBLOCK];
                this.transform.localPosition = new Vector3(this.transform.localPosition.x, this.transform.localPosition.y, GameData.upperBlockZOrderPos);
                break;
            case BlockType.GROWUPBLOCK:
                spriteRenderer.sprite = ResManager.instance.blockRes[(int)BlockType.GROWUPBLOCK];
                this.transform.localPosition = new Vector3(this.transform.localPosition.x, this.transform.localPosition.y, GameData.upperBlockZOrderPos);
                break;
            default:
                break;
        }
    }
}
