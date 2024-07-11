using UnityEditor;
using UnityEngine;

public class ObstacleTool : EditorWindow
{
    private ObstacleData obstacleData;

    [MenuItem("Tools/Obstacle Tool")]
    public static void ShowWindow()
    {
        GetWindow<ObstacleTool>("Obstacle Tool");
    }

    private void OnGUI()
    {
        if (obstacleData == null)
        {
            GUILayout.Label("Obstacle Data", EditorStyles.boldLabel);
            obstacleData = (ObstacleData)EditorGUILayout.ObjectField(obstacleData, typeof(ObstacleData), false);
        }

        if (obstacleData != null)
        {
            GUILayout.Label("Obstacle Grid", EditorStyles.boldLabel);
            for (int y = 0; y < 10; y++)
            {
                GUILayout.BeginHorizontal();
                for (int x = 0; x < 10; x++)
                {
                    int index = y * 10 + x;
                    obstacleData.obstacleGrid[index] = GUILayout.Toggle(obstacleData.obstacleGrid[index], "");
                }
                GUILayout.EndHorizontal();
            }

            if (GUILayout.Button("Save"))
            {
                EditorUtility.SetDirty(obstacleData);
                AssetDatabase.SaveAssets();
            }
        }
    }
}
