using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacStudent : MonoBehaviour
{
    public int x;
    public int y;
    public float moveSpeed = 1;
    public int targetX;
    public int targetY;

    private Vector3 targetPosition;
   
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        transform.position = LevelMapGenerator.Instance.GetPosition(x, y);
        targetX = x + 1;
        targetY = y;
        targetPosition = LevelMapGenerator.Instance.GetPosition(targetX, targetY);
    }

    private void Update()
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        Vector3 movement = direction * Time.deltaTime * moveSpeed;
        animator.SetInteger("X", targetX - x);
        animator.SetInteger("Y", y - targetY);
        if (Vector3.Distance(transform.position, targetPosition) <= movement.magnitude)
        {
            transform.position = targetPosition;
            if (x != targetX) //横向移动
            {
                int dir = targetX - x;
                x = targetX;
                y = targetY;
                if (dir > 0) //向右
                {
                    if (LevelMapGenerator.Instance.CanMove(targetX, targetY + 1))
                        targetY++;
                    else
                        targetX++;
                }
                else //向左
                {
                    if (LevelMapGenerator.Instance.CanMove(targetX, targetY - 1))
                        targetY--;
                    else
                        targetX--;
                }
            }
            else if (y != targetY) //纵向移动
            {
                int dir = targetY - y;
                x = targetX;
                y = targetY;
                if (dir > 0) //向下
                {
                    if (LevelMapGenerator.Instance.CanMove(targetX - 1, targetY))
                        targetX--;
                    else
                        targetY++;
                }
                else //向上
                {
                    if (LevelMapGenerator.Instance.CanMove(targetX + 1, targetY))
                        targetX++;
                    else
                        targetY--;
                }
            }
           
            targetPosition = LevelMapGenerator.Instance.GetPosition(targetX, targetY);
        }
        else
        {
            transform.Translate(movement, Space.World);
        }
    }
}
