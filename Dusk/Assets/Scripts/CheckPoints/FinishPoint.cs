using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishPoint : MonoBehaviour
{
    private Animator anim => GetComponent<Animator>();

    private void Update()
    {
        ActivateFinishPoint();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();

        if (player != null && GameManager.instance.batteriesCollected == GameManager.instance.totalBatteries)
        {
            AudioManager.instance.PlaySFX(2);

            GameManager.instance.LevelFinished();
        }
        else
        {
            Debug.Log("Batteries not collected");
            UI_InGame.instance.ShowBatteriesNotCollectedMessage();
        }
    }

    private void ActivateFinishPoint() //Activates the finish point if all items are collected.
    {
        if (GameManager.instance.batteriesCollected == GameManager.instance.totalBatteries)
            anim.SetBool("isActive", true);
    }
}
