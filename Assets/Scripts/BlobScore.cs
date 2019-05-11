using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BlobScore : MonoBehaviour
{
    [SerializeField] private TextMeshPro[] texts;
    private int a, b;

    public void IncreaseScore(int index)
    {
        if (index == 0) a++;
        else b++;
        foreach (var text in texts)
        {
            text.text = string.Format("{0} - {1}", a, b);
        }
    }
}
