public class Player
{
    public float Speed { get; private set; }
    public float JumpForce { get; private set; }

    public float SprintMultiplier { get; private set; }
    public Player(float speed, float jumpForce = 5f, float sprintMultiplier = 1.5f)
    {
        Speed = speed;
        JumpForce = jumpForce;
        SprintMultiplier = sprintMultiplier;
    }
}