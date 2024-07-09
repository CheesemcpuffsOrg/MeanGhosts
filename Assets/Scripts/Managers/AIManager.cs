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

    GlobalAIBehaviourState currentGlobalState;

    GameObject player;

    [SerializeField] ReferenceScriptableObject playerReference;

    [Header("Tags")]
    [SerializeField]TagScriptableObject playerTag;

    private void Awake()
    {
        AIManagerInstance = this;
    }

    void Start()
    {
       // player = TagExtensions.FindWithTag(gameObject, playerTag);
        currentGlobalState = GlobalAIBehaviourState.Timid;
        player = ReferenceManager.ReferenceManagerInstance.GetReference(playerReference);
    }


}
