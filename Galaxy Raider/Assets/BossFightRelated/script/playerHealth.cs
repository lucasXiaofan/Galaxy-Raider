using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerHealth : MonoBehaviour
{
    [SerializeField] float playerHitPoint = 100f;
    public GameObject GameOverUI;
    private void Start()
    {
        playerHitPoint = 100f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        GameOverUI.SetActive(false);
    }
    public void playerTakeDamge(float damage)
    {
        playerHitPoint -= damage;
        print(playerHitPoint);
        if (playerHitPoint <= 0)
        {
            afterDeath();
        }
    }

    private void afterDeath()
    {
        GameOverUI.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
