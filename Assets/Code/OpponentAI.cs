using UnityEngine;
using System.Collections;

public class OpponentAI : MonoBehaviour
{
    [Header("References")]
    public Transform player;     // drag your Player here (or leave null to auto-find by tag)
    public Transform ball;       // optional, used to reset ball with player

    [Header("Movement")]
    public float moveSpeed = 3f;
    public float reactionDistance = 10f;   // start chasing if within this distance
    public float stopDistance = 1.5f;      // how close they stop while chasing

    [Header("Reset (proximity)")]
    public float resetRadius = 1.0f;       // if opponent is this close to player -> reset
    public float resetDelay = 0.25f;       // small delay before reset (optional)

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;

        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
            else Debug.LogWarning("[OpponentAI] Player not assigned and no tag 'Player' found.");
        }

        if (ball == null)
        {
            GameObject b = GameObject.FindGameObjectWithTag("Ball");
            if (b != null) ball = b.transform;
        }
    }

    void Update()
    {
        if (player == null) return;

        float distToPlayer = Vector3.Distance(transform.position, player.position);

        // movement: chase if within reactionDistance, else return to start
        if (distToPlayer < reactionDistance)
        {
            MoveTowards(player.position);
        }
        else
        {
            MoveTowards(startPosition);
        }

        // proximity reset check
        if (distToPlayer <= resetRadius)
        {
            // start reset coroutine and optionally stop moving further
            StartCoroutine(DoResetAfterDelay(resetDelay));
        }
    }

    private void MoveTowards(Vector3 target)
    {
        Vector3 dir = target - transform.position;
        dir.y = 0f;
        float d = dir.magnitude;
        if (d > stopDistance)
        {
            transform.position += dir.normalized * moveSpeed * Time.deltaTime;
            transform.LookAt(new Vector3(target.x, transform.position.y, target.z));
        }
    }

    private bool resetInProgress = false;
    private IEnumerator DoResetAfterDelay(float delay)
    {
        if (resetInProgress) yield break;
        resetInProgress = true;

        yield return new WaitForSeconds(delay);

        // call player's ResetToStart if available
        if (player != null)
        {
            PlayerController pc = player.GetComponent<PlayerController>();
            if (pc != null)
            {
                Debug.Log($"[OpponentAI] {name} triggered ResetToStart on player.");
                // optional: reset the ball inside player.ResetToStart() or do it here:
                pc.ResetToStart();

                // if you want the ball reset from here instead of PlayerController, uncomment:
                // ResetBallToPlayer();
            }
            else
            {
                Debug.LogWarning("[OpponentAI] PlayerController not found on player object.");
            }
        }

        // short cooldown so we don't spam resets every frame while still within radius
        yield return new WaitForSeconds(0.5f);
        resetInProgress = false;
    }

    private void ResetBallToPlayer()
    {
        if (ball == null || player == null) return;
        Rigidbody rb = ball.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
        }

        ball.position = player.position + Vector3.up * 1.0f;
    }

    // optional editor visualization
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, reactionDistance);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, resetRadius);
    }
}
