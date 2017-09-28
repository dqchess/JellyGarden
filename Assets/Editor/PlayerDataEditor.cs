using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEngine;

public class PlayerDataEditor : EditorWindow
{

    static public PlayerData playerData;   

    [MenuItem("GameEditor/PlayerDataEditor")]
    static public void PlayerDataEditorWindow()
    {
        EditorWindow.GetWindow<PlayerDataEditor>("PlayerDataEditor");
        string path = Application.streamingAssetsPath + "/playerData.gd";
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
    }

    private void OnGUI()
    {
        
        playerData.life = (int)EditorGUILayout.IntField("Life", playerData.life);
        
        if (GUILayout.Button("Save"))
        {
            SaveChanges();
        }
    }

    private void SaveChanges()
    {
        string path = Application.streamingAssetsPath + "/playerData.gd";
        BinaryFormatter bf = new BinaryFormatter();
        if (File.Exists(path))
        {
            File.Delete(path);
        }
        FileStream file = File.Create(path);
        bf.Serialize(file, playerData);
        file.Close();
    }
}
