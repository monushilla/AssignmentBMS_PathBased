using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    public ObstacleData obstacleData;
    public GameObject obstaclePrefab;

    void Start()
    {
        GenerateObstacles();
    }

    void GenerateObstacles()
    {
        if (obstacleData == null || obstaclePrefab == null)
        {
            Debug.LogError("Obstacle Data or Prefab not assigned.");
            return;
        }

        for (int y = 0; y < 10; y++)
        {
            for (int x = 0; x < 10; x++)
            {
                int index = y * 10 + x;
                if (obstacleData.obstacleGrid[index])
                {
                    Vector3 position = new Vector3(x, 0.5f, y); // Adjust height as needed
                    Instantiate(obstaclePrefab, position, Quaternion.identity);
                }
            }
        }
    }
}
