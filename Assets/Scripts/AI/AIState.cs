using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIState : MonoBehaviour
{
    public abstract void EnterState(AIStateManager state);

    public abstract void UpdateState(AIStateManager state);

    public abstract void ExitState(AIStateManager state);
}
