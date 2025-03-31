using UnityEngine;

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
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        InitializePlayer();
    }

    public void InitializePlayer()
    {
        if (player == null)
        {
            return;
        }

        if (playerHealthBar == null)
        {
            return;
        }

        HealthManager playerHealth = new HealthManager(100);
        DamageHandler playerDamageHandler = new DamageHandler();

        player.Initialize(playerHealth, playerDamageHandler, playerHealthBar);
    }

}
