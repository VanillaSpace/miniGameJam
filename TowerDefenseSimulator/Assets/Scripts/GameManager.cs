using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public int health;
    public int money;
    private bool gameActive;

    [Header("Components")]
    public TextMeshProUGUI healthAndMoneyText;
    public EnemyPath enemyPath;
    public TowerPlacement towerPlacement;
    public EndScreenUI endScreen;
    public WaveSpawner waveSpawner;

    [Header("Events")]
    public UnityEvent onEnemyDestoryed;
    public UnityEvent onMoneyChanged;
    
    // Singleton
    public static GameManager instance;

    void Awake()
    {
        instance = this;
    }

    void Start ()
    {
        gameActive = true;
        Time.timeScale = 1;
        UpdateHealthAndMoneyText();
    }

    public bool GetActivateGame()
    {
        return gameActive;
    }

    void UpdateHealthAndMoneyText()
    {
        if(healthAndMoneyText != null)
        {
            healthAndMoneyText.text = $"HP: {health}\nGOLD: ${money}";
        }
    }

    public void AddMoney(int amount)
    {
        money += amount;
        UpdateHealthAndMoneyText();
        onMoneyChanged.Invoke();
    }
    public void TakeMoney(int amount)
    {
        money -= amount;
        UpdateHealthAndMoneyText();
        onMoneyChanged.Invoke();
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        UpdateHealthAndMoneyText();
        if (health <= 0)
        {
            GameOver();
        }
    }
    void GameOver()
    {
        gameActive = false;
        Time.timeScale = 0;
        endScreen.gameObject.SetActive(true);
        endScreen.SetEndScreen(false, waveSpawner.curWave);
        
    }
    public void WinGame()
    {
        gameActive = false;
        endScreen.gameObject.SetActive(true);
        endScreen.SetEndScreen(true, waveSpawner.curWave);
    }

    
    //public void OnEnemyDestroyed()
    //{
    //    if (!gameActive) { return; }

    //    if (waveSpawner.remainingEnemies == 0 && waveSpawner.curWave == waveSpawner.waves.Length)
    //    {
    //        WinGame();
    //    }
    //}
}
