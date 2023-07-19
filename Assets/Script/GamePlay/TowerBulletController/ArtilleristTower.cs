using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using Random = UnityEngine.Random;

public class ArtilleristTower : MonoBehaviour
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
    private static float rotationAngle = 0f;
    private int minDamage = 15;
    private int maxDamage = 27;
    private float positionTolerance = 0.1f;
    private float aoeRadius = 15f;
    private bool receivedNotification = false;
    private bool bulletReachTarget = false;

    private void Start()
    {
        tower = gameObject;
    }

    public void Shoot()
    {
        firepower = GameObject.Find("Canvas/Manager/Firepower/ArtiProjectile");
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
            if (target != null && enemyStatus.health == 0 || target != null && receivedNotification)
            {
                if (bulletReachTarget)
                {
                SwitchTarget();
                }
            }
            if (target != null)
            {
                newBullet.transform.SetParent(firepower.transform);
                newBullet.transform.localScale = new Vector3(8f, 8f, 1f);
                towerX = transform.position.x;
                targetX = target.transform.position.x;

                dist = targetX - towerX;
                float distanceX = Mathf.Abs(targetX - towerX);
                CalculateSpeedAndArcHeight(distanceX);
                nextX = Mathf.MoveTowards(newBullet.transform.position.x, targetX, speed * Time.deltaTime);
                baseY = Mathf.Lerp(transform.position.y, target.transform.position.y, (nextX - towerX) / dist);
                height = arcHeight * (nextX - towerX) * (nextX - targetX) / (-0.25f * dist * dist);



                Vector3 movePos = new Vector3(nextX, baseY + height, 0f);
                newBullet.transform.rotation = RotateContinuously();
                newBullet.transform.position = movePos;
                newBullet.transform.localPosition = movePos;
                Vector2 positionDifference = (Vector2)newBullet.transform.position - (Vector2)target.transform.position;
                if (positionDifference.magnitude <= positionTolerance)
                {
                    ApplyAOE(newBullet.transform.position, aoeRadius, target);
                    Destroy(newBullet);
                    readyToShot = false;
                    bulletReachTarget = true;
                }
            }
        }
    }

    public static Quaternion RotateContinuously()
    {
        rotationAngle += -300f * Time.deltaTime;
        return Quaternion.Euler(0, 0, rotationAngle);
    }

    private void CalculateSpeedAndArcHeight(float distanceX)
    {
        float[][] thresholds = new float[][]
        {
            new float[] { 60f, finalSpeed, finalArcHeight },
            new float[] { 50f, finalSpeed - 20f, finalArcHeight }, //ArcHeight = 60
            new float[] { 30f, finalSpeed - 30f, finalArcHeight }, //FinalSpeed = 80
            new float[] { 15f, finalSpeed - 40f, finalArcHeight },
            new float[] { 10f, finalSpeed - 60f, finalArcHeight },
            new float[] { 7f, finalSpeed - 60f, finalArcHeight },
            new float[] { 5f, finalSpeed - 65f, finalArcHeight },
            new float[] { 3f, finalSpeed - 65f, finalArcHeight },
            new float[] { 0f, finalSpeed - 70f, finalArcHeight }
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

    private void OnEnable()
    {
        PathScript.TargetExited += OnTargetExited;
    }

    private void OnDisable()
    {
        PathScript.TargetExited -= OnTargetExited;
    }


    private void OnTargetExited(GameObject exitedObject)
    {
        if (exitedObject == target)
        {
            target = null;
            receivedNotification = true;
        }
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

    private void ApplyAOE(Vector2 center, float radius, GameObject target)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(center, radius);

        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                EnemyStatus enemy = collider.GetComponent<EnemyStatus>();
                if (enemy != null)
                {
                    float damageMultiplier = target == collider.gameObject ? 1f : Random.Range(0.5f,0.75f);
                    int damage = (int)(Random.Range(minDamage, maxDamage) * damageMultiplier);
                    enemy.TakeDamage(damage, "Physical", "Artillerist", target, damageMultiplier);
                }
            }
        }
    }
}