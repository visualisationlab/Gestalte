using System.Collections;
using System.Collections.Generic;
using Director;
using Mediator;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public List<Sprite> weaponSprites;
    public SpriteRenderer handSprite;
    public Inventory inventory;
    public InputActionReference useKey;
    public GameObject slashHitBox;

    public float slashCooldown = 0.2f;
    private bool coolDown;
    public int health = 3;
    public ExternalProbe healthProbe;
    public HealthIndicator HealthIndicator;
    
    void Start()
    {
        SelectWeapon(inventory.selectedSlot);
        inventory.OnSwitch.AddListener(SelectWeapon);
        useKey.action.performed +=  ctx => UseWeapon(ctx);
        slashHitBox.SetActive(false);
        healthProbe.value = health;
    }

    void SelectWeapon(int weapon)
    {
        handSprite.sprite = weaponSprites[weapon];
    }

    void UseWeapon(InputAction.CallbackContext ctx)
    {
        Debug.Log("Use Weapon!!!");
        if (inventory.selectedSlot == 0)
        {
            Slash();
        }
    }

    void Slash()
    {
        if (coolDown) return;
        slashHitBox.SetActive(true);
        coolDown = true;
        StartCoroutine(DisableSlash());
    }
    
    void EndSlash()
    {
        slashHitBox.SetActive(false);
        coolDown = false;
    }

    IEnumerator DisableSlash()
    {
        yield return new WaitForSeconds(slashCooldown);
        EndSlash();
    }
    
    [ContextMenu("Reduce Health")]
    public void ReduceHealth()
    {
        health--;
        healthProbe.value = health;
        HealthIndicator.SetHealth(health);
    }
    
    [ExposeMethod("Heals the player by 1 health point.")]
    public void Heal()
    {
        health++;
        healthProbe.value = health;
        HealthIndicator.SetHealth(health);
        Debug.Log("Heal! Called");
    }
    
}
