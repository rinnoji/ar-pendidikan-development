using UnityEngine;
using UnityEngine.SceneManagement; // Wajib dipanggil untuk pindah scene
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [Header("Tombol Menu")]
    public Button startButton;
    public Button quitButton;

    void Start()
    {
        // Menyambungkan tombol dengan fungsinya lewat script (agar rapi)
        if (startButton != null)
        {
            startButton.onClick.AddListener(StartGame);
        }

        if (quitButton != null)
        {
            quitButton.onClick.AddListener(QuitGame);
        }
    }

    void StartGame()
    {
        // 1 adalah urutan indeks GameScene di Build Settings (kita akan atur setelah ini)
        SceneManager.LoadScene(1); 
    }

    void QuitGame()
    {
        Debug.Log("Aplikasi Keluar. (Catatan: Fungsi Quit tidak terlihat saat Play di Unity Editor, hanya berfungsi saat sudah di-Build)");
        Application.Quit();
    }
}