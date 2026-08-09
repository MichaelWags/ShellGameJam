using UnityEngine;

public class CollectableSpawner : MonoBehaviour
{
    [SerializeField] private GameObject collectablePrefab;
    [SerializeField] private int min = 1;
    [SerializeField] private int max = 3;

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
        int amount = Random.Range(max, min);
        for(int i = 0; i < amount; i++)
        {
            Instantiate(collectablePrefab, transform.position, transform.rotation);
        }
    }
}
