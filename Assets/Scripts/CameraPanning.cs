using UnityEngine;
using System.Collections;

public class CameraPanning : MonoBehaviour
{
    private Camera cam;
    [SerializeField] private float edgeThreshold = 0.01f;
    [SerializeField] private float panningSpeed = 5f;
    private Vector3 camTargetPos;
    private bool isPanning = false;
    private PlayerController playerController;

    void Start()
    {
        cam = Camera.main;
        camTargetPos = cam.transform.position;
        playerController = GetComponent<PlayerController>();
    }

    void Update()
    {
        // Convert player world position to viewport coordinates
        Vector3 viewportPoint = cam.WorldToViewportPoint(transform.position);

        // Check if the player is near any edge
        bool isAtLeftEdge = viewportPoint.x <= edgeThreshold;
        bool isAtRightEdge = viewportPoint.x >= 1f - edgeThreshold;
        bool isAtBottomEdge = viewportPoint.y <= edgeThreshold;
        bool isAtTopEdge = viewportPoint.y >= 1f - edgeThreshold;

        if (!isPanning && (isAtLeftEdge || isAtRightEdge || isAtBottomEdge || isAtTopEdge))
        {
            Debug.Log("at edge");

            Vector2 movement = transform.position;
            movement.x += isAtLeftEdge ? -1f : 0f;
            movement.x += isAtRightEdge ? 1f : 0f;
            movement.y += isAtBottomEdge ? -1f : 0f;
            movement.y += isAtTopEdge ? 1f : 0f;

            transform.position = movement;

            camTargetPos.x += isAtLeftEdge ? -20f : 0f;
            camTargetPos.x += isAtRightEdge ? 20f : 0f;
            camTargetPos.y += isAtBottomEdge ? -11f : 0f;
            camTargetPos.y += isAtTopEdge ? 11f : 0f;

            if (!isPanning)
            {
                StartCoroutine(Panning());
            }
        }
    }

    private IEnumerator Panning()
    {
        isPanning = true;
        //pause player movement
        playerController.canMove = false;

        while (Vector3.Distance(cam.transform.position, camTargetPos) > 0.05f)
        {
            cam.transform.position = Vector3.Lerp(cam.transform.position, camTargetPos, panningSpeed * Time.deltaTime);
            yield return null;
        }

        cam.transform.position = camTargetPos;
        isPanning = false;
        playerController.canMove = true;
    }
}
