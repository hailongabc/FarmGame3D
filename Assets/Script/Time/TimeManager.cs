using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; private set; }
    [Header("Internal Clock")]
    [SerializeField] 
    GameTime timeStamp;

    public float timeScale = 1.0f;

    [Header ("Day and Night cycle")]
    //The transform of the directional light (sun)
    public Transform sunTransform;

    //List of Objects to inform of changes to the time
    List<ITimeTracker> listeners = new List<ITimeTracker>();
    private void Awake()
    {
        //If there is more than one instance, destroy the extra
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            //Set the static instance to this instance
            Instance = this;
        }
    }

    private void Start()
    {
        //Initialise the time stamp
        timeStamp = new GameTime(0, GameTime.Season.Spring, 1, 6, 0);
        StartCoroutine(TimeUpdate());
    }

    IEnumerator TimeUpdate()
    {
        while (true)
        {
            Tick();
           yield return new WaitForSeconds(1 / timeScale);
        }
        
    }

    //A tick of the in-game time
    public void Tick()
    {
        timeStamp.UpdateClock();

        //Inform each of the listeners of the new time state
        foreach(ITimeTracker listener in listeners)
        {
            listener.ClockUpdate(timeStamp);
        }
        UpdateSunMovement();

    }

    //Day and night cycle
    void UpdateSunMovement()
    {
        //Convert the current time to minutes
        int timeInMinutes = GameTime.HourToMinutes(timeStamp.hour) + timeStamp.minute;

        //Sun moves 15 degrees in an hour
        //0.25 degree in a minute
        //At midnight (0:00) the angle of the sun should be -90
        float sunAngle = 0.25f * timeInMinutes - 90;

        //Apply the angle to the directional light
        sunTransform.eulerAngles = new Vector3(sunAngle, 0, 0);
    }

    //Get the timestamp
    public GameTime getGameTimestamp()
    {
        //Return a cloned instace
        return new GameTime(timeStamp);
    }


    //Handling Listeners

    //Add the object to the list of listeners
    public void RegisterTracker(ITimeTracker listener)
    {
        listeners.Add(listener);
    }

    //Remove the object from the list of listeners
    public void UnregisterTracker(ITimeTracker listener)
    {
        listeners.Remove(listener);
    }
}
