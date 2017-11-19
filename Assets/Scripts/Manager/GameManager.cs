using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    static public GameManager instance;

    //Managers
    public UIManager uiManager;//UI管理
    public ResManager resManager;//资源管理
    public PlayerDataOperator playerDataOperator;//玩家数据操作(读取和保存)

    //
    public int runningLevel;//正在运行的level

    void Awake()
    {
        instance = this;
        GameObject.DontDestroyOnLoad(this.gameObject);       
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
