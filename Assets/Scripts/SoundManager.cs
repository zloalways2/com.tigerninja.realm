using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public Button soundRedButton; // Reference to the sound off button (red)
    public Button soundGreenButton; // Reference to the sound on button (green)
    public Button musicRedButton; // Reference to the music off button (red)
    public Button musicGreenButton; // Reference to the music on button (green)
    
    public AudioSource musicSource; // Reference to the music audio source


    public AudioSource soundSource; // Reference to the sound effects audio source
    public AudioSource loseSound;
    public AudioSource collectSound;



    private void Start()
    {
        // Load initial settings
        LoadSettings();
        UpdateButtonStates(); // Update button visuals on start
    }

    public void ToggleSoundOn()
    {
        // Set sound effects off
        soundSource.mute = true; // Mute the sound effects
        loseSound.mute = true; // Mute lose sound
        collectSound.mute = true; // Mute collect sound
        UpdateButtonStates(); // Update button visuals
        //SaveSettings(); // Save the current state
    }

    public void ToggleSoundOff()
    {
        // Set sound effects on
        soundSource.mute = false; // Unmute the sound effects
        loseSound.mute = false; // Unmute lose sound
        collectSound.mute = false; // Unmute collect sound
        UpdateButtonStates(); // Update button visuals
        //SaveSettings(); // Save the current state
    }

    public void ToggleMusicOff()
    {
        // Set music off
        musicSource.mute = true; // Mute the music
        UpdateButtonStates(); // Update button visuals
        //SaveSettings(); // Save the current state
    }

    public void ToggleMusicOn()
    {
        // Set music on
        musicSource.mute = false; // Unmute the music
        UpdateButtonStates(); // Update button visuals
        //SaveSettings(); // Save the current state
    }

    private void UpdateButtonStates()
    {
        // Update sound effect button states
        soundRedButton.gameObject.SetActive(soundSource.mute); // Show red button if sound is off
        soundGreenButton.gameObject.SetActive(!soundSource.mute); // Show green button if sound is on

        // Update music button states
        musicRedButton.gameObject.SetActive(musicSource.mute); // Show red button if music is off
        musicGreenButton.gameObject.SetActive(!musicSource.mute); // Show green button if music is on
    }

     public void SaveSettings()
    {
        // Save the mute state to PlayerPrefs
        PlayerPrefs.SetInt("SoundEnabled", soundSource.mute ? 0 : 1);
        PlayerPrefs.SetInt("MusicEnabled", musicSource.mute ? 0 : 1);
        PlayerPrefs.Save(); // Save PlayerPrefs

    }


    public void MainMenuButton()
    {
        LoadSettings();

    }

    public void LoadSettings()
    {
        // Load the mute state from PlayerPrefs
        soundSource.mute = PlayerPrefs.GetInt("SoundEnabled", 1) == 0; // Default to muted
        musicSource.mute = PlayerPrefs.GetInt("MusicEnabled", 1) == 0; // Default to muted

        UpdateButtonStates(); // Ensure buttons reflect loaded states
    }
}
