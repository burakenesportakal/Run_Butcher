using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicDefaultVolume : MonoBehaviour
{
    [Range(0f, 1f)]
    public float defaultVolume = 0.7f;

    private void Awake()
    {
        AudioSource audio = GetComponent<AudioSource>();
        audio.volume = defaultVolume;
    }
}
