using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[System.Serializable]
public struct PlayerLevelRecord
{
    public int starCount;
    public int playerScore;
}

[System.Serializable]
public class PlayerData
{ 
    //游戏设置
    public bool isMusicOn=true;
    public bool isSoundOn=true;


    //玩家数据
    public int life=5;//生命
    public int reachedLevel=1;//达到的关卡
    public List<PlayerLevelRecord> list_levelScore=new List<PlayerLevelRecord>();//每关的分数

    public PlayerData()
    {
        PlayerLevelRecord record = new PlayerLevelRecord();
        record.starCount = 0;
        record.playerScore = 0;
        list_levelScore.Add(record);
    }
}


public class PlayerDataOperator : MonoBehaviour {
   
    public PlayerData playerData;

    string path;
	// Use this for initialization
	void Awake () {
        SetPath();
        //LoadPlayerData();
	}
	
	// Update is called once per frame
	//void Update () {		
	//}

    public PlayerData LoadPlayerData()
    {
        if (File.Exists(path))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(path, FileMode.Open);
            playerData = (PlayerData)bf.Deserialize(file);
            file.Close();
        }
        else
        {
            playerData = new PlayerData();
        }

        return playerData;
    }

    public void SavePlayerData()
    {        
        BinaryFormatter bf = new BinaryFormatter();
        if (File.Exists(path))
        {
            File.Delete(path);
        }
        FileStream file = File.Create(path);      
        bf.Serialize(file, playerData);
        file.Close();       
        
    }

    void SetPath()
    {
        if (Application.platform==RuntimePlatform.Android)
        {
            path = Application.persistentDataPath + "/playerData.gd";
        }
        else if (Application.platform==RuntimePlatform.WindowsEditor)
        {
            path = Application.streamingAssetsPath + "/playerData.gd";
        }
    }
}
