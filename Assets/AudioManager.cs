using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Singleton pattern agar mudah diakses dari script lain
    public static AudioManager instance; 

    [Header("Komponen Audio (Wajib Diisi)")]
    public AudioSource sfxSource;

    [Header("File Suara (Audio Clip)")]
    public AudioClip correctClip;
    public AudioClip wrongClip;
    public AudioClip clickClip;

    void Awake()
    {
        // Setup Singleton
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    // Fungsi-fungsi yang akan dipanggil oleh DecisionManager
    public void PlayCorrect() { 
        if (sfxSource != null && correctClip != null) sfxSource.PlayOneShot(correctClip); 
    }
    
    public void PlayWrong() { 
        if (sfxSource != null && wrongClip != null) sfxSource.PlayOneShot(wrongClip); 
    }
    
    public void PlayClick() { 
        if (sfxSource != null && clickClip != null) sfxSource.PlayOneShot(clickClip); 
    }
}