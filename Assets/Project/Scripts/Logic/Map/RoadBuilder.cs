using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadMapBuilder : MonoBehaviour
{

    //纵横
    public int row = 30;
    public int col = 35;

    public Vector2 originPoint;
    public float offset;
    public GameObject floorPrefab;
    public GameObject wallPrefab;

    //生成的逻辑地图
    private int[,] maze;
    //世界地图
    private GameObject[,] map;
    //
    public List<Vector2> moves = new List<Vector2>();

    private bool[,] mapArray;

    // Use this for initialization
    void Start()
    {
        InitMap();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void InitMap()
    {
        maze = new int[row, col];

        map = new GameObject[row, col];

        QueryRoad(0, 0);
    }

    void QueryRoad(int r, int c)
    {
        string dirs = "";

        if ((r - 2 >= 0) && (maze[r - 2, c] == 0)) dirs += "N"; //检查西面的格子是否被访问 
        if ((c - 2 >= 0) && (maze[r, c - 2] == 0)) dirs += "W"; //检查南面的格子是否被访问 
        if ((r + 2 < row) && (maze[r + 2, c] == 0)) dirs += "S"; //检查东面的格子是否被访问 
        if ((c + 2 < col) && (maze[r, c + 2] == 0)) dirs += "E";

        if (dirs.Equals(""))
        {
            //删除顶上的这个格子
            moves.RemoveAt(moves.Count - 1);

            if (moves.Count == 0)
            {
                //如果容器空了，说明迷宫生成完毕，可以开始绘制迷宫了

                DrawMap();
            }
            else
            {
                //否则基于新的点，继续查找下一个目标点
                QueryRoad((int)moves[moves.Count - 1].x, (int)moves[moves.Count - 1].y);
            }
        }
        else
        {
            //随机一个可以被访问的点
            int ran = Random.Range(0, dirs.Length);
            char dir = dirs[ran];

            //连通目标点和当前点之间的这个点 
            switch (dir)
            {
                case 'E': //将中间这个点设置为已访问的 
                    maze[r, c + 1] = 1; c = c + 2; break;
                case 'S': //将中间这个点设置为已访问的 
                    maze[r + 1, c] = 1; r = r + 2; break;
                case 'W': //将中间这个点设置为已访问的 
                    maze[r, c - 1] = 1; c = c - 2; break;
                case 'N': //将中间这个点设置为已访问的 
                    maze[r - 1, c] = 1; r = r - 2; break;
            }

            //将这个新的目标点设置为已访问的 
            maze[r, c] = 1;
            //将这个新的目标点加入容器 
            moves.Add(new Vector2(r, c));
            //基于新的点，继续查找下一个目标点 
            QueryRoad(r, c); 
            
        }
    }


    void DrawMap()
    {
        for (int i = 0; i < row; i++) {
            for (int j = 0; j < col; j++) {
                switch (maze[i, j]) {
                    case 1:
                        if (map[i, j] != null) {
                            if (map[i, j].tag == "Floor")
                            {
                                continue;
                            } else if (map[i, j].tag == "Wall")
                            {
                                Destroy(map[i, j]); map[i, j] = null;
                            }
                        }
                        map[i, j] = Instantiate(floorPrefab, originPoint + 
                            new Vector2(j * offset, i * offset), Quaternion.identity);
                        break;
                    case 0:
                        if (map[i, j] != null) {
                            if (map[i, j].tag == "Wall")
                            {
                                continue;
                            } else if (
                                map[i, j].tag == "Floor")
                            {
                                Destroy(map[i, j]);
                                map[i, j] = null;
                            }
                        }
                        map[i, j] = Instantiate(wallPrefab, originPoint +
                            new Vector2(j * offset, i * offset), Quaternion.identity);
                        break;
                }
            }
        }
        
    }

}
