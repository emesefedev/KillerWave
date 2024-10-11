using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class Pause : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    
    [SerializeField] private Toggle pauseButton;
    [SerializeField] private GameObject pauseIcon;

    [SerializeField] private Button resumeButton;
    [SerializeField] private Button quitButton;

    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    [SerializeField] private AudioMixer audioMixer;   

    private void Awake()
    {
        pausePanel.SetActive(false);
        SetPauseButtonActive(false);
        Invoke("DelayPauseButtonApperance", 3);

        pauseButton.onValueChanged.AddListener((boolValue) => PauseGame());
        resumeButton.onClick.AddListener(ResumeGame);
        quitButton.onClick.AddListener(Quit);

        musicSlider.onValueChanged.AddListener(SetMusicLevelFromSlider);
        sfxSlider.onValueChanged.AddListener(SetSFXLevelFromSlider);

        float savedMusicVolume = PlayerPrefs.GetFloat("musicVolume");
        float savedSFXVolume = PlayerPrefs.GetFloat("sfxVolume");
        
        musicSlider.value = savedMusicVolume;
        sfxSlider.value = savedSFXVolume;
    }

    private void PauseGame()
    {
        pausePanel.SetActive(true);
        SetPauseButtonActive(false);
        Time.timeScale = 0;
    }

    private void ResumeGame()
    {
        pausePanel.SetActive(false);
        SetPauseButtonActive(true);
        Time.timeScale = 1;
    }

    private void Quit()
    {
        Time.timeScale = 1;

        GameManager.Instance.GetScoreManager().ResetScore();
        GameManager.Instance.GetScenesManager().BeginGame(0);
    }

    private void SetMusicLevelFromSlider(float value)
    {
        audioMixer.SetFloat("musicVolume", value);
        PlayerPrefs.SetFloat("musicVolume", value);
    }

    private void SetSFXLevelFromSlider(float value)
    {
        audioMixer.SetFloat("sfxVolume", value);
        PlayerPrefs.SetFloat("sfxVolume", value);
    }

    private void DelayPauseButtonApperance()
    {
        SetPauseButtonActive(true);
    }

    private void SetPauseButtonActive(bool switchButton)
    {
        ColorBlock colorBlock = pauseButton.colors;
        if (switchButton)
        {
            Color gray = new Color(245, 245, 245, 255);

            colorBlock.normalColor = gray;
            colorBlock.highlightedColor = gray;
            colorBlock.pressedColor = new Color(200, 200, 200, 255);
            colorBlock.disabledColor = new Color(200, 200, 200, 128);
        }
        else 
        {
            Color transparent = new Color(0, 0, 0, 0);

            colorBlock.normalColor = transparent;
            colorBlock.highlightedColor = transparent;
            colorBlock.pressedColor = transparent;
            colorBlock.disabledColor = transparent;
        }

        pauseButton.interactable = switchButton;
        pauseIcon.SetActive(switchButton);
        
        pauseButton.colors = colorBlock;
    }
}
