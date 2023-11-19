using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
[CreateAssetMenu(menuName = "Description", fileName = "DescriptionDFS")]
public class GenerateTileMap : MonoBehaviour
{
    public int width = 40;
    public int height = 40;
    public Tilemap Wall;
    public Tilemap InsidePath;
    public Tilemap PathFound;
    public Tilemap StartTile;
    public Tilemap EndTile;
    public Tilemap CatStep;
    public Tile wallTile;
    public Tile pathTile;
    public Tile startPointTile;
    public Tile endPointTile;
    // Tilemap mới để vẽ các đường đã tìm được
    public Tile foundPathTile;
    public Tile catStepTile;
    private bool[,] visited;
    Vector3Int startPoint = Vector3Int.zero;  // góc dưới bên trái
    Vector3Int endPoint = Vector3Int.zero;
    private List<Vector3Int> currentPath = new List<Vector3Int>();
    private List<Vector3Int> allPath = new List<Vector3Int>();

    void Start()
    {
        Generate();
        ResetVisited();
        FindAllPaths(startPoint);
    }

    void Generate()
    {
        visited = new bool[width, height];

        // Khởi tạo toàn bộ tilemap là wall
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Wall.SetTile(new Vector3Int(x, y, 0), wallTile);
            }
        }

        DFS(new Vector3Int(2, 2, 0));
        // After the DFS(new Vector3Int(2, 2, 0)); in your Generate() method
        SetStartAndEndPoints();

    }
    void SetStartAndEndPoints()
    {
        startPoint = new Vector3Int(2, 2, 0);  // góc dưới bên trái
        endPoint = new Vector3Int(width - 2, height - 2, 0);  // góc trên bên phải

        StartTile.SetTile(startPoint, startPointTile);
        EndTile.SetTile(endPoint, endPointTile);
        Manager.Instance.SpawnPlayerAtStart();
    }
    void ResetVisited()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                visited[x, y] = false;
            }
        }
    }
    void FindAllPaths(Vector3Int position)
    {
        if (position == endPoint)
        {
            DrawFoundPath();
            return;
        }

        Tile currentTile = Wall.GetTile(position) as Tile;
        if (!WithinBounds(position) || visited[position.x, position.y] || (currentTile != null && currentTile.name == wallTile.name))
            return;


        visited[position.x, position.y] = true;
        currentPath.Add(position);
        allPath.Add(position);
        // Di chuyển đến các lân cận
        FindAllPaths(position + Vector3Int.left);
        FindAllPaths(position + Vector3Int.right);
        FindAllPaths(position + Vector3Int.up);
        FindAllPaths(position + Vector3Int.down);

        visited[position.x, position.y] = false;
        currentPath.RemoveAt(currentPath.Count - 1);
    }

    void DrawFoundPath()
    {
        foreach (var pos in currentPath)
        {
            PathFound.SetTile(pos, foundPathTile);
        }
        foreach (var pos in allPath)
        {
            CatStep.SetTile(pos, catStepTile);
        }
    }
    void DFS(Vector3Int position)
    {
        visited[position.x, position.y] = true;

        // Đánh dấu vị trí hiện tại là path

        Wall.SetTile(position, null);
        InsidePath.SetTile(position, pathTile);

        List<Vector3Int> neighbours = GetNeighbours(position);
        Shuffle(neighbours);

        foreach (var neighbour in neighbours)
        {
            Vector3Int direction = neighbour - position;
            Vector3Int inBetween = position + direction / 2;

            if (WithinBounds(neighbour) && !visited[neighbour.x, neighbour.y])
            {
                // Đánh dấu vị trí trung gian là path
                Wall.SetTile(inBetween, null);
                InsidePath.SetTile(inBetween, pathTile);
                DFS(neighbour);
            }
        }
    }

    List<Vector3Int> GetNeighbours(Vector3Int position)
    {
        List<Vector3Int> neighbours = new List<Vector3Int>();

        if (position.x > 2) neighbours.Add(position + Vector3Int.left * 2);
        if (position.x < width - 3) neighbours.Add(position + Vector3Int.right * 2);
        if (position.y > 2) neighbours.Add(position + Vector3Int.down * 2);
        if (position.y < height - 3) neighbours.Add(position + Vector3Int.up * 2);

        return neighbours;
    }

    bool WithinBounds(Vector3Int position)
    {
        return position.x >= 0 && position.x < width && position.y >= 0 && position.y < height;
    }

    void Shuffle<T>(List<T> list)
    {
        int count = list.Count;
        for (int i = 0; i < count - 1; i++)
        {
            int randomIndex = Random.Range(i, count);
            T temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
}
