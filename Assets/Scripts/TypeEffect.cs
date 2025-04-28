using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TypeEffect : MonoBehaviour
{
    public int CharsPerSec;
    public Text targetTxt;

    private int tfxIndex;
    private string tfxStr;

    private void Awake()
    {
        targetTxt = GetComponent<Text>();
    }

    private void TfxStart()
    {
        
    }

    private void Tfx()
    {

    }

    private void TfxEnd()
    {

    }
}
