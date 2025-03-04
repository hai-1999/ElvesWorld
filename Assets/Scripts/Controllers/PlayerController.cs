using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float moveSpeed = 3;
    private bool isMoving;

    private Vector2 input;

    private Animator animator;

    public LayerMask buildingLayer;
    public LayerMask grassLayer;

    private void Awake()
    {
        animator = GetComponent<Animator>();//从当前游戏对象获取动画控制器组件
    }
     
    private void Update()//每帧调用一次
    {
        if (!isMoving)
        {
            input.x = Input.GetAxisRaw("Horizontal");// 获取X轴的输入值
            input.y = Input.GetAxisRaw("Vertical");// 获取Y轴的输入值

            if (input.x != 0) input.y = 0;//禁止斜向移动

            if (input != Vector2.zero)//如果X轴和Y轴输入不为零
            {
                animator.SetFloat("moveX", input.x);
                animator.SetFloat("moveY", input.y);

                var targetPos =this.transform.position;//获得目标位置坐标
                targetPos.x += input.x;//计算移动后的X轴坐标
                targetPos.y += input.y;//计算移动后的Y轴坐标

                if (IsWalkable(targetPos))
                {
                    StartCoroutine(Move(targetPos));//开启协程
                }
            }
        }
        animator.SetBool("isMoving", isMoving);
    }

 /*协程是一种能在一定条件下暂停执行，并在稍后恢复执行的函数。它允许将一个任务拆分为多个小任务，每一小段任务可以在多个帧内完成，而不是在一帧内阻塞主线程。在Unity中，协程通过StartCoroutine方法启动，并可以通过yield关键字来暂停执行，直到满足特定条件（例如等待时间、等帧、等待另一个协程完成等）后继续运行。*/
    IEnumerator Move(Vector3 targetPos)
    {
        isMoving = true;
        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
           
            yield return null;//等待一帧         
        }
        //transform.position = targetPos;
        isMoving = false;

        CheckForEncounter();
    }

    //遇到建筑物禁止移动函数
    private bool IsWalkable(Vector3 targetPos)
    {
        if (Physics2D.OverlapCircle(targetPos, 0.2f, buildingLayer) != null)//目标位置是否在建筑图层里
        {
            return false;
        }
        return true;
    }

    //草丛遇敌函数
    private void CheckForEncounter()
    {
        if (Physics2D.OverlapCircle(transform.position, 0.2f, grassLayer) != null)
        {
            if (Random.Range(1, 100) <= 10)
            {
                Debug.Log("遇敌 ！！");
            }
        }
    }
}
