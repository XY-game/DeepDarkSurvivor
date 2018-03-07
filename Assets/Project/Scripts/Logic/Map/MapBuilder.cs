using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBuilder : MonoBehaviour {

    public int row = 30;
    public int col = 35;
    private bool[,] mapArray;
    public GameObject cube1, cube2;
    GameObject cubes;

    public float cubeDis = 1.2f;
    int times;
    //int a;
    public int randRange = 60;

    public int smoothRangeMax = 5;
    public int smoothRangeMin = 2;

    // Use this for initialization
    void Start () {
        cubes = new GameObject();
        mapArray = InitMapArray();
        mapArray = SmoothMapArray(mapArray);
        mapArray = SmoothMapArray(mapArray);
        mapArray = SmoothMapArray(mapArray);
        mapArray = SmoothMapArray(mapArray);
        mapArray = SmoothMapArray(mapArray);
        CreateMap(mapArray);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    bool[,] InitMapArray()
    {
        bool[,] array = new bool[row, col];
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                array[i, j] = Random.Range(0, 100) < randRange;
                if (i == 0 || i == row - 1 || j == 0 || j == col - 1)
                {
                    array[i, j] = false;
                }
            }
        }


        return array;
    }

    bool[,] SmoothMapArray(bool[,] array)
    {
        bool[,] newArray = new bool[row, col];
        int count1, count2;

        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                count1 = CheckNeighborWalls(array, i, j, 1);
                count2 = CheckNeighborWalls(array, i, j, 2);


                if (count1 >= smoothRangeMax || count2 <= smoothRangeMin)
                {
                    newArray[i, j] = false;
                }
                else
                {
                    newArray[i, j] = true;
                }


                if (i == 0 || i == row - 1 || j == 0 || j == col - 1)
                {
                    newArray[i, j] = false;
                }


                // newArray[i, j] = count1 >= 5 || count2 <= 2 ? true : false;
            }
        }
        return newArray;
    }


    int CheckNeighborWalls(bool[,] array, int i, int j, int t)
    {
        int count = 0;


        for (int i2 = i - t; i2 < i + t + 1; i2++)
        {
            for (int j2 = j - t; j2 < j + t + 1; j2++)
            {
                if (i2 > 0 && i2 < row && j2 >= 0 && j2 < col)
                {
                    if (!array[i2, j2])
                    {
                        count++;
                    }
                }
            }
        }
        if (!array[i, j])
            count--;
        return count;
    }


    void CreateMap(bool[,] array)
    {
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                if (!array[i, j])
                {
                    GameObject go = Instantiate(cube1, new Vector3(i, j, 0) * cubeDis - new Vector3(cubeDis * row / 2, cubeDis * col /2,0), Quaternion.identity) as GameObject;
                    go.transform.SetParent(cubes.transform);
                }
                else
                {
                    GameObject go = Instantiate(cube2, new Vector3(i, j, 0) * cubeDis - new Vector3(cubeDis * row / 2, cubeDis * col / 2, 0), Quaternion.identity) as GameObject;
                    go.transform.SetParent(cubes.transform);
                }
            }
        }
    }
}
