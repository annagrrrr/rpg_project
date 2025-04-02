using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager { get; private set; }
    [SerializeField] private HealthBar playerHealthBar;
    public PlayerController player;

    private void Awake()
    {
        if (gameManager == null)
        {
            gameManager = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        FindNewPlayer();
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindNewPlayer();
    }

    private void FindNewPlayer()
    {
        player = FindFirstObjectByType<PlayerController>();

        if (playerHealthBar == null)
        {
            playerHealthBar = FindFirstObjectByType<HealthBar>();

            if (playerHealthBar == null)
            {
                GameObject healthBarObject = new GameObject("HealthBar");
                playerHealthBar = healthBarObject.AddComponent<HealthBar>();
            }
        }

        InitializePlayer();
    }

    public void InitializePlayer()
    {
        if (player == null || playerHealthBar == null)
        {
            return;
        }

        HealthManager playerHealth = new HealthManager(100);
        DamageHandler playerDamageHandler = new DamageHandler();
        player.Initialize(playerHealth, playerDamageHandler, playerHealthBar);
    }
}
