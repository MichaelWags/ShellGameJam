using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]

public class Attack : MonoBehaviour
{
    [SerializeField] private MeshFromPolygonCollider2D meshFromPolygonCollider;
    private PolygonCollider2D polygonCollider2D;
    public float damageAmount = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        polygonCollider2D = GetComponent<PolygonCollider2D>();
        meshFromPolygonCollider.CreateMesh();
        meshFromPolygonCollider.CreateLine();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("" + collision.gameObject.name);
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("enemy caught");
            GameObject enemy = collision.gameObject;
            enemy.GetComponent<Enemy>().TakeDamage(damageAmount);
        }
    }
}
