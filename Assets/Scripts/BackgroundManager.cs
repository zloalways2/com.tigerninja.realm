using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BackgroundManager : MonoBehaviour
{
    public Sprite[] choseBackgrounds; // Array to hold background choices
    public Sprite[] choseThumbnailImages; // Array to hold thumbnail images
    public Image backgroundImage; // Reference to the UI Image component displaying the background
    public Image thumbnailImage; // Reference to the UI Image component displaying the chosen thumbnail
    public Button leftButton; // Reference to the left button
    public Button rightButton; // Reference to the right button

    private int currentIndex = 0; // Current index of the background

    private void Start()
    {
        // Set the initial background image
        UpdateBackground();
        UpdateButtonStates(); // Update button states on start
    }

    public void NextBackground()
    {
        currentIndex++;
        if (currentIndex >= choseBackgrounds.Length)
        {
            currentIndex = choseBackgrounds.Length - 1; // Stay on the last background
        }
        UpdateBackground();
        UpdateButtonStates(); // Update button states after changing background
    }

    public void PreviousBackground()
    {
        currentIndex--;
        if (currentIndex < 0)
        {
            currentIndex = 0; // Stay on the first background
        }
        UpdateBackground();
        UpdateButtonStates(); // Update button states after changing background
    }

    private void UpdateBackground()
    {
        backgroundImage.sprite = choseBackgrounds[currentIndex]; // Change the displayed background
        thumbnailImage.sprite = choseThumbnailImages[currentIndex]; // Change the displayed thumbnail
    }

    private void UpdateButtonStates()
    {
        // Enable/Disable buttons based on the current index
        leftButton.interactable = currentIndex > 0; // Disable left button if at first image
        rightButton.interactable = currentIndex < choseBackgrounds.Length - 1; // Disable right button if at last image
    }
}