using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{

    [SerializeField] private TMP_Text consoleTxt;

    public void SetConsoleText(string text)
    {
        consoleTxt.SetText(text);
    }
}
