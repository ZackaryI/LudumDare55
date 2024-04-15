using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
[RequireComponent(typeof(TextMeshProUGUI))]

public class RoomStatusText : MonoBehaviour
{
    public TextMeshProUGUI text;
    public float delayForFade = 2f; 
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateText(string v)
    {
        StartCoroutine(UpdateTextSeconds(v));
    }
    public IEnumerator UpdateTextSeconds(string v)
    {
        text.text = v;
        yield return new WaitForSeconds(delayForFade);
        text.text = "";
    }
}
