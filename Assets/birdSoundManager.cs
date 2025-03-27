using UnityEngine;
using System.Collections;
using UnityEngine.Audio;
public class birdSoundManager : MonoBehaviour
{
    public AudioClip[] birdSounds;
    public Transform player;
    public float minDistance = 20f;
    public float maxDistance = 30f;
    public float minInterval = 5f;
    public float maxInterval = 9f;
    public float soundVolume = 0.15f;

    private void Start()
    {
        StartCoroutine(PlayBirdSound());
    }

    private IEnumerator PlayBirdSound()
    {
        while(true)
        {
            Vector3 randomDirection = Random.insideUnitSphere * maxDistance;
            randomDirection.y = 8;
            Vector3 randomPosition = player.position + randomDirection.normalized * Random.Range(minDistance, maxDistance);
            GameObject birdSoundObject = new GameObject("BirdSound");
            birdSoundObject.transform.position = randomPosition;
            AudioSource audioSource = birdSoundObject.AddComponent<AudioSource>();
            audioSource.clip = birdSounds[Random.Range(0, birdSounds.Length)];
            audioSource.spatialBlend = 1.0f;
            audioSource.rolloffMode = AudioRolloffMode.Logarithmic;
            audioSource.volume = soundVolume;
            audioSource.Play();

            Destroy(birdSoundObject, audioSource.clip.length);

            yield return new WaitForSeconds(Random.Range(minInterval,maxInterval));
        }
    }
}
