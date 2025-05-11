using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Fusion;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonCamera : NetworkBehaviour, IDamageable
{
    [Header("References")]
    public BloodFootstepSpawner bloodStepSpawner;
    // Tout en haut dans les variables privées :
    private bool isAutoRunning = false;

    [SerializeField] private SpellEffectManager spellEffectManager;

    [Header("Quantum Mechanics")]
    public GameObject particlePrefab;

    public float energy = 100f;
    public float energyConsumption = 20f;
    public float gravityForce = -9.81f;
    public float gravityDuration = 5f;
    private bool gravityInverted = false;
    private Vector3 pointA;
    private Vector3 pointB;
    private bool pointASet = false;
    
    [Header("Quantum Mechanics")]
    public float maxEnergy = 100f;

    private GameObject activeParticle;
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
    // 🔓 Ajoute ceci dans FirstPersonCamera.cs pour exposer les variables au GUI
    public float CurrentHealth => currentHealth;
    public bool IsFlying => isFlying;
    public bool IsSwimming => isSwimming;
    public bool IsAutoRunning => isAutoRunning;
    public float MaxHealth => maxHealth;
    [Header("Flying Mechanics")]


    public float flyAcceleration = 15f;
    public float maxFlySpeed = 20f;
    private Vector3 flyVelocity;

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
    private FirstPersonCamera playerCamera;

    void Start()
    {
        playerCamera = FindObjectOfType<FirstPersonCamera>();
    }

    public void CastSpell(string spellName)
    {
        if (playerCamera != null)
        {
            playerCamera.ExecuteSpell(spellName);
        }
        else
        {
            Debug.LogError("FirstPersonCamera introuvable.");
        }
    }
    void FireSolarProjectile()
    {
        if (!Runner.IsRunning || !Object.HasInputAuthority) return;

       // animator?.SetTrigger("Fire");

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

        SetupSmartCamera();

        SpawnHealthBar();
        isReady = true;
    }

    void SetupSmartCamera()
    {
        if (!Object.HasInputAuthority) return;

        GameObject cam = GameObject.FindWithTag("MainCamera");
        if (cam != null)
        {
            cam.transform.SetParent(transform);
            cam.transform.localPosition = new Vector3(0f, 1.6f, 0f); // hauteur des yeux
            cam.transform.localRotation = Quaternion.identity;
        }
        else
        {
            Debug.LogWarning("No MainCamera found in scene.");
        }
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
        var health = GetComponent<PlayerHealth>();
        if (healthBar != null && health != null)
            healthBar.Setup(health); // ✅

    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(respawnDelay);
        currentHealth = maxHealth;
        if (respawnPoint != null) transform.position = respawnPoint.position;
     
        else SpawnHealthBar();
    }

    void Update()
    {
        // ✅ Vérification initiale : Sortie immédiate si conditions non remplies
        if (!isReady || !Object.HasInputAuthority || Time.timeScale == 0f || UIBlocker.IsUIOpen) return;

        // ✅ Actions de gameplay
        if (Input.GetKeyDown(KeyCode.F)) FireSolarProjectile();
        if (Input.GetKeyDown(KeyCode.V)) ToggleFlying();
        if (Input.GetKeyDown(KeyCode.B)) CondenserParticules();
        if (Input.GetKeyDown(KeyCode.P)) TeleportationQuantique();
        if (Input.GetKeyDown(KeyCode.Alpha5)) SetPointA();
        if (Input.GetKeyDown(KeyCode.Alpha6)) SetPointB();
        if (Input.GetKeyDown(KeyCode.J)) AbsorberEnergie();
        if (Input.GetKeyDown(KeyCode.G)) ToggleGravity();
        if (Input.GetKeyDown(KeyCode.R)) isAutoRunning = !isAutoRunning;

        // ✅ Gestion des états (nage)
        UpdateSwimmingState();

        // ✅ Gestion du mouvement
        if (isFlying)
        {
            HandleFlyingMovement();
        }
        else
        {
            HandleMovement();
        }

        // ✅ Gestion de la caméra
        HandleCameraLook();

        // ✅ Animation de vol
        if (animator != null)
            animator.SetBool("Fly", isFlying);
    }


    void CondenserParticules()
    {
        if (energy < energyConsumption) return;
        if (activeParticle != null) Destroy(activeParticle);

        energy -= energyConsumption;
        Vector3 spawnPos = transform.position + transform.forward * 2;
        activeParticle = Instantiate(particlePrefab, spawnPos, Quaternion.identity);
        Destroy(activeParticle, 10f); // Durée de vie de la particule
        Debug.Log("Particule condensée.");
    }

    // ✅ Téléportation Quantique
    void SetPointA()
    {
        pointA = transform.position;
        pointASet = true;
        Debug.Log("Point A défini.");
    }

    void SetPointB()
    {
        pointB = transform.position;
        Debug.Log("Point B défini.");
    }

    void TeleportationQuantique()
    {
        if (!pointASet)
        {
            Debug.Log("Point A non défini.");
            return;
        }

        float distance = Vector3.Distance(pointA, pointB);
        if (distance > 20f)
        {
            Debug.Log("Distance trop grande pour la téléportation.");
            return;
        }

        if (energy >= energyConsumption)
        {
            energy -= energyConsumption;
            transform.position = pointB;
            Debug.Log("Téléporté de A à B.");
        }
        else
        {
            Debug.Log("Pas assez d'énergie pour se téléporter.");
        }
    }

    // ✅ Absorption Énergétique
    void AbsorberEnergie()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 10f))
        {
            if (hit.collider.CompareTag("Energie"))
            {
                float absorbed = 20f;
                energy = Mathf.Min(energy + absorbed, maxEnergy);
                Debug.Log("Énergie absorbée : " + absorbed);
            }
            else if (hit.collider.CompareTag("Radiation"))
            {
                float damage = 10f;
                energy = Mathf.Max(energy - damage, 0f);
                Debug.Log("Radiation absorbée, dégâts subis : " + damage);
            }
        }
        else
        {
            // Régénération lente si aucune source
            energy = Mathf.Min(energy + Time.deltaTime * 5f, maxEnergy);
        }
    }

    // ✅ Distorsion de Gravité
    void ToggleGravity()
    {
        if (gravityInverted)
        {
            ResetGravity();
            return;
        }

        gravityInverted = true;
        Physics.gravity = new Vector3(0, Mathf.Abs(gravityForce), 0);
        Invoke("ResetGravity", gravityDuration);
        Debug.Log("Gravité inversée.");
    }

    void ResetGravity()
    {
        gravityInverted = false;
        Physics.gravity = new Vector3(0, gravityForce, 0);
        Debug.Log("Gravité réinitialisée.");
    }

    void HandleCameraLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -85f, 85f); // empêche de trop lever/baisser

        // Rotation verticale (haut/bas) sur l'axe X
        Camera.main.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Rotation horizontale (gauche/droite) sur le corps du joueur
        transform.Rotate(Vector3.up * mouseX);
    }
    // ✅ Méthode publique ExecuteSpell dans FirstPersonCamera
    public void ExecuteSpell(string spellName)
    {
        switch (spellName)
        {
            case "Spell of Light":
                spellEffectManager?.PlayEffect(spellName, firePointLeftHand);
                currentHealth = Mathf.Min(currentHealth + 25f, maxHealth);
                break;

            case "Arcane Blast":
                spellEffectManager?.PlayEffect(spellName, firePointLeftHand);
                break;

            case "Stone Shield":
                Debug.Log("Activating Stone Shield!");
                spellEffectManager?.PlayEffect(spellName, firePointLeftHand);
                // Ajoutez ici un booléen de protection temporaire si vous le souhaitez
                break;

            default:
                Debug.LogWarning("Unknown spell: " + spellName);
                break;
        }
    }

    public static class UIBlocker
    {
        public static bool IsUIOpen = false;
    }

   
    // ✅ Mise à jour du mouvement
    void HandleMovement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // ✅ Gestion de l'auto-run
        if (isAutoRunning && z == 0f) z = 1f;

        // ✅ Gestion de la natation
        if (isSwimming)
        {
            HandleSwimmingMovement(x, z);
            return;
        }

        // ✅ Gestion du vol
        if (isFlying)
        {
            HandleFlyingMovement();
            return;
        }

        // ✅ Mouvement standard (sol)
        HandleGroundMovement(x, z);
    }

    // ✅ Gestion de la natation (swim)
    void HandleSwimmingMovement(float x, float z)
    {
        Vector3 input = new Vector3(x, 0, z);
        Vector3 swimDirection = transform.TransformDirection(input).normalized;
        Vector3 swimVelocity = swimDirection * swimSpeed;
        swimVelocity.y = 0f;

        if (Input.GetKey(KeyCode.Space)) swimVelocity.y += swimSpeed * 0.5f;
        if (Input.GetKey(KeyCode.LeftControl)) swimVelocity.y -= swimSpeed * 0.5f;

        controller.Move(swimVelocity * Time.deltaTime);

        // Animation de natation
        if (animator != null)
        {
            animator.SetBool("Swim", true);
            animator.SetFloat("Speed", swimVelocity.magnitude);
        }
    }

    // ✅ Gestion du vol (fly)
    // ✅ Gestion du vol (fly) avec montée/descente fluide
    void HandleFlyingMovement()
    {
        // ✅ Capture de l'input (horizontal et avant/arrière)
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // ✅ Calcul de la direction de vol
        Vector3 input = new Vector3(x, 0, z);
        Vector3 flyDirection = transform.TransformDirection(input).normalized;
        Vector3 flyVelocity = flyDirection * flySpeed;

        // ✅ Contrôle de l'altitude (espace et contrôle) avec douceur
        float targetVerticalSpeed = 0f;

        if (Input.GetKey(KeyCode.Space))
            targetVerticalSpeed = flySpeed;
        else if (Input.GetKey(KeyCode.LeftControl))
            targetVerticalSpeed = -flySpeed;

        // ✅ Utilisation de Mathf.Lerp pour lisser la transition
        float smoothVerticalSpeed = Mathf.Lerp(velocity.y, targetVerticalSpeed, Time.deltaTime * 5f);
        flyVelocity.y = smoothVerticalSpeed;

        // ✅ Application de la vélocité (mouvement global)
        controller.Move(flyVelocity * Time.deltaTime);

        // ✅ Animation de vol (activation uniquement si le joueur est en mouvement)
        if (animator != null)
        {
            animator.SetBool("Fly", true);
            animator.SetFloat("Speed", flyVelocity.magnitude);
        }
    }



    // ✅ Mouvement standard (sol)
    void HandleGroundMovement(float x, float z)
    {
        Vector3 moveInput = new Vector3(x, 0, z);
        Vector3 moveDir = transform.TransformDirection(moveInput).normalized;
        float speed = moveDir.magnitude;
        Vector3 horizontalMove = moveDir * moveSpeed;

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

        // Animation de course/marche
        if (animator != null)
        {
            animator.SetBool("Fly", false);
            animator.SetBool("Swim", false);
            animator.SetFloat("Speed", speed);
        }
    }

    // ✅ Mise à jour de l'état de natation
    void UpdateSwimmingState()
    {
        float playerY = transform.position.y;
        isSwimming = (playerY < waterSurfaceY - waterThreshold);
    }

    // ✅ Activation du vol (Toggle)
    void ToggleFlying()
    {
        isFlying = !isFlying;
        if (!isFlying) velocity = Vector3.zero; // Réinitialiser la vélocité au sol

        if (animator != null)
        {
            animator.SetBool("Fly", isFlying);
            animator.SetBool("Swim", false);
        }
    }
}
