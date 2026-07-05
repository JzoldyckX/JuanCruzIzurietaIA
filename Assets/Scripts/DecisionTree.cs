using UnityEngine;

public class DecisionTree : MonoBehaviour
{
    public enum EnemyState
    {
        Patrol,
        Pursuit,
        Flee
    }

    public EnemyState currentState;

    public void UpdateTree(bool canSeePlayer, bool shouldFlee)
    {
        if (shouldFlee)
        {
            if (canSeePlayer)
                currentState = EnemyState.Flee;
            else
                currentState = EnemyState.Patrol;
        }
        else
        {
            if (canSeePlayer)
                currentState = EnemyState.Pursuit;
            else
                currentState = EnemyState.Patrol;
        }
    }
}