//Script written by Giovanni Cartella - giovanni.cartella@hotmail.com || www.giovannicartella.weebly.com
//You are allowed to use this only if you have "Horror Development Kit" license, so only if you bought it officially

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;
using UnityStandardAssets.ImageEffects;

public class HDK_InventoryManager : MonoBehaviour {

    [Header("Loot")]
    public GameObject[] inventorySlots;
    public int freeSlots;
    int totalSlots;
    public GameObject meleeSlot;
    public GameObject firegunSlot;
    public GameObject keySlot;

    [Header("Selected Loot Slot")]
    public GameObject selectedSlot;
    public ItemType selectedSlotType;
    string slotType;
    bool emptySlot;

    [Header("UI")]
    public GameObject Inventory;
    public GameObject[] ToDisableGUI;
    public Text itemName;
    public Text itemInfo;
    public Text itemType;
    public Text ammo_health_Value;
    public Text errorLine;
    public Text slotText;

    public static bool inventoryOpen;
    GameObject Player;

    [Header("SFX")]
    public AudioClip openCloseSound;
    public AudioClip itemDestorySound;
    public AudioClip mouseHover;
    public AudioClip mouseClick;
    public float mouseVolume;
    AudioSource sourceAudio;

    public void EquipItem ()
    {
        if (selectedSlot == meleeSlot || selectedSlot == firegunSlot || selectedSlot == keySlot)
        {
            if (!selectedSlot.GetComponent<HDK_InventorySlot>().Empty)
            {
                StartCoroutine(ErrorText("ITEM ALREADY EQUIPPED"));
            }
            else
            {
                StartCoroutine(ErrorText("EMPTY SLOT"));
            }
        }
        else if (selectedSlot != null && selectedSlot != meleeSlot && selectedSlot != firegunSlot && selectedSlot != keySlot)
        {
            if (selectedSlot.GetComponent<HDK_InventorySlot>().itemType == ItemType.NONE)
            {
                StartCoroutine(ErrorText("EMPTY SLOT"));
            }
            if (selectedSlot.GetComponent<HDK_InventorySlot>().itemType == ItemType.Firegun)
            {
                HDK_InventorySlot selSlot = selectedSlot.GetComponent<HDK_InventorySlot>();
                Sprite itemIcon = selSlot.GetComponent<Image>().sprite;
                GameObject weaponTarget = selSlot.GetComponent<HDK_InventorySlot>().WeaponTarget;
                if (firegunSlot.GetComponent<HDK_InventorySlot>().Empty)
                {
                    firegunSlot.GetComponent<HDK_InventorySlot>().UpdateSlot(selSlot.itemName, selSlot.itemInfo, itemIcon, ItemType.Firegun, 0, 0, null, weaponTarget);
                    selSlot.ResetSlot();
                    freeSlots += 1;
                    FindObjectOfType<HDK_WeaponsManager>().currentGun = weaponTarget;
                }
                else
                {
                    StartCoroutine(ErrorText("YOU ARE ALREADY USING A FIREGUN"));
                }
            }
            else if (selectedSlot.GetComponent<HDK_InventorySlot>().itemType == ItemType.Melee)
            {
                if (meleeSlot.GetComponent<HDK_InventorySlot>().Empty)
                {
                    HDK_InventorySlot selSlot = selectedSlot.GetComponent<HDK_InventorySlot>();
                    Sprite itemIcon = selSlot.GetComponent<Image>().sprite;
                    GameObject weaponTarget = selSlot.GetComponent<HDK_InventorySlot>().WeaponTarget;
                    if (meleeSlot.GetComponent<HDK_InventorySlot>().Empty)
                    {
                        meleeSlot.GetComponent<HDK_InventorySlot>().UpdateSlot(selSlot.itemName, selSlot.itemInfo, itemIcon, ItemType.Melee, 0, 0, null, weaponTarget);
                        selSlot.ResetSlot();
                        freeSlots += 1;
                        FindObjectOfType<HDK_WeaponsManager>().currentMelee = weaponTarget;
                    }
                }
                else
                {
                    StartCoroutine(ErrorText("YOU ARE ALREADY USING A MELEE"));
                }
            }
            else if (selectedSlot.GetComponent<HDK_InventorySlot>().itemType == ItemType.Ammo)
            {
                StartCoroutine(ErrorText("YOU DON'T NEED TO EQUIP THIS ITEM"));
            }
            else if (selectedSlot.GetComponent<HDK_InventorySlot>().itemType == ItemType.Flashlight)
            {
                StartCoroutine(ErrorText("YOU DON'T NEED TO EQUIP THIS ITEM"));
            }
            else if (selectedSlot.GetComponent<HDK_InventorySlot>().itemType == ItemType.FlashlightBatteries)
            {
                StartCoroutine(ErrorText("YOU DON'T NEED TO EQUIP THIS ITEM"));
            }
            else if (selectedSlot.GetComponent<HDK_InventorySlot>().itemType == ItemType.DigitalCamera)
            {
                StartCoroutine(ErrorText("YOU DON'T NEED TO EQUIP THIS ITEM"));
            }
            else if (selectedSlot.GetComponent<HDK_InventorySlot>().itemType == ItemType.Medikit)
            {
                StartCoroutine(ErrorText("YOU DON'T NEED TO EQUIP THIS ITEM"));
            }
            else if (selectedSlot.GetComponent<HDK_InventorySlot>().itemType == ItemType.Eatable)
            {
                StartCoroutine(ErrorText("YOU DON'T NEED TO EQUIP THIS ITEM"));
            }
            else if (selectedSlot.GetComponent<HDK_InventorySlot>().itemType == ItemType.Key)
            {
                if (keySlot.GetComponent<HDK_InventorySlot>().Empty)
                {
                    HDK_InventorySlot selSlot = selectedSlot.GetComponent<HDK_InventorySlot>();
                    Sprite itemIcon = selSlot.GetComponent<Image>().sprite;
                    GameObject targetDoor = selSlot.GetComponent<HDK_InventorySlot>().TargetObject;
                    if (keySlot.GetComponent<HDK_InventorySlot>().Empty)
                    {
                        keySlot.GetComponent<HDK_InventorySlot>().UpdateSlot(selSlot.itemName, selSlot.itemInfo, itemIcon, ItemType.Key, 0, 0, targetDoor, null);
                        selSlot.ResetSlot();
                        freeSlots += 1;
                    }
                }
                else
                {
                    StartCoroutine(ErrorText("YOU ALREADY EQUIPPED ANOTHER KEY"));
                }
            }
        }
        else if (selectedSlot == null)
        {
            StartCoroutine(ErrorText("YOU MUST SELECT AN ITEM"));
        }
    }

