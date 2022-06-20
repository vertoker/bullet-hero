using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine;
using Data.Enum;
using TMPro;

namespace Audio.UI
{
    public class ParserInput : MonoBehaviour
    {
        [SerializeField] private LinkStatus statusLink;
        [SerializeField] private TMP_InputField link;
        [SerializeField] private Image progressBar;
        [SerializeField] private GameObject downloadIcon;
        [SerializeField] private GameObject cancelIcon;
        [SerializeField] private GameObject doneIcon;

        public void Set(string link)
        {
            if (link.Length == 0)
            {
                statusLink = LinkStatus.NotSpecified;
            }
        }

        public void Check()// продолжить
        {
            string url = link.text;
            var request = UnityWebRequest.Get(url);

            if (request.result != UnityWebRequest.Result.Success)
            {
                statusLink = LinkStatus.SpecifiedError;
                return;
            }

            statusLink = LinkStatus.Specified;
        }
    }
}