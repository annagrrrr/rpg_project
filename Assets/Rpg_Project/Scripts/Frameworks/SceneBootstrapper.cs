using UnityEngine;

public class SceneBootstrapper : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField] private DynamicJoystick mobileJoystick;
    [SerializeField] private MobileButton jumpButton;
    [SerializeField] private MobileButton attackButton;
    [SerializeField] private MobileButton magicButton;
    [SerializeField] private MobileButton pickupButton;
    [SerializeField] private MobileButton sprintButton;
    [SerializeField] private Transform playerSpawnPoint;
    [SerializeField] private PlayerControllerr playerPrefab;

    [Header("Enemy Settings")]
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private Transform[] enemySpawnPoints;

    [Header("Boss Settings")]
    [SerializeField] private GameObject bossPrefab;
    [SerializeField] private Transform bossSpawnPoint;
    [SerializeField] private GameObject bossContainer;

    [Header("UI References")]
    [SerializeField] private CameraController cameraController;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private VictoryMusicPlayer victoryMusicPlayer;
    [SerializeField] private EnemyKillTracker killTracker;
    [SerializeField] private PlayerHealthView playerHealthView;
    [SerializeField] private AttackCooldownPresenter cooldownPresenter;

    // Services
    private GameStatsService _statsService;
    private EndGameUseCase _endGameUseCase;
    private bool _bossSpawned = false;
    private bool _gameEnded = false;

    private void Start()
    {
        InitializeGame();
    }

    private void InitializeGame()
    {
        var statsRepository = new PlayerPrefsStatsRepository();
        _statsService = new GameStatsService(statsRepository);
        _statsService.StartNewSession();

        var sceneLoader = new SceneLoader();
        _endGameUseCase = new EndGameUseCase(_statsService, sceneLoader);

        var playerInstance = Instantiate(playerPrefab, playerSpawnPoint.position, playerSpawnPoint.rotation);
        var rb = playerInstance.GetComponent<Rigidbody>();
        var groundChecker = playerInstance.GetComponent<IPlayerGroundChecker>();
        var animationPresenter = playerInstance.GetComponent<PlayerAnimatorPresenter>();

        IInputService inputService = CreateInputService();

        InitializeCamera(playerInstance.transform);

        InitializePlayerSystems(playerInstance, inputService, rb, groundChecker, animationPresenter);

        SpawnEnemies(playerInstance);

        InitializeGameEvents();

        Debug.Log("Game initialized successfully!");
    }

    private IInputService CreateInputService()
    {
#if UNITY_ANDROID || UNITY_IOS
        if (mobileJoystick != null && jumpButton != null && attackButton != null && 
            magicButton != null && pickupButton != null && sprintButton != null)
        {
            return new MobileInputService(
                mobileJoystick, 
                jumpButton, 
                attackButton, 
                magicButton,
                pickupButton,
                sprintButton
            );
        }
        else
        {
            Debug.LogError("Mobile controls not fully assigned!");
            return new InputService();
        }
#else
        return new InputService();
#endif
    }

    private void InitializeCamera(Transform playerTransform)
    {
        if (cameraController == null || cameraTransform == null) return;

        ICameraInputService cameraInput;
#if UNITY_ANDROID || UNITY_IOS
        cameraInput = new MobileCameraInputService();
#else
        cameraInput = new CameraInputService();
#endif

        var cameraPresenter = new CameraPresenter(cameraTransform);
        var cameraSettings = new CameraSettings
        {
            Offset = new Vector3(0, 2, -6),
            Sensitivity = 3f,
            MinPitch = -40f,
            MaxPitch = 80f,
            Distance = 8f,
            CollisionMask = LayerMask.GetMask("Environment", "Obstacles")
        };

        var followCameraUseCase = new FollowCameraUseCase(
            cameraInput,
            cameraPresenter,
            playerTransform,
            cameraSettings
        );

        cameraController.Initialize(followCameraUseCase);
    }

    private void InitializePlayerSystems(PlayerControllerr playerInstance, IInputService inputService,
                                       Rigidbody rb, IPlayerGroundChecker groundChecker,
                                       PlayerAnimatorPresenter animationPresenter)
    {
        var repository = new InMemoryPlayerRepository();
        var presenter = new PlayerPresenter(playerInstance.transform);

        var playerStunState = new PlayerStunState();
        var stunPlayerUseCase = new StunPlayerUseCase(playerStunState, animationPresenter);

        var cameraPresenter = new CameraPresenter(cameraTransform);
        var moveUseCase = new MovePlayerUseCase(repository, presenter, cameraPresenter, animationPresenter);
        var rotationPresenter = new PlayerRotationPresenter(playerInstance.transform);

        var inventory = new WeaponInventory();
        var attackPresenter = new AttackPresenter();
        var attackUseCase = new AttackUseCase(
            inventory,
            attackPresenter,
            playerInstance.transform,
            animationPresenter,
            cooldownPresenter,
            _statsService
        );

        var pickupProvider = playerInstance.GetComponent<WeaponTriggerPickupProvider>();
        var pickupUseCase = new PickupWeaponUseCase(pickupProvider, inventory);

        var jumpPresenter = new PlayerJumpPresenter(rb);
        var jumpUseCase = new JumpUseCase(jumpPresenter, groundChecker, 6f, animationPresenter);

        var health = new Health(1000);
        var healthPresenter = new PlayerHealthPresenter(
            health,
            playerHealthView,
            stunPlayerUseCase,
            animationPresenter,
            _statsService,
            _endGameUseCase
        );

        var healthController = playerInstance.GetComponent<PlayerHealthController>();
        if (healthController != null)
        {
            healthController.Initialize(healthPresenter);
        }

        playerInstance.Initialize(
            inputService,
            moveUseCase,
            attackUseCase,
            pickupUseCase,
            jumpUseCase,
            inventory,
            healthPresenter,
            stunPlayerUseCase
        );
    }

    private void SpawnEnemies(PlayerControllerr playerInstance)
    {
        if (enemyPrefabs == null || enemySpawnPoints == null)
        {
            Debug.LogWarning("Enemy prefabs or spawn points not assigned");
            return;
        }

        var healthController = playerInstance.GetComponent<PlayerHealthController>();

        for (int i = 0; i < enemyPrefabs.Length && i < enemySpawnPoints.Length; i++)
        {
            if (enemyPrefabs[i] != null && enemySpawnPoints[i] != null)
            {
                var enemyInstance = Instantiate(enemyPrefabs[i], enemySpawnPoints[i].position, Quaternion.identity);
                var enemyController = enemyInstance.GetComponent<EnemyController>();
                if (enemyController != null && healthController != null)
                {
                    enemyController.Initialize(healthController, killTracker);
                }
            }
        }
    }

    private void InitializeGameEvents()
    {
        if (killTracker == null)
        {
            Debug.LogWarning("KillTracker not assigned in SceneBootstrapper");
            return;
        }

        killTracker.OnEnemyKilled += () =>
        {
            _statsService.RecordEnemyKill();
        };

        killTracker.OnThreeEnemiesKilled += () =>
        {
            if (_bossSpawned || _gameEnded) return;

            Debug.Log("3 enemies killed! Spawning boss...");
            SpawnBoss();
            _bossSpawned = true;
        };

        killTracker.OnFiveEnemiesKilled += () =>
        {
            if (_gameEnded) return;

            Debug.Log("5 enemies killed! Victory!");
            EndGame(true);
        };
    }

    private void SpawnBoss()
    {
        if (bossPrefab == null || bossSpawnPoint == null)
        {
            Debug.LogWarning("Boss references not assigned!");
            return;
        }

        var bossInstance = Instantiate(bossPrefab, bossSpawnPoint.position,
            Quaternion.identity);

        if (bossContainer != null)
        {
            bossInstance.transform.SetParent(bossContainer.transform);
        }

        Debug.Log($"Boss spawned at {bossSpawnPoint.position}");

        var bossController = bossInstance.GetComponent<BossController>();
        if (bossController != null)
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                bossController.Initialize(player.transform);

                bossController.OnBossDied += () =>
                {
                    Debug.Log("Boss killed! Victory!");
                    EndGame(true);
                };
            }
        }

        Debug.Log("Boss spawned successfully!");
    }

    private void EndGame(bool isVictory)
    {
        if (_gameEnded) return;

        _gameEnded = true;

        if (isVictory && victoryMusicPlayer != null)
        {
            victoryMusicPlayer.PlayVictory();
        }

        Invoke(nameof(CompleteGame), 2f);
    }

    private void CompleteGame()
    {
        _endGameUseCase.Execute(true);
    }
}