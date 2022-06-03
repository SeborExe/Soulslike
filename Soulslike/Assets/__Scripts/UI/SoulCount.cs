using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SoulCount : MonoBehaviour
{
    public TMP_Text soulsCountText;

    public void SetSoulsCountText(int soulsCount)
    {
        soulsCountText.text = soulsCount.ToString();
    }
}
