using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelPanel : MonoBehaviour
{
    public GameObject levelButtonPrefab; // Reference to the level button prefab
    public Transform levelContainer; // Parent object where buttons will be instantiated
    public Button playButton; // Button to start the selected level
    public Button nextPageButton; // Button to go to the next page
    public Button previousPageButton; // Button to go to the previous page
    public int levelsPerPage = 9; // Number of levels per page
    private int currentLevel = 0; // Track the current level
    private int selectedLevel = -1; // Track the selected level (-1 means no level selected)
    private int currentPage = 0; // Track the current page of levels


    


    public GameObject levelPanel;
    public GameObject playPanel;
    public GameObject winPanel;
    public GameObject losePanel;

    private void Start()
    {
        LoadLevelProgress(); // Load level progress when starting
        GenerateInitialLevels(); // Generate levels 1-9 on start

        // Ensure the Play button starts as inactive
        playButton.interactable = false;
        UpdateNavigationButtons(); // Update navigation buttons on start
    }


    private void GenerateInitialLevels()
    {
        int maxLvl =  PlayerPrefs.GetInt("CurrentLevel", 0);
  
        Debug.Log($"Current lvl : {PlayerPrefs.GetInt("CurrentLevel", 0)} ");
        // Instantiate level buttons for the first 9 levels
        for (int i = 0; i < levelsPerPage; i++)
        {
            int levelIndex = i + 1; // Levels are 1-based
            GameObject levelButton = Instantiate(levelButtonPrefab, levelContainer);
            levelButton.GetComponentInChildren<Text>().text = levelIndex.ToString(); // Set button text
            int index = levelIndex; // Capture the level index for the button event
            levelButton.GetComponent<Button>().onClick.AddListener(() => SelectLevel(index)); // Add listener

            // Enable only the first level button, others are disabled initially

            levelButton.GetComponent<Button>().interactable = (i <= maxLvl);
        }

        currentLevel = levelsPerPage; // Start at level 1
    }

    public void CompleteLevel()
    {
        if (currentLevel < levelsPerPage - 1) // If not the last level on this page
        {
            currentLevel++; // Unlock the next level
            SaveLevelProgress(); // Save progress
            UpdateLevelButtons(); // Update the buttons to reflect the new state
        }
        else
        {
            // When the last level is completed, generate new levels
            GenerateNewLevels();
        }
    }

    private void GenerateNewLevels()
    {
        int startingLevel = currentLevel + 1; // Get the starting level number


        for (int i = 0; i < levelsPerPage; i++)
        {
            int levelIndex = startingLevel + i;
            GameObject levelButton = Instantiate(levelButtonPrefab, levelContainer);
            levelButton.GetComponentInChildren<TextMeshProUGUI>().text = "Level " + levelIndex; // Set button text
            int index = levelIndex; // Capture the level index for the button event
            levelButton.GetComponent<Button>().onClick.AddListener(() => SelectLevel(index)); // Add listener

            // Set the button interactable state
            if (levelIndex <= PlayerPrefs.GetInt("CurrentLevel", 0) + 1) // Check if this level should be enabled
            {
                levelButton.GetComponent<Button>().interactable = true; // Enable new buttons if they are unlocked
            }
            else
            {
                levelButton.GetComponent<Button>().interactable = false; // Disable new buttons initially
            }

        }

        currentLevel += levelsPerPage; // Update current level to reflect the new maximum level
        SaveLevelProgress(); // Save the updated level progress
        UpdateNavigationButtons(); // Update navigation buttons
    }

    private void SaveLevelProgress()
    {
        PlayerPrefs.SetInt("CurrentLevel", currentLevel); // Save the current level
        PlayerPrefs.Save();
    }

    private void LoadLevelProgress()
    {
        currentLevel = PlayerPrefs.GetInt("CurrentLevel", 0); // Load the last completed level
    }

    public void UpdateLevelButtons()
    {
        currentLevel = PlayerPrefs.GetInt("CurrentLevel", 0);
        // Clear existing buttons
        var count = levelContainer.childCount;
        List<Transform> children  = new List<Transform>();
        for(int i = 0; i < count;i++) children.Add(levelContainer.GetChild(i).transform);
        foreach (Transform child in children)
        {
            Destroy(child.gameObject);
        }

        // Instantiate buttons for the current page
        int startLevelIndex = currentPage * levelsPerPage;
        for (int i = 0; i < levelsPerPage; i++)
        {
            int levelIndex = startLevelIndex + i +1; // Levels are 1-based
            GameObject levelButton = Instantiate(levelButtonPrefab, levelContainer);
            levelButton.GetComponentInChildren<Text>().text =levelIndex.ToString(); // Set button text

            // Enable or disable the button based on the current level
            if (levelIndex <= currentLevel) // Check if this level is unlocked
            {
                levelButton.GetComponent<Button>().onClick.AddListener(() => SelectLevel(levelIndex)); // Add listener
                levelButton.GetComponent<Button>().interactable = true; // Enable unlocked levels
            }
            else
            {
                levelButton.GetComponent<Button>().interactable = false; // Disable locked level
            }
        }

        UpdateNavigationButtons(); // Update navigation buttons based on current page
    }


    public void UpdateNavigationButtons()
    {
         currentLevel = PlayerPrefs.GetInt("CurrentLevel", 0);
        // Update the navigation buttons based on the current page
        previousPageButton.interactable = currentPage > 0; // Disable if on the first page
        nextPageButton.interactable = (currentPage + 1) * levelsPerPage < currentLevel; // Disable if no more pages
        Debug.Log($"current page : {currentPage}");
        Debug.Log($"current lvl: {currentLevel}");
    }

    private void SelectLevel(int levelIndex)
    {
        PlayerPrefs.SetInt("SelectedLevel", levelIndex);
        selectedLevel = levelIndex; // Store the selected level

        // Enable the Play button if a level is selected
        playButton.interactable = true;
    }

    public void StartLevel()
    {
        if (selectedLevel != -1) // Check if a level is selected
        {
            
            levelPanel.SetActive(false);
            playPanel.SetActive(true);
            winPanel.SetActive(false);
            losePanel.SetActive(false);
            PlayerManager.Instance.IsGameActive = true;

            Debug.Log("Starting Level " + selectedLevel);
            // Load the level scene here (e.g., SceneManager.LoadScene("Level" + selectedLevel));
        }
        else
        {
            Debug.Log("No level selected!");
        }
    }

    public void NextPage()
    {
        currentPage++; // Move to the next page
        UpdateLevelButtons(); // Update the level buttons to reflect the new page
    }

    public void PreviousPage()
    {
        currentPage--; // Move to the previous page
        UpdateLevelButtons(); // Update the level buttons to reflect the new page
    }

   
}