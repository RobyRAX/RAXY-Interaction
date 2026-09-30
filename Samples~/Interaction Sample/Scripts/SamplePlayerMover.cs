using UnityEngine;
using UnityEngine.InputSystem;

public class SamplePlayerMover : MonoBehaviour
{
    [SerializeField]
    float moveSpeed = 5f;

    Rigidbody _body;
    Vector3 _wish;

    void Awake()
    {
        _body = GetComponent<Rigidbody>();
    }

    void Update()
    {
        var keyboard = Keyboard.current;
        if (keyboard == null)
        {
            _wish = Vector3.zero;
            return;
        }

        float x = 0f;
        float z = 0f;

        if (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed)
            x -= 1f;
        if (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed)
            x += 1f;
        if (keyboard.sKey.isPressed || keyboard.downArrowKey.isPressed)
            z -= 1f;
        if (keyboard.wKey.isPressed || keyboard.upArrowKey.isPressed)
            z += 1f;

        var forward = Vector3.forward;
        var right = Vector3.right;
        var cam = Camera.main;

        if (cam != null)
        {
            forward = cam.transform.forward;
            forward.y = 0f;
            if (forward.sqrMagnitude > 0.001f)
                forward.Normalize();
            else
                forward = Vector3.forward;

            right = cam.transform.right;
            right.y = 0f;
            if (right.sqrMagnitude > 0.001f)
                right.Normalize();
            else
                right = Vector3.right;
        }

        _wish = right * x + forward * z;
        if (_wish.sqrMagnitude > 1f)
            _wish.Normalize();
    }

    void FixedUpdate()
    {
        var step = _wish * (moveSpeed * Time.fixedDeltaTime);

        if (_body != null)
            _body.MovePosition(_body.position + step);
        else
            transform.position += step;
    }
}
