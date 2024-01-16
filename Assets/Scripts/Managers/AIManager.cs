using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour
{

    public static AIManager AIManagerInstance;

    GameObject player;

    List<GameObject> ghostCaughtByNormalBeam;
    List<GameObject> ghostCaughtByHighBeam;

    [Header("Tags")]
    [SerializeField]TagScriptableObject playerTag;

    void Start()
    {
        AIManagerInstance = this;

        player = TagExtensions.FindWithTag(gameObject, playerTag);
    }


}
