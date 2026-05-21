using UnityEngine;
using TMPro;

public class DecisionManager : MonoBehaviour
{
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI timerText;
    public AudioManager audioManager;

    public float timeLeft = 5f;
    private bool isAnswered = false;

    void Update()
    {
        if (isAnswered) return;

        timeLeft -= Time.deltaTime;
        timerText.text = Mathf.Ceil(timeLeft).ToString();

        if (timeLeft <= 0)
        {
            Timeout();
        }
    }

    public void Choose(string choice)
    {
        if (isAnswered) return;

        isAnswered = true;

        if (choice == "Meja")
        {
            resultText.text = "Benar!";
            resultText.color = Color.green;
            audioManager.PlayCorrect();

            Invoke("NextStage", 2f);
        }
        else
        {
            resultText.text = "Salah!";
            resultText.color = Color.red;
            audioManager.PlayWrong();

            Invoke("RestartStage", 2f);
        }
    }

    void Timeout()
    {
        isAnswered = true;

        resultText.text = "Terlambat!";
        resultText.color = Color.red;

        audioManager.PlayWrong();

        Invoke("RestartStage", 2f);
    }

    void NextStage()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Stage2");
    }

    void RestartStage()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
    }
}
