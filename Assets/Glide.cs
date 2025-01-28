using UnityEngine;
using UnityEngine.UI;

public class SquirrelGlideController : MonoBehaviour
{
    public float glideSpeed = 5f;
    public float glideStaminaCostPerSecond = 20f;
    public float maxStamina = 100f;
    public float minStamina = 0f;
    public float gravityMultiplier = 0.5f;

    private float currentStamina;
    private bool isGliding = false;
    private bool isTransformed = false;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    public Image staminaBarForeground;

    public Color transformedColor = new Color(1f, 1f, 0f); // Yellowish for transformed
    public Color normalColor = Color.white; // White for normal form
    public Vector3 transformedScale = new Vector3(1f, 0.5f, 1f); // Flat for transformed version
    public Vector3 normalScale = new Vector3(1f, 1f, 1f); // Normal scale

    public float staminaRegenRate = 15f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentStamina = maxStamina;
        UpdateStaminaBar();
    }

    void Update()
    {
        HandleInput();
        HandleGlide();
        UpdateStamina();
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            ToggleSquirrelForm();
        }
    }

    private void ToggleSquirrelForm()
    {
        if (isTransformed)
        {
            // Revert to normal form
            isTransformed = false;
            StopGliding();
        }
        else if (currentStamina > minStamina) // Only transform if there is stamina
        {
            // Transform to squirrel
            isTransformed = true;
            StartGliding();
        }
    }

    private void StartGliding()
    {
        if (isTransformed && currentStamina > minStamina)
        {
            isGliding = true;
            spriteRenderer.color = transformedColor;
            transform.localScale = transformedScale;
            rb.gravityScale = gravityMultiplier;
        }
    }

    private void HandleGlide()
    {
        if (isGliding)
        {
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -glideSpeed));

            // Linear, smooth stamina drain
            currentStamina -= glideStaminaCostPerSecond * Time.deltaTime;

            if (currentStamina <= minStamina)
            {
                StopGliding();
            }
        }
    }

    private void StopGliding()
    {
        isGliding = false;
        isTransformed = false;
        spriteRenderer.color = normalColor;
        transform.localScale = normalScale;
        rb.gravityScale = 1;
    }

    private void UpdateStamina()
    {
        if (!isGliding && currentStamina < maxStamina)
        {
            currentStamina += staminaRegenRate * Time.deltaTime;
        }

        currentStamina = Mathf.Clamp(currentStamina, minStamina, maxStamina);

        UpdateStaminaBar();
    }

    private void UpdateStaminaBar()
    {
        if (staminaBarForeground != null)
        {
            float fillAmount = currentStamina / maxStamina;
            staminaBarForeground.fillAmount = fillAmount;
        }
    }
}
