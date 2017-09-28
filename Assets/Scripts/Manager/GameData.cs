using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData
{
    public static GameData instance = new GameData();

    public const int maxRow = 11;
    public const int maxCol = 9;

    public const float tileWidth = 0.6f;
    public const float tileHeight = 0.6f;

    public const float jellyZOrderPos = -0.2f;
    public const float upperBlockZOrderPos = -0.3f;
    public const float belowBlockZOrderPos = -0.1f;
    public const float squareZOrderPos = 0f;

	
}
