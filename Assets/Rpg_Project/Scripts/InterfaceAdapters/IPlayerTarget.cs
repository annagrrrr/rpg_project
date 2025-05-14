using UnityEngine;

public interface IPlayerTarget
{
    Transform Transform { get; }
    void ReceiveDamage(int amount);
}