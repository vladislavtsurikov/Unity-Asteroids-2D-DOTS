using TMPro;
using UnityEngine;

namespace VladislavTsurikov.Asteroids.Runtime.PlayerStats
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public abstract class StatsUI : MonoBehaviour
    {
        private TextMeshProUGUI _textMeshProUGUI;

        protected abstract string StringFormat { get; }

        private void Start()
        {
            _textMeshProUGUI = GetComponent<TextMeshProUGUI>();
        }

        protected void SetText(params object[] args)
        {
            _textMeshProUGUI.text = string.Format(StringFormat, args);
        }
    }
}