using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 2f;
    private bool isMoving = false;
    private Vector3 targetPosition;
    public bool Moving = false;
    private Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        if (isMoving)
        {
            float step = moveSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);

            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                isMoving = false;
            }
        }
        

    }

    public void MoveTo(Vector3 position)
    {
        targetPosition = position;
        isMoving = true;
    }

    public bool IsMoving()
    {
        return isMoving;
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
