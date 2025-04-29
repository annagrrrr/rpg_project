using UnityEngine;

public class SceneBootstrapper : MonoBehaviour
{
    [SerializeField] private PlayerControllerr playerPrefab;
    [SerializeField] private EnemyController[] enemyPrefabs;
    [SerializeField] private Transform[] enemySpawnPoints;

    [SerializeField] private CameraController cameraController;
    [SerializeField] private Transform cameraTransform;

    private void Start()
    {
        var playerInstance = Instantiate(playerPrefab);
        var rb = playerInstance.GetComponent<Rigidbody>();
        var groundChecker = playerInstance.GetComponent<IPlayerGroundChecker>();

        var input = new InputService();
        var repository = new InMemoryPlayerRepository();
        var presenter = new PlayerPresenter(playerInstance.transform);

        var cameraInput = new CameraInputService();
        var cameraPresenter = new CameraPresenter(cameraTransform);
        var cameraSettings = new CameraSettings
        {
            Offset = new Vector3(0, 2, -4),
            Sensitivity = 3f,
            MinPitch = -30f,
            MaxPitch = 60f,
            Distance = 5f
        };

        var followCameraUseCase = new FollowCameraUseCase(
            cameraInput,
            cameraPresenter,
            playerInstance.transform,
            cameraSettings
        );
        cameraController.Initialize(followCameraUseCase);

        var moveUseCase = new MovePlayerUseCase(repository, presenter, cameraPresenter);
        var rotationPresenter = new PlayerRotationPresenter(playerInstance.transform);

        var inventory = new WeaponInventory();
        //var sword = new MeleeWeapon(20);
        //var staff = new RangedWeapon(15);
        //inventory.EquipRightHand(sword);
        //inventory.EquipLeftHand(staff);

        var attackPresenter = new AttackPresenter();
        var attackUseCase = new AttackUseCase(inventory, attackPresenter, playerInstance.transform);

        var pickupProvider = playerInstance.GetComponent<WeaponTriggerPickupProvider>();
        var pickupUseCase = new PickupWeaponUseCase(pickupProvider, inventory);

        var jumpPresenter = new PlayerJumpPresenter(rb);
        var jumpUseCase = new JumpUseCase(jumpPresenter, groundChecker, jumpForce: 6f);
        var health = new Health(100);
        var healthPresenter = new PlayerHealthPresenter(health);

        var healthController = playerInstance.GetComponent<PlayerHealthController>();
        healthController.Initialize(healthPresenter);

        playerInstance.Initialize(
            input,
            moveUseCase,
            attackUseCase,
            pickupUseCase,
            jumpUseCase,
            inventory,
            healthPresenter
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
    }
}
