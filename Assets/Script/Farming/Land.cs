using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Land : MonoBehaviour, ITimeTracker
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

    //Cache the time the land was watered
    GameTime timeWatered;

    [Header("Crops")]
    //The crop prefab to instantiate
    public GameObject cropPrefab;

    //The crop currently planted on the land
    CropBehaviour cropPlanted = null;

    void Start()
    {
        //Get the renderer component
        renderer = GetComponent<Renderer>();

        //Set the land to soil by default
        SwitchLandStatus(LandStatus.Soil);

        //Deselect the land by the default
        Select(false);

        //Add this to TimeManager's Listener list
        TimeManager.Instance.RegisterTracker(this);
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

                //Cache the time it was watered 
                timeWatered = TimeManager.Instance.getGameTimestamp();
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

        //If there's nothing equipped, return
        if(toolSlot == null)
        {
            return;
        }

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
            //We don't need to check for seeds if we have already confirmed the tool to be an equipment
            return;
        }

        //Try casting the itemdata in the toolslot as SeedData
        SeedData seedTool = toolSlot as SeedData;


        //Conditions for the player to be able to plant a seed
        //1: He is holding a tool of type SeedData
        //2: The Land State must be either watered or farmland
        //3: There isn't already a crop that has been planted
        if (seedTool != null && landStatus != LandStatus.Soil && cropPlanted == null)
        {
            //Instantiate the crop object parented to the land
            GameObject cropObject = Instantiate(cropPrefab, transform);

            //Move the crop object to the top of the land gameobject
            cropObject.transform.position = new Vector3(transform.position.x, 0, transform.position.z);

            //Access the CropBehaviour of the crop we're going to plant
            cropPlanted = cropObject.GetComponent<CropBehaviour>();
            //Plant it
            cropPlanted.Plant(seedTool);

        }
    }

    public void ClockUpdate(GameTime timeStamp)
    {
        //Check if 24 hours has passed since last watered
        if(landStatus == LandStatus.Water)
        {
            //Hours since the land was watered
            int hoursElaped =  GameTime.CompareTimestamps(timeWatered, timeStamp);

            //Grow the planted crop, if any
            if(cropPlanted != null)
            {
                cropPlanted.Grow();
            }

            if(hoursElaped > 24)
            {
                //Dry up (switch back to farmland)
                SwitchLandStatus(LandStatus.Farmland);
            }
        }
    }
}
