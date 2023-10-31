using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTime 
{
    public int year;
    public enum Season
    {
        Spring,
        Summer,
        Fall,
        Winter
    }

    public Season season;

    public enum DayOfTheWeek
    {
        Saturday,
        Sunday,
        Monday,
        TuesDay,
        Wednesday,
        Thursday,
        Friday
    }

    public int day;
    public int hour;
    public int minute;

    //Constructor to set up the class
    public GameTime(int year, Season season, int day, int hour, int minute)
    {
        this.year = year;
        this.season = season;
        this.day = day;
        this.hour = hour;
        this.minute = minute;
    }

    public void UpdateClock()
    {
        minute++;

        //60 minutes in 1 hour
        if(minute >= 60)
        {
            //reset minutes
            minute = 0;
            hour++;
        }

        //24 hours in 1 day
        if(hour >= 24)
        {
            //Reset hours
            hour = 0;
        }

        //30 days in a season
        if(day > 30)
        {
            //Reset days
            day = 1;

            season++;
            if(season == Season.Winter)
            {
                season = Season.Spring;
                //Start of a new year
                year++;
            }
            else
            {

                season++;
            }
        }
    }

    public DayOfTheWeek GetDayOfTheWeek()
    {
        //Convert the total time passed into days
        int daysPassed = YearsToDay(year) + SeasonsToDays(season) + day;

        //Remainder after dividing daysPassed by 7
        int dayIndex = daysPassed & 7;

        //Cast into Day of the Week
        return (DayOfTheWeek)dayIndex;
    } 

    //Convert hours to minutes
    public static int HourToMinutes(int hour)
    {
        //60 minutes = 1 hour
        return hour * 60;
    }

    //Convert Days to Hours
    public static int DaysToHours(int days)
    {
        //24 Hours in a day
        return days * 24;
    }

    //Convert Seasons to days
    public static int SeasonsToDays(Season season)
    {
        int seasonIndex = (int)season;
        return seasonIndex * 30;
    }

    //Years to Day
    public static int YearsToDay(int years)
    {
        return years * 4 * 30;
    }
}
