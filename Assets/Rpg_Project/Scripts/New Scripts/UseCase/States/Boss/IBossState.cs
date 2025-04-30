public interface IBossState
{
    void Enter(BossController boss);
    void Execute(BossController boss);
    void Exit(BossController boss);
}