    public void UseItem()
    {
        if (selectedSlot == meleeSlot || selectedSlot == firegunSlot || selectedSlot == keySlot)
        {
            if (!selectedSlot.GetComponent<HDK_InventorySlot>().Empty)
            {
                if(selectedSlot == meleeSlot)
                {
                    StartCoroutine(ErrorText("USE KEY << 1 >> TO DRAW MELEE"));
                }
                else if(selectedSlot == firegunSlot)
                {
                    StartCoroutine(ErrorText("USE KEY << 2 >> TO DRAW FIREGUN"));
                }
                else if (selectedSlot == keySlot)
                {
                    keySlot.GetComponent<HDK_InventorySlot>().TargetObject.SendMessage("UnlockDoor");
                    keySlot.GetComponent<HDK_InventorySlot>().ResetSlot();
                }
            }
            else
            {
                StartCoroutine(ErrorText("EMPTY SLOT"));
            }
        }
        else if (selectedSlot != null && selectedSlot != meleeSlot && selectedSlot != firegunSlot && selectedSlot != keySlot)
        {
            HDK_InventorySlot selSlot = selectedSlot.GetComponent<HDK_InventorySlot>();

            if (selectedSlot.GetComponent<HDK_InventorySlot>().itemType == ItemType.NONE)
            {
                StartCoroutine(ErrorText("EMPTY SLOT"));
            }
            if (selectedSlot.GetComponent<HDK_InventorySlot>().itemType == ItemType.Firegun)
            {
                StartCoroutine(ErrorText("YOU MUST EQUIP FIRST"));
            }
            else if (selectedSlot.GetComponent<HDK_InventorySlot>().itemType == ItemType.Melee)
            {
                StartCoroutine(ErrorText("YOU MUST EQUIP FIRST"));
            }
            else if (selectedSlot.GetComponent<HDK_InventorySlot>().itemType == ItemType.Ammo)
            {
                if (firegunSlot.GetComponent<HDK_InventorySlot>().Empty)
                {
                    StartCoroutine(ErrorText("YOU MUST EQUIP THE FIREGUN FIRST"));
                }
                else if(firegunSlot.GetComponent<HDK_InventorySlot>().WeaponTarget == selectedSlot.GetComponent<HDK_InventorySlot>().WeaponTarget)
                {
                    FindObjectOfType<HDK_WeaponsManager>().AddAmmos(selectedSlot.GetComponent<HDK_InventorySlot>().AmmosQuantity);
                    selSlot.ResetSlot();
                    freeSlots += 1;
                }
                else if (firegunSlot.GetComponent<HDK_InventorySlot>().WeaponTarget != selectedSlot.GetComponent<HDK_InventorySlot>().WeaponTarget)
                {
                    StartCoroutine(ErrorText("YOU ARE USING A DIFFERENT FIREGUN"));
                }
            }
            else if (selectedSlot.GetComponent<HDK_InventorySlot>().itemType == ItemType.Flashlight)
            {
                FindObjectOfType<HDK_Flashlight>().DrawFlashlight();
                CloseInventory();
            }
            else if (selectedSlot.GetComponent<HDK_InventorySlot>().itemType == ItemType.FlashlightBatteries)
            {
                if (FindObjectOfType<HDK_WeaponsManager>().usingFlashlight)
                {
                    if (FindObjectOfType<HDK_Flashlight>().health <= 80)
                    {
                        FindObjectOfType<HDK_Flashlight>().Recharge();
                        selSlot.ResetSlot();
                        freeSlots += 1;
                    }
                    else
                    {
                        StartCoroutine(ErrorText("FLASHLIGHT HEALTH IS FULL"));
                    }
                }
                else
                {
                    StartCoroutine(ErrorText("YOU MUST EQUIP THE FLASHLIGHT FIRST"));
                }
            }
            else if (selectedSlot.GetComponent<HDK_InventorySlot>().itemType == ItemType.DigitalCamera)
            {
                CloseInventory();
                if (!FindObjectOfType<HDK_DigitalCamera>().UsingCamera)
                {
                    FindObjectOfType<HDK_DigitalCamera>().CameraUse(true);
                }else
                {
                    FindObjectOfType<HDK_DigitalCamera>().CameraUse(false);
                }
            }
            else if (selectedSlot.GetComponent<HDK_InventorySlot>().itemType == ItemType.Medikit)
            {
                if (selectedSlot.GetComponent<HDK_InventorySlot>().HealthValue > 0)
                {
                    if (Player.GetComponent<HDK_PlayerHealth>().Health < 100f)
                    {
                        Player.GetComponent<HDK_PlayerHealth>().Health += selectedSlot.GetComponent<HDK_InventorySlot>().HealthValue;
                        selSlot.ResetSlot();
                        freeSlots += 1;
                    }
                    else
                    {
                        StartCoroutine(ErrorText("YOUR HEALTH IS FULL"));
                    }
                }
            }
            else if (selectedSlot.GetComponent<HDK_InventorySlot>().itemType == ItemType.Eatable)
            {
                if (selectedSlot.GetComponent<HDK_InventorySlot>().HealthValue > 0)
                {
                    if (Player.GetComponent<HDK_PlayerHealth>().Health < 100f)
                    {
                        Player.GetComponent<HDK_PlayerHealth>().Health += selectedSlot.GetComponent<HDK_InventorySlot>().HealthValue;
                        selSlot.ResetSlot();
                        freeSlots += 1;
                    }
                    else
                    {
                        StartCoroutine(ErrorText("YOUR HEALTH IS FULL"));
                    }
                }
                else
                {
                    Player.GetComponent<HDK_PlayerHealth>().FallingDamage(selectedSlot.GetComponent<HDK_InventorySlot>().HealthValue);
                }
            }
            else if (selectedSlot.GetComponent<HDK_InventorySlot>().itemType == ItemType.Key)
            {
                StartCoroutine(ErrorText("YOU CAN ONLY EQUIP THIS ITEM"));
            }
        }
        else if(selectedSlot == null)
        {
            StartCoroutine(ErrorText("YOU MUST SELECT AN ITEM"));
        }
    }

