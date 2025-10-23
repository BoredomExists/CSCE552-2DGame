using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the Inputs that the user can give the player
/// </summary>
public class UserInput : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;                        // Move speed of the player
    public float sprintSpeed = 12f;                     // Sprint Speed of the player
    public float jumpForce = 10f;                       // Jump Force of the player for jumping

    [Header("Ground Settings")]
    public Transform groundCheck;                       // Check to see if the player is on the "ground"
    public float groundCheckRadius = 0.2f;              // Extra check in case the groundCheck may not be interacting with the ground
    public LayerMask ground;
    public bool isGrounded;

    [Header("Air Settings")]
    public float airSpeed = 20f;                        // Move speed of player when in the air
    public float fastFallSpeed = 20f;                   // How fast the player falls down when pressing KeyCode.S

    [Header("Camera Settings")]
    public Transform mainCamera;                        // Main Camera that focuses on the player or room anchor when entered
    public Transform player;                            // Player
    public float rotationSpeed = 10f;                   // How fast the player rotates to new gravity field

    [Header("Player UI Settings")]
    public Image gravityIcon;                           // Gravity Icon Object to change sprite
    public Sprite gravityUp;                            // Gravity direction sprites to show the user the current direction of gravity
    public Sprite gravityDown;
    public Sprite gravityLeft;
    public Sprite gravityRight;
    public Image weaponType;                            // Weapon Type Image that changes based on weapon mode
    public Sprite swordSprite;                          // Changes sprite to sword when in sword mode
    public Sprite gauntletSprite;                       // Changes sprite to gauntlet when in gauntlet mode

    [Header("Weapon Settings")]
    public int playerDamage = 30;                       // Damage player can do
    public Transform projPOS;                           // Starting Position of projectile
    public GameObject projectilePrefab;                 // Prefab of player projectile
    public float projSpeed = 15f;                       // Projectile Speed


    private Rigidbody2D rb;
    private Vector2 moveVector;                         // Vector in which direction the player can move
    private float lastGroundSpeed;                      // Check the ground speed to manage air momentum
    private float zRotation = 0f;                       // Sets rotation of camera to 0 by default
    private CinemachineCamera ccam;                     // Cinemachine Camera
    private Camera activeCam;                           // Main Camera
    private bool isSwordUser = true;                    // Starting mode is Sword

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();                       // Gets the rigidbody
        lastGroundSpeed = moveSpeed;                            // Sets last ground speed to move speed
    
        ccam = FindFirstObjectByType<CinemachineCamera>();      // Gets Cinemachine Camera
        if (ccam != null)
            mainCamera = ccam.transform;

        activeCam = Camera.main;                                // Sets the active cam to the main camera
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = CheckIsGrounded();                                                                             // Check if the user is grounded
        float moveX = 0f;
        if (Input.GetKey(KeyCode.A)) moveX = -1f;                                                                   // Check if the player is moving left
        if (Input.GetKey(KeyCode.D)) moveX = 1f;                                                                    // Check if the player is moving right        

        moveVector = new Vector2(moveX, 0f);                                                                        // Set the vector in which direction the player is moving

        // Jump Function
        if (Input.GetButtonDown("Jump") && isGrounded)
            rb.AddForce(-Physics2D.gravity.normalized * jumpForce, ForceMode2D.Impulse);

        // Fast Fall Function
        if (Input.GetKeyDown(KeyCode.S) && !isGrounded)
            rb.linearVelocity += Physics2D.gravity.normalized * fastFallSpeed;

        // Swap weapon modes
        if (Input.GetKeyDown(KeyCode.F))
        {
            isSwordUser = !isSwordUser;
            SwapWeapon();
        }


        lastGroundSpeed = (Input.GetKey(KeyCode.LeftShift) && isGrounded) ? sprintSpeed : moveSpeed;               // Determines if the player is sprinting or not

        // Changes rotation settings based on arrow key input
        ChangeRotation();

        // Sets the rotation direction to turn to
        Quaternion rotationToTurnTo = Quaternion.Euler(0f, 0f, zRotation);

        // Changes the camera and player rotation
        if (mainCamera != null)
            if (!RoomCameraTrigger.roomEntered)
                mainCamera.rotation = Quaternion.Lerp(mainCamera.rotation, rotationToTurnTo, rotationSpeed * Time.deltaTime);
            else
                mainCamera.rotation = Quaternion.identity;
        if (player != null)
            player.rotation = Quaternion.Lerp(player.rotation, rotationToTurnTo, rotationSpeed * Time.deltaTime);
    }

    void FixedUpdate()
    {
        isGrounded = CheckIsGrounded();

        Vector2 lateralAxis = (Quaternion.Euler(0f, 0f, zRotation) * Vector2.right).normalized;         // Gets the current lateral axis representing the player moving left or right in the rotation frame
        float currentLateral = Vector2.Dot(rb.linearVelocity, lateralAxis);                             // Gets the dot product the current velocity onto the lateral to get the signed lateral speed

        float groundTarget = moveVector.x * lastGroundSpeed;                                            // Desired lateral speeds for on the ground
        float airTarget = moveVector.x * lastGroundSpeed;                                               // or in the air
        float finalLateral;

        if (isGrounded)
            finalLateral = groundTarget;
        else
            finalLateral = Mathf.MoveTowards(currentLateral, airTarget, airSpeed * Time.fixedDeltaTime);

        Vector2 lateralVelocity = lateralAxis * finalLateral;                                          // Reconstruct the new velocity
        Vector2 gravityVelocity = Vector2Project(rb.linearVelocity, Physics2D.gravity.normalized);     // Keep only the component of current velocity along gravity

        rb.linearVelocity = lateralVelocity + gravityVelocity;
    }

    private Vector2 Vector2Project(Vector2 a, Vector2 b)
    {
        if (b.sqrMagnitude < 1e-6f) return Vector2.zero;
        return (Vector2.Dot(a, b) / b.sqrMagnitude) * b;
    }

    // Check if the player is grounded
    public bool CheckIsGrounded()
    {
        if (groundCheck == null) return false;

        Vector2 gravityDirection = Physics2D.gravity.normalized;
        float castDistance = groundCheckRadius + 0.05f;

        RaycastHit2D hit = Physics2D.CircleCast(groundCheck.position, groundCheckRadius, gravityDirection, castDistance, ground);
        if (hit.collider != null)
        {
            float dot = Vector2.Dot(hit.normal, -gravityDirection);
            if (dot > 0.7f) return true;
        }
        return false;
    }

    // Handles input for rotating camera and player
    private void ChangeRotation()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            zRotation = 90f;
            ChangeGravity();
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            zRotation = -90f;
            ChangeGravity();
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            zRotation = 180f;
            ChangeGravity();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            zRotation = 0f;
            ChangeGravity();
        }
        ChangeGravityIcon();
    }

    // Creates the new gravity when the rotation is changed
    private void ChangeGravity()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();

        Vector2 oldGravity = Physics2D.gravity.sqrMagnitude < 1e-6f ? Vector2.down : Physics2D.gravity.normalized;
        Vector2 newGravity = Quaternion.Euler(0f, 0f, zRotation) * Vector2.down * 9.81f;
        Physics2D.gravity = newGravity;

        float deltaAngle = Vector2.SignedAngle(oldGravity, newGravity.normalized);
        rb.linearVelocity = RotateVector(rb.linearVelocity, deltaAngle);
    }

    // Rotates a vector by X degrees in 2D
    private Vector2 RotateVector(Vector2 v, float degrees)
    {
        float rad = degrees * Mathf.Deg2Rad;
        float c = Mathf.Cos(rad);
        float s = Mathf.Sin(rad);
        return new Vector2(v.x * c - v.y * s, v.x * s + v.y * c);
    }

    // Changes the gravity icon based on direction of gravity
    public void ChangeGravityIcon()
    {
        float z = Mathf.DeltaAngle(0f, player.eulerAngles.z);
        int snapped = Mathf.RoundToInt(z / 90f) * 90;
        if (snapped == -180) snapped = 180;
        switch (snapped)
        {
            case 0:
                gravityIcon.sprite = gravityDown;
                break;

            case 90:
                gravityIcon.sprite = gravityRight;
                break;

            case 180:
                gravityIcon.sprite = gravityUp;
                break;

            case -90:
                gravityIcon.sprite = gravityLeft;
                break;
        }
    }

    // Swaps weapon mode
    private void SwapWeapon()
    {
        if (isSwordUser)
        {
            weaponType.sprite = swordSprite;
            projPOS.gameObject.SetActive(false);
        }
        else
        {
            weaponType.sprite = gauntletSprite;
            projPOS.gameObject.SetActive(true);
        }
    }

    // Shoots gauntlet function when in gauntlet mode
    public void ShootGauntlet()
    {
        Camera cam = (activeCam != null) ? activeCam : Camera.main;

        Vector3 mouseScreen = Input.mousePosition;
        Vector3 screenPointWithZ = new Vector3(mouseScreen.x, mouseScreen.y, cam.WorldToScreenPoint(projPOS.position).z);
        Vector3 mouseWorld = cam.ScreenToWorldPoint(screenPointWithZ);

        Vector2 direction = (mouseWorld - projPOS.position).normalized;
        var projGO = Instantiate(projectilePrefab, projPOS.position, Quaternion.identity);
        var projComp = projGO.GetComponent<Projectile>();
        projComp.Fire(direction, playerDamage, Projectile.ProjectileOwner.Player);
    }

    // Gets if the player is in sword mode
    public bool GetSwordUser()
    {
        return isSwordUser;
    }

    // Gets and set the player damage (Meant for Secret 🤫)
    public int GetDamage()
    {
        return playerDamage;
    }

    public void SetDamage(int damage)
    {
        playerDamage = damage;
    }
}
