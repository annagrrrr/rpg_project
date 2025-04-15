using UnityEngine;

public interface IEnemyContext
{
    Transform EnemyTransform { get; }
    Transform PlayerTransform { get; }
    void SetBehaviour(IEnemyBehaviourr behaviour);
}