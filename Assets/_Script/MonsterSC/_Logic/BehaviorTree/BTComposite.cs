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

// public class ActionNode : BTNode
// {
//     private System.Action action;

//     public ActionNode(System.Action action)
//     {
//         this.action = action;
//     }

//     public override NodeState Evaluate()
//     {
//         action.Invoke();
//         return NodeState.Success;
//     }
// }

public class Sequence : BTNode
{
    private List<BTNode> children;
    private int currentIndex = 0; // 현재 실행 중인 노드 인덱스

    public Sequence(List<BTNode> children)
    {
        this.children = children;
    }

    public override void OnEnter()
    {
        currentIndex = 0; // 처음 실행될 때 첫 번째 노드부터 시작
    }

    public override NodeState Evaluate()
    {
        while (currentIndex < children.Count)
        {
            NodeState state = children[currentIndex].Evaluate();

            if (state == NodeState.Running)
            {
                return NodeState.Running; // 현재 노드가 Running이면 다음 노드 실행하지 않음
            }

            if (state == NodeState.Failure)
            {
                return NodeState.Failure; // 실패하면 즉시 실패 반환
            }
            // 성공이면 다음 노드로 이동
            currentIndex++;
        }

        return NodeState.Success; // 모든 노드 성공 시 Success 반환
    }

    public override void OnExit()
    {
        currentIndex = 0; // 시퀀스가 끝나면 다시 초기화
    }
}

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

public class ActionNode : BTNode
{
    private System.Func<NodeState> action;

    public ActionNode(System.Func<NodeState> action)
    {
        this.action = action;
    }

    public override NodeState Evaluate()
    {
        return action();
    }
}

public class LookAtTargetNode : BTNode
{
    private Transform monster;
    private Transform player;
    private Animator animator;
    private float rotationSpeed;
    
    public LookAtTargetNode(Transform monster, Transform player, Animator animator, float rotationSpeed)
    {
        this.monster = monster;
        this.player = player;
        this.animator = animator;
        this.rotationSpeed = rotationSpeed;
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

public class AttackTargetNode : BTNode
{
    Animator animator;
    public AttackTargetNode(Animator animator)
    {
        this.animator = animator;
    }

    public override NodeState Evaluate()
    {
        if (animator == null) return NodeState.Failure;
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            return NodeState.Running;
        }
        animator.SetTrigger("Attack");

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
            Debug.Log("대기 완료");
            return NodeState.Success; // 대기 완료
        }
        Debug.Log("대기중...");
        return NodeState.Running; // 아직 대기 중
    }
}
