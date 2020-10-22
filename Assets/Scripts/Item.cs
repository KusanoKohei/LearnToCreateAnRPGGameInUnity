using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [Header("Item Type")]
    public bool isItem;
    public bool isWeapon;
    public bool isArmor;

    [Header("Item Details")]
    public string itemName;
    public string description;
    public int value;
    public Sprite itemSprite;

    [Header("Item Details")]
    public int amountToChange;
    public bool affectHP, affectMP, affectStr;

    [Header("Weapon/Armor Details")]
    public int weaponStrength;
    public int armorStrength;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void Use(int charToUseOn)
    {
        CharStats selectedChar = GameManager.instance.playerStats[charToUseOn];

        if (isItem)
        {
            if (affectHP)
            {
                Debug.Log(selectedChar.currentHP + " is target preHP");

                selectedChar.currentHP += amountToChange;
                
                if(selectedChar.currentHP > selectedChar.maxHP)
                {
                    selectedChar.currentHP = selectedChar.maxHP;
                }
            }

            if (affectMP)
            {
                Debug.Log("It's not for affectHP ! for affect MP");
                selectedChar.currentMP += amountToChange;

                if (selectedChar.currentMP > selectedChar.maxMP)
                {
                    selectedChar.currentMP = selectedChar.maxMP;
                }
            }

            if (affectStr)
            {
                selectedChar.strength += amountToChange;
            }
        }

        if (isWeapon)
        {
            if(selectedChar.equippedWpn != "")
            {
                GameManager.instance.AddItem(selectedChar.equippedWpn);
            }

            selectedChar.equippedWpn = itemName;
            selectedChar.wpnPwr = weaponStrength;
        }

        if (isArmor)
        {
            if (selectedChar.equippedArmr != "")
            {
                GameManager.instance.AddItem(selectedChar.equippedArmr);
            }

            Debug.Log(itemName);
            selectedChar.equippedArmr = itemName;
            selectedChar.armrPwr = armorStrength;
        }

        GameManager.instance.RemoveItem(itemName);
    }

    public void UseInBattle(int target)
    {
        BattleChar selectedBattler = BattleManager.instance.activeBattlers[target];

        if (isItem)
        {
            if (affectHP)
            {
                Debug.Log("通った");
                // BattleManager.instance.activeBattlers[target].currentHP += amountToChange;
                selectedBattler.currentHP += amountToChange;
                Debug.Log("amountToChange is " + amountToChange);

                if (selectedBattler.currentHP > selectedBattler.maxHP)
                {
                    selectedBattler.currentHP = selectedBattler.maxHP;
                }

                Debug.Log(selectedBattler.name + "'s currentHP is " + selectedBattler.currentHP);
            }

            if (affectMP)
            {
                Debug.Log("It's not for affectHP ! for affect MP");
                selectedBattler.currentMP += amountToChange;

                if (selectedBattler.currentMP > selectedBattler.maxMP)
                {
                    selectedBattler.currentMP = selectedBattler.maxMP;
                }
            }

            if (affectStr)
            {
                selectedBattler.strength += amountToChange;
            }
        }

        GameManager.instance.RemoveItem(itemName);
    }
}
