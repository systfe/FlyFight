using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Ability
{
    public KeyCode key;
    public float cooldown;
    public Image cd_image;
    private bool on_cd;

    private delegate void Ability_T();

    public Ability(KeyCode key, float cooldown, Image cd_image = null)
    {
        this.key = key;
        this.cooldown = cooldown;
        this.cd_image = cd_image;
        this.on_cd = false;
    }

    public void Try_Use(MonoBehaviour owner, Action action, bool require_hold = true)
    {
        if (require_hold ? !Input.GetKey(key) : !Input.GetKeyDown(key)) return;
        if (on_cd) return;
        owner.StartCoroutine(Cooldown_Routine(action));
    }

    private IEnumerator Cooldown_Routine(Action action)
    {
        action?.Invoke();
        on_cd = true;
        float elapsed = 0f;
        if (cd_image != null) cd_image.fillAmount = 1f;
        while (elapsed < cooldown)
        {
            elapsed += Time.deltaTime;
            if (cd_image != null) cd_image.fillAmount = 1f - Mathf.Clamp01(elapsed / cooldown);
            yield return null;
        }
        on_cd = false;
        if (cd_image != null) cd_image.fillAmount = 0f;
    }
}