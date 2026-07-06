
using System;
using UnityEngine;

public class HunterDecisionTree : MonoBehaviour
{
    public enum HunterState
    {
        Patrol,
        Chase,
        Recalculate
    }

    [Header("Decision tree: rest settings")]
    [SerializeField] private float recInterval = 8f;
    [SerializeField] private float recDuration = 1f;


    public HunterState currentState;


    private float restAccumulator = 0f;
    private float restTimer = 0f;
    private bool isResting = false;


    private DecisionNode root;


    private bool canSeePlayerFlag = false;

    private void Awake()
    {
        BuildTree();
    }


    public void UpdateTree(bool canSeePlayer)
    {
        canSeePlayerFlag = canSeePlayer;


        if (canSeePlayerFlag)
        {
            isResting = false;
            restAccumulator = 0f;
            restTimer = 0f;
        }
        else
        {

            UpdateRestTimers();
        }


        if (root != null)
            currentState = root.Evaluate();
        else
            currentState = HunterState.Patrol;
    }

    private void UpdateRestTimers()
    {
        if (isResting)
        {
            restTimer += Time.deltaTime;
            if (restTimer >= recDuration)
            {
                isResting = false;
                restTimer = 0f;
                restAccumulator = 0f;
            }
        }
        else
        {
            restAccumulator += Time.deltaTime;
            if (restAccumulator >= recInterval)
            {

                isResting = true;
                restTimer = 0f;
            }
        }
    }

    private void BuildTree()
    {

        var chaseAction = new ActionNode(HunterState.Chase);
        var recalcAction = new ActionNode(HunterState.Recalculate);
        var patrolAction = new ActionNode(HunterState.Patrol);

        var restQuestion = new QuestionNode(

            () => isResting,

            recalcAction,

            patrolAction
        );

        root = new QuestionNode(
            () => canSeePlayerFlag,

            chaseAction,

            restQuestion
        );
    }



    private abstract class DecisionNode
    {
        public abstract HunterState Evaluate();
    }

    private class ActionNode : DecisionNode
    {
        private readonly HunterState state;

        public ActionNode(HunterState state)
        {
            this.state = state;
        }

        public override HunterState Evaluate()
        {
            return state;
        }
    }

    private class QuestionNode : DecisionNode
    {
        private readonly Func<bool> condition;
        private readonly DecisionNode trueNode;
        private readonly DecisionNode falseNode;

        public QuestionNode(Func<bool> condition, DecisionNode trueNode, DecisionNode falseNode)
        {
            this.condition = condition ?? throw new ArgumentNullException(nameof(condition));
            this.trueNode = trueNode ?? throw new ArgumentNullException(nameof(trueNode));
            this.falseNode = falseNode ?? throw new ArgumentNullException(nameof(falseNode));
        }

        public override HunterState Evaluate()
        {
            return condition() ? trueNode.Evaluate() : falseNode.Evaluate();
        }
    }


}