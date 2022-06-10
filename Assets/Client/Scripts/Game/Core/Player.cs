using System.Collections;
using UnityEngine.Events;
using UnityEngine;

namespace Game.Core
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private int maxHealth = 3;
        [SerializeField] private float damageCooldown = 0.5f;

        private bool inImmortality = false;
        private int health = 3;

        public const float RADIUS = 0.25f;

        private Transform tr;
        private Coroutine immortalityActivity;
        private UnityEvent<int, int> healthEvent = new UnityEvent<int, int>();

        public event UnityAction<int, int> HealthUpdate
        {
            add => healthEvent.AddListener(value);
            remove => healthEvent.RemoveListener(value);
        }

        public PositionDelegate GetPositionDelegate => GetPosition;
        public ImmortalityDelegate GetImmortalityDelegate => SetImmortality;
        public HealthDelegate GetDamageDelegate => Damage;
        public HealthDelegate GetHealDelegate => Heal;


        private void Awake()
        {
            tr = GetComponent<Transform>();
            Activate();
        }

        public void Activate()
        {
            health = maxHealth;
        }
        public void DeActivate()
        {

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
            healthEvent.Invoke(health, maxHealth);
        }
        public void Damage(int count = 1)
        {
            if (inImmortality)
                return;

            healthEvent.Invoke(health, maxHealth);
            health -= count;
            if (health <= 0)
            {
                health = 0;
                DeActivate();
            }
            else
            {
                inImmortality = true;
                StartCoroutine(ImmortalityActivity(damageCooldown));
            }
        }

        private IEnumerator ImmortalityActivity(float time)
        {
            yield return new WaitForSeconds(time);
            inImmortality = false;
        }

        private Vector2 GetPosition()
        {
            return tr.position;
        }
    }

    public delegate Vector2 PositionDelegate();
    public delegate void HealthDelegate(int count);
    public delegate void ImmortalityDelegate(float time);
}
