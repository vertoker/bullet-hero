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
        [SerializeField] private float sfi;// Начало убывания музыки (start fade out)
        [SerializeField] private float efi;// Конец убывания музыки (end fade out)
        [SerializeField] private float sfo;// Начало убывания музыки (start fade out)
        [SerializeField] private float efo;// Конец убывания музыки (end fade out)

        public string LinkPath => lp;
        public AudioLinkType LinkType => lt;
        public LinkStatus LinkStatus => ls;
        public float StartFadeIn => sfi;
        public float EndFadeIn => efi;
        public float StartFadeOut => sfo;
        public float EndFadeOut => efo;

        public void SetLinkPath(string link_path) { lp = link_path; }
        public void SetLinkType(AudioLinkType link_type) { lt = link_type; }
        public void SetLinkStatus(LinkStatus link_status) { ls = link_status; }
        public void SetStartFadeIn(float start_fade_in) { sfi = start_fade_in; }
        public void SetEndFadeIn(float end_fade_in) { efi = end_fade_in; }
        public void SetStartFadeOut(float start_fade_out) { sfi = start_fade_out; }
        public void SetEndFadeOut(float end_fade_out) { efi = end_fade_out; }

        public AudioSourceData(string link_path,
            AudioLinkType link_type,
            LinkStatus link_status,
            float start_fade_in,
            float end_fade_in,
            float start_fade_out,
            float end_fade_out)
        {
            lp = link_path;
            lt = link_type;
            ls = link_status;
            sfi = start_fade_in;
            efi = end_fade_in;
            sfo = start_fade_out;
            efo = end_fade_out;
        }
    }
}
