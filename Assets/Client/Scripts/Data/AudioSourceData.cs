using UnityEngine;
using System;
using Data.Enum;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    [Serializable]
    public struct AudioSourceData
    {
        [SerializeField] private string lp;// Путь или ссылка на данные для аудио (link path)
        [SerializeField] private AudioLinkType lt;// Тип ссылки на данные аудио (link type)
        [SerializeField] private LinkStatus ls;// Статус ссылки на данные (link status)
        [SerializeField] private float sfo;// Начало убывания музыки (start fade out)
        [SerializeField] private float efo;// Конец убывания музыки (end fade out)

        public string LinkPath { get { return lp; } set { lp = value; } }
        public AudioLinkType LinkType { get { return lt; } set { lt = value; } }
        public LinkStatus LinkStatus { get { return ls; } set { ls = value; } }
        public float StartFadeOut { get { return sfo; } set { sfo = value; } }
        public float EndFadeOut { get { return efo; } set { efo = value; } }

        public AudioSourceData(string link_path,
            AudioLinkType link_type,
            LinkStatus link_status,
            float start_fade_out,
            float end_fade_out)
        {
            lp = link_path;
            lt = link_type;
            ls = link_status;
            sfo = start_fade_out;
            efo = end_fade_out;
        }
    }
}
