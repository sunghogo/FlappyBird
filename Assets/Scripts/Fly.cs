using UnityEngine;

public class Fly : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] float jumpVelocity = 5f;
    [SerializeField] float maxTurnRotation = 30f;
    [SerializeField] float minTurnRotation = -90f;
    [SerializeField] float maxClicksPerSecond = 5f;
    float clickDuration;
    float duration;
    Rigidbody2D rb;
    Animator animator;
    Vector2 gravity;
    FlappyAudio flappyAudio;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        gravity = Physics2D.gravity * rb.gravityScale;
        flappyAudio = GetComponent<FlappyAudio>();
        clickDuration = 1 / maxClicksPerSecond;
        duration = clickDuration;
    }

    void Update()
    {
        if (!GameManager.Instance.GameStart) return;

        // Get z Rotation and default downward rotation
        float zRotation = transform.eulerAngles.z;
        if (zRotation > 180f) zRotation -= 360f; // Transform from [0, 360] to [-180, 180] range
        float zRotationVel = gravity.y * jumpVelocity * Time.deltaTime;

        // Jump and rotate up by fixed constant velocity
        if (duration >= clickDuration && Input.GetMouseButtonDown(0))
        {
            rb.velocity = new Vector2(0, jumpVelocity);
            zRotation = maxTurnRotation;
            zRotationVel = 0f;
            duration = 0f;
            animator.SetTrigger("Flap");
            flappyAudio.PlayClip(FlappyClip.Wing);
        }
        duration += Time.deltaTime;

        // Clamp and apply rotations
        zRotation = Mathf.Clamp(zRotation + zRotationVel, minTurnRotation, maxTurnRotation);
        transform.eulerAngles = new Vector3(0, 0, zRotation);

        // Clamp y velocity
        rb.velocity = new Vector2(0, Mathf.Clamp(rb.velocity.y, -jumpVelocity, jumpVelocity));
    }
}
