using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] TMP_Text timeText;
    [SerializeField] GameObject gameOverText;
    [SerializeField] float starTime = 60f;
    bool gameOver = false;
    float timeLeft;

    public bool GameOver => gameOver;

    void Start()
    {
        timeLeft = starTime;
    }

    void Update()
    {
        if (gameOver) return;

        timeLeft -= Time.deltaTime;
        timeText.text = "Remaining Time: " + timeLeft.ToString("F1");

        if (timeLeft <= 0f) PlayerGameOver();
    }

    void PlayerGameOver()
    {
        gameOver = true;
        playerController.enabled = false;
        gameOverText.SetActive(true);
        Time.timeScale = .1f;
    }

    public void IncreaseTime(float amount)
    {
        timeLeft += amount;
    }

}
