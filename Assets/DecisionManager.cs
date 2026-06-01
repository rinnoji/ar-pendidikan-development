using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Wajib untuk kembali ke menu

public class DecisionManager : MonoBehaviour
{
    [System.Serializable]
    public class DisasterStage
    {
        public string stageName;
        [TextArea(2, 5)]
        public string questionText;
        public string[] answers = new string[4];
        public int correctAnswerIndex;
        [TextArea(3, 5)]
        public string educationFeedback;
        
        [Header("Objek 3D / Visual Bencana")]
        public GameObject stageVisualContainer; // Wadah visual yang akan dinyalakan
    }

    [Header("UI References")]
    public TMP_Text questionTextMesh;
    public Button[] answerButtons;
    public TMP_Text timerTextMesh;
    
    [Header("Layar Result Akhir")]
    public GameObject resultPanel;
    public TMP_Text resultTextMesh;
    public Button backToMenuButton; // Tombol baru

    [Header("Pop-Up Edukasi")]
    public GameObject feedbackPanel;
    public TMP_Text feedbackTextMesh;
    public Button nextButton;

    [Header("Konfigurasi Data")]
    public DisasterStage[] stages;
    public float timePerStage = 15f;

    private int currentStageIndex = 0;
    private float currentTimer;
    private bool isGameActive = false;
    private int correctAnswersCount = 0;

    void Start()
    {
        if(nextButton != null)
        {
            nextButton.onClick.AddListener(() => {
                if (AudioManager.instance != null) AudioManager.instance.PlayClick();
                ProceedToNextStage();
            });
        }

        // Event untuk tombol kembali ke menu
        if(backToMenuButton != null)
        {
            backToMenuButton.onClick.AddListener(() => {
                if (AudioManager.instance != null) AudioManager.instance.PlayClick();
                SceneManager.LoadScene(0); // 0 adalah index MenuScene
            });
        }

        if (stages != null && stages.Length > 0) StartGame();
    }

    public void StartGame()
    {
        currentStageIndex = 0;
        correctAnswersCount = 0;
        if (resultPanel != null) resultPanel.SetActive(false);
        if (feedbackPanel != null) feedbackPanel.SetActive(false);
        LoadStage(currentStageIndex);
    }

    void Update()
    {
        if (!isGameActive) return;

        if (currentTimer > 0)
        {
            currentTimer -= Time.deltaTime;
            timerTextMesh.text = "Waktu: " + Mathf.CeilToInt(currentTimer).ToString() + "s";
        }
        else
        {
            timerTextMesh.text = "Waktu Habis!";
            OnAnswerSelected(-1); 
        }
    }

    void LoadStage(int index)
    {
        if (index >= stages.Length)
        {
            EndGame();
            return;
        }

        currentTimer = timePerStage;
        questionTextMesh.text = stages[index].questionText;

        // MATIKAN SEMUA VISUAL LEBIH DULU
        foreach (var stage in stages)
        {
            if (stage.stageVisualContainer != null) 
                stage.stageVisualContainer.SetActive(false);
        }

        // NYALAKAN VISUAL UNTUK STAGE SAAT INI SAJA
        if (stages[index].stageVisualContainer != null) 
            stages[index].stageVisualContainer.SetActive(true);

        for (int i = 0; i < answerButtons.Length; i++)
        {
            int buttonIndex = i;
            TMP_Text buttonText = answerButtons[i].GetComponentInChildren<TMP_Text>();
            if (buttonText != null) buttonText.text = stages[index].answers[i];

            answerButtons[i].interactable = true;
            answerButtons[i].onClick.RemoveAllListeners();
            answerButtons[i].onClick.AddListener(() => {
                if (AudioManager.instance != null) AudioManager.instance.PlayClick();
                OnAnswerSelected(buttonIndex);
            });
        }

        isGameActive = true;
    }

    void OnAnswerSelected(int selectedIndex)
    {
        isGameActive = false; 
        foreach (var btn in answerButtons) btn.interactable = false;

        bool isCorrect = (selectedIndex == stages[currentStageIndex].correctAnswerIndex);
        
        if (isCorrect) 
        {
            correctAnswersCount++;
            if (AudioManager.instance != null) AudioManager.instance.PlayCorrect();
        }
        else
        {
            if (AudioManager.instance != null) AudioManager.instance.PlayWrong();
        }

        string statusText = isCorrect ? "<color=green>BENAR!</color>" : "<color=red>SALAH / WAKTU HABIS!</color>";
        feedbackTextMesh.text = $"{statusText}\n\n{stages[currentStageIndex].educationFeedback}";
        feedbackPanel.SetActive(true);
    }

    void ProceedToNextStage()
    {
        feedbackPanel.SetActive(false);
        currentStageIndex++;
        LoadStage(currentStageIndex);
    }

    void EndGame()
    {
        isGameActive = false;
        
        // Matikan visual stage terakhir
        if (stages[currentStageIndex].stageVisualContainer != null) 
            stages[currentStageIndex].stageVisualContainer.SetActive(false);

        if (resultPanel != null)
        {
            resultPanel.SetActive(true);
            resultTextMesh.text = $"Simulasi Selesai!\n\nSkor: {correctAnswersCount} / {stages.Length}\n\nPresentasi UAS Siap.";
        }
        timerTextMesh.text = "Selesai";
    }
}