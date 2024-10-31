using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerText;
    private float time; // Time in seconds

    public GameObject[] collectiblePrefabs; // Assign your prefab in the Inspector
    public GameObject bombPrefab;

    public int score = 0;
    public int scoreLimit;
    public int baseScoreLimit = 1000;
    private bool isGameActive = true; // Track if the game is active
    private bool isPaused = false; // Track if the game is paused
    private bool mainMenuSetting = true;

    public float spawnInterval; // Time interval for spawning collectibles
    public float spawnRangeMinX; // Adjust based on your game view width
    public float spawnRangeMaxX;
    public float spawnRangeMinY; // Adjust based on your game view height
    public float spawnRangeMaxY;
    public float baseFallSpeed = 1.0f;



    public GameObject gamePanel;
    public GameObject winPanel;
    public GameObject losePanel;
    public GameObject settingsPanel;
    public GameObject mainMenuPanel;

    public Button settingButton;

    public SoundManager soundManager;
    public LevelPanel levelPanel;
    

    public TextMeshProUGUI winPanelLevelText;
    public TextMeshProUGUI losePanelLvlText;

    public AudioSource winSound;
    public AudioSource loseSound;
    public AudioSource collectSound;


    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        int currentLvl = PlayerPrefs.GetInt("SelectedLevel", 0);
        scoreLimit = baseScoreLimit + (currentLvl * 50);
        Debug.Log("ScoreLinit :" + scoreLimit);

        //PlayerPrefs.DeleteAll();
        ResetScore();
        InvokeRepeating("SpawnCollectible", 0f, spawnInterval);

        
        
    }

    void Update()
    {
        if (isGameActive && !isPaused)
        {
            time += Time.deltaTime;
            timerText.text = "Time: " + Mathf.FloorToInt(time).ToString();
        }
    }

    public void AddScore(int points)
    {
        collectSound.Play();
        score += points;
        scoreText.text = "SCORE:\n" + score + "/" + baseScoreLimit;

        // Check if the score reaches 10
        if (score >= scoreLimit)
        {
            LevelCompleted(); // Call the level completion method
        }
    }

    private void LevelCompleted()
    {
        winSound.Play();
        ResetScore();
        ResetTimer();

        Debug.Log("Level Completed!");
        isGameActive = false; // Stop the game
        winPanel.SetActive(true); // Show the win panel

        // Get the current level and calculate the next level
        int currentLvl = PlayerPrefs.GetInt("SelectedLevel", 0);
        int nextLevel = currentLvl + 1; // Increment level by 1 for the next level

        // Update the win panel text to show the current level (level just completed)
        winPanelLevelText.text = "LEVEL " + currentLvl.ToString();

        // Update the SelectedLevel in PlayerPrefs to the next level
        PlayerPrefs.SetInt("SelectedLevel", nextLevel);

        // Update CurrentLevel if the next level is higher
        if (nextLevel > PlayerPrefs.GetInt("CurrentLevel", 0))
        {
            PlayerPrefs.SetInt("CurrentLevel", nextLevel); // Update the current level in PlayerPrefs
            PlayerPrefs.Save();
        }

        Debug.Log("Next level = " + nextLevel);
        levelPanel.UpdateLevelButtons();
        

    }

    public void NextLevelButton()
    {
        winPanel.SetActive(false);
        ResetScore();
        isGameActive = true;
    }

    public void ResetScore()
    {
        score = 0; // Reset score to 0 for a fresh start
        scoreText.text = "SCORE:\n" + score + "/" + scoreLimit;
    }

    private void ResetTimer()
    {
        time = 0f;
        timerText.text = "Time: 0";
    }

    private void SpawnCollectible()
    {
        if (!isGameActive || !gamePanel.activeSelf) return; // Stop spawning if the game is over or gamePanel is not active

        Debug.Log(GetRandomSpawnPosition());
        GameObject collectible;

        // Randomly choose to spawn a collectible or a bomb
        if (Random.Range(0, 10) < 8) // 80% chance to spawn a collectible
        {
            collectible = Instantiate(collectiblePrefabs[Random.Range(0, collectiblePrefabs.Length)],
                                       GetRandomSpawnPosition(), Quaternion.identity);
        }
        else // 20% chance to spawn a bomb
        {
            collectible = Instantiate(bombPrefab, GetRandomSpawnPosition(), Quaternion.identity);
        }

        // Set the parent to GamePanel if the game is active
        if (gamePanel.activeSelf)
        {
            collectible.transform.SetParent(GameObject.Find("GamePanel").transform, false);
        }

        
    }

    private Vector2 GetRandomSpawnPosition()
    {
        return new Vector2(Random.Range(spawnRangeMinX, spawnRangeMaxX), Random.Range(spawnRangeMinY, spawnRangeMaxY));
    }

    public void EndGame()
    {
        ResetScore();
        ResetTimer();
        int currentLvl = PlayerPrefs.GetInt("SelectedLevel", 0);
        losePanelLvlText.text = "LEVEL " + currentLvl.ToString();
        // Show game over screen, disable player movement, etc.
        Debug.Log("Game Over!");
        isGameActive = false; // Set game state to inactive
        //gamePanel.SetActive(false);
        losePanel.SetActive(true);
        loseSound.Play();
        
    }
    

    public void GameSettingButton()
    {
        isPaused = !isPaused; // Toggle the pause state
        

        if (isPaused)
        {
            mainMenuSetting = false;
            // Pause the game
            Time.timeScale = 0; // Freeze time
            soundManager.LoadSettings();
            settingsPanel.SetActive(true); // Show settings panel
            settingButton.gameObject.SetActive(false); // Hide settings button
        }
        else
        {
            // Resume the game
            Time.timeScale = 1; // Resume time
            settingsPanel.SetActive(false); // Hide settings panel
            settingButton.gameObject.SetActive(true); // Show settings button again
        }
    }


    public void Restartbutton()
    {
        losePanel.SetActive(false);
        ResetScore();
        isGameActive = true;
    }


    


    public void SettingBackButton()
    {
        if (mainMenuSetting) 
        { 
            settingsPanel.SetActive(false);
            mainMenuPanel.SetActive(true);
        }
        else
        {
            GameSettingButton();
        }
    }

    public void SaveSettingsButton()
    {
        if (mainMenuSetting)
        {
            soundManager.SaveSettings();
            settingsPanel.SetActive(false);
            mainMenuPanel.SetActive(true);
        }
        else
        {
            soundManager.SaveSettings();
            GameSettingButton();
        }
    }

    public void MainMenuSettingButtonPressed ()
    {
        mainMenuSetting=true;
        mainMenuPanel.SetActive(false );
        settingsPanel.SetActive(true);
    }


    public bool IsGameActive
    {
        get { return isGameActive; }
        set { isGameActive = value; }
    }
    
    public void MainMenuButton()
    {
        isGameActive=false;
        mainMenuPanel.SetActive(true);
        gamePanel.SetActive(false);
        ResetScore() ;
        ResetTimer();
    }


    public void QuitGame()
    {
        Application.Quit();
    }
}