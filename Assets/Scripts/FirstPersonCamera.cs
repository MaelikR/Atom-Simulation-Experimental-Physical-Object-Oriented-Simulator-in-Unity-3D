using UnityEngine;
using System.Collections;
using Fusion;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonCamera : NetworkBehaviour, IDamageable
{
    [Header("References")]
    public Transform cameraHolder;
    public Transform cameraTransform;
    public BloodFootstepSpawner bloodStepSpawner;

    [Header("Movement Settings")]
    public float mouseSensitivity = 2f;
    public float moveSpeed = 5f;
    public float swimSpeed = 3f;
    public float gravity = -9.81f;
    public float jumpHeight = 1.5f;

    [Header("Swimming Settings")]
    public float waterSurfaceY = 0f;
    public float waterThreshold = 1f;

    [Header("Health Settings")]
    public float maxHealth = 100f;
    [Networked] private float currentHealth { get; set; }
    [Networked] private float lastDamageTime { get; set; }
    public float damageCooldown = 1.5f;

    [Header("Blood FX")]
    public GameObject bloodImpactPrefab;
    public Transform bloodSpawnPoint;

    public Transform respawnPoint;
    public float respawnDelay = 3f;
    public GameObject healthBarPrefab;

    private HealthBarUI healthBar;
    private Transform healthBarInstance;

    [Header("FX")]
    public GameObject bloodDecalPrefab;

    private CharacterController controller;
    private Vector3 velocity;
    private float xRotation = 0f;
    private bool isSwimming = false;

    public override void Spawned()
    {
        controller = GetComponent<CharacterController>();
        currentHealth = maxHealth;
        lastDamageTime = -999f;

        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;

        if (cameraHolder == null)
            cameraHolder = cameraTransform.parent;

        if (Object.HasInputAuthority)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        SpawnHealthBar();
    }

    public void TakeDamage(float amount, GameObject source = null)
    {
        if (Time.time - lastDamageTime < damageCooldown) return;

        currentHealth -= amount;
        lastDamageTime = Time.time;

        if (Object.HasInputAuthority)
        {
            SpawnBloodFX();
            bloodStepSpawner?.Activate();
        }

        if (currentHealth <= 0f)
        {
            Die();
        }

        if (healthBar != null)
            healthBar.UpdateHealth(currentHealth);
    }

    void SpawnBloodFX()
    {
        if (bloodImpactPrefab == null) return;

        Vector3 spawnPos = bloodSpawnPoint != null ? bloodSpawnPoint.position : transform.position + Vector3.up * 1.5f;
        Quaternion spawnRot = Quaternion.LookRotation(-transform.forward);
        spawnRot *= Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);

        GameObject blood = Instantiate(bloodImpactPrefab, spawnPos, spawnRot);
        blood.transform.localScale *= Random.Range(0.8f, 1.2f);
    }

    void Die()
    {
        if (Object.HasInputAuthority)
            Debug.Log("Player is dead!");

        Runner.StartCoroutine(Respawn());
    }

    void SpawnHealthBar()
    {
        if (healthBarPrefab == null) return;

        var ui = Instantiate(healthBarPrefab, transform.position + Vector3.up * 2f, Quaternion.identity);
        healthBar = ui.GetComponent<HealthBarUI>();
        if (healthBar != null)
            healthBar.Setup(transform, maxHealth);
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(respawnDelay);

        currentHealth = maxHealth;

        if (respawnPoint != null)
            transform.position = respawnPoint.position;

        if (healthBar != null)
            healthBar.UpdateHealth(currentHealth);
        else
            SpawnHealthBar();
    }

    void Update()
    {
        if (!Object.HasInputAuthority) return;
        if (Time.timeScale == 0f) return;
        if (healthBar != null)
            healthBar.UpdateHealth(currentHealth);

        CheckSwimmingState();
        HandleMouseLook();
        HandleMovement();
    }

    void CheckSwimmingState()
    {
        float headHeight = cameraTransform.position.y;
        isSwimming = headHeight < (waterSurfaceY - waterThreshold);
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cameraHolder.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    void HandleMovement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        if (isSwimming)
        {
            float y = 0f;
            if (Input.GetKey(KeyCode.Space)) y += 1f;
            if (Input.GetKey(KeyCode.LeftControl)) y -= 1f;
            move += Vector3.up * y;
            controller.Move(move * swimSpeed * Time.deltaTime);
            velocity = Vector3.zero;
        }
        else
        {
            controller.Move(move * moveSpeed * Time.deltaTime);

            if (controller.isGrounded && velocity.y < 0)
                velocity.y = -2f;

            if (Input.GetButtonDown("Jump") && controller.isGrounded)
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }
    }
}
