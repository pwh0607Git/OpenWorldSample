using System.Collections.Generic;

public enum NodeState
{
    Success,
    Failure,
    Running
}

public abstract class BTNode
{
    public abstract NodeState Evaluate();
}

// 자식 노드중 가장 먼저 성공한 노드를 실행 => 순서가 중요 !!!!
public class Selector : BTNode
{
    private List<BTNode> children = new List<BTNode>();

    public Selector(List<BTNode> children)
    {
        this.children = children;
    }   

    public override NodeState Evaluate()
    {
        foreach (var node in children)
        {
            switch (node.Evaluate())
            {
                case NodeState.Success:
                    return NodeState.Success;
                case NodeState.Running:
                    return NodeState.Running;
            }
        }
        return NodeState.Success;
    }
}

// 자식 노드가 모두 성공해야한다.
public class Sequence : BTNode
{
    private List<BTNode> children;

    public Sequence(List<BTNode> children)
    {
        this.children = children;
    }

    public override NodeState Evaluate()
    {
        bool anyChildRunning = false;

        foreach (var node in children)
        {
            switch (node.Evaluate())
            {
                case NodeState.Failure: return NodeState.Failure;
                case NodeState.Running:
                {
                    anyChildRunning = true;
                    break;
                }
            }
        }

        //모두 성공한 경우...
        return anyChildRunning ? NodeState.Running : NodeState.Success; 
    }
}

// 특정 조건 평가
public class ConditionNode : BTNode
{
    private System.Func<bool> condition;

    public ConditionNode(System.Func<bool> condition)
    {
        this.condition = condition;
    }

    public override NodeState Evaluate()
    {
        return condition.Invoke()? NodeState.Success : NodeState.Failure;
    }
}

// 특정 행동 실행
public class ActionNode : BTNode
{
    private System.Action action;

    public ActionNode(System.Action action)
    {
        this.action = action;
    }

    public override NodeState Evaluate()
    {
        action.Invoke();
        return NodeState.Success;
    }
}