    IEnumerator ErrorText(string error)
    {
        errorLine.text = error;
        errorLine.gameObject.GetComponent<CanvasGroup>().alpha = 1f;
        yield return new WaitForSeconds(2);
        errorLine.gameObject.GetComponent<CanvasGroup>().alpha = 0f;
    }

    public void DestroyItem ()
    {
        if (selectedSlot == null)
        {
            StartCoroutine(ErrorText("YOU MUST SELECT AN ITEM"));
        }
        else if (selectedSlot.GetComponent<HDK_InventorySlot>().Empty)
        {
            StartCoroutine(ErrorText("EMPTY SLOT"));
        }
        else if (selectedSlot.GetComponent<HDK_InventorySlot>().itemType == ItemType.Flashlight)
        {
            if (FindObjectOfType<HDK_Flashlight>().usingFlashlight)
            {
                FindObjectOfType<HDK_Flashlight>().PutdownFlashlight(0);
            }
            selectedSlot.GetComponent<HDK_InventorySlot>().ResetSlot();
            freeSlots += 1;
            FindObjectOfType<HDK_Flashlight>().hasFlashlight = false;
            sourceAudio.clip = itemDestorySound;
            sourceAudio.Play();
        }
        else if (selectedSlot.GetComponent<HDK_InventorySlot>().itemType == ItemType.DigitalCamera)
        {
            if (FindObjectOfType<HDK_DigitalCamera>().UsingCamera)
            {
                FindObjectOfType<HDK_DigitalCamera>().CameraUse(false);
            }
            selectedSlot.GetComponent<HDK_InventorySlot>().ResetSlot();
            freeSlots += 1;
            FindObjectOfType<HDK_DigitalCamera>().HasCamera = false;
            sourceAudio.clip = itemDestorySound;
            sourceAudio.Play();
        }
        else if (selectedSlot != meleeSlot && selectedSlot != firegunSlot && selectedSlot != keySlot)
        {
            if (selectedSlot != null && !selectedSlot.GetComponent<HDK_InventorySlot>().Empty)
            {
                selectedSlot.GetComponent<HDK_InventorySlot>().ResetSlot();
                freeSlots += 1;
                sourceAudio.clip = itemDestorySound;
                sourceAudio.Play();
            }
        }
        else if (selectedSlot != null && !selectedSlot.GetComponent<HDK_InventorySlot>().Empty)
        {
            if (freeSlots > 0)
            {
                if(selectedSlot == meleeSlot)
                {
                    string name = meleeSlot.GetComponent<HDK_InventorySlot>().itemName;
                    string info = meleeSlot.GetComponent<HDK_InventorySlot>().itemInfo;
                    Sprite icon = meleeSlot.GetComponent<Image>().sprite;
                    ItemType type = meleeSlot.GetComponent<HDK_InventorySlot>().itemType;
                    GameObject weapon = meleeSlot.GetComponent<HDK_InventorySlot>().WeaponTarget;
                    AddItem(name, info, icon, type, 0, 0, null, weapon);
                    meleeSlot.GetComponent<HDK_InventorySlot>().ResetSlot();
                    FindObjectOfType<HDK_WeaponsManager>().melee_Putdown(false, true);
                }
                else if (selectedSlot == firegunSlot)
                {
                    string name = firegunSlot.GetComponent<HDK_InventorySlot>().itemName;
                    string info = firegunSlot.GetComponent<HDK_InventorySlot>().itemInfo;
                    Sprite icon = firegunSlot.GetComponent<Image>().sprite;
                    ItemType type = firegunSlot.GetComponent<HDK_InventorySlot>().itemType;
                    GameObject weapon = firegunSlot.GetComponent<HDK_InventorySlot>().WeaponTarget;
                    AddItem(name, info, icon, type, 0, 0, null, weapon);
                    firegunSlot.GetComponent<HDK_InventorySlot>().ResetSlot();
                    FindObjectOfType<HDK_WeaponsManager>().gun_Putdown(false, true);
                }
                else if (selectedSlot == keySlot)
                {
                    string name = keySlot.GetComponent<HDK_InventorySlot>().itemName;
                    string info = keySlot.GetComponent<HDK_InventorySlot>().itemInfo;
                    Sprite icon = keySlot.GetComponent<Image>().sprite;
                    ItemType type = keySlot.GetComponent<HDK_InventorySlot>().itemType;
                    GameObject targetDoor = keySlot.GetComponent<HDK_InventorySlot>().TargetObject;
                    AddItem(name, info, icon, type, 0, 0, null, targetDoor);
                    keySlot.GetComponent<HDK_InventorySlot>().ResetSlot();
                }
            }
            else
            {
                StartCoroutine(ErrorText("THERE AREN'T FREE SLOTS"));
            }
        }
	}

