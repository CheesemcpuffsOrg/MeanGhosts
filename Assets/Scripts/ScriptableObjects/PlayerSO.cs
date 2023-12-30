using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player", menuName = "ScriptableObject/Player")]
public class PlayerSO : ScriptableObject
{
    public float speed;
    public float chaseSpeed;
    public float agroRange;
}
