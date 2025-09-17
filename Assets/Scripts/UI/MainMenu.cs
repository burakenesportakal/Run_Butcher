using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button startButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button musicButton;

    [Header("Music Control")]
    [SerializeField] private Image soundOnImage;
    [SerializeField] private Image soundOffImage;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private AudioSource musicSource;

    private float lastVolume = 1f; // Müziğin hatırlanacak son sesi
    private bool isMuted = false;

    private void Start()
    {
        // Button listener'lar
        startButton.onClick.AddListener(PlayGame);
        quitButton.onClick.AddListener(QuitGame);
        musicButton.onClick.AddListener(ToggleMusic);
        volumeSlider.onValueChanged.AddListener(SetVolume);

        // İlk değerleri ayarla
        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            lastVolume = PlayerPrefs.GetFloat("MusicVolume");
        }

        musicSource.volume = lastVolume;
        volumeSlider.value = lastVolume;

        UpdateMusicIcons();
    }

    private void PlayGame()
    {
        startButton.interactable = false; // Tekrar basmayı engelle
        StartCoroutine(FadeOutMusicAndLoadScene(1f)); // 3 saniyelik fade
    }

    private void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Editörde çalıştırmayı durdur
#endif
    }

    private void ToggleMusic()
    {
        if (isMuted)
        {
            // Önceki sesi geri getir
            musicSource.volume = lastVolume;
            volumeSlider.value = lastVolume;
            isMuted = false;
        }
        else
        {
            // Sesi kapat
            lastVolume = musicSource.volume; // son ses değerini hatırla
            musicSource.volume = 0;
            volumeSlider.value = 0;
            isMuted = true;
        }

        PlayerPrefs.SetFloat("MusicVolume", musicSource.volume);
        PlayerPrefs.Save();

        UpdateMusicIcons();
    }

    private void SetVolume(float value)
    {
        musicSource.volume = value;

        if (value > 0)
        {
            lastVolume = value;
            isMuted = false;
        }
        else
        {
            isMuted = true;
        }

        PlayerPrefs.SetFloat("MusicVolume", value);
        PlayerPrefs.Save();

        UpdateMusicIcons();
    }

    private void UpdateMusicIcons()
    {
        soundOnImage.gameObject.SetActive(!isMuted);
        soundOffImage.gameObject.SetActive(isMuted);
    }

    private IEnumerator FadeOutMusicAndLoadScene(float duration)
{
    float startVolume = musicSource.volume;
    float elapsed = 0f;

    while (elapsed < duration)
    {
        elapsed += Time.deltaTime;
        musicSource.volume = Mathf.Lerp(startVolume, 0, elapsed / duration);
        volumeSlider.value = musicSource.volume; // Slider ile senkronize et
        yield return null;
    }

    musicSource.volume = 0;
    volumeSlider.value = 0;

    SceneManager.LoadScene("GameScene");
}

}
