using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Timer
{
    [Header(" Settings ")]
    private int timerSeconds;
    private int timer;
    private bool timerIsActive;
    private float startingTime;

    [Header(" Events ")]
    [HideInInspector] public UnityAction OnTimerStartedEvent;
    [HideInInspector] public UnityAction<float> OnTimerUpdatedEvent;
    [HideInInspector] public UnityAction OnTimerEndedEvent;

    // Update is called once per frame
    public void Update()
    {
        if (timerIsActive)
            ManageTimer();
    }

    public void StartTimer(int seconds)
    {
        if (timerIsActive)
        {
            Debug.Log("Timer is already active");
            return;
        }

        startingTime = Time.time;
        timer = seconds;

        OnTimerStartedEvent?.Invoke();
        OnTimerUpdatedEvent?.Invoke(timer);

        timerIsActive = true;
    }

    private void ManageTimer()
    {
        if (Time.time - startingTime >= 1)
        {
            startingTime = Time.time;
            DiminishTimer();
        }
    }

    private void DiminishTimer()
    {
        timer--;
        OnTimerUpdatedEvent?.Invoke(timer);

        if (timer <= -1)
            EndTimer();
    }

    private void EndTimer()
    {
        timerIsActive = false;
        OnTimerEndedEvent?.Invoke();
    }

    public float GetTimer()
    {
        return timer;
    }
}