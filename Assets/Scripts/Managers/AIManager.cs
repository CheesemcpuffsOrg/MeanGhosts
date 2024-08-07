using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum GlobalAIBehaviourState
{
    Timid,
    Curious,
    Angry,
    Aggresive
}

public class AIManager : MonoBehaviour
{
    public static AIManager AIManagerInstance;

    [SerializeField] GlobalAIBehaviourState currentGlobalState;

    public event Action <GlobalAIBehaviourState> StateChanged;

    GameObject player;

    bool started = false;

    [Header("References")]
    [SerializeField] ReferenceScriptableObject playerReference;

    private void Awake()
    {
        AIManagerInstance = this;
    }

    void Start()
    {
       // currentGlobalState = GlobalAIBehaviourState.Timid;
        player = ReferenceManager.ReferenceManagerInstance.GetReference(playerReference);

        started = true;
        OnStartOrEnable();

    }

    void ScoreChanged(int score)
    {
        switch (score)
        {
            case 0:
                currentGlobalState = GlobalAIBehaviourState.Timid; 
                break;
            case 1:
                currentGlobalState = GlobalAIBehaviourState.Curious; 
                break;
            case 3:
                currentGlobalState = GlobalAIBehaviourState.Angry;
                break;
            case 5:
                currentGlobalState= GlobalAIBehaviourState.Aggresive; 
                break;
        }
    }  

    public GlobalAIBehaviourState GetCurrentState()
    {
        return currentGlobalState;
    }

    void OnStartOrEnable()
    {
        GameManager.GameManagerInstance.scoreChanged += ScoreChanged;
    }

    private void OnEnable()
    {
        if (started) OnStartOrEnable();
    }

    private void OnDisable()
    {
        GameManager.GameManagerInstance.scoreChanged -= ScoreChanged;
    }

}
