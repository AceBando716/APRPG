using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class InventoryManager : MonoBehaviour
{
    public GameObject Item;

    public WeaponData[] weapons;
    
    public Image[] weaponSlots;

    private int equippedWeaponIndex = 0;

    void Start()
    {
        for (int i = 0; i < Mathf.Min(weapons.Length, weaponSlots.Length); i++)
        {
            weaponSlots[i].sprite = weapons[i].weaponIcon;
        }
    }

    public void SwitchWeapon(int slotIndex)
    {
        equippedWeaponIndex = slotIndex;
        Debug.Log("Weapon switched:" + weapons[slotIndex].weaponName);
    }

}
[System.Serializable]
public class WeaponData
{
    public string weaponName;
    public Sprite weaponIcon;
    public GameObject weaponPrefab;

    public int damage;

    public float attackspeed;

    public float range;

    public float knockback;

    public float strength;
    
}