using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Optional")]
    public Transform startPoint;   // drag an empty GameObject placed at spawn
    public Transform ball;         // optional - assign ball if you want it reset with player

    private Vector3 startPosition;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (startPoint != null) startPosition = startPoint.position;
        else startPosition = transform.position;

        Debug.Log($"[PlayerController] startPosition = {startPosition}");
    }

    public void ResetToStart()
    {
        Debug.Log("[PlayerController] ResetToStart invoked.");

        // reset physics
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        transform.position = startPosition;
        transform.rotation = Quaternion.identity;

        // optional: reset ball near player
        if (ball != null)
        {
            Rigidbody br = ball.GetComponent<Rigidbody>();
            if (br != null)
            {
                br.velocity = Vector3.zero;
                br.angularVelocity = Vector3.zero;
                br.isKinematic = true; // set to kinematic while held
            }
            ball.position = startPosition + Vector3.up * 1f;
            Debug.Log("[PlayerController] Ball reset to player's start.");
        }
    }
}
