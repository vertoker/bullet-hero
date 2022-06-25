using System.Collections;
using System.Collections.Generic;
using LevelEditor.Windows.Menu;
using UnityEngine.Events;
using LevelEditor.UI;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using Data;

namespace Audio.UI
{
    public class ParserInput : MonoBehaviour
    {
        [SerializeField] private RectTransform self;
        [SerializeField] private TMP_InputField linkInputField;
        [SerializeField] private TMP_InputField sfiInputField;
        [SerializeField] private TMP_InputField efiInputField;
        [SerializeField] private TMP_InputField sfoInputField;
        [SerializeField] private TMP_InputField efoInputField;
        [SerializeField] private IconDropdown linkTypeSelect;
        [SerializeField] private TMP_Text statusText;

        [SerializeField] private Button downloadBtn;
        [SerializeField] private Button deleteBtn;
        [SerializeField] private Button clearBtn;
        [SerializeField] private Button downBtn;
        [SerializeField] private Button upBtn;

        public const float OFFSET_HEIGTH = 500f;

        private int id;
        public int ID 
        {
            set
            {
                id = value;
                statusText.text = (value + 1).ToString();
                self.anchoredPosition = new Vector2(0f, id * -OFFSET_HEIGTH);
            }
        }
        public UnityAction<int> activeDropdownSelect;

        #region Events
        private UnityEvent<int> tapDownload = new UnityEvent<int>();
        private UnityEvent<int> tapClear = new UnityEvent<int>();
        private UnityEvent<int> tapDelete = new UnityEvent<int>();
        private UnityEvent<int> tapUp = new UnityEvent<int>();
        private UnityEvent<int> tapDown = new UnityEvent<int>();

        private UnityEvent<int, string> updateLink = new UnityEvent<int, string>();
        private UnityEvent<int, float> updateSFI = new UnityEvent<int, float>();
        private UnityEvent<int, float> updateEFI = new UnityEvent<int, float>();
        private UnityEvent<int, float> updateSFO = new UnityEvent<int, float>();
        private UnityEvent<int, float> updateEFO = new UnityEvent<int, float>();

        public event UnityAction<int> TapDownload
        {
            add => tapDownload.AddListener(value);
            remove => tapDownload.RemoveListener(value);
        }
        public event UnityAction<int> TapClear
        {
            add => tapClear.AddListener(value);
            remove => tapClear.RemoveListener(value);
        }
        public event UnityAction<int> TapDelete
        {
            add => tapDelete.AddListener(value);
            remove => tapDelete.RemoveListener(value);
        }
        public event UnityAction<int> TapUp
        {
            add => tapUp.AddListener(value);
            remove => tapUp.RemoveListener(value);
        }
        public event UnityAction<int> TapDown
        {
            add => tapDown.AddListener(value);
            remove => tapDown.RemoveListener(value);
        }

        public event UnityAction<int, int> UpdateLinkType
        {
            add => AddUpdateLinkType(value);
            remove => linkTypeSelect.SelectEvent -= activeDropdownSelect;
        }
        private void AddUpdateLinkType(UnityAction<int, int> value)
        {
            activeDropdownSelect = (int select) => { value.Invoke(id, select); };
            linkTypeSelect.SelectEvent += activeDropdownSelect;
        }

        public event UnityAction<int, string> UpdateLink
        {
            add => updateLink.AddListener(value);
            remove => updateLink.RemoveListener(value);
        }
        public event UnityAction<int, float> UpdateSFI
        {
            add => updateSFI.AddListener(value);
            remove => updateSFI.RemoveListener(value);
        }
        public event UnityAction<int, float> UpdateEFI
        {
            add => updateEFI.AddListener(value);
            remove => updateEFI.RemoveListener(value);
        }
        public event UnityAction<int, float> UpdateSFO
        {
            add => updateSFO.AddListener(value);
            remove => updateSFO.RemoveListener(value);
        }
        public event UnityAction<int, float> UpdateEFO
        {
            add => updateEFO.AddListener(value);
            remove => updateEFO.RemoveListener(value);
        }
        #endregion

        public void SetData(AudioSourceData data)
        {
            linkInputField.text = data.LinkPath;
            sfiInputField.text = data.StartFadeIn.ToString();
            efiInputField.text = data.EndFadeIn.ToString();
            sfoInputField.text = data.StartFadeOut.ToString();
            efoInputField.text = data.EndFadeOut.ToString();
        }

        public void Download()
        {
            tapDownload.Invoke(id);
        }
        public void Clear()
        {
            tapClear.Invoke(id);
        }
        public void Delete()
        {
            tapDelete.Invoke(id);
        }
        public void MoveUp()
        {
            tapUp.Invoke(id);
        }
        public void MoveDown()
        {
            tapDown.Invoke(id);
        }

        public void UpdateLinkInputField()
        {
            updateLink.Invoke(id, linkInputField.text);
        }
        public void UpdateSFIInputField()
        {
            updateSFI.Invoke(id, float.Parse(sfiInputField.text));
        }
        public void UpdateEFIInputField()
        {
            updateEFI.Invoke(id, float.Parse(efiInputField.text));
        }
        public void UpdateSFOInputField()
        {
            updateSFO.Invoke(id, float.Parse(sfoInputField.text));
        }
        public void UpdateEFOInputField()
        {
            updateEFO.Invoke(id, float.Parse(efoInputField.text));
        }
    }
}