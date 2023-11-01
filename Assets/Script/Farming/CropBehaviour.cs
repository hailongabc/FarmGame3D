using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropBehaviour : MonoBehaviour
{
    //Information on what the crop will grow into
    SeedData seedToGrow;

    [Header("Stages of Life")]
    public GameObject seed;
    private GameObject seedling;
    private GameObject harvestable;

    //The growth points of the crop
    int growth;
    //How many growth points it takes before it becomes harvestable 
    int maxGrowth;

    public enum CropState
    {
        Seed, Seedling, Harvestable
    }

    //The current stage in the crop's growth
    public CropState cropState;

    //Initialisation for the crop GameObject
    //Called when the player plants a seed
   public void Plant(SeedData seedToGrow)
    {
        //Save the seed infomation
        this.seedToGrow = seedToGrow;

        //Instantiate the seedling and harvestable GameObjects
        seedling = Instantiate(seedToGrow.seedling, transform);

        //Access the crop item data
        ItemData cropToYield = seedToGrow.cropToYeild;

        //Instantiate the harvestable crop
        harvestable = Instantiate(cropToYield.gameModel, transform);

        //Convert Days To Grow into hours
        int hoursToGrow = GameTime.DaysToHours(seedToGrow.daysToGrow);
        //Convert it to minutes
        maxGrowth = GameTime.HourToMinutes(hoursToGrow);

        //Set the initial state to Seed
        SwitchState(CropState.Seed);
    }

    //The crop will grow when watered
    public void Grow()
    {
        //Increase the growth point by 1
        growth++;

        //The seed wwill sprout into a seedling when the growth is at 50%
        if(growth >= maxGrowth / 2 && cropState == CropState.Seed)
        {
            SwitchState(CropState.Seedling);
        }

        //Grow from seedling to harvestable
        if(growth >= maxGrowth && cropState == CropState.Seedling)
        {
            SwitchState(CropState.Harvestable);
        }
    }

    //Function to handle the state changes
    public void SwitchState(CropState stateToSwitch)
    {
        //Reset everything and set all GameObjects to inactive
        seed.SetActive(false);
        seedling.SetActive(false);
        harvestable.SetActive(false);

        switch (stateToSwitch)
        {
            case CropState.Seed:
                //Enable the Seed GameObject
                seed.SetActive(true);
                break;
            case CropState.Seedling:
                //Enable the Seedling GameObject
                seedling.SetActive(true);
                break;
            case CropState.Harvestable:
                //Enable the Harvestable GameObject
                harvestable.SetActive(true);
                //Unparent it to the crop
                harvestable.transform.parent = null;

                Destroy(gameObject);
                break;
        }

        //Set the current crop state to the state we're switching to
        cropState = stateToSwitch;
    }
}
