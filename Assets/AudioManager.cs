using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource sfxSource;

    public AudioClip correct;
    public AudioClip wrong;

    public void PlayCorrect()
    {
        sfxSource.PlayOneShot(correct);
    }

    public void PlayWrong()
    {
        sfxSource.PlayOneShot(wrong);
    }
}
