using System.Collections;
using System.Collections.Generic;
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
    void Start()
    {
        SelectWeapon(inventory.selectedSlot);
        inventory.OnSwitch.AddListener(SelectWeapon);
        useKey.action.performed +=  ctx => UseWeapon(ctx);
        slashHitBox.SetActive(false);
    }

    void SelectWeapon(int weapon)
    {
        handSprite.sprite = weaponSprites[weapon];
    }

    void UseWeapon(InputAction.CallbackContext ctx)
    {
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

}
