using UnityEngine;

[CreateAssetMenu(fileName = "ObstacleData", menuName = "ScriptableObjects/ObstacleData", order = 1)]
public class ObstacleData : ScriptableObject
{
    public bool[] obstacleGrid = new bool[100]; // 10x10 grid flattened into a 1D array
}
