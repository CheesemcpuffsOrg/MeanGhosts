using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AI", menuName = "ScriptableObject/AI")]
public class AIScriptableObjects : ScriptableObject
{
    public float speed;
    public float chaseSpeed;
    public float agroRange;
}
