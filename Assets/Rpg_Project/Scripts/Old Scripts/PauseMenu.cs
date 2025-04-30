
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using Unity.VisualScripting;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuUI;
    public GameObject player;
    private bool isPaused = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void SaveGame()
    {
        PlayerPrefs.SetFloat("PlayerPosX", player.transform.position.x);
        PlayerPrefs.SetFloat("PlayerPosY", player.transform.position.y);
        PlayerPrefs.SetFloat("PlayerPosZ", player.transform.position.z);

        
        var playerController = player.GetComponent<PlayerController>();
        if (playerController != null)
        {
            HealthManager healthManager = playerController.GetHealthManager();
            if (healthManager != null)
            {
                PlayerPrefs.SetInt("PlayerHealth", healthManager.currentHealth);
            }
        }

        PlayerPrefs.Save();
        EnemySaveManager.Instance.SaveEnemies();

        Debug.Log("Игра сохранена: " + player.transform.position);
    }


    public void LoadGame()
    {
        if (PlayerPrefs.HasKey("PlayerPosX") && PlayerPrefs.HasKey("PlayerPosY") && PlayerPrefs.HasKey("PlayerPosZ"))
        {
            float posX = PlayerPrefs.GetFloat("PlayerPosX");
            float posY = PlayerPrefs.GetFloat("PlayerPosY");
            float posZ = PlayerPrefs.GetFloat("PlayerPosZ");

            player.transform.position = new Vector3(posX, posY, posZ);
            Debug.Log("Игрок телепортирован в: " + new Vector3(posX, posY, posZ));
        }

        EnemySaveManager.Instance.LoadEnemies();

        if (PlayerPrefs.HasKey("PlayerHealth"))
        {
            int savedHealth = PlayerPrefs.GetInt("PlayerHealth");
            var playerController = player.GetComponent<PlayerController>();
            if (playerController != null)
            {
                HealthManager healthManager = playerController.GetHealthManager();
                if (healthManager != null)
                {
                    
                    healthManager.SetHealth(savedHealth);
                    Debug.Log("Здоровье игрока восстановлено до: " + savedHealth);
                }
            }
        }
        else
        {
            Debug.Log("Нет сохранённых данных для здоровья.");
        }
    }

    public void SetPeacefulMode()
    {
        Debug.Log("peaceful");
    }
    public void SetNormalMode()
    {
        Debug.Log("normal");
    }
}








