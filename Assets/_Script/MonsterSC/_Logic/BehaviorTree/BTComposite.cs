using System;
using System.Collections.Generic;
using UnityEngine;

public enum NodeState
{
    Success,
    Failure,
    Running
}

public abstract class BTNode
{
    public virtual void OnEnter() { }  // 노드 시작 시 실행
    public abstract NodeState Evaluate();  // 매 프레임 실행
    public virtual void OnExit() { }  // 노드 종료 시 실행
}

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
        return NodeState.Failure;
    }
}

public class Sequence : BTNode
{
    private List<BTNode> nodes;
    private int currentIndex = 0;  // 현재 실행 중인 노드 인덱스

    public Sequence(List<BTNode> nodes)
    {
        this.nodes = nodes;
    }

    public override void OnEnter()
    {
        currentIndex = 0;  // 시퀀스 시작 시 첫 번째 노드부터 실행
    }

    public override NodeState Evaluate()
    {
        while (currentIndex < nodes.Count)
        {
            NodeState result = nodes[currentIndex].Evaluate();

            if (result == NodeState.Running)
            {
                return NodeState.Running;  // 현재 노드가 실행 중이면 전체도 Running
            }

            if (result == NodeState.Failure)
            {
                currentIndex = 0;  // 실패하면 처음부터 다시 실행
                return NodeState.Failure;
            }

            // 성공하면 다음 노드로 이동
            currentIndex++;
        }

        currentIndex = 0;  // 모든 노드가 성공하면 초기화
        return NodeState.Success;
    }
}

public class ConditionNode : BTNode
{
    private Func<bool> condition;

    public ConditionNode(Func<bool> condition)
    {
        this.condition = condition;
    }

    public override NodeState Evaluate()
    {
        return condition.Invoke()? NodeState.Success : NodeState.Failure;
    }
}
public class ActionNode : BTNode
{
    private Action action;

    public ActionNode(Action action)
    {
        this.action = action;
    }

    public override NodeState Evaluate()
    {
        action.Invoke();
        return NodeState.Success;
    }
}

public class IntervalDebugNode : BTNode{

    private Action action;

    public IntervalDebugNode(String str)
    {
        this.action = () => Debug.Log($"Test : {str}");
    }

    public override NodeState Evaluate()
    {
        action.Invoke();
        return NodeState.Success;
    }
}

public class LookAtTargetNode : BTNode
{
    private Transform monster;
    private Transform player;
    private Animator animator;
    private float rotationSpeed;
    
    public LookAtTargetNode(Transform monster, Transform player, Animator animator)
    {
        this.monster = monster;
        this.player = player;
        this.animator = animator;
        this.rotationSpeed = 20.0f;
    }

    public override NodeState Evaluate()
    {
        if (player == null) return NodeState.Failure;

        Vector3 direction = (player.position - monster.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        monster.transform.rotation = Quaternion.Slerp(monster.transform.rotation, lookRotation, Time.deltaTime * 10f);
        float angle = Quaternion.Angle(monster.rotation, lookRotation);
        Debug.Log($"현재 몬스터-플레이어 각도: {angle}");
        
        if (angle > 10f)
        {
            return NodeState.Running;
        }

        return NodeState.Success;
    }
}

public class WaitNode : BTNode
{
    private float waitTime;
    private float elapsedTime;
    public WaitNode(float time)
    {
        this.waitTime = time;
    }

    public override void OnEnter()
    {
        elapsedTime = 0f; // 노드 시작 시 초기화
    }

    public override NodeState Evaluate()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= waitTime)
        {
            elapsedTime= 0f;
            return NodeState.Success; // 대기 완료
        }
        // Debug.Log($"Delaying... {elapsedTime:F2}s / {waitTime}s");
        return NodeState.Running; // 아직 대기 중
    }

    public override void OnExit()
    {
        Debug.Log("대기 종료...");
    }
}
