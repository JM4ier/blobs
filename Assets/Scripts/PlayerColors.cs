using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColors : MonoBehaviour
{
    [SerializeField] private ColorPair[] colors;

    [System.Serializable]
    public struct ColorPair
    {
        public Color bodyColor;
        public Color barfColor;
    }

    public Color GetParticleColor(int index)
    {
        if (colors == null || index < 0 || index >= colors.Length)
        {
            return Color.black;
        }
        return colors[index].barfColor;
    }
    
    public Color GetPlayerColor(int index)
    {
        if (colors == null || index < 0 || index >= colors.Length)
        {
            return Color.black;
        }
        return colors[index].bodyColor;
    }

    public int Players { get { return colors.Length; } }
}
