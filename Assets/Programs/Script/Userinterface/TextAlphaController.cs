using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextAlphaController : MonoBehaviour
{
    TMPro.TextMeshProUGUI text;
    CycleValueFloat cycleValueFloat;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TMPro.TextMeshProUGUI>();
        cycleValueFloat = GetComponent<CycleValueFloat>();
    }

    // Update is called once per frame
    void Update()
    {
        text.alpha = cycleValueFloat.Value;
    }
}
