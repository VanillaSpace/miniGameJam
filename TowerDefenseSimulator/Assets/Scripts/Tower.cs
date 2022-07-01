using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public enum TowerTargetPrio
    {
        First,
        Close,
        Strong
    }

    [Header("Info")]
    public float range;
    private List<Enemy> curEnemiesInRange = new List<Enemy>();
    private Enemy curEnemy;

    public TowerTargetPrio targetPriority;
    public bool rotateTowardsTarget;

    [Header("Attacking")]
    public float attackRate;
    private float lastAttackTime;
    public GameObject projectilePrefab;
    public Transform projectileSpawnPos;

    public int projectileDamage;
    public float projectileSpeed;

    void Update ()
    {
        AttackRateSeconds();
    }

    private void AttackRateSeconds()
    {
        if (Time.time - lastAttackTime > attackRate)
        {
            lastAttackTime = Time.time;
            curEnemy = GetEnemy();

            if (curEnemy != null)
            {
                Attack();
            }
        }
    }

    Enemy GetEnemy()
    {
        curEnemiesInRange.RemoveAll(x => x == null);

        if (curEnemiesInRange.Count == 0)
        {
            return null;
        }
        if (curEnemiesInRange.Count == 1)
        {
            return curEnemiesInRange[0];
        }

        switch(targetPriority)
        {
            case TowerTargetPrio.First:
                {
                    return curEnemiesInRange[0];
                }
            case TowerTargetPrio.Close:
                {
                    Enemy closest = null;
                    float dist = 99;

                    for (int x = 0; x < curEnemiesInRange.Count; x++)
                    {
                        float d = (transform.position - curEnemiesInRange[x].transform.position).sqrMagnitude;
                        if (d < dist)
                        {
                            closest = curEnemiesInRange[x];
                            dist = d;
                        }
                    }

                    return closest;
                }

            case TowerTargetPrio.Strong:
                {
                    Enemy strongest = null;
                    int strongestHealth = 0;
                    foreach (Enemy enemy in curEnemiesInRange)
                    {
                        if (enemy.health > strongestHealth)
                        {
                            strongest = enemy;
                            strongestHealth = enemy.health;
                        }
                    }
                    return strongest;
                }
        }

        return null;
    }

    private void Attack()
    {
        if (rotateTowardsTarget)
        {
            transform.LookAt(curEnemy.transform);
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
        }

        GameObject proj = Instantiate(projectilePrefab, projectileSpawnPos.position, Quaternion.identity);
        proj.GetComponent<Projectile>().Initialize(curEnemy, projectileDamage, projectileSpeed);
    }

    private void OnTriggerEnter (Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            curEnemiesInRange.Add(other.GetComponent<Enemy>());
        }
    }

    private void OnTriggerExit (Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            curEnemiesInRange.Remove(other.GetComponent<Enemy>());
        }
    }

}
