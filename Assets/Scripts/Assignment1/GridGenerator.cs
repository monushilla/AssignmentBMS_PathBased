using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    public GameObject cubePrefab; // Prefab of the cube
    public int gridSize = 10; // Size of the grid

    void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                Vector3 position = new Vector3(x, 0, y);
                GameObject cube = Instantiate(cubePrefab, position, Quaternion.identity);
                cube.transform.parent = this.transform;
                TileInfo tileInfo = cube.GetComponent<TileInfo>();
                tileInfo.gridPosition = new Vector2Int(x, y);
            }
        }
    }
}