    public void AddItem(string name, string info, Sprite icon, ItemType type, int ammo, float health, GameObject door, GameObject weapon)
    {
        if(freeSlots > 0)
        {
            for (int i = 0; i <= inventorySlots.Length; i++)
            {
                if (inventorySlots[i] == inventorySlots[i].GetComponent<HDK_InventorySlot>().Empty)
                {
                    inventorySlots[i].GetComponent<HDK_InventorySlot>().UpdateSlot(name, info, icon, type, ammo, health, door, weapon);
                    freeSlots -= 1;
                    break;
                }
            }
        }
        else
        {
            //No space in the inventory
        }
    }

    public void DeselectItem()
    {
        SlotSelection(true, null);
    }

    public void SlotSelection(bool empty, GameObject slot)
    {
        selectedSlot = slot;
        if (empty)
        {
            emptySlot = true;
        }else
        {
            emptySlot = false;
        }
    }

    public void PlayHover()
    {
        GetComponent<AudioSource>().PlayOneShot(mouseHover, mouseVolume);
    }

    public void PlayClick()
    {
        GetComponent<AudioSource>().PlayOneShot(mouseClick, mouseVolume);
    }

    void Start()
    {
        Player = GameObject.Find("Player");
        totalSlots = inventorySlots.Length;
        freeSlots = totalSlots;
        sourceAudio = GetComponent<AudioSource>();
    }

