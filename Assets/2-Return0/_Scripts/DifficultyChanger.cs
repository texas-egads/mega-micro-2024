using System;
using UnityEngine;

namespace Return0
{
    public class DifficultyChanger : MonoBehaviour
    {
        public static string dif = "";
        public static int count = 0;
        public static GameObject parentGrips;

        public static void GetGripsCount()
        {
            parentGrips = GameObject.Find("GRIPS");
            dif = Managers.MinigamesManager.GetCurrentMinigameDifficulty().ToString();
            switch (dif)
            {
                case "EASY":
                    count = 8;
                    break;
                case "MEDIUM":
                    count = 12;
                    break;
                case "HARD":
                    count = 16;
                    break;
            }
        }

        public static void AssignGrips()
        {
            for (int i = 0; i < count; i++)
            {
                PandaClimber.grips[i] = parentGrips.transform.GetChild(i).gameObject;
            }
        }
    }
}
