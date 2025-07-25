using System;
using System.Collections;
using UnityEngine;

public class HurtCheck : MonoBehaviour
{
    public SpriteRenderer sprite;
    public float cooldown = 1.0f;
    private bool onCoolDown;
    private void OnTriggerEnter2D(Collider2D col)
    {
        CheckHurtTrigger(col);
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        CheckHurtTrigger(col);
    }

    private void CheckHurtTrigger(Collider2D col)
    {
        if (!onCoolDown && col.CompareTag("WeaponHurt"))
        {
            onCoolDown = true;
            sprite.color = Color.red;
            StartCoroutine(ResetHurt()); 
        }
    }

    IEnumerator ResetHurt()
    {
        yield return new WaitForSeconds(cooldown);
        onCoolDown = false;
        sprite.color = Color.white;
    }
}
