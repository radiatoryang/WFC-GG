﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class SkillControl : MonoBehaviour
{
    public SkillListScriptableObject equippedSkill;
    public GameObject EquippedPosition;
    public GameObject attackButton;
    public GameObject spellIcon;
    public GameObjectList enemiesInRange;
    public static GameObject tmpSkill;
    public ParticleSystem attack;
    public ParticleSystem hit;
    public SkillListScriptableObject equippedAimed;
    PlayerControl playerControl;

    void Start()
    {
        tmpSkill = Instantiate(equippedSkill.list[0].skillPrefab, EquippedPosition.transform.position, EquippedPosition.transform.rotation * Quaternion.Euler(new Vector3(0, 0, 60)), EquippedPosition.transform);
       
        enemiesInRange.list.Clear();
        attack = Instantiate(attack, transform);
        playerControl = GetComponent<PlayerControl>();
        attackButton.GetComponent<Image>().sprite = equippedSkill.list[0].icon;
        spellIcon.GetComponent<Image>().sprite = equippedAimed.list[0].icon;
        hit = Instantiate(hit, gameObject.transform);
    }


    public void Attack()
    {
        if (!attack.isPlaying)
        {
            attack.Play();
            playerControl.anim.SetBool("swordSlash", true);

            foreach (var item in enemiesInRange.list)
            {
                item.GetComponent<EnemyIO>().TakeDamage(equippedSkill.list[0].dmg);
            }
        }
    }

    public void ChangeEquippedSkill(SkillListScriptableObject newWeapon)
    {

        if (!newWeapon.list[0].isRanged) { 
        Destroy(tmpSkill);
         equippedSkill.list.Clear();
        equippedSkill.list.Add( newWeapon.list[0]);
        tmpSkill = Instantiate(equippedSkill.list[0].skillPrefab, EquippedPosition.transform.position, EquippedPosition.transform.rotation * Quaternion.Euler(new Vector3(0,0,60)), EquippedPosition.transform);
        attackButton.GetComponent<Image>().sprite = equippedSkill.list[0].icon;
        }
        else
        {
            equippedAimed.list.Clear();
            equippedAimed.list.Add(newWeapon.list[0]);
            spellIcon.GetComponent<Image>().sprite = equippedAimed.list[0].icon;
        }

    }

    public void RangedAttack(Vector2 attackDir )
    {
        attackDir.Normalize();
        GameObject proj = Instantiate(equippedAimed.list[0].skillPrefab, transform.position + new Vector3(attackDir.x/2, 0.4f, attackDir.y/2), Quaternion.Euler(0, Vector2.SignedAngle( attackDir, Vector2.up), 0));
        hit.startColor = equippedAimed.list[0].hitProjectileColor;
        proj.GetComponent<Projectile>().skillcontrol = this;
        proj.GetComponent<Rigidbody>().AddForce(new Vector3(attackDir.x,0,attackDir.y) * 100);
        proj.GetComponent<Projectile>().attackDamage = equippedAimed.list[0].dmg;
    }
 }
