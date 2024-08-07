﻿using System.Collections;
using UnityEngine.Events;
using UnityEngine;
using Data;

namespace Game.Core
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private int maxHealth = 3;
        [SerializeField] private float speedFollow = 0.1f;
        [SerializeField] private float damageCooldown = 0.5f;
        private bool rulesImmortality = false;
        private bool inImmortality = false;
        private bool isActive = false;
        private int health = 3;

        public int Health => health;
        public const float RADIUS = 0.25f;

        [SerializeField] private Transform player;
        [SerializeField] private Transform target;
        [SerializeField] private GameObject deathEffect;
        private Coroutine immortalityActivity;
        private GameObject preset;

        private UnityEvent<int> healthEvent = new UnityEvent<int>();
        private UnityEvent deathEvent = new UnityEvent();

        public int MaxHealth => maxHealth;
        public event UnityAction<int> HealthUpdate
        {
            add => healthEvent.AddListener(value);
            remove => healthEvent.RemoveListener(value);
        }
        public event UnityAction DeathCaller
        {
            add => deathEvent.AddListener(value);
            remove => deathEvent.RemoveListener(value);
        }

        public PositionDelegate GetPositionDelegate => GetPosition;
        public ImmortalityDelegate GetImmortalityDelegate => SetImmortality;
        public HealthDelegate GetDamageDelegate => Damage;
        public HealthDelegate GetHealDelegate => Heal;

        public void Update()
        {
            float targetLocal = 0, rotVelocity = 0;
            Vector2 offset = player.position - target.localPosition;

            if (offset.sqrMagnitude > 0.01f)
            { targetLocal = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg + 90; }

            player.localEulerAngles = new Vector3(0, 0, Mathf.SmoothDampAngle(player.localEulerAngles.z, targetLocal, ref rotVelocity, 0.02f));
        }
        private void FixedUpdate()
        {
            if (isActive)
                player.position = Vector3.Lerp(player.position, target.position, speedFollow);
        }

        public void SetPlayerPreset(Skin skin, bool immortality, int lifeCount)
        {
            maxHealth = lifeCount;
            rulesImmortality = immortality;
            if (preset == null)
                preset = Instantiate(skin.Preset, player);
            Enable();
        }
        public void Enable()
        {
            health = maxHealth;
            preset.SetActive(true);
            isActive = true;
        }
        public void OnDisable()
        {
            isActive = false;
            preset.SetActive(false);
        }

        public void SetImmortality(float time)
        {
            if (immortalityActivity != null)
                StopCoroutine(immortalityActivity);
            inImmortality = true;
            immortalityActivity = StartCoroutine(ImmortalityActivity(time));
        }
        public void Heal(int count = 1)
        {
            health += count;
            if (health > maxHealth)
                health = maxHealth;
            healthEvent.Invoke(health);
        }
        public void Damage(int count = 1)
        {
            if (inImmortality)
                return;

            if (!rulesImmortality)
            {
                health -= count;
                Destroy(Instantiate(deathEffect, GetPosition(), Quaternion.identity), 2f);
            }
            if (health <= 0)
            {
                healthEvent.Invoke(0);
                deathEvent.Invoke();
                OnDisable();
            }
            else
            {
                healthEvent.Invoke(health);
                SetImmortality(damageCooldown);
            }
        }

        private IEnumerator ImmortalityActivity(float time)
        {
            yield return new WaitForSeconds(time);
            inImmortality = false;
        }

        private Vector2 GetPosition()
        {
            return player.position;
        }
    }

    public delegate Vector2 PositionDelegate();
    public delegate void HealthDelegate(int count);
    public delegate void ImmortalityDelegate(float time);
}
