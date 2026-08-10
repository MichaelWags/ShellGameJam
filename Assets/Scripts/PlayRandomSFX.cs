using UnityEngine;

[RequireComponent(typeof(AudioSource))]

public class PlayRandomSFX : MonoBehaviour
{
    [SerializeField] private AudioClip[] clips;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("sfx start");
        if(clips.Length > 0)
        {
            if(Random.Range(0, 15) == 1)
            {
                GetComponent<AudioSource>().PlayOneShot(clips[Random.Range(1, clips.Length)]);
                Debug.Log("sfx play nates");
            }
            else
            {
                GetComponent<AudioSource>().PlayOneShot(clips[0]);
                Debug.Log("sfx play" + clips[0].name);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
