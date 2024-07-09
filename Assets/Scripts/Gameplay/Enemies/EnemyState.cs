using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]

public class EnemyState
{
    public string stateName;
    private bool active = false;

    [Space(10)]
    [Header("--Start Events--")]
    [Space(10)]
    public UnityEvent eventOnStart = null;

    [Space(10)]
    [Header("--End Events--")]
    [Space(10)]
    public UnityEvent eventonEnd   = null;

    [Space(10)]
    [Header("--Timer Events--")]
    [Space(10)]
    public UnityEvent eventofTime  = null;

    public bool   userTimer   = false;
    public float  timer       = 0;
    private float currentTime = 0;

    public void Enable()
    {
        eventOnStart.Invoke();
    }

    public void Disable()
    {
        eventonEnd.Invoke();
    }
}
