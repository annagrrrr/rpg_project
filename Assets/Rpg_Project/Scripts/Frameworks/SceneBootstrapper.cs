using UnityEngine;

public class SceneBootstrapper : MonoBehaviour
{
    [SerializeField] private Transform playerSpawnPoint;
    [SerializeField] private PlayerControllerr playerPrefab;
    [SerializeField] private EnemyController[] enemyPrefabs;
    [SerializeField] private Transform[] enemySpawnPoints;
    [SerializeField] private BossController bossPrefab;
    [SerializeField] private Transform bossSpawnPoint;
    [SerializeField] private CameraController cameraController;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private VictoryMusicPlayer victoryMusicPlayer;
    [SerializeField] private GameObject bossContainer;
    [SerializeField] private EnemyKillTracker killTracker;

    [Header("UI")]
    [SerializeField] private PlayerHealthView playerHealthView;
    [SerializeField] private AttackCooldownPresenter cooldownPresenter;
    [SerializeField] private StatisticsUI statisticsUI;


    private IStatisticsRepository statisticsRepo;

    private AddKillUseCase addKill;
    private AddDamageDealtUseCase addDealt;
    private AddDamageReceivedUseCase addReceived;
    private UpdatePlayTimeUseCase updateTime;
    private FinishRunUseCase finishRun;

    private bool gameEnded = false;


    private void Start()
    {
        // --------------------------
        //   INIT STATISTICS
        // --------------------------
        statisticsRepo = new InMemoryStatisticsRepository();
        addKill = new AddKillUseCase(statisticsRepo);
        addDealt = new AddDamageDealtUseCase(statisticsRepo);
        addReceived = new AddDamageReceivedUseCase(statisticsRepo);
        updateTime = new UpdatePlayTimeUseCase(statisticsRepo);
        finishRun = new FinishRunUseCase(statisticsRepo);

        // --------------------------
        //   PLAYER INIT
        // --------------------------
        var player = Instantiate(playerPrefab, playerSpawnPoint.position, playerSpawnPoint.rotation);

        var rb = player.GetComponent<Rigidbody>();
        var groundChecker = player.GetComponent<IPlayerGroundChecker>();
        var animPresenter = player.GetComponent<PlayerAnimatorPresenter>();

        var input = new InputService();
        var repo = new InMemoryPlayerRepository();
        var presenter = new PlayerPresenter(player.transform);

        var stunState = new PlayerStunState();
        var stunUC = new StunPlayerUseCase(stunState, animPresenter);

        var cameraInput = new CameraInputService();
        var camPresenter = new CameraPresenter(cameraTransform);

        var camSettings = new CameraSettings()
        {
            Offset = new Vector3(0, 2, -6),
            Sensitivity = 3f,
            MinPitch = -40,
            MaxPitch = 80,
            Distance = 8,
            CollisionMask = LayerMask.GetMask("Environment", "Obstacles")
        };

        var followUC = new FollowCameraUseCase(cameraInput, camPresenter, player.transform, camSettings);
        cameraController.Initialize(followUC);

        var moveUC = new MovePlayerUseCase(repo, presenter, camPresenter, animPresenter);

        var inventory = new WeaponInventory();
        var attackPresenter = new AttackPresenter();

        var attackUC = new AttackUseCase(
            inventory,
            attackPresenter,
            player.transform,
            animPresenter,
            cooldownPresenter,
            addDealt
        );

        var pickupProvider = player.GetComponent<WeaponTriggerPickupProvider>();
        var pickupUC = new PickupWeaponUseCase(pickupProvider, inventory);

        var jumpPresenter = new PlayerJumpPresenter(rb);
        var jumpUC = new JumpUseCase(jumpPresenter, groundChecker, 6f, animPresenter);

        var health = new Health(100);
        var healthPresenter = new PlayerHealthPresenter(health, playerHealthView, stunUC, animPresenter);

        var healthCtrl = player.GetComponent<PlayerHealthController>();
        healthCtrl.Initialize(healthPresenter);

        // ?????? ?????? ? ??????????
        healthPresenter.OnPlayerDied += () =>
        {
            if (gameEnded) return;
            gameEnded = true;

            addReceived.Execute(9999); // ??????????? ????????
            var stats = finishRun.Execute();

            statisticsUI.Show(stats);
        };

        player.Construct(
            input,
            moveUC,
            attackUC,
            pickupUC,
            jumpUC,
            inventory,
            healthPresenter,
            stunUC
        );

        // --------------------------
        //   ENEMIES
        // --------------------------
        for (int i = 0; i < enemyPrefabs.Length && i < enemySpawnPoints.Length; i++)
        {
            var instance = Instantiate(enemyPrefabs[i], enemySpawnPoints[i].position, Quaternion.identity);
            var ctrl = instance.GetComponent<EnemyController>();
            var hpPresenter = instance.GetComponent<EnemyHealthPresenter>();

            ctrl.Construct(healthCtrl, hpPresenter, killTracker);

            ctrl.OnEnemyKilled += () =>
            {
                addKill.Execute();
            };
        }

        // spawn boss
        killTracker.OnThreeEnemiesKilled += () =>
        {
            var b = Instantiate(bossPrefab, bossSpawnPoint.position, Quaternion.identity, bossContainer.transform);
            b.Initialize(player.transform);
        };

        killTracker.OnFiveEnemiesKilled += () =>
        {
            victoryMusicPlayer.PlayVictory();
        };
    }

    private void Update()
    {
        if (!gameEnded)
            updateTime.Execute(Time.deltaTime);
    }
}
