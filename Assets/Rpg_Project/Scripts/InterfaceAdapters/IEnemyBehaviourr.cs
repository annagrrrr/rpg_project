using UnityEngine;

public interface IEnemyBehaviourr
{
    void Tick(Vector3 enemyPosition, Transform playerTransform);
}