using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputComponent : MonoBehaviour
{
    [Serializable]
    public class InputAxis
    {
        public KeyCode positive;
        public KeyCode negative;

        public float Value { get; protected set; }

        public InputAxis(KeyCode positive, KeyCode negative)
        {
            this.positive = positive;
            this.negative = negative;
        }

        public void Get()
        {
            bool positiveHeld = Input.GetKey(positive);
            bool negativeHeld = Input.GetKey(negative);

            if (positiveHeld == negativeHeld)
                Value = 0f;
            else if (positiveHeld)
                Value = 1f;
            else
                Value = -1f;
        }
    }
}
