using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using Utils.Pool;
using Game.Core;

namespace Game.UI
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Player player;
        private PoolSpawner spawner;
        private int maxHealth = 0;
        private Image[] cells;

        private void Awake()
        {
            spawner = GetComponent<PoolSpawner>();
        }
        private void OnEnable()
        {
            player.HealthUpdate += UpdateHealth;
            cells = new Image[maxHealth];
            for (int i = 0; i < maxHealth; i++)
                cells[i] = spawner.Dequeue().GetComponent<Image>();
            UpdateHealth(player.Health);
        }
        private void OnDisable()
        {
            for (int i = 0; i < maxHealth; i++)
                spawner.Enqueue(cells[i].gameObject);
            player.HealthUpdate -= UpdateHealth;
        }

        public void SetPlayerRules(int lifeCount)
        {
            OnDisable();
            maxHealth = lifeCount;
            OnEnable();
        }

        private void UpdateHealth(int health)
        {
            if (maxHealth < health)
                return;
            for (int i = 0; i < health; i++)
                cells[i].color = Color.red;
            for (int i = health; i < maxHealth; i++)
                cells[i].color = Color.white;
        }
    }
}