using UnityEngine;

public class SceneBootstrapper : MonoBehaviour
{
    [Header("Mobile Input")]
    [SerializeField] private DynamicJoystick mobileJoystick;
    [SerializeField] private MobileButton jumpButton;
    [SerializeField] private MobileButton attackButton;
    [SerializeField] private MobileButton magicButton;
    [SerializeField] private MobileButton pickupButton;
    [SerializeField] private MobileButton sprintButton;

    [Header("Player Settings")]
    [SerializeField] private Transform playerSpawnPoint;
    [SerializeField] private PlayerControllerr playerPrefab;

    [Header("Enemies")]
    [SerializeField] private EnemyController[] enemyPrefabs;
    [SerializeField] private Transform[] enemySpawnPoints;
    [SerializeField] private BossController bossPrefab;
    [SerializeField] private Transform bossSpawnPoint;

    [Header("Camera")]
    [SerializeField] private CameraController cameraController;
    [SerializeField] private Transform cameraTransform;

    [Header("Game Flow")]
    [SerializeField] private VictoryMusicPlayer victoryMusicPlayer;
    [SerializeField] private GameObject bossContainer;
    [SerializeField] private EnemyKillTracker killTracker;

    [Header("UI")]
    [SerializeField] private PlayerHealthView playerHealthView;
    [SerializeField] private AttackCooldownPresenter cooldownPresenter;

    [Header("Stats")]
    [SerializeField] private GameStatsView statsViewPrefab;

    private GameStatsService _statsService;
    private EndGameUseCase _endGameUseCase;

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

        InitializePlayerSystems(playerInstance, inputService, rb, groundChecker, animationPresenter);
        InitializeEnemies(playerInstance);
        InitializeGameEvents();
    }

    private IInputService CreateInputService()
    {
#if UNITY_ANDROID || UNITY_IOS
        if (mobileJoystick != null && jumpButton != null && attackButton != null && 
            magicButton != null && pickupButton != null && sprintButton != null)
        {
            Debug.Log("🎮 Creating MobileInputService with ALL 6 controls");
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
            Debug.LogError("❌ Mobile controls not fully assigned!");
            if (mobileJoystick == null) Debug.LogError("   - Mobile Joystick");
            if (jumpButton == null) Debug.LogError("   - Jump Button");
            if (attackButton == null) Debug.LogError("   - Attack Button");
            if (magicButton == null) Debug.LogError("   - Magic Button");
            if (pickupButton == null) Debug.LogError("   - Pickup Button");
            if (sprintButton == null) Debug.LogError("   - Sprint Button");
            Debug.LogError("Using PC input as fallback");
            return new InputService();
        }
#else
        Debug.Log("🖥️ Creating PC InputService");
        return new InputService();
#endif
    }

    private void InitializePlayerSystems(PlayerControllerr playerInstance, IInputService inputService,
                                       Rigidbody rb, IPlayerGroundChecker groundChecker,
                                       PlayerAnimatorPresenter animationPresenter)
    {
        var repository = new InMemoryPlayerRepository();
        var presenter = new PlayerPresenter(playerInstance.transform);

        var playerStunState = new PlayerStunState();
        var stunPlayerUseCase = new StunPlayerUseCase(playerStunState, animationPresenter);

        var cameraInput = new CameraInputService();
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
            playerInstance.transform,
            cameraSettings
        );

        if (cameraController != null)
        {
            cameraController.Initialize(followCameraUseCase);
        }
        else
        {
            Debug.LogWarning("CameraController not assigned in SceneBootstrapper");
        }

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
        var jumpUseCase = new JumpUseCase(jumpPresenter, groundChecker, jumpForce: 6f, animationPresenter);

        var health = new Health(500);

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

        SetupPickupButton(playerInstance);

        Debug.Log("✅ Player systems initialized successfully");
        Debug.Log("🎮 Mobile controls: Joystick + 5 buttons (Jump, Attack, Magic, Pickup, Sprint)");
    }

    private void SetupPickupButton(PlayerControllerr playerInstance)
    {
        var pickupProvider = playerInstance.GetComponent<WeaponTriggerPickupProvider>();

        if (pickupButton != null && pickupProvider != null)
        {
            var pickupButtonController = gameObject.AddComponent<SimplePickupButtonController>();
            pickupButtonController.Initialize(pickupButton, pickupProvider);

            Debug.Log("✅ Pickup button controller initialized");
        }
    }

    private void InitializeEnemies(PlayerControllerr playerInstance)
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

        Debug.Log($"✅ Spawned {Mathf.Min(enemyPrefabs.Length, enemySpawnPoints.Length)} enemies");
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
            if (bossPrefab != null && bossSpawnPoint != null && bossContainer != null)
            {
                var bossInstance = Instantiate(bossPrefab, bossSpawnPoint.position, Quaternion.identity, bossContainer.transform);
                bossInstance.Initialize(playerPrefab.transform);
                Debug.Log("🎯 Boss spawned!");
            }
        };

        killTracker.OnFiveEnemiesKilled += () =>
        {
            if (victoryMusicPlayer != null)
            {
                victoryMusicPlayer.PlayVictory();
            }
            _endGameUseCase.Execute(true);
            Debug.Log("🎉 Victory! Game completed!");
        };

        Debug.Log("✅ Game events initialized");
    }
}

public class SimplePickupButtonController : MonoBehaviour
{
    private MobileButton _pickupButton;
    private WeaponTriggerPickupProvider _pickupProvider;
    private UnityEngine.UI.Image _buttonImage;

    public void Initialize(MobileButton pickupButton, WeaponTriggerPickupProvider pickupProvider)
    {
        _pickupButton = pickupButton;
        _pickupProvider = pickupProvider;
        _buttonImage = pickupButton.GetComponent<UnityEngine.UI.Image>();

        if (_buttonImage != null)
        {
            _buttonImage.color = new Color(1, 1, 1, 0.4f);
        }
    }

    private void Update()
    {
        if (_pickupButton == null || _pickupProvider == null)
            return;

        bool hasWeaponNearby = CheckForWeaponNearby();

        UpdateButtonState(hasWeaponNearby);
    }

    private bool CheckForWeaponNearby()
    {
        var field = typeof(WeaponTriggerPickupProvider).GetField("_currentWeaponPickup",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        if (field != null)
        {
            var weaponPickup = (WeaponPickup)field.GetValue(_pickupProvider);
            return weaponPickup != null;
        }

        return true;
    }

    private void UpdateButtonState(bool isActive)
    {
        if (_buttonImage != null)
        {
            _buttonImage.color = isActive ? Color.white : new Color(1, 1, 1, 0.4f);
        }

        var unityButton = _pickupButton.GetComponent<UnityEngine.UI.Button>();
        if (unityButton != null)
        {
            unityButton.interactable = isActive;
        }
    }
}