using UnityEngine;
using System.Collections;

public class MapGenerator : MonoBehaviour
{
    public int width;
    public int height;

    public string seed;
    public bool useRandomSeed;

    [Range(0, 100)]
    public int randomFillPercent;

    int[,] map;
    // Use this for initialization  

    public GameObject cube1, cube2;
    GameObject cubes;

    public float cubeDis = 1.2f;
    public int smoothCount = 3;

    void Start()
    {
       
        GenerateMap();
    }

    // Update is called once per frame  
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            GenerateMap();
        }
    }

    void GenerateMap()
    {
        if (cubes)
            Destroy(cubes);

        cubes = new GameObject("Map");
        map = new int[width, height];
        RandomFillMap();

        for (int i = 0; i < smoothCount; i++)
        {
            SmoothMap();
        }

        DrawMap();
    }

    void RandomFillMap()
    {
        if (useRandomSeed)
        {
            seed = Time.time.ToString();
        }

        System.Random pseudoRandom = new System.Random(seed.GetHashCode());

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
                    map[x, y] = (pseudoRandom.Next(0, 100) < randomFillPercent) ? 1 : 0;
                }
            }
        }
    }

    void SmoothMap()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int neighbourWallTiles = GetSurroundingWallCount(x, y);

                if (neighbourWallTiles > 4)
                    map[x, y] = 1;
                else if (neighbourWallTiles < 4)
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

    void DrawMap() {
//        if (map != null)
//        {
//            for (int x = 0; x < width; x++)
//            {
//                for (int y = 0; y < height; y++)
//                {
//                    if (map[x, y] == 1)
//                    {
//                        GameObject go = Instantiate(cube1, new Vector3(x, y, 0) * cubeDis - new Vector3(cubeDis * width / 2, cubeDis * height / 2, 0), Quaternion.identity) as GameObject;
//                        go.transform.SetParent(cubes.transform);
//                        go.isStatic = true;
//                    }
//                    else
//                    {
//                        GameObject go = Instantiate(cube2, new Vector3(x, y, 0) * cubeDis - new Vector3(cubeDis * width / 2, cubeDis * height / 2, 0), Quaternion.identity) as GameObject;
//                        go.transform.SetParent(cubes.transform);
//                        go.isStatic = true;
//                    }
//                }
//            }
//        }
		StartCoroutine(EnDrawMap());
    }

	IEnumerator EnDrawMap(){
		if (map != null)
		{
			
			for (int x = 0; x < width; x++)
			{
				for (int y = 0; y < height; y++)
				{
					float randX = Random.value;
					float randY = Random.value;
					float xSample = (randX + x/width)/2;
					float ySample = (randY + y/height)/2;
					if (map[x, y] == 1)
					{
						GameObject go = Instantiate(cube1, new Vector3(x, y, 0) * cubeDis - new Vector3(cubeDis * width / 2, cubeDis * height / 2, 0), Quaternion.identity) as GameObject;
						go.transform.SetParent(cubes.transform);
						go.GetComponent<MeshRenderer> ().material.color = new Color (0, 0, 1, xSample);
						go.isStatic = true;
					}
					else
					{
						GameObject go = Instantiate(cube2, new Vector3(x, y, 0) * cubeDis - new Vector3(cubeDis * width / 2, cubeDis * height / 2, 0), Quaternion.identity) as GameObject;
						go.transform.SetParent(cubes.transform);
						go.GetComponent<MeshRenderer> ().material.color = new Color (0, 1, 0, ySample);
						go.isStatic = true;
					}
				}
				yield return new WaitForSeconds (0.1f);
			}
		}
		yield return 0;
	}

    //void OnDrawGizmos()
    //{
    //    if (map != null)
    //    {
    //        for (int x = 0; x < width; x++)
    //        {
    //            for (int y = 0; y < height; y++)
    //            {
    //                Gizmos.color = (map[x, y] == 1) ? Color.black : Color.white;
    //                Vector3 pos = new Vector3(-width / 2 + x + .5f, 0, -height / 2 + y + .5f);
    //                Gizmos.DrawCube(pos, Vector3.one);
    //            }
    //        }
    //    }
    //}
}