    public void CloseInventory()
    {
        Player.GetComponentInChildren<BlurOptimized>().enabled = false;
        Player.GetComponentInChildren<HDK_RaycastManager>().enabled = true;
        Player.GetComponent<HDK_MouseZoom>().canZoom = true;
        Player.GetComponentInChildren<HeadBobController>().enabled = true;
        Player.GetComponentInChildren<SwayWeapon>().enabled = true;
        Player.GetComponent<HDK_Stamina>().Busy(false);
        Inventory.SetActive(false);
        Player.GetComponent<FirstPersonController>().enabled = true;
        inventoryOpen = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        sourceAudio.clip = openCloseSound;
        sourceAudio.Play();
        foreach (GameObject obj in ToDisableGUI)
        {
            obj.SetActive(true);
        }
    }

    public void OpenInventory()
    {
        Player.GetComponentInChildren<BlurOptimized>().enabled = true;
        Player.GetComponentInChildren<HDK_RaycastManager>().enabled = false;
        Player.GetComponent<HDK_MouseZoom>().ZoomOut();
        Player.GetComponentInChildren<SwayWeapon>().enabled = false;
        Player.GetComponentInChildren<HeadBobController>().enabled = false;
        Player.GetComponent<HDK_Stamina>().Busy(true);
        Player.GetComponent<FirstPersonController>().enabled = false;
        Inventory.SetActive(true);
        inventoryOpen = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        sourceAudio.clip = openCloseSound;
        sourceAudio.Play();    
        foreach (GameObject obj in ToDisableGUI)
        {
            obj.SetActive(false);
        }
    }

