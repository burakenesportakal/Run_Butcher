using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class PauseMenu : MonoBehaviour
{
    [Header("Root")]
    [SerializeField] private GameObject pausePanel;     // ResumeMenu/Panel

    [Header("Buttons")]
    [SerializeField] private Button resumeButton;       // Resume
    [SerializeField] private Button mainMenuButton;     // MainMenu
    [SerializeField] private Button restartButton;      // Restart
    [SerializeField] private Button quitButton;         // Quit
    [SerializeField] private Button musicToggleButton;  // Music On Off objesinin üstündeki buton (veya Image’a Button ekle)

    [Header("Music Control")]
    [SerializeField] private Image soundOnImage;        // Music On Off > SoundOn
    [SerializeField] private Image soundOffImage;       // Music On Off > SoundOff
    [SerializeField] private Slider volumeSlider;       // Slider Blue
    [SerializeField] private AudioSource musicSource;   // Arka plan müziğin AudioSource’u

    [Header("Options")]
    [SerializeField] private float resumeFadeInSeconds = 0.0f; // 0 istersen anında devam eder

    private bool isPaused = false;
    private bool isMuted = false;
    private float lastVolume = 1f;

    GameManager gameManager;

    private const string PREF_MUSIC_VOLUME = "MusicVolume";

    private void Awake()
    {

        gameManager = FindFirstObjectByType<GameManager>();

        // İlk durumlar
        if (PlayerPrefs.HasKey(PREF_MUSIC_VOLUME))
            lastVolume = PlayerPrefs.GetFloat(PREF_MUSIC_VOLUME);

        musicSource.volume = lastVolume;
        volumeSlider.value = lastVolume;
        UpdateMusicIcons();

        pausePanel.SetActive(false);
        SetTimeScale(1f);


    }

    private void OnEnable()
    {
        resumeButton.onClick.AddListener(Resume);
        mainMenuButton.onClick.AddListener(GoMainMenu);
        restartButton.onClick.AddListener(RestartLevel);
        quitButton.onClick.AddListener(QuitGame);
        musicToggleButton.onClick.AddListener(ToggleMusic);
        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    private void OnDisable()
    {
        resumeButton.onClick.RemoveAllListeners();
        mainMenuButton.onClick.RemoveAllListeners();
        restartButton.onClick.RemoveAllListeners();
        quitButton.onClick.RemoveAllListeners();
        musicToggleButton.onClick.RemoveAllListeners();
        volumeSlider.onValueChanged.RemoveAllListeners();
    }

    private void Update()
    {

        if (gameManager.GameOver == true) return;
        // ESC ile aç/kapat
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) Resume();
            else Pause();
        }
    }

    // ----- Pause / Resume -----
    private void Pause()
    {
        isPaused = true;
        pausePanel.SetActive(true);
        SetTimeScale(0f);
        // Cursor
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void Resume()
    {
        isPaused = false;
        pausePanel.SetActive(false);
        if (resumeFadeInSeconds > 0f)
            StartCoroutine(FadeTimeScale(0f, 1f, resumeFadeInSeconds));
        else
            SetTimeScale(1f);

        Cursor.visible = false; // oyununa göre değiştir
        Cursor.lockState = CursorLockMode.Locked;
    }

    private IEnumerator FadeTimeScale(float from, float to, float duration)
    {
        float t = 0f;
        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            SetTimeScale(Mathf.Lerp(from, to, t / duration));
            yield return null;
        }
        SetTimeScale(to);
    }

    private void SetTimeScale(float value)
    {
        Time.timeScale = value;
        // UI animasyonları düzgün aksın diye:
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
    }

    // ----- Scene / App kontrolü -----
    private void GoMainMenu()
    {
        // Zamanı normale alıp yükle
        SetTimeScale(1f);
        SceneManager.LoadScene("MainMenu");
    }

    private void RestartLevel()
    {
        SetTimeScale(1f);
        Scene current = SceneManager.GetActiveScene();
        SceneManager.LoadScene(current.name);
    }

    private void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    // ----- Müzik kontrolü -----
    private void ToggleMusic()
    {
        if (isMuted)
        {
            musicSource.volume = lastVolume;
            volumeSlider.value = lastVolume;
            isMuted = false;
        }
        else
        {
            lastVolume = musicSource.volume;
            musicSource.volume = 0f;
            volumeSlider.value = 0f;
            isMuted = true;
        }

        PlayerPrefs.SetFloat(PREF_MUSIC_VOLUME, musicSource.volume);
        PlayerPrefs.Save();
        UpdateMusicIcons();
    }

    private void SetVolume(float value)
    {
        musicSource.volume = value;

        if (value > 0f)
        {
            lastVolume = value;
            isMuted = false;
        }
        else
        {
            isMuted = true;
        }

        PlayerPrefs.SetFloat(PREF_MUSIC_VOLUME, value);
        PlayerPrefs.Save();
        UpdateMusicIcons();
    }

    private void UpdateMusicIcons()
    {
        soundOnImage.gameObject.SetActive(!isMuted);
        soundOffImage.gameObject.SetActive(isMuted);
    }
}
