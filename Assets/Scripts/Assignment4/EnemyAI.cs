using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour, IAI
{
    public Transform player;
    public Pathfinding pathfinding;
    public float moveSpeed = 2f;
    private bool isMoving = false;
    private bool targetReached = false; // Flag to indicate if the enemy has reached its target
    private Vector2Int lastPlayerPosition; // Track the player's last position
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        lastPlayerPosition = new Vector2Int(Mathf.RoundToInt(player.position.x), Mathf.RoundToInt(player.position.z));
    }

    void Update()
    {
        if (!isMoving && !targetReached)
        {
            Move();
        }
        // Check if the player has moved
        Vector2Int currentPlayerPosition = new Vector2Int(Mathf.RoundToInt(player.position.x), Mathf.RoundToInt(player.position.z));
        if (currentPlayerPosition != lastPlayerPosition)
        {
            targetReached = false;
            lastPlayerPosition = currentPlayerPosition;
        }
    }

    public void Move()
    {
        if (player == null)
        {
            return;
        }
        if (player.GetComponent<PlayerController>().Moving) return;

        Vector2Int playerPosition = new Vector2Int(Mathf.RoundToInt(player.position.x), Mathf.RoundToInt(player.position.z));
        Vector2Int enemyPosition = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.z));

        Vector2Int targetPosition = GetClosestAdjacentTile(playerPosition, enemyPosition);

        if (targetPosition != enemyPosition)
        {
            List<Vector2Int> path = pathfinding.FindPath(enemyPosition, targetPosition);
            if (path != null && path.Count > 0)
            {
                StartCoroutine(MoveAlongPath(path));
            }
        }
    }

    private Vector2Int GetClosestAdjacentTile(Vector2Int playerPosition, Vector2Int enemyPosition)
    {
        List<Vector2Int> adjacentPositions = new List<Vector2Int>
        {
            new Vector2Int(playerPosition.x + 1, playerPosition.y),
            new Vector2Int(playerPosition.x - 1, playerPosition.y),
            new Vector2Int(playerPosition.x, playerPosition.y + 1),
            new Vector2Int(playerPosition.x, playerPosition.y - 1)
        };

        adjacentPositions.RemoveAll(position => pathfinding.IsObstacle(position) || position == enemyPosition);

        if (adjacentPositions.Count == 0)
        {
            return enemyPosition; // Stay in place if no valid adjacent positions are available
        }

        adjacentPositions.Sort((a, b) =>
        {
            List<Vector2Int> pathA = pathfinding.FindPath(enemyPosition, a);
            List<Vector2Int> pathB = pathfinding.FindPath(enemyPosition, b);
            return pathA.Count.CompareTo(pathB.Count);
        });

        return adjacentPositions[0];
    }

    private IEnumerator MoveAlongPath(List<Vector2Int> path)
    {
        isMoving = true;
        startWalking();

        foreach (Vector2Int position in path)
        {
            Vector3 worldPosition = new Vector3(position.x, transform.position.y, position.y);
            Vector3 direction = (worldPosition - transform.position).normalized;

            // Rotate to face the direction of movement quickly
            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
            }

            while (Vector3.Distance(transform.position, worldPosition) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, worldPosition, moveSpeed * Time.deltaTime);
                yield return null;
            }

            transform.position = worldPosition; // Snap to grid position
        }

        isMoving = false;
        targetReached = true; // Mark target as reached
        stopWalking();
    }

    public void startWalking()
    {
        animator.SetBool("isMoving", true);
    }

    public void stopWalking()
    {
        animator.SetBool("isMoving", false);
    }
}
