using UnityEngine;

public class Fly : MonoBehaviour
{
    [SerializeField] private float _jumpVelocity;
    private Rigidbody2D _rigidbody2D;

    void Start() {
        _jumpVelocity = 5f;
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Get z Rotation and default downward rotation
        float zRotation = transform.eulerAngles.z;
        if (zRotation > 180f) zRotation -= 360f; // Transform from [0, 360] to [-180, 180] range
        float zRotationVel = -10 * _jumpVelocity * Time.deltaTime;

        // Jump and rotate up by fixed constant velocity
        if (Input.GetMouseButtonDown(0)) {
            _rigidbody2D.velocity += new Vector2(0, _jumpVelocity);
            zRotationVel = 10 * _jumpVelocity;
        }

        // Clamp and apply rotations
        zRotation = Mathf.Clamp(zRotation + zRotationVel, -30f, 30f);
        transform.eulerAngles = new Vector3(0, 0, zRotation);

        // Clamp y velocity
        _rigidbody2D.velocity = new Vector2(0, Mathf.Clamp(_rigidbody2D.velocity.y, -_jumpVelocity, _jumpVelocity));
    }
}
