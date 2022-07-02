using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public int health;
    public int damageToPlayer;
    public int moneyOnDeath;
    public float moveSpeed;

    // pathing
    private Transform[] path;
    private int curPathWaypoint;
    public GameObject healthBarPrefab;

    public static event UnityAction OnDestroyed;

    void Start()
    {
        path = GameManager.instance.enemyPath.waypoints;

        CreateHealthBar();
    }

    void Update()
    {
        MoveAlongPath();
    }

    void CreateHealthBar()
    {
        Canvas canvas = FindObjectOfType<Canvas>();
        GameObject healthBar = Instantiate(healthBarPrefab, canvas.transform);
        healthBar.GetComponent<EnemyHealthBar>().Initialize(this);
    }

    // called every frame to move the enemy towards the end of the path
    void MoveAlongPath()
    {
        //the step size is equal to speed times frame rate
        float StepSize = moveSpeed * Time.deltaTime;

        if (curPathWaypoint < path.Length)
        {
            //To stop it from rotating backwards from the start
            if (curPathWaypoint > 1)
            {
                Vector3 newDirection = Vector3.RotateTowards(transform.forward, path[curPathWaypoint].position, StepSize, 0.0f);
                transform.rotation = Quaternion.LookRotation(newDirection);
            }

            transform.position = Vector3.MoveTowards(transform.position, path[curPathWaypoint].position, StepSize);

            if (transform.position == path[curPathWaypoint].position)
            {
                curPathWaypoint++;
            }
        }
        // if we're at the end of the path
        else
        {
            GameManager.instance.TakeDamage(damageToPlayer);
            OnDestroyed.Invoke();
            Destroy(gameObject);
        }
    }

    // called when a tower deals damage to the enemy
    public void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            GameManager.instance.AddMoney(moneyOnDeath);
            OnDestroyed.Invoke();
            Destroy(gameObject);
        }
    }
}
