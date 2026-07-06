using UnityEngine;

public class CowardFSM : MonoBehaviour
{
    public CowardState CurrentState { get; private set; }

    private CowardPatrolState patrolState;
    private CowardEscapeState escapeState;

    private void Awake()
    {
        patrolState = new CowardPatrolState(this);
        escapeState = new CowardEscapeState(this);

        CurrentState = patrolState;
    }

    public void UpdateState(bool canSeePlayer)
    {
        CurrentState.Update(canSeePlayer);
    }

    public void ChangeToPatrol()
    {
        ChangeState(patrolState);
    }

    public void ChangeToEscape()
    {
        ChangeState(escapeState);
    }

    private void ChangeState(CowardState newState)
    {
        if (CurrentState == newState)
            return;

        CurrentState.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }

    public bool IsEscaping()
    {
        return CurrentState == escapeState;
    }
}

public abstract class CowardState
{
    protected CowardFSM fsm;

    public CowardState(CowardFSM fsm)
    {
        this.fsm = fsm;
    }

    public virtual void Enter() { }

    public virtual void Exit() { }

    public abstract void Update(bool canSeePlayer);
}

public class CowardPatrolState : CowardState
{
    public CowardPatrolState(CowardFSM fsm) : base(fsm) { }

    public override void Enter()
    {

    }

    public override void Update(bool canSeePlayer)
    {
        if (canSeePlayer)
        {
            fsm.ChangeToEscape();
        }
    }

    public override void Exit()
    {

    }
}

public class CowardEscapeState : CowardState
{
    public CowardEscapeState(CowardFSM fsm) : base(fsm) { }

    public override void Enter()
    {

    }

    public override void Update(bool canSeePlayer)
    {
        if (!canSeePlayer)
        {
            fsm.ChangeToPatrol();
        }
    }

    public override void Exit()
    {

    }
}