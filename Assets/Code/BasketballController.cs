using UnityEngine;

public class BasketballController : MonoBehaviour
{
    [Header("Movement")]
    public float MoveSpeed = 10f;

    [Header("Ball")]
    public Transform Ball;
    public Transform PosDribble;
    public Transform PosOverHead;
    public Transform Arms;
    public Transform Target;

    private bool IsBallInHands = true;
    private bool IsBallFlying = false;
    private float T = 0f;
    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;

        if (Ball != null)
        {
            Rigidbody rb = Ball.GetComponent<Rigidbody>();
            if (rb != null) rb.isKinematic = true; // start held
        }
    }

    void Update()
    {
        // Movement
        Vector3 direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
        if (direction.sqrMagnitude > 0.001f)
        {
            transform.position += direction.normalized * MoveSpeed * Time.deltaTime;
            transform.LookAt(transform.position + direction);
        }

        // Ball logic
        if (IsBallInHands)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                Ball.position = PosOverHead.position;
                Arms.localEulerAngles = Vector3.right * 180f;
                if (Target != null && Target.parent != null) transform.LookAt(Target.parent.position);
            }
            else
            {
                Ball.position = PosDribble.position + Vector3.up * Mathf.Abs(Mathf.Sin(Time.time * 5f));
                Arms.localEulerAngles = Vector3.right * 0f;
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                // release ball
                IsBallInHands = false;
                IsBallFlying = true;
                T = 0f;
                Rigidbody rb = Ball.GetComponent<Rigidbody>();
                if (rb != null) rb.isKinematic = false; // enable physics
            }
        }

        if (IsBallFlying)
        {
            T += Time.deltaTime;
            float duration = 0.66f;
            float t01 = T / duration;

            Vector3 A = PosOverHead.position;
            Vector3 B = Target.position;
            Vector3 pos = Vector3.Lerp(A, B, t01);
            Vector3 arc = Vector3.up * 5f * Mathf.Sin(t01 * Mathf.PI);
            Ball.position = pos + arc;

            if (t01 >= 1f)
            {
                IsBallFlying = false;
                Rigidbody rb = Ball.GetComponent<Rigidbody>();
                if (rb != null) rb.isKinematic = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // pick up ball when it's not flying
        if (!IsBallInHands && !IsBallFlying && other.CompareTag("Ball"))
        {
            IsBallInHands = true;
            Rigidbody rb = Ball.GetComponent<Rigidbody>();
            if (rb != null) rb.isKinematic = true;
        }
    }

    // Called by Opponent when player is hit
    public void ResetToStart()
    {
        Debug.Log("Player reset to start.");
        transform.position = startPosition;

        // Reset ball to dribble position and hold it
        if (Ball != null)
        {
            Rigidbody rb = Ball.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // only set velocities if rb is non-kinematic
                if (!rb.isKinematic)
                {
                    rb.velocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                }
                rb.isKinematic = true;
            }
            Ball.position = PosDribble.position;
        }

        IsBallInHands = true;
        IsBallFlying = false;
        T = 0f;
    }
}
