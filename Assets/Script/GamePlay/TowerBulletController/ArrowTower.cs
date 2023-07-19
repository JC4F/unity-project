using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using Random = UnityEngine.Random;

public class ArrowTower : MonoBehaviour
{
    public PolygonCollider2D polygonCollider;
    public GameObject tower;
    public GameObject bulletPrefab;
    public float arcHeight;
    public float speed;
    private float finalArcHeight;
    private float finalSpeed;
    private bool isStore;

    public GameObject target;
    private float towerX;
    private float targetX;

    private float dist;
    private float nextX;
    private float baseY;
    private float height;

    private bool readyToShot = false;
    private GameObject firepower;
    private GameObject newBullet;
    private EnemyStatus enemyStatus;
    private int minDamage = 4;
    private int maxDamage = 6;
    private float positionTolerance = 0.1f;
    private bool receivedNotification = false;

    private void Start()
    {
        tower = gameObject;
    }

    public void Shoot()
    {
        firepower = GameObject.Find("Canvas/Manager/Firepower/ArrowProjectile");
        
        newBullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        readyToShot = true;
        if (!isStore)
        {
            finalArcHeight = arcHeight;
            finalSpeed = speed;
            isStore = !isStore;
        }
    }

    private void Update()
    {
        if (readyToShot == true)
        {
            if (target == null)
            {
                SwitchTarget();
                return;
            }
            if ((target != null && enemyStatus.health == 0) || (target != null && receivedNotification))
            {
                SwitchTarget();
            }
            if (target != null)
            {
                newBullet.transform.SetParent(firepower.transform);
                newBullet.transform.localScale = new Vector3(4f, 4f, 1f);
                towerX = transform.position.x;
                targetX = target.transform.position.x;

                dist = targetX - towerX;
                float distanceX = Mathf.Abs(targetX - towerX);
                CalculateSpeedAndArcHeight(distanceX);
                nextX = Mathf.MoveTowards(newBullet.transform.position.x, targetX, speed * Time.deltaTime);
                baseY = Mathf.Lerp(transform.position.y, target.transform.position.y, (nextX - towerX) / dist);
                height = arcHeight * (nextX - towerX) * (nextX - targetX) / (-0.25f * dist * dist);


                Vector3 movePos = new Vector3(nextX, baseY + height, newBullet.transform.position.z);
                newBullet.transform.rotation = LookAtTarget(movePos - newBullet.transform.position);
                newBullet.transform.position = movePos;
                Vector2 positionDifference = (Vector2)newBullet.transform.position - (Vector2)target.transform.position;
                if (positionDifference.magnitude <= positionTolerance)
                {
                    Destroy(newBullet);
                    readyToShot = false;
                    enemyStatus.TakeDamage(Random.Range(minDamage, maxDamage), "Physical", "Arrow", target, 0f);
                }
            }
        }
    }

    public static Quaternion LookAtTarget(Vector2 rotation)
    {
        return Quaternion.Euler(0, 0, Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg);
    }

    private void CalculateSpeedAndArcHeight(float distanceX)
    {
        float[][] thresholds = new float[][]
        {
            new float[] { 60f, finalSpeed, 0f },
            new float[] { 50f, finalSpeed - 20f, 0f },
            new float[] { 30f, finalSpeed - 30f, 0f },
            new float[] { 15f, finalSpeed - 40f, 0f },
            new float[] { 10f, finalSpeed - 55f, 0f },
            new float[] { 7f, finalSpeed - 65f, 0f },
            new float[] { 5f, finalSpeed - 75f, 0f },
            new float[] { 3f, finalSpeed - 85f, 0f },
            new float[] { 0f, finalSpeed - 95f, 0f }
        };

        foreach (float[] threshold in thresholds)
        {
            if (distanceX > threshold[0])
            {
                speed = threshold[1];
                arcHeight = threshold[2];
                return;
            }
        }
    }

    private GameObject FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject nearestEnemy = null;
        float nearestDistance = float.MaxValue;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestEnemy = enemy;
            }
        }

        return nearestEnemy;
    }

    private void OnTargetExited(GameObject exitedObject)
    {
        if (exitedObject == target)
        {
            target = null;
            receivedNotification = true;
        }
    }

    private void OnEnable()
    {
        PathScript.TargetExited += OnTargetExited;
    }

    private void OnDisable()
    {
        PathScript.TargetExited -= OnTargetExited;
    }

    private void SwitchTarget()
    {
        target = FindNearestEnemy();
        if (target != null)
        {
            enemyStatus = target.GetComponent<EnemyStatus>();
        }
        receivedNotification = false;
    }
}