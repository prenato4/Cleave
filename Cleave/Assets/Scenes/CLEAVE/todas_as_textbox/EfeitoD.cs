using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class EfeitoD : MonoBehaviour
{
    private TextMeshProUGUI CT;
    private AudioSource AD;
    private string MO;
    public bool Imprimindo;
    public float TEL = 0.08f;


    private void Awake()
    {
        TryGetComponent(out CT);
        TryGetComponent(out AD);
        MO = CT.text;
        CT.text = "";
    }

    private void OnEnable()
    {
        ImprimirMensagem(MO);
    }

    private void OnDisable()
    {
        CT.text = MO;
        StopAllCoroutines();
    }

    public void ImprimirMensagem(string msge)
    {
        if (gameObject.activeInHierarchy)
        {
            if (Imprimindo) return;
            {
                Imprimindo = true;
                StartCoroutine(LetraPorletra(msge));
            }
        }
    }

    IEnumerator LetraPorletra(string msge)
    {
        string msg = "";
        foreach (var Letra in msge)
        {
            msg += Letra;
            CT.text = msg;
            AD.Play();
            yield return new WaitForSeconds(TEL);
        }

        Imprimindo = false;
        StopAllCoroutines();
    }
}