using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public ObstacleData obstacleData;
    private int gridSize = 10;

    public List<Vector2Int> FindPath(Vector2Int start, Vector2Int target)
    {
        List<Vector2Int> openSet = new List<Vector2Int> { start };
        HashSet<Vector2Int> closedSet = new HashSet<Vector2Int>();
        Dictionary<Vector2Int, Vector2Int> cameFrom = new Dictionary<Vector2Int, Vector2Int>();
        Dictionary<Vector2Int, float> gScore = new Dictionary<Vector2Int, float>();
        Dictionary<Vector2Int, float> fScore = new Dictionary<Vector2Int, float>();

        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                gScore[new Vector2Int(x, y)] = float.MaxValue;
                fScore[new Vector2Int(x, y)] = float.MaxValue;
            }
        }

        gScore[start] = 0;
        fScore[start] = Heuristic(start, target);

        while (openSet.Count > 0)
        {
            Vector2Int current = GetLowestFScore(openSet, fScore);
            if (current == target)
            {
                return ReconstructPath(cameFrom, current);
            }

            openSet.Remove(current);
            closedSet.Add(current);

            foreach (Vector2Int neighbor in GetNeighbors(current))
            {
                if (closedSet.Contains(neighbor) || IsObstacle(neighbor))
                {
                    continue;
                }

                float tentativeGScore = gScore[current] + 1; // Distance between current and neighbor is always 1

                if (!openSet.Contains(neighbor))
                {
                    openSet.Add(neighbor);
                }
                else if (tentativeGScore >= gScore[neighbor])
                {
                    continue;
                }

                cameFrom[neighbor] = current;
                gScore[neighbor] = tentativeGScore;
                fScore[neighbor] = gScore[neighbor] + Heuristic(neighbor, target);
            }
        }

        return null; // Path not found
    }

    private float Heuristic(Vector2Int a, Vector2Int b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y); // Manhattan distance
    }

    private Vector2Int GetLowestFScore(List<Vector2Int> openSet, Dictionary<Vector2Int, float> fScore)
    {
        Vector2Int lowest = openSet[0];
        foreach (Vector2Int node in openSet)
        {
            if (fScore[node] < fScore[lowest])
            {
                lowest = node;
            }
        }
        return lowest;
    }

    private List<Vector2Int> ReconstructPath(Dictionary<Vector2Int, Vector2Int> cameFrom, Vector2Int current)
    {
        List<Vector2Int> path = new List<Vector2Int> { current };
        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            path.Insert(0, current);
        }
        return path;
    }

    private List<Vector2Int> GetNeighbors(Vector2Int node)
    {
        List<Vector2Int> neighbors = new List<Vector2Int>();

        if (node.x > 0) neighbors.Add(new Vector2Int(node.x - 1, node.y));
        if (node.x < gridSize - 1) neighbors.Add(new Vector2Int(node.x + 1, node.y));
        if (node.y > 0) neighbors.Add(new Vector2Int(node.x, node.y - 1));
        if (node.y < gridSize - 1) neighbors.Add(new Vector2Int(node.x, node.y + 1));

        return neighbors;
    }

    public bool IsObstacle(Vector2Int position)
    {
        if (position.x < 0 || position.x >= gridSize || position.y < 0 || position.y >= gridSize)
        {
            return true; // Treat out of bounds as obstacles
        }

        int index = position.y * gridSize + position.x;
        return obstacleData.obstacleGrid[index];
    }
}
