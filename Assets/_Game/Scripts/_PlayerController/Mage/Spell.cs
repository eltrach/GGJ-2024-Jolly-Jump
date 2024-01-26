using UnityEngine;

[CreateAssetMenu(fileName = "New Spell", menuName = "Spells/Spell")]
public class Spell : ScriptableObject
{
    public string spellName;
    public float manaCost;
    public float cooldownDuration;
    public int spellIndex;

    // Define the prefab for the spell projectile
    public SpellProjectile spellPrefab;
    public GameObject decalPrefab;

    // Additional properties, effects, and requirements for the spell
}
