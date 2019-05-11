using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGen : MonoBehaviour
{
    public int width;
    public int height;

    [Range(0, 100)]
    public int PercentFilled;

    int[,] map;

    void Start()
    {
        GenerateMap();
    }

    void GenerateMap()
    {
        map = new int[width, height];
    }
}
