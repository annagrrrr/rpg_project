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

    private void Start()
    {
        var playerInstance = Instantiate(playerPrefab, playerSpawnPoint.position, playerSpawnPoint.rotation);
        var rb = playerInstance.GetComponent<Rigidbody>();
        var groundChecker = playerInstance.GetComponent<IPlayerGroundChecker>();
        var animationPresenter = playerInstance.GetComponent<PlayerAnimatorPresenter>();


        var input = new InputService();
        var repository = new InMemoryPlayerRepository();
        var presenter = new PlayerPresenter(playerInstance.transform);

        var playerStunState = new PlayerStunState();
        var stunPlayerUseCase = new StunPlayerUseCase(playerStunState, animationPresenter);

        var cameraInput = new CameraInputService();
        var cameraPresenter = new CameraPresenter(cameraTransform);
        var cameraSettings = new CameraSettings
        {
            Offset = new Vector3(0, 2, -4),
            Sensitivity = 3f,
            MinPitch = -30f,
            MaxPitch = 60f,
            Distance = 5f,
            CollisionMask = LayerMask.GetMask("Environment", "Obstacles")
        };
        if (bossPrefab != null && bossSpawnPoint != null)
        {
            var bossInstance = Instantiate(bossPrefab, bossSpawnPoint.position, Quaternion.identity);
            bossInstance.Initialize(playerInstance.transform);

            bossInstance.GetComponent<BossController>().enabled = true;
        }

        var followCameraUseCase = new FollowCameraUseCase(
            cameraInput,
            cameraPresenter,
            playerInstance.transform,
            cameraSettings
        );
        cameraController.Initialize(followCameraUseCase);

        var moveUseCase = new MovePlayerUseCase(repository, presenter, cameraPresenter, animationPresenter);
        var rotationPresenter = new PlayerRotationPresenter(playerInstance.transform);

        var inventory = new WeaponInventory();
        var attackPresenter = new AttackPresenter();
        var attackUseCase = new AttackUseCase(inventory, attackPresenter, playerInstance.transform, animationPresenter, cooldownPresenter);

        var pickupProvider = playerInstance.GetComponent<WeaponTriggerPickupProvider>();
        var pickupUseCase = new PickupWeaponUseCase(pickupProvider, inventory);

        var jumpPresenter = new PlayerJumpPresenter(rb);
        var jumpUseCase = new JumpUseCase(jumpPresenter, groundChecker, jumpForce: 6f, animationPresenter);

        var health = new Health(100);
        var healthPresenter = new PlayerHealthPresenter(health, playerHealthView, stunPlayerUseCase, animationPresenter);

        var healthController = playerInstance.GetComponent<PlayerHealthController>();
        healthController.Initialize(healthPresenter);

        playerInstance.Initialize(
            input,
            moveUseCase,
            attackUseCase,
            pickupUseCase,
            jumpUseCase,
            inventory,
            healthPresenter,
            stunPlayerUseCase
        );

        for (int i = 0; i < enemyPrefabs.Length && i < enemySpawnPoints.Length; i++)
        {
            var enemyInstance = Instantiate(enemyPrefabs[i], enemySpawnPoints[i].position, Quaternion.identity);
            var enemyController = enemyInstance.GetComponent<EnemyController>();
            if (enemyController != null)
            {
                enemyController.Initialize(healthController);
            }
        }
        killTracker.OnThreeEnemiesKilled += () =>
        {
            if (bossPrefab != null && bossSpawnPoint != null)
            {
                var bossInstance = Instantiate(bossPrefab, bossSpawnPoint.position, Quaternion.identity, bossContainer.transform);
                bossInstance.Initialize(playerInstance.transform);
            }
        };

        killTracker.OnFiveEnemiesKilled += () =>
        {
            victoryMusicPlayer.PlayVictory();
        };

        
    }
}
