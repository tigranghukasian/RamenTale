using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Smashlab 
{
    public class SafeAreaDetection : MonoBehaviour
    {
        private Rect _safeArea;

        public delegate void SafeAreaChanged(Rect safeArea);
        public static event SafeAreaChanged onOnSafeAreaChanged;

        private void Awake()
        {
            _safeArea = Screen.safeArea;
        }

        private void Update()
        {
            if(_safeArea != Screen.safeArea)
            {
                _safeArea = Screen.safeArea;
                onOnSafeAreaChanged?.Invoke(_safeArea);
            }
        }
    }
}

