using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowSpellScript : MonoBehaviour
{
    public GameObject spellPrefab;
    public float spellSpawnDist = 0.2f;
    public float spellSpawnHight = 1f;

    public void ThrowSpell()
    {
        Vector3 spawnPos = transform.position + new Vector3(0, spellSpawnHight) + (PlayerController.Instance.transform.position - transform.position).normalized * spellSpawnDist;
        GameObject spell = Instantiate(spellPrefab, spawnPos, Quaternion.identity);
        spell.GetComponent<SpellProjectile>().SetInfo(PlayerController.Instance.transform);
    }
}
