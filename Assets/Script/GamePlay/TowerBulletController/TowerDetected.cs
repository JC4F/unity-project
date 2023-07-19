using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerDetected : MonoBehaviour
{
    public GameObject objectTower;
    public PolygonCollider2D polygonCollider;
    public float radius = 3f;
    public int numPoints = 8;
    

    private bool isSoliderATurn = true;
    private bool SoliderAReady = true;
    private bool SoliderBReady = true; 
    private bool isFiring = false;

    public GameObject SoliderA;
    public GameObject SoliderB;
    public GameObject BarrageFirePower;

    private void OnValidate()
    {
        GenerateCircleCollider();
    }

    private void GenerateCircleCollider()
    {
        Vector2[] points = new Vector2[numPoints];

        float angleStep = 360f / numPoints;
        for (int i = 0; i < numPoints; i++)
        {
            float angle = i * angleStep;
            float x = radius * Mathf.Cos(Mathf.Deg2Rad * angle);
            float y = radius * Mathf.Sin(Mathf.Deg2Rad * angle);
            points[i] = new Vector2(x, y);
        }

        polygonCollider.points = points;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (objectTower.gameObject.tag == "ArrowSentines")
            {
                if (isSoliderATurn && SoliderAReady && !isFiring)
                {
                    StartCoroutine(ShootBullet(SoliderA, 0.5f, 0.5f));
                    isSoliderATurn = false; 
                }
                else if (!isSoliderATurn && SoliderBReady && !isFiring)
                {
                    StartCoroutine(ShootBullet(SoliderB, 0.5f, 0.5f));
                    isSoliderATurn = true; 
                }
            }
            else if (objectTower.gameObject.tag == "MageSentines")
            {
                if (!isFiring)
                {
                    StartCoroutine(ShootBullet(SoliderA, 1.5f, 0.75f));
                    isSoliderATurn = false;
                }
            }
            else if (objectTower.gameObject.tag == "ArtilleristSentines")
            {
                if (!isFiring)
                {
                    StartCoroutine(ArtilleristBullet(SoliderA, SoliderB, BarrageFirePower, 1f));
                    isSoliderATurn = false;
                }
            }
        }
    }

    private IEnumerator ShootBullet(GameObject solider, float delayBetweenShots, float shootDuration)
    {
        isFiring = true;

        Animator animator = solider.GetComponent<Animator>();

        animator.SetTrigger("Fire");

        yield return new WaitForSeconds(delayBetweenShots);

        animator.SetTrigger("Reset");
        yield return new WaitForSeconds(shootDuration);

        if (solider == SoliderA)
        {
            SoliderAReady = true;
        }
        else if (solider == SoliderB)
        {
            SoliderBReady = true;
        }

        isFiring = false;
    }

    private IEnumerator ArtilleristBullet(GameObject SoliderA, GameObject SoliderB, GameObject BarrageFire, float delayBetweenShots)
    {
        isFiring = true;
        Animator AniSoliderA = SoliderA.GetComponent<Animator>();
        Animator AniSoliderB = SoliderB.GetComponent<Animator>();
        Animator AniBarrageFire = BarrageFire.GetComponent<Animator>();

        AniSoliderA.SetTrigger("isShoot");
        yield return new WaitForSeconds(delayBetweenShots);
        AniBarrageFire.SetTrigger("BarrageFire");
        yield return new WaitForSeconds(delayBetweenShots);
        AniSoliderB.SetTrigger("Reload");
        yield return new WaitForSeconds(1f);

        isFiring = false;
    }
}
