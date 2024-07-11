using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    public Camera mainCamera;
    public Pathfinding pathfinding;
    public GameObject playerPrefab; // Assign the player prefab in the inspector
    public GameObject enemyPrefab; // Assign the enemy prefab in the inspector
    private PlayerController playerController;
    private bool isMoving = false;

    void Start()
    {
        Vector2Int playerPosition = InstantiatePlayerAtRandomPosition();
        InstantiateEnemyAtRandomPosition(playerPosition);
    }

    void Update()
    {
        if (isMoving || (playerController != null && playerController.IsMoving()))
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                TileInfo tileInfo = hit.collider.GetComponent<TileInfo>();
                if (tileInfo != null && playerController != null)
                {
                    StartCoroutine(MovePlayer(tileInfo.gridPosition));
                }
            }
        }
    }

    private Vector2Int InstantiatePlayerAtRandomPosition()
    {
        Vector2Int randomPosition = GetRandomValidPosition();
        Vector3 worldPosition = new Vector3(randomPosition.x, 0.5f, randomPosition.y); // Adjust height as needed
        GameObject player = Instantiate(playerPrefab, worldPosition, Quaternion.identity);
        playerController = player.GetComponent<PlayerController>();
        return randomPosition;
    }

    private void InstantiateEnemyAtRandomPosition(Vector2Int playerPosition)
    {
        Vector2Int randomPosition = GetRandomValidPositionExcluding(playerPosition);
        Vector3 worldPosition = new Vector3(randomPosition.x, 0.5f, randomPosition.y); // Adjust height as needed
        GameObject enemy = Instantiate(enemyPrefab, worldPosition, Quaternion.identity);
        EnemyAI enemyAI = enemy.GetComponent<EnemyAI>();
        enemyAI.player = playerController.transform;
        enemyAI.pathfinding = pathfinding;
    }

    private Vector2Int GetRandomValidPosition()
    {
        List<Vector2Int> validPositions = new List<Vector2Int>();

        for (int y = 0; y < 10; y++)
        {
            for (int x = 0; x < 10; x++)
            {
                Vector2Int position = new Vector2Int(x, y);
                if (!pathfinding.IsObstacle(position))
                {
                    validPositions.Add(position);
                }
            }
        }

        if (validPositions.Count == 0)
        {
            Debug.LogError("No valid positions available!");
            return Vector2Int.zero; // Default to (0, 0) if no valid positions
        }

        int randomIndex = Random.Range(0, validPositions.Count);
        return validPositions[randomIndex];
    }

    private Vector2Int GetRandomValidPositionExcluding(Vector2Int excludePosition)
    {
        List<Vector2Int> validPositions = new List<Vector2Int>();

        for (int y = 0; y < 10; y++)
        {
            for (int x = 0; x < 10; x++)
            {
                Vector2Int position = new Vector2Int(x, y);
                if (position != excludePosition && !pathfinding.IsObstacle(position))
                {
                    validPositions.Add(position);
                }
            }
        }

        if (validPositions.Count == 0)
        {
            Debug.LogError("No valid positions available!");
            return Vector2Int.zero; // Default to (0, 0) if no valid positions
        }

        int randomIndex = Random.Range(0, validPositions.Count);
        return validPositions[randomIndex];
    }

    private IEnumerator MovePlayer(Vector2Int targetPosition)
    {
        isMoving = true;
        playerController.Moving = true;
        playerController.startWalking();
        Vector2Int startPosition = new Vector2Int(Mathf.RoundToInt(playerController.transform.position.x), Mathf.RoundToInt(playerController.transform.position.z));
        List<Vector2Int> path = pathfinding.FindPath(startPosition, targetPosition);

        if (path != null)
        {
            foreach (Vector2Int position in path)
            {
                Vector3 worldPosition = new Vector3(position.x, playerController.transform.position.y, position.y);
                Vector3 direction = (worldPosition - playerController.transform.position).normalized;

                // Rotate to face the direction of movement quickly
                if (direction != Vector3.zero)
                {
                    playerController.transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
                }
                playerController.MoveTo(worldPosition);

                while (playerController.IsMoving())
                {
                    yield return null;
                }
            }
        }

        isMoving = false;
        playerController.Moving = false;
        playerController.stopWalking();
    }
    
}
