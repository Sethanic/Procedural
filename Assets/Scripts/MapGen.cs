using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGen : MonoBehaviour
{
    public int width;
    public int height;

    [Range(0, 100)]
    public int PercentFilled;

    public string Seed;
    public bool UseRandomSeed;

    int[,] map;

    void Start()
    {
        GenerateMap();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GenerateMap();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            SmoothMap(4, 5);
            Debug.Log("Smoothening");
            MeshGen meshGen = GetComponent<MeshGen>();
            meshGen.GenerateMesh(map, 1);
        }

        if (Input.GetMouseButton(1))
        {
            SmoothMap();
            Debug.Log("Smoothening");
            MeshGen meshGen = GetComponent<MeshGen>();
            meshGen.GenerateMesh(map, 1);
        }
    }

    void GenerateMap()
    {
        map = new int[width, height];
        RandomFillMap();

        for (int i = 0; i < 16; i ++)
        {
            //SmoothMap();
        }

        MeshGen meshGen = GetComponent<MeshGen>();
        meshGen.GenerateMesh(map, 1);
    }

    void RandomFillMap () 
    {
        if (UseRandomSeed)
        {
            Seed = Time.time.ToString();
        }

        System.Random prng = new System.Random(Seed.GetHashCode());

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (x == 0 || x == width - 1 || y == 0 || y == height - 1)
                {
                    map[x, y] = 1;
                }
                else
                {
                    map[x, y] = (prng.Next(0, 100) < PercentFilled) ? 1 : 0;
                }
            }
        }
    }

    void SmoothMap(int var1 = 4, int var2 = 4)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int neighbourWallTiles = GetSurroundingWallCount(x, y);
                // Standard     45%     4   4
                // Archipelago  45%     4   3
                // Simple Arch. 70%     4   5
                // Water Splash 75%     6   4
                // Starry Sky   95%     6   5
                // Swiss Cheese 30%     4   3
                if (neighbourWallTiles > var1)
                    map[x, y] = 1;
                else if (neighbourWallTiles < var2)
                    map[x, y] = 0;

            }
        }
    }

    int GetSurroundingWallCount(int gridX, int gridY)
    {
        int wallCount = 0;
        for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++)
        {
            for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++)
            {
                if (neighbourX >= 0 && neighbourX < width && neighbourY >= 0 && neighbourY < height)
                {
                    if (neighbourX != gridX || neighbourY != gridY)
                    {
                        wallCount += map[neighbourX, neighbourY];
                    }
                }
                else
                {
                    wallCount++;
                }
            }
        }
        return wallCount;
    }
}
