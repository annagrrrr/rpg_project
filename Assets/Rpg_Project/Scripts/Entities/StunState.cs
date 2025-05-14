using UnityEngine;

public class StunState
{
    private float _stunDuration;
    private float _stunEndTime;

    public bool IsStunned => Time.time < _stunEndTime;

    public void ApplyStun(float duration)
    {
        _stunDuration = duration;
        _stunEndTime = Time.time + duration;
    }

    public float Remaining => Mathf.Max(0, _stunEndTime - Time.time);
}
