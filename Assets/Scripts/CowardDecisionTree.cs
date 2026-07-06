using UnityEngine;

public class CowardDecisionTree : MonoBehaviour
{
    public enum CowardState
    {
        Patrol,
        Escape
    }

    public CowardState currentState;

    public void UpdateTree(bool canSeePlayer)
    {
        if (canSeePlayer)
            currentState = CowardState.Escape;
        else
            currentState = CowardState.Patrol;
    }
}