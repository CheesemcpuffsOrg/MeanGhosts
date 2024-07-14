using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIState : MonoBehaviour
{

    public abstract void EnterState(AIStateController state);

    public abstract void UpdateState(AIStateController state);

    public abstract void ExitState(AIStateController state);
}
