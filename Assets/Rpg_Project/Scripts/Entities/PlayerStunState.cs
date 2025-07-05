using UnityEngine;

public class PlayerStunState
{
    public bool IsStunned { get; private set; }
    private float _stunEndTime;

    public void Stun(float duration)
    {
        IsStunned = true;
        _stunEndTime = Time.time + duration;
    }

    public void Update()
    {
        if (IsStunned && Time.time >= _stunEndTime)
        {
            IsStunned = false;
        }
    }
}
