using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Checkpoints : MonoBehaviour
{
    [Header("Checkpoints")]
    public GameObject start;
    public GameObject finish;
    public GameObject[] checkpoints;

    [Header("GUI")]
    public TextMeshProUGUI timeText;

    [Header("Player")]
    public GameObject player;

    [Header("Information")]
    private float currentCheckpoint;
    private bool started;
    private bool finished;

    private float time;
    private float timeToTeleport;
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
                    while(timeToTeleport < 5000)
                    {
                        timeToTeleport += Time.deltaTime;
                    }
                    SceneManager.UnloadSceneAsync("demoMap");
                    SceneManager.LoadScene("mainMenu");
                    Destroy(player);
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
