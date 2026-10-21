
using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;

    [Header("Background Bounds")]
    public Transform background;
    public float boundsPadding = 0.5f;

    [Header("Camera Bounds")]
    public Camera mainCamera;
    public bool stayInsideCamera = true;

    [Header("Hit Effect")]
    public float knockbackDuration = 0.25f;
    public float shakeAmount = 0.08f;
    public float knockbackDistance = 0.4f;

    private Rigidbody2D rb;
    private Vector2 movement;

    private Vector2 touchStart;
    private bool isDragging;

    private SpriteRenderer spriteRenderer;
    private bool isKnockedBack;
    private Coroutine knockbackCoroutine;

    private float minX;
    private float maxX;
    private float minY;
    private float maxY;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (mainCamera == null)
            mainCamera = Camera.main;

        CalculateBackgroundBounds();
    }

    void CalculateBackgroundBounds()
    {
        if (background == null)
        {
            Debug.LogWarning(
                "Please assign the Background in PlayerMovement.");
            return;
        }

        SpriteRenderer bgRenderer =
            background.GetComponent<SpriteRenderer>();

        if (bgRenderer == null)
        {
            Debug.LogWarning(
                "Background needs a SpriteRenderer.");
            return;
        }

        Bounds bgBounds = bgRenderer.bounds;

        float playerHalfWidth = 0f;
        float playerHalfHeight = 0f;

        if (spriteRenderer != null)
        {
            playerHalfWidth = spriteRenderer.bounds.extents.x;
            playerHalfHeight = spriteRenderer.bounds.extents.y;
        }

        minX = bgBounds.min.x + playerHalfWidth + boundsPadding;
        maxX = bgBounds.max.x - playerHalfWidth - boundsPadding;
        minY = bgBounds.min.y + playerHalfHeight + boundsPadding;
        maxY = bgBounds.max.y - playerHalfHeight - boundsPadding;
    }

    void Update()
    {
        if (isKnockedBack)
            return;

        movement = Vector2.zero;

        // Touch controls
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                touchStart = touch.position;
                isDragging = true;
            }
            else if (touch.phase == TouchPhase.Moved ||
                     touch.phase == TouchPhase.Stationary)
            {
                if (isDragging)
                {
                    Vector2 dragDirection =
                        touch.position - touchStart;

                    if (dragDirection.magnitude > 20f)
                        movement = dragDirection.normalized;
                }
            }
            else if (touch.phase == TouchPhase.Ended ||
                     touch.phase == TouchPhase.Canceled)
            {
                isDragging = false;
                movement = Vector2.zero;
            }
        }

        // Mouse controls for Simulator testing
        if (Input.GetMouseButtonDown(0))
        {
            touchStart = Input.mousePosition;
            isDragging = true;
        }

        if (Input.GetMouseButton(0) && isDragging)
        {
            Vector2 dragDirection =
                (Vector2)Input.mousePosition - touchStart;

            if (dragDirection.magnitude > 20f)
                movement = dragDirection.normalized;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            movement = Vector2.zero;
        }

        // Flip diver sprite
        if (spriteRenderer != null)
        {
            if (movement.x > 0.05f)
                spriteRenderer.flipX = false;
            else if (movement.x < -0.05f)
                spriteRenderer.flipX = true;
        }
    }

    void FixedUpdate()
    {
        if (rb == null || isKnockedBack)
            return;

        Vector2 nextPosition =
            rb.position +
            movement * speed * Time.fixedDeltaTime;

        nextPosition = ClampPosition(nextPosition);

        rb.MovePosition(nextPosition);
    }

    Vector2 ClampPosition(Vector2 position)
    {
        // Keep diver inside background
        if (background != null)
        {
            position.x = Mathf.Clamp(position.x, minX, maxX);
            position.y = Mathf.Clamp(position.y, minY, maxY);
        }

        // Keep diver inside camera view
        if (stayInsideCamera && mainCamera != null &&
            mainCamera.orthographic)
        {
            float halfHeight = mainCamera.orthographicSize;
            float halfWidth = halfHeight * mainCamera.aspect;

            float playerHalfWidth = 0f;
            float playerHalfHeight = 0f;

            if (spriteRenderer != null)
            {
                playerHalfWidth = spriteRenderer.bounds.extents.x;
                playerHalfHeight = spriteRenderer.bounds.extents.y;
            }

            float camMinX =
                mainCamera.transform.position.x -
                halfWidth + playerHalfWidth;

            float camMaxX =
                mainCamera.transform.position.x +
                halfWidth - playerHalfWidth;

            float camMinY =
                mainCamera.transform.position.y -
                halfHeight + playerHalfHeight;

            float camMaxY =
                mainCamera.transform.position.y +
                halfHeight - playerHalfHeight;

            position.x = Mathf.Clamp(position.x, camMinX, camMaxX);
            position.y = Mathf.Clamp(position.y, camMinY, camMaxY);
        }

        return position;
    }

    public void TakeHit(Vector2 hazardPosition)
    {
        if (isKnockedBack || rb == null)
            return;

        if (knockbackCoroutine != null)
            StopCoroutine(knockbackCoroutine);

        knockbackCoroutine =
            StartCoroutine(KnockbackEffect(hazardPosition));
    }

    IEnumerator KnockbackEffect(Vector2 hazardPosition)
    {
        isKnockedBack = true;

        Vector2 startPosition = rb.position;
        Vector2 direction = startPosition - hazardPosition;

        // Determine the direction away from the rock
        if (direction.sqrMagnitude < 0.001f)
            direction = -movement;

        if (direction.sqrMagnitude < 0.001f)
            direction = Vector2.down;

        direction.Normalize();

        Vector2 targetPosition = ClampPosition(
            startPosition + direction * knockbackDistance
        );

        float elapsed = 0f;

        while (elapsed < knockbackDuration)
        {
            float t = Mathf.Clamp01(
                elapsed / knockbackDuration
            );

            float smoothT = t * t * (3f - 2f * t);

            rb.MovePosition(
                Vector2.Lerp(startPosition, targetPosition, smoothT)
            );

            elapsed += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        rb.MovePosition(targetPosition);

        movement = Vector2.zero;
        isKnockedBack = false;
        knockbackCoroutine = null;
    }
}
