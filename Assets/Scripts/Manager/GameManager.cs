using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    static public GameManager instance;

    //Managers
    public UIManager uiManager;
    public ResManager resManager;
    public PlayerDataOperator playerDataOperator;

    //
    public int runningLevel;

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
