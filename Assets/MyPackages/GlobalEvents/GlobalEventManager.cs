using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalEventManager : MonoBehaviour
{

    public static GlobalEventManager globalEventManagerInstance;

    public event Action UpdateEvent;
    public event Action FixedUpdateEvent;
    public event Action LateUpdateEvent;

    private void Awake()
    {
        globalEventManagerInstance = this;
    }

    void Update()
    {
        UpdateEvent?.Invoke();
    }

    private void FixedUpdate()
    {
        FixedUpdateEvent?.Invoke();
    }

    private void LateUpdate()
    {
        LateUpdateEvent?.Invoke();
    }
}
