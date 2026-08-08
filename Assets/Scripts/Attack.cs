using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]

public class Attack : MonoBehaviour
{
    [SerializeField] private MeshFromPolygonCollider2D meshFromPolygonCollider;
    private PolygonCollider2D polygonCollider2D;
    private LineRenderer lr;
    private Material mat;
    public float damageAmount = 1;
    private float alphaRate = 0.01f;
    private float newAlphaPercent = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        polygonCollider2D = GetComponent<PolygonCollider2D>();
        lr = GetComponent<LineRenderer>();
        mat = GetComponent<MeshRenderer>().material;
        meshFromPolygonCollider.CreateMesh();
        meshFromPolygonCollider.CreateLine();

        Disappear();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(newAlphaPercent);
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
        
        //fade mesh renderer material
        Color newColor = new Color(mat.color.r, mat.color.g, mat.color.b, newAlphaPercent*255);
        mat.color = newColor;
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

    private void Disappear()
    {
        while (newAlphaPercent > 0)
        {
            
        }
        //Destroy(gameObject);
    }
}
