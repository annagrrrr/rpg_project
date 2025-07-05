using UnityEngine;
using System;

public class EnemyAttackUseCase
{
    private float _cooldown;
    private float _timer;

    public EnemyAttackUseCase(float cooldown)
    {
        _cooldown = cooldown;
        _timer = 0f;
    }

    public void TryAttack(Action onAttack)
    {
        _timer -= Time.deltaTime;

        if (_timer <= 0f)
        {
            onAttack?.Invoke();
            _timer = _cooldown;
        }
    }
}
