using UnityEngine;

public class FSM : MonoBehaviour
{
    public enum EnemyState
    {
        Patrol,
        Pursuit,
        Flee
    }

    public EnemyState currentState = EnemyState.Patrol;

    public void UpdateState(bool canSeePlayer, bool shouldFlee)
    {
        switch (currentState)
        {
            case EnemyState.Patrol:

                if (shouldFlee && canSeePlayer)
                {
                    currentState = EnemyState.Flee;
                    Debug.Log("Switch to Flee");
                }
                else if (canSeePlayer)
                {
                    currentState = EnemyState.Pursuit;
                    Debug.Log("Switch to Pursuit");
                }

                break;

            case EnemyState.Pursuit:

                if (shouldFlee && canSeePlayer)
                {
                    currentState = EnemyState.Flee;
                    Debug.Log("Switch to Flee");
                }
                else if (!canSeePlayer)
                {
                    currentState = EnemyState.Patrol;
                    Debug.Log("Switch to Patrol");
                }

                break;

            case EnemyState.Flee:

                if (!shouldFlee)
                {
                    currentState = canSeePlayer ? EnemyState.Pursuit : EnemyState.Patrol;
                    Debug.Log("Exit Flee");
                }

                break;
        }
    }
}