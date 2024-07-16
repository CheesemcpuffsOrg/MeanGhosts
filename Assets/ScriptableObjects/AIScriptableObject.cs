using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AI", menuName = "ScriptableObject/AI")]
public class AIScriptableObject : ScriptableObject
{
    public float speed;
    public float chaseSpeed;
    public float fleeSpeed;
    public float agroRange;
}
