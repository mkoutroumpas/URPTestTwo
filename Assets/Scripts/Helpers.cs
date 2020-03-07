﻿using Unity.Mathematics;
using UnityEngine;

namespace Assets.Scripts.Classes
{
    public static class Helpers
    {
        public static void AddDummyHeavyTask(int length = 1000)
        {
            float value = 0f;
            for (var i = 0; i < length; i++)
            {
                value = math.exp10(math.sqrt(value));
            }
        }

        public static Color GenerateColor(int i = 0)
        {
            return Color.blue;
        }
    }
}
