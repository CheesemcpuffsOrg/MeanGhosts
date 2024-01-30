using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour
{

    public static AIManager AIManagerInstance;

    GameObject player;

    [Header("Tags")]
    [SerializeField]TagScriptableObject playerTag;

    private void Awake()
    {
        AIManagerInstance = this;
    }

    void Start()
    {
        player = TagExtensions.FindWithTag(gameObject, playerTag);
    }


}
