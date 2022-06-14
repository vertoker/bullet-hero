using System.Collections;
using System.Collections.Generic;
using Game.Provider;
using UnityEngine;
using UI;

namespace Game.Core
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] private Player player;
        [SerializeField] private GameObject loseWindow;
        [SerializeField] private GameObject endWindow;

        private void Awake()
        {
            loseWindow.SetActive(false);
            endWindow.SetActive(false);
        }
        public void Enable()
        {
            player.DeathCaller += Death;
            Runtime.UpdateFrame += UpdateFrame;
        }
        public void Disable()
        {
            player.DeathCaller -= Death;
            Runtime.UpdateFrame -= UpdateFrame;
        }

        public void Death()
        {
            loseWindow.SetActive(true);
        }
        public void UpdateFrame(int frame, int maxFrame)
        {
            if (frame < maxFrame)
                return;

            endWindow.SetActive(true);
        }
    }
}