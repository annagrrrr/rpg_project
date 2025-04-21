public class Health
{
    public int Current { get; private set; }
    public int Max { get; }

    public Health(int max)
    {
        Max = max;
        Current = max;
    }

    public void TakeDamage(int amount)
    {
        Current = System.Math.Max(0, Current - amount);
    }

    public bool IsDead => Current <= 0;
}
