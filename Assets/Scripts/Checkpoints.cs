using TMPro;
using UnityEngine;

public class Checkpoints : MonoBehaviour
{
    [Header("Checkpoints")]
    public GameObject start;
    public GameObject finish;
    public GameObject[] checkpoints;

    [Header("GUI")]
    public TextMeshProUGUI timeText;

    [Header("Information")]
    private float currentCheckpoint;
    private bool started;
    private bool finished;

    private float time;

    private void Start()
    {
        currentCheckpoint = 0;
        started = false;
        finished = false;
        time = 0;
    }
    private void Update()
    {
        if(started && !finished)
        {
            time += Time.deltaTime;
        }
        timeText.text = $"{Mathf.FloorToInt(time / 60)}:{time % 60:00}";
    }
    private void OnTriggerEnter(UnityEngine.Collider other)
    {
        if (other.CompareTag("Checkpoint"))
        {
            GameObject thisCheckpoint = other.gameObject;

            if (thisCheckpoint == start && !started)
            {
                print("Start");
                started = true;
            }
            else if (thisCheckpoint == finish && started)
            {
                if(currentCheckpoint == checkpoints.Length)
                {
                    print($"Finished, time: {time}");
                    finished = true;
                }
            }
            for (int i = 0; i < checkpoints.Length; i++)
            {
                if (finished)
                    return;
                if (thisCheckpoint == checkpoints[i] && i == currentCheckpoint)
                {
                    currentCheckpoint++;
                    print($"Control {thisCheckpoint}");
                }
            }
        }
    }
}
