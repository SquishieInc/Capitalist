using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("SFX Clips")]
    public AudioClip prestigeSFX;
    public AudioClip milestoneSFX;
    public AudioClip buttonClickSFX;

    private AudioSource audioSource;

    private void Awake()
    {
        if (Instance == null) Instance = this;

        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip != null)
            audioSource.PlayOneShot(clip);
    }
}