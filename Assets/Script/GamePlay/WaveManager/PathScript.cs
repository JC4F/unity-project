using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathScript : MonoBehaviour
{
    public int targetNumber = 0;
    private Transform target;
    private Animator animator;
    public Transform[] checkpoints;
    public Transform exitPoint;
    public float navTimeUpdate;
    public float currentNavTime;
    public float moveSpeed;
    public static event Action<GameObject> TargetExited;
    public delegate void TargetExitedHandler(GameObject exitedObject);
    private GameSystem gameSystem;
    private EnemyStatus enemyStatus;
    private bool canIncreaseTargetNumber = true;
    private bool alreadyReachToEndPoint = false;

    private void Start()
    {
        target = GetComponent<Transform>();
        animator = GetComponent<Animator>();
        gameSystem = GameObject.FindGameObjectWithTag("GameSystem").GetComponent<GameSystem>();
        enemyStatus = GetComponent<EnemyStatus>();
    }

    private void Update()
    {
        if (checkpoints != null)
        {
            currentNavTime += Time.deltaTime * moveSpeed;
            
            if (currentNavTime > navTimeUpdate)
            {
                if (targetNumber < checkpoints.Length)
                {
                    target.localPosition = Vector2.MoveTowards(target.localPosition, checkpoints[targetNumber].localPosition, currentNavTime);
                   
                }
                else
                {
                    target.localPosition = Vector2.MoveTowards(target.localPosition, exitPoint.localPosition, currentNavTime);
                }
                currentNavTime = 0;

                if (Vector2.Distance(target.localPosition, exitPoint.localPosition) < 0.1f && !alreadyReachToEndPoint)
                {
                    alreadyReachToEndPoint = true;
                    EnemyReachedDestination();
                }
            }
        }
    }

    private void EnemyReachedDestination()
    {
        gameSystem.LoseLife(enemyStatus.livesTaken);
        Destroy(gameObject, 0.5f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("checkpoint"))
        {
            if (canIncreaseTargetNumber) 
            {
                checkPointPaths(other.gameObject);
                canIncreaseTargetNumber = false; 
            }
        }
        else if (other.CompareTag("checkpoint02"))
        {
            if (canIncreaseTargetNumber)
            {
                checkPointPaths(other.gameObject);
                canIncreaseTargetNumber = false;
            }
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("checkpoint") || other.CompareTag("checkpoint02"))
        {
            canIncreaseTargetNumber = true;
        }
        if (other.CompareTag("RangeTower"))
        {
            TargetExited?.Invoke(gameObject);
        }
    }

    private void checkPointPaths(GameObject checkpoint)
    {
        if (checkpoint != null)
        {
            if (checkpoint.name.Contains("UpPoint"))
            {
                animator.SetBool("isUpper", true);
                animator.SetBool("isDown", false);
            }
            else if (checkpoint.name.Contains("DownPoint"))
            {
                animator.SetBool("isDown", true);
                animator.SetBool("isUpper", false);
            }
            else
            {
                animator.SetBool("isDown", false);
                animator.SetBool("isUpper", false);
            }
            targetNumber += 1;
        }
    }
}