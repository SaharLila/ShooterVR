using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

public class BloodScript : MonoBehaviour
{
    private BloodImageScript[] m_BloodImages;

    // Start is called before the first frame update
    void Start()
    {
        this.m_BloodImages = this.GetComponentsInChildren<BloodImageScript>();
    }

    public void Show()
    {
        int numOfElementsInArray = this.m_BloodImages.Length - 1;
        this.m_BloodImages[new System.Random().Next(0, numOfElementsInArray)].Show();
    }
}
