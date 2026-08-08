using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]

public class Attack : MonoBehaviour
{
    [SerializeField] private MeshFromPolygonCollider2D meshFromPolygonCollider;
    private PolygonCollider2D polygonCollider2D;
    private LineRenderer lr;
    private MeshRenderer mr;
    public float damageAmount = 1;
    private float alphaRate = 0.5f;
    private float newAlphaPercent = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        polygonCollider2D = GetComponent<PolygonCollider2D>();
        lr = GetComponent<LineRenderer>();
        mr = GetComponent<MeshRenderer>();
        meshFromPolygonCollider.CreateMesh();
        meshFromPolygonCollider.CreateLine();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(newAlphaPercent);
        newAlphaPercent -= alphaRate * Time.deltaTime;
        
        //fade lines
        Gradient gradient = lr.colorGradient;
        GradientAlphaKey[] alphaKeys = gradient.alphaKeys;
        for (int i = 0; i < alphaKeys.Length; i++)
        {
            alphaKeys[i].alpha = newAlphaPercent;
        }
        gradient.SetKeys(gradient.colorKeys, alphaKeys);
        lr.colorGradient = gradient;

        if (newAlphaPercent <= 0.55)
        {
            mr.enabled = false;
        }

        if (newAlphaPercent <= 0)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("enemy caught");
            GameObject enemy = collision.gameObject;
            enemy.GetComponent<Enemy>().TakeDamage(damageAmount);
        }
    }
}
