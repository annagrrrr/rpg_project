using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager { get; private set; }

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

        player.Initialize(new HealthManager(100), new DamageHandler());
    }
}
