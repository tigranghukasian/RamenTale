using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Smashlab 
{
    public class SafeAreaPanel : MonoBehaviour
    {
        private RectTransform _rectTransform;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            RefreshPanel(Screen.safeArea);
        }

        private void OnEnable()
        {
            SafeAreaDetection.onOnSafeAreaChanged += RefreshPanel;
        }
        private void OnDisable()
        {
            SafeAreaDetection.onOnSafeAreaChanged -= RefreshPanel;
        }

        private void RefreshPanel(Rect safeArea)
        {
            Vector2 anchorMin = safeArea.position;
            Vector3 anchorMax = safeArea.position + safeArea.size;

            anchorMin.x /= Screen.width;
            anchorMin.y /= Screen.height;

            anchorMax.x /= Screen.width;
            anchorMax.y /= Screen.height;

            _rectTransform.anchorMin = anchorMin;
            _rectTransform.anchorMax = anchorMax;

        }
    }
}

