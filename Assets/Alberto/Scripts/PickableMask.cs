using UnityEngine;

public class PickableMask : Interactuable
{
    public string pickupText;
    public InventoryObject inventoryObject;
    private GameObject inventoryReference;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InitializePickup();
    }

    public void InitializePickup()
    {
        switch (inventoryObject)
        {
            case InventoryObject.Mask:
                inventoryReference = Inventario.instance.maskReference;
                break;
            case InventoryObject.NightVisionGoogles:
                inventoryReference = Inventario.instance.nightVisionReference;
                break;
            case InventoryObject.Key:
                inventoryReference = Inventario.instance.keyReference;
                break;
        }
    }

    public override void Usar(PlayerController p)
    {
        this.gameObject.SetActive(false);
        inventoryReference.SetActive(true);
        p.ShowFloatingText(pickupText);
        if (inventoryObject == InventoryObject.Mask)
        {
            p.maskUnlocked = true;
        }
        else if (inventoryObject == InventoryObject.NightVisionGoogles)
        {
            p.nightVisionUnlocked = true;
        }
    }
}