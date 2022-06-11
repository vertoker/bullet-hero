using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using Utils.Pool;
using Game.Core;

namespace Game.UI
{
    public class HealthBar : MonoBehaviour
    {
        private PoolSpawner spawner;
        private int maxHealth = 0;
        private Image[] cells;
        private Player player;

        private void Awake()
        {
            spawner = GetComponent<PoolSpawner>();
        }
        private void OnEnable()
        {
            if (player != null)
            {
                player.HealthUpdate += UpdateHealth;
                maxHealth = player.MaxHealth;
                cells = new Image[maxHealth];
                for (int i = 0; i < maxHealth; i++)
                    cells[i] = spawner.Dequeue().GetComponent<Image>();
                UpdateHealth(maxHealth);
            }
        }
        private void OnDisable()
        {
            if (player != null)
            {
                for (int i = 0; i < maxHealth; i++)
                    spawner.Enqueue(cells[i].gameObject);
                player.HealthUpdate -= UpdateHealth;
            }
        }

        public void SetPlayer(Player player)
        {
            OnDisable();
            this.player = player;
            OnEnable();
        }

        private void UpdateHealth(int health)
        {
            for (int i = 0; i < health; i++)
                cells[i].color = Color.red;
            for (int i = health; i < maxHealth; i++)
                cells[i].color = Color.white;
        }
    }
}