using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace ScavengerWorld
{
    public class TheGame : MonoBehaviour
    {
        public float SpeedMultiplier { get; set; }

        public static TheGame Instance { get; private set; }

        private int gameTimeMinute;
        public int GameTimeMinute => gameTimeMinute;

        public float GameHourPerRealHour { get; private set; }     // game-hours per 1 real-time hours
        public float GameHourPerRealSecond { get; private set; }   // game-hours per 1 real-time seconds

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(this);
                Debug.LogError("More than one game manager found!");
            }
            Instance = this;
            GameHourPerRealHour = 60f;
            GameHourPerRealSecond = 1 / 60f;
        }

        // Start is called before the first frame update
        void Start()
        {
            SpeedMultiplier = 1f;
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}