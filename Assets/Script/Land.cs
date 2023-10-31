using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Land : MonoBehaviour
{
    public enum LandStatus
    {
        Soil, Farmland, Water
    }

    public LandStatus landStatus;

    public Material soilMat, farmlandMat, waterMat;
    new Renderer renderer;

    //The select gameobject to enable when the player is selecting the land
    public GameObject select;

    void Start()
    {
        //Get the renderer component
        renderer = GetComponent<Renderer>();

        //Set the land to soil by default
        SwitchLandStatus(LandStatus.Soil);

        //Deselect the land by the default
        Select(false);
    }

    public void SwitchLandStatus(LandStatus statusToSwitch)
    {
        //Set land status accordingly
        landStatus = statusToSwitch;
        Material materialToSwitch = soilMat;
        switch (statusToSwitch)
        {
            case LandStatus.Soil:
                //Switch to the soil material
                materialToSwitch = soilMat;
                break;
            case LandStatus.Farmland:
                //Switch to the soil material
                materialToSwitch = farmlandMat;
                break;
            case LandStatus.Water:
                //Switch to the soil material
                materialToSwitch = waterMat;
                break;

        }

        //GetComponent the renderer to apply the changes
        renderer.material = materialToSwitch;
    }

    public void Select(bool toggle)
    {
        select.SetActive(toggle);
    }

    //When the player process the interact button while selecting this land
    public void Interact()
    {
        //Check the player's tool slot
        ItemData toolSlot = InventoryManager.Instance.equippedTool;

        //Try casting the itemdata in the toolslot as EquipmentData
        EquipmentData equipmentTool = toolSlot as EquipmentData;

        //Check if it is of type EquipmentData
        if (equipmentTool != null)
        {
            //Get the tool type
            EquipmentData.ToolType toolType = equipmentTool.toolType;
            switch (toolType)
            {
                case EquipmentData.ToolType.Hoe:
                    SwitchLandStatus(LandStatus.Farmland);
                    break;
                case EquipmentData.ToolType.WateringCan:
                    SwitchLandStatus(LandStatus.Water);
                    break;
            }
        }
    }
}