    void Update()
    {
        if (selectedSlot != null)
        {
            if (selectedSlot.GetComponent<HDK_InventorySlot>().Empty)
            {
                emptySlot = true;
            }
        }

        bool examining = HDK_RaycastManager.ExaminingObject;
        bool reading = HDK_RaycastManager.ReadingPaper;
        bool security = HDK_RaycastManager.UsingSecurityCam;
        bool paused = HDK_PauseManager.GamePaused;

        if (!paused && !examining && !reading && !security && !inventoryOpen)
        {
            //if (Input.GetKeyDown(KeyCode.Tab))
            if (Input.GetButtonDown("Inventary"))
            {
                OpenInventory();
            }
        }
        else if (examining)
        {
            if (Input.GetButtonDown("Inventary"))
            {
                HDK_WeaponsManager wepMan = FindObjectOfType<HDK_WeaponsManager>();
                if (wepMan.usingGun)
                {
                    wepMan.currentGun.SetActive(true);
                }
                else if (wepMan.usingMelee)
                {
                    wepMan.currentMelee.SetActive(true);
                }
                else if (wepMan.usingFlashlight)
                {
                    Player.GetComponent<HDK_Flashlight>().ArmsAnims.SetActive(true);
                }
                Player.GetComponentInChildren<HDK_RaycastManager>().PutDownObject();
                OpenInventory();
            }
        }
        else if (reading)
        {
            if (Input.GetButtonDown("Inventary"))
            {
                Player.GetComponentInChildren<HDK_RaycastManager>().ClosePaper();
                OpenInventory();
            }
        }
        else if (security)
        {
            if (Input.GetButtonDown("Inventary"))
            {
                Player.GetComponentInChildren<HDK_RaycastManager>().CloseCam();
                OpenInventory();
            }
        }
        else if (paused)
        {
            if (Input.GetButtonDown("Inventary"))
            {
                FindObjectOfType<HDK_PauseManager>().UnPause();
                OpenInventory();
            }
        }
        else if (inventoryOpen)
        {
            if (Input.GetButtonDown("Inventary"))
            {
                CloseInventory();
            }
        }

        switch (selectedSlotType)
        {
            case ItemType.NONE:
                slotType = "NONE";
                break;

            case ItemType.Firegun:
                slotType = "FIREGUN";
                break;

            case ItemType.Melee:
                slotType = "MELEE";
                break;

            case ItemType.Ammo:
                slotType = "AMMO";
                break;

            case ItemType.Flashlight:
                slotType = "FLASHLIGHT";
                break;

            case ItemType.FlashlightBatteries:
                slotType = "FLASHLIGHT BATTERY";
                break;

            case ItemType.DigitalCamera:
                slotType = "DIGITAL CAMERA";
                break;

            case ItemType.Medikit:
                slotType = "MEDIKIT";
                break;

            case ItemType.Eatable:
                slotType = "EATABLE";
                break;

            case ItemType.Key:
                slotType = "KEY";
                break;
        }

        slotText.text = freeSlots.ToString() + " / " + totalSlots.ToString();

        if (selectedSlot == null)
        {
            itemName.text = "NO ITEM SELECTED";
            itemInfo.text = "NO ITEM SELECTED";
            itemType.text = "NO ITEM SELECTED";
            ammo_health_Value.text = "NO ITEM SELECTED";
        }
        else if (emptySlot)
        {
            itemName.text = "EMPTY SLOT";
            itemInfo.text = "EMPTY SLOT";
            itemType.text = "EMPTY SLOT";
            ammo_health_Value.text = "EMPTY SLOT";
        }
        else if(!emptySlot && selectedSlot != null)
        {
            itemName.text = selectedSlot.GetComponent<HDK_InventorySlot>().itemName;
            itemInfo.text = selectedSlot.GetComponent<HDK_InventorySlot>().itemInfo;
            itemType.text = slotType;
            if(selectedSlot.GetComponent<HDK_InventorySlot>().itemType == ItemType.Ammo)
            {
                ammo_health_Value.text = selectedSlot.GetComponent<HDK_InventorySlot>().AmmosQuantity.ToString();
            }
            else if (selectedSlot.GetComponent<HDK_InventorySlot>().itemType == ItemType.Medikit || selectedSlot.GetComponent<HDK_InventorySlot>().itemType == ItemType.Eatable)
            {
                ammo_health_Value.text = selectedSlot.GetComponent<HDK_InventorySlot>().HealthValue.ToString();
            }
            else if (selectedSlot.GetComponent<HDK_InventorySlot>().itemType != ItemType.Medikit 
                    && selectedSlot.GetComponent<HDK_InventorySlot>().itemType != ItemType.Eatable
                    && selectedSlot.GetComponent<HDK_InventorySlot>().itemType != ItemType.Ammo)
            {
                ammo_health_Value.text = "NONE";
            }
        }
    }
}