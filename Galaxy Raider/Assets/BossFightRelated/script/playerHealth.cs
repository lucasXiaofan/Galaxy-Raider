using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerHealth : MonoBehaviour
{
    [SerializeField] float playerHitPoint = 100f;
    [SerializeField] float totalHitPoint = 100f;
    public GameObject GameOverUI;
    public HealthBar healthBarObject;
    private void Start()
    {

        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
        print(playerHitPoint);
        Cursor.visible = false;
        GameOverUI.SetActive(false);
    }
    public void playerTakeDamge(float damage)
    {
        playerHitPoint -= damage;
        healthBarObject.UpdateHealth((float)playerHitPoint / (float)totalHitPoint);
        // print(playerHitPoint);
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
