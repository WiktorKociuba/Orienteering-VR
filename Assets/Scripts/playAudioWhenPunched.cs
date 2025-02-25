using UnityEngine;

public class playAudioWhenPunched : MonoBehaviour
{
    [Header("Settings")]
    public Checkpoints checkpoints;
    public string controlName;
    public AudioClip punchControlSound;

    private AudioSource audioSource;
    private bool ifPunched;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        ifPunched = false;
    }
    private void Update()
    {
        if (!ifPunched && checkpoints.punchedControl == controlName)
        {
            audioSource.clip = punchControlSound;
            audioSource.Play();
            ifPunched = true;
        }
    }
}
