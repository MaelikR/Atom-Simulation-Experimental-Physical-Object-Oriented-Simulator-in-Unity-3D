using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Fusion;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonCamera : NetworkBehaviour, IDamageable
{
    [Header("References")]
    public Transform fpsCameraHolder;
    public Transform tpsCameraHolder;
    public Camera fpsCamera;
    public Camera tpsCamera;
    public BloodFootstepSpawner bloodStepSpawner;

    [Header("Movement Settings")]
    public float mouseSensitivity = 2f;
    public float moveSpeed = 5f;
    public float swimSpeed = 3f;
    public float gravity = -9.81f;
    public float jumpHeight = 1.5f;
    public float flySpeed = 6f;

    [Header("Animation")]
    public Animator animator;

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

    [Header("FX")]
    public GameObject bloodDecalPrefab;

    [Header("Disable For Remote Players")]
    public List<GameObject> objectsToDisableIfNotLocal;
    public List<MonoBehaviour> componentsToDisableIfNotLocal;

    private CharacterController controller;
    private Vector3 velocity;
    private float xRotation = 0f;
    private bool isSwimming = false;
    private bool isFlying = false;
    private bool isReady = false;
    private bool isFPS = true;
    public Transform firePointLeftHand;

    [SerializeField] private GameObject solarAtomPrefab;
    [SerializeField] private Transform castOrigin;

    void FireSolarProjectile()
    {
        if (!Runner.IsRunning || !Object.HasInputAuthority) return;

        animator?.SetTrigger("Fire");

        Runner.Spawn(solarAtomPrefab, castOrigin.position, castOrigin.rotation, Object.InputAuthority);
    }

    public override void Spawned()
    {
        controller = GetComponent<CharacterController>();
        currentHealth = maxHealth;
        lastDamageTime = -999f;

        if (!Object.HasInputAuthority)
        {
            controller.enabled = false;
            DisableRemoteVisuals();
            return;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        SetCameraMode(true);

        SpawnHealthBar();
        isReady = true;
    }

    void SetCameraMode(bool fps)
    {
        isFPS = fps;
        fpsCamera.enabled = fps;
        tpsCamera.enabled = !fps;

        fpsCameraHolder.gameObject.SetActive(fps);
        tpsCameraHolder.gameObject.SetActive(!fps);

        var collisionScript = tpsCamera.GetComponent<CameraCollision>();
        if (collisionScript) collisionScript.enabled = !fps;
    }

    void DisableRemoteVisuals()
    {
        foreach (GameObject go in objectsToDisableIfNotLocal)
            if (go != null) go.SetActive(false);

        foreach (MonoBehaviour comp in componentsToDisableIfNotLocal)
            if (comp != null) comp.enabled = false;
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

        if (currentHealth <= 0f) Die();
        if (healthBar != null) healthBar.UpdateHealth(currentHealth);
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
        if (Object.HasInputAuthority) Debug.Log("Player is dead!");
        Runner.StartCoroutine(Respawn());
    }

    void SpawnHealthBar()
    {
        if (healthBarPrefab == null) return;
        var ui = Instantiate(healthBarPrefab, transform.position + Vector3.up * 2f, Quaternion.identity);
        healthBar = ui.GetComponent<HealthBarUI>();
        if (healthBar != null) healthBar.Setup(transform, maxHealth);
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(respawnDelay);
        currentHealth = maxHealth;
        if (respawnPoint != null) transform.position = respawnPoint.position;
        if (healthBar != null) healthBar.UpdateHealth(currentHealth);
        else SpawnHealthBar();
    }

    void Update()
    {
        if (!isReady || !Object.HasInputAuthority || Time.timeScale == 0f) return;

        if (Input.GetKeyDown(KeyCode.F)) SetCameraMode(!isFPS);
        if (Input.GetKeyDown(KeyCode.A)) FireSolarProjectile();
        if (Input.GetKeyDown(KeyCode.V)) isFlying = !isFlying;

        if (animator != null)
            animator.SetBool("Fly", isFlying); // 
        if (healthBar != null)
            healthBar.UpdateHealth(currentHealth);

        CheckSwimmingState();
        HandleMouseLook();
        HandleMovement(); // 
    }


    void CheckSwimmingState()
    {
        float headHeight = isFPS ? fpsCamera.transform.position.y : tpsCamera.transform.position.y;
        isSwimming = headHeight < (waterSurfaceY - waterThreshold);
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -75f, 75f);

        if (isFPS && fpsCameraHolder != null)
            fpsCameraHolder.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        else if (!isFPS && tpsCameraHolder != null)
            tpsCameraHolder.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        transform.Rotate(Vector3.up * mouseX);
    }

    void HandleMovement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        float y = 0f;

        if (isFlying)
        {
            if (Input.GetKey(KeyCode.Space)) y += 1f;
            if (Input.GetKey(KeyCode.LeftControl)) y -= 1f;
        }

        Vector3 moveInput = new Vector3(x, y, z);
        Vector3 moveDir = transform.TransformDirection(moveInput).normalized;

        float speed = new Vector3(moveDir.x, 0, moveDir.z).magnitude;

        if (isSwimming)
        {
            Vector3 swimMove = moveDir * swimSpeed;
            controller.Move(swimMove * Time.deltaTime);
            animator?.SetBool("Swim", true);
            animator?.SetFloat("Speed", speed);
            return;
        }
        else
        {
            animator?.SetBool("Swim", false);
        }
        if (isFlying)
        {
            float flyY = 0f;
            if (Input.GetKey(KeyCode.Space)) flyY += 1f;
            if (Input.GetKey(KeyCode.LeftControl)) flyY -= 1f;

            Vector3 flyInput = new Vector3(x, flyY, z);
            Vector3 flyMove = transform.TransformDirection(flyInput).normalized * flySpeed;

            controller.Move(flyMove * Time.deltaTime);

            if (animator != null)
            {
                animator.SetBool("Fly", true);
                animator.SetFloat("Speed", flyInput.magnitude);
            }

            return;
        }
        else
        {
            if (animator != null)
                animator.SetBool("Fly", false);
        }


        Vector3 horizontalMove = moveDir * moveSpeed;
        animator?.SetFloat("Speed", speed);

        if (!isFPS && z > 0.1f)
        {
            Vector3 lookDirection = new Vector3(moveDir.x, 0f, moveDir.z);
            if (lookDirection.sqrMagnitude > 0.01f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
            }
        }

        if (controller.isGrounded)
        {
            velocity.y = -1f;
            if (Input.GetButtonDown("Jump"))
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                animator?.SetTrigger("Jump");
            }
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }

        Vector3 finalMove = horizontalMove + Vector3.up * velocity.y;
        controller.Move(finalMove * Time.deltaTime);
    }
}
