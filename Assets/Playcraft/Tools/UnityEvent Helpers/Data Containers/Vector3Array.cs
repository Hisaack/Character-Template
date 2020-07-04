﻿using UnityEngine;

namespace Playcraft
{
    [CreateAssetMenu(menuName = "Playcraft/Basic Data Types/Vector3 Array", fileName = "Vector3 Array")]
    public class Vector3Array : ScriptableObject
    {
        public Vector3[] values;
    }
}
