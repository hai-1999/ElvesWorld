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
        animator = GetComponent<Animator>();//�ӵ�ǰ��Ϸ�����ȡ�������������
    }
     
    private void Update()//ÿ֡����һ��
    {
        if (!isMoving)
        {
            input.x = Input.GetAxisRaw("Horizontal");// ��ȡX�������ֵ
            input.y = Input.GetAxisRaw("Vertical");// ��ȡY�������ֵ

            if (input.x != 0) input.y = 0;//��ֹб���ƶ�

            if (input != Vector2.zero)//���X���Y�����벻Ϊ��
            {
                animator.SetFloat("moveX", input.x);
                animator.SetFloat("moveY", input.y);

                var targetPos =this.transform.position;//���Ŀ��λ������
                targetPos.x += input.x;//�����ƶ����X������
                targetPos.y += input.y;//�����ƶ����Y������

                if (IsWalkable(targetPos))
                {
                    StartCoroutine(Move(targetPos));//����Э��
                }
            }
        }
        animator.SetBool("isMoving", isMoving);
    }

 /*Э����һ������һ����������ִͣ�У������Ժ�ָ�ִ�еĺ�����������һ��������Ϊ���С����ÿһС����������ڶ��֡����ɣ���������һ֡���������̡߳���Unity�У�Э��ͨ��StartCoroutine����������������ͨ��yield�ؼ�������ִͣ�У�ֱ�������ض�����������ȴ�ʱ�䡢��֡���ȴ���һ��Э����ɵȣ���������С�*/
    IEnumerator Move(Vector3 targetPos)
    {
        isMoving = true;
        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
           
            yield return null;//�ȴ�һ֡         
        }
        //transform.position = targetPos;
        isMoving = false;

        CheckForEncounter();
    }

    //�����������ֹ�ƶ�����
    private bool IsWalkable(Vector3 targetPos)
    {
        if (Physics2D.OverlapCircle(targetPos, 0.2f, buildingLayer) != null)//Ŀ��λ���Ƿ��ڽ���ͼ����
        {
            return false;
        }
        return true;
    }

    //�ݴ����к���
    private void CheckForEncounter()
    {
        if (Physics2D.OverlapCircle(transform.position, 0.2f, grassLayer) != null)
        {
            if (Random.Range(1, 100) <= 10)
            {
                Debug.Log("���� ����");
            }
        }
    }
}
