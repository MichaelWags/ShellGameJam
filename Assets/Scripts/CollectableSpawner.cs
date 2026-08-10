using System.Collections;
using UnityEngine;

public class CollectableSpawner : MonoBehaviour
{
    [SerializeField] private GameObject collectablePrefab;
    [SerializeField] private int min = 1;
    [SerializeField] private int max = 3;
    [SerializeField] private float delay = 0.1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Collect()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        StartCoroutine(CollectRoutine());
    }

    public IEnumerator CollectRoutine()
    {
        Debug.Log("here");

        int amount = Random.Range(max, min);
        for(int i = 0; i < amount; i++)
        {
            yield return new WaitForSeconds(delay);
            Instantiate(collectablePrefab, transform.position, transform.rotation);
        }

        Destroy(gameObject);
    }
}
