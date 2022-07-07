using UnityEngine;

public class EnemyCrawling : MonoBehaviour
{
    private Rigidbody2D enemyRB;

    private Vector2 dir;
    private Vector2 modifiedPos;
    private float angle;

    private bool setAngleOfRotation;
    private bool isGrounded;

    [Header("Settings")]
    [SerializeField] private Transform rayStartPoint;
    [SerializeField] private float rayLength;
    [SerializeField] private float rayXOffset;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float crawlingSpeed;
    [SerializeField] private float positionOffset;

    private void Start()
    {
        enemyRB = GetComponent<Rigidbody2D>();
        dir = Vector2.right;
        modifiedPos = Vector2.right * positionOffset;
        angle = 0.0f;
    }

    void FixedUpdate()
    {
        enemyRB.MovePosition(enemyRB.position + dir * (1 - Mathf.Exp(-crawlingSpeed / 2.0f * Time.fixedDeltaTime)));

        isGrounded = Physics2D.Raycast(rayStartPoint.position + transform.right * rayXOffset, -rayStartPoint.up, rayLength, groundMask);

        Debug.DrawRay(rayStartPoint.position + transform.right * rayXOffset, -rayStartPoint.up * rayLength, Color.green);

        enemyRB.MoveRotation(angle);

        if (!isGrounded && !setAngleOfRotation)
        {
            if (angle == -360.0f)
                angle = -90.0f;
            else
                angle -= 90.0f;

            DirectionAndPositionOffset(angle);
            enemyRB.MovePosition(enemyRB.position + modifiedPos);
            setAngleOfRotation = true;
        }
        if ((angle <= 0.0f) && isGrounded)
        {
            setAngleOfRotation = false;
        }
    }

    private void DirectionAndPositionOffset(float angle)
    {
        switch (angle)
        {
            case -90.0f:
                dir = Vector2.down;
                modifiedPos = Vector2.right * positionOffset;
                break;
            case -180.0f:
                dir = Vector2.left;
                modifiedPos = Vector2.up * -positionOffset;
                break;
            case -270.0f:
                dir = Vector2.up;
                modifiedPos = Vector2.right * -positionOffset;
                break;
            case -360.0f:
                dir = Vector2.right;
                modifiedPos = Vector2.up * positionOffset;
                break;
        }
    }
}
