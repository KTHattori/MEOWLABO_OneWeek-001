using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayManager : MonoSingleton<DayManager>
{
    private int currentDay = 0;
    private WeekDay currentWeekDay = WeekDay.Monday;

    public int CurrentDay { get => currentDay; set => currentDay = value; }
    public WeekDay CurrentWeekDay { get => currentWeekDay; set => currentWeekDay = value; }
}
