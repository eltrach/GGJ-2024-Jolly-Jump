using Sirenix.OdinInspector;
using UnityEngine;

public class ThirdPersonMage : IHuman
{
    // Define the available spells
    public Spell[] spells;

    // Define the mana or energy system
    public float maxMana = 100f;
    public float manaRegenerationRate = 10f;
    [SerializeField, ProgressBar(0, "maxMana", ColorGetter = "GetMageBarColor"), ReadOnly] private float currentMana = 100;

    // Define the cooldown system
    private float[] spellCooldowns;

    // Define the spawn point for the spell projectile
    public Transform spellSpawnPoint;

    public LayerMask aimColliderLayerMask = new();

    // Define the currently selected spell
    private Spell selectedSpell;
    private Animator animator;
    private ThirdPersonInputs input;

    public bool debug;
    private Camera mainCamera;
    public Transform debugTransform;

    public float CurrentMana { get => currentMana; set => currentMana = value; }

    // Initialize the mage character
    private void Start()
    {
        CurrentMana = maxMana;
        spellCooldowns = new float[spells.Length];
        selectedSpell = spells[0]; // Set the initial spell

        // Get the Animator component
        animator = GetComponent<Animator>();
        input = GetComponent<ThirdPersonInputs>();
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    private void Update()
    {
        // Regenerate mana over time
        CurrentMana += manaRegenerationRate * Time.deltaTime;
        CurrentMana = Mathf.Clamp(CurrentMana, 0f, maxMana);
        if (CurrentMana < 100) { GlobalRoot.PlayerUI.UpdateManaBar(); Debug.Log("updating bar"); }

        // Update spell cooldowns
        for (int i = 0; i < spellCooldowns.Length; i++)
        {
            if (spellCooldowns[i] > 0f)
            {
                spellCooldowns[i] -= Time.deltaTime;
            }
        }

        // Check for spell input and cast the selected spell
        if (input.Shot)
        {
            if (CanCastSpell())
            {
                CastSpell();
                GlobalRoot.PlayerUI.UpdateManaBar();
            }
        }
    }

    // Check if the mage has enough mana and the selected spell is off cooldown
    private bool CanCastSpell()
    {
        return CurrentMana >= selectedSpell.manaCost && spellCooldowns[selectedSpell.spellIndex] <= 0f;
    }

    // Cast the selected spell
    private void CastSpell()
    {
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f); // screen center point
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);

        if (Physics.Raycast(ray, out RaycastHit raycastHit, Mathf.Infinity, aimColliderLayerMask))
        {
            debugTransform.position = raycastHit.point;

            // Deduct mana cost
            CurrentMana -= selectedSpell.manaCost;

            // Play the spell casting animation
            animator.SetTrigger("CastSpell");
            RotateThePlayerToCameraRot();

            // Set the target position of the spell to the raycast hit point
            Vector3 targetPosition = raycastHit.point;

            // Instantiate the spell projectile
            SpellProjectile spellProjectile = Instantiate(selectedSpell.spellPrefab, spellSpawnPoint.position, spellSpawnPoint.rotation);

            // Assign properties to the spell projectile
            SpellProjectile spellProjectileScript = spellProjectile;
            spellProjectileScript.Initialize(selectedSpell, targetPosition);

            // Start the cooldown for the selected spell
            spellCooldowns[selectedSpell.spellIndex] = selectedSpell.cooldownDuration;
        }
    }
    private void RotateThePlayerToCameraRot()
    {
        // Set the target rotation to match the camera's rotation, while preserving the y-rotation of the player
        Quaternion targetRotation = mainCamera.transform.rotation;
        targetRotation.eulerAngles = new(transform.rotation.eulerAngles.x, targetRotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        transform.rotation = targetRotation;
    }

    // ~~Unfinished~~
    // Change the currently selected spell
    public void ChangeSpell(int spellIndex)
    {
        if (spellIndex >= 0 && spellIndex < spells.Length)
        {
            selectedSpell = spells[spellIndex];
        }
    }
    private void OnDrawGizmos()
    {
        if (!debug) return;
        // Draw a line representing the raycast for the shooted spell
        Gizmos.color = Color.red;
        Gizmos.DrawLine(spellSpawnPoint.position, debugTransform.position);
    }

    // Odin inspector methods
    private Color GetMageBarColor(float value)
    {
        return Color.Lerp(Color.red, Color.cyan, Mathf.Pow(value / 100f, 2));
    }
}
