using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.InputSystem.InputAction;

public class PlayerController : MonoBehaviour
{
    [Range(0.1f, 1.0f)]
    [SerializeField] float movementFalloffPercent = 0.1f;
    [SerializeField] float movementFalloffThreshold = 1.0f;
    [SerializeField] float movementSpeed = 1.0f;
    [SerializeField] bool hasMomentum = false;
    [SerializeField] float momentumBuildupSpeed = 0.25f;
    [Tooltip("Momentum lerps between movementFalloffPercent and this value, then uses the resulting value to add momentum to the player.")]
    [SerializeField] float maxMomentum = 0.1f;
    [SerializeField] float jumpScalar = 1.0f;
    [Tooltip("True: Allows the player to move while in the air.\nFalse: Will use momentum instead of direct input instead.")]
    [SerializeField] bool allowXAxisInputWhileJumping = true;

    [Header("References")]
    [SerializeField] new Rigidbody2D rigidbody = null;
    [SerializeField] GroundCollider groundCollider = null;


    Vector2 _moveVec = new Vector2();
    bool _canMove = true;
    [HideInInspector] public bool doMovementFalloff = true;
    float _momentumTValue = 0.0f;
    bool _canJump = false;

    Vector2 _lookVec = new Vector2();
    [HideInInspector] public bool canLook = true;

    // Start is called before the first frame update
    void Start()
    {

        void EnableJump()
        {
            _canJump = true;
        }
        void DisableJump()
        {
            _canJump = false;
        }
        groundCollider.OnStayGround.AddListener(EnableJump);
        groundCollider.OnLeaveGround.AddListener(DisableJump);
    }

    private void Update()
    {
        if (_moveVec.magnitude == 0.0f)
        {
            _momentumTValue -= Time.deltaTime * momentumBuildupSpeed * 2.0f;
            if (_momentumTValue <= 0.0f)
                _momentumTValue = 0.0f;
            return;
        }
        _momentumTValue += Time.deltaTime * momentumBuildupSpeed;
        if (_momentumTValue >= 1.0f)
            _momentumTValue = 1.0f;
    }

    private void FixedUpdate()
    {
        if (!_canMove)
            _moveVec = Vector2.zero;

        if (allowXAxisInputWhileJumping)
            rigidbody.AddForce(transform.right * _moveVec.x * movementSpeed, ForceMode2D.Impulse);
        else
        {
            hasMomentum = !_canJump;
            if (hasMomentum)
                _momentumTValue = 1.0f;
            else
                rigidbody.AddForce(transform.right * _moveVec.x * movementSpeed, ForceMode2D.Impulse);

        }

        if (!doMovementFalloff)
            return;
        //ignore y component
        var vel = rigidbody.velocity;
        vel.y = 0.0f;
        float falloff = movementFalloffPercent;
        if (hasMomentum)
            falloff = Mathf.Lerp(movementFalloffPercent, maxMomentum, _momentumTValue);

        if (vel.magnitude > movementFalloffThreshold)
            rigidbody.AddForce(-vel * falloff, ForceMode2D.Impulse);
    }

    public void OnMove(CallbackContext ctx)
    {
        _moveVec = ctx.ReadValue<Vector2>();
    }

    public void OnJump(CallbackContext ctx)
    {
        if (!_canJump || !ctx.performed || !_canMove)
            return;
        rigidbody.AddForce(Vector2.up * jumpScalar, ForceMode2D.Impulse);
    }

    public void OnLook(CallbackContext ctx)
    {
        if (!canLook)
            return;
        _lookVec = ctx.ReadValue<Vector2>();
    }

    public void OnFire(CallbackContext ctx)
    {
        _canMove = !ctx.performed;
        if (!ctx.performed)
            return;
        //this is here because webGL sucks basically
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

}
