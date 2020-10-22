using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ItemMenuInBattle : MonoBehaviour
{
    [SerializeField]
    private GameObject itemMenu;
    [SerializeField]
    private GameObject itemWindow;
    [SerializeField]
    private GameObject[] functions;

    public bool itemMenuOpened;

    [SerializeField]
    private ItemButton[] itemButtons;
    [SerializeField]
    private Item activeItem;
    [SerializeField]
    private Text noticeForCancel, itemName, itemDescription, useButtonText;
    [SerializeField]
    private Text[] itemCharChoiceNames;

    private enum MODE
    {
        CHOOSE_ITEM,
        CHOOSE_ACTION,
        CHOOSE_TARGET
    }

    MODE nowMode = MODE.CHOOSE_ITEM;

    public static ItemMenuInBattle instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    public void ToggleItemWindow()
    {
        // 開いていたら閉じて、閉じていたら開く.
        itemWindow.SetActive(!itemWindow.activeInHierarchy);
        itemMenuOpened = true;
        
        nowMode = MODE.CHOOSE_ITEM;
        ToggleFunctions();
    }

    public void ToggleFunctions()
    {
        switch (nowMode)
        {
            case MODE.CHOOSE_ITEM:
                functions[0].SetActive(true);
                functions[1].SetActive(false);
                functions[2].SetActive(false);

                noticeForCancel.gameObject.SetActive(true);
                itemName.gameObject.SetActive(false);
                itemDescription.gameObject.SetActive(false);
                break;

            case MODE.CHOOSE_ACTION:
                functions[0].SetActive(true);
                functions[1].SetActive(true);
                functions[2].SetActive(false);
                
                noticeForCancel.gameObject.SetActive(false);
                itemName.gameObject.SetActive(true);
                itemDescription.gameObject.SetActive(true);
                break;

            case MODE.CHOOSE_TARGET:
                functions[0].SetActive(false);
                functions[1].SetActive(false);
                functions[2].SetActive(true);

                noticeForCancel.gameObject.SetActive(false);
                itemName.gameObject.SetActive(false);
                itemDescription.gameObject.SetActive(false);
                break;

            default:
                functions[0].SetActive(true);
                functions[1].SetActive(false);
                functions[2].SetActive(false);

                noticeForCancel.gameObject.SetActive(true);
                itemName.gameObject.SetActive(false);
                itemDescription.gameObject.SetActive(false);
                break;
        }

    }

    public void ShowItems()
    {
        GameManager.instance.SortItems();
        /*-- 装備品を省く処理を --*/

        for(int i=0; i<itemButtons.Length; i++)
        {
            itemButtons[i].buttonValue = i;

            if(GameManager.instance.itemsHeld[i] != "")
            {
                itemButtons[i].buttonImage.gameObject.SetActive(true);
                itemButtons[i].buttonImage.sprite = GameManager.instance.GetItemDetails(GameManager.instance.itemsHeld[i]).itemSprite;
                itemButtons[i].amountText.text = GameManager.instance.numberOfItems[i].ToString();
            }
            else
            {
                itemButtons[i].buttonImage.gameObject.SetActive(false);
                itemButtons[i].amountText.text = "";
            }
        }
    }

    public void SelectItem(Item newItem)
    {
        activeItem = newItem;

        if (activeItem.isItem)
        {
            useButtonText.text = "つかう";
            itemName.text = activeItem.itemName;
            itemDescription.text = activeItem.description;

            nowMode = MODE.CHOOSE_ACTION;
            ToggleFunctions();
        }

        if(activeItem.isWeapon || activeItem.isArmor)
        {
            BattleManager.instance.battleNotice.theText.text = "装備品はアイテムで使うことはできません";
            BattleManager.instance.battleNotice.Activate();
        }
    }

    public void OpenItemCharChoice()
    {
        for(int i=0; i<itemCharChoiceNames.Length; i++)
        {
            itemCharChoiceNames[i].text = GameManager.instance.playerStats[i].charName; // 選択対象にキャラクターの名前を表示.
            itemCharChoiceNames[i].transform.parent.gameObject.SetActive(GameManager.instance.playerStats[i].gameObject.activeInHierarchy);
        }

        nowMode = MODE.CHOOSE_TARGET;
        ToggleFunctions();
    }

    public void UseItem(int target)
    {
        CloseMenu();                            // アイテムメニューを閉じる.
        StartCoroutine(BattleManager.instance.UseItemMove(activeItem, target));   // アイテムを使うコルーチン.
    }

    public void CloseMenu()
    {
        itemMenuOpened = false;
        itemWindow.SetActive(false);
        // BattleManager.instance.uiButtonsHolder.SetActive(!BattleManager.instance.uiButtonsHolder.activeInHierarchy);
    }

    public void PlayButtonSound()
    {
        AudioManager.instance.PlaySFX(4);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            Debug.Log("B button Pushed");

            switch (nowMode)
            {
                case MODE.CHOOSE_ITEM:
                    CloseMenu();
                    break;

                case MODE.CHOOSE_ACTION:
                    CloseMenu();
                    break;

                case MODE.CHOOSE_TARGET:
                    nowMode = MODE.CHOOSE_ACTION;
                    ToggleFunctions();
                    break;

                default:
                    break;
            }
        }
    }
}
