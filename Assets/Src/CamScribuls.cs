using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamScribuls : MonoBehaviour
{

    PositionCalculator positionCalculator;

    void Start()
    {
        GameObject[] lefties = GameObject.FindGameObjectsWithTag("Lefty");
        Transform[] leftyTrans = new Transform[lefties.Length];
        for (int i = 0; i < lefties.Length; i++)
        {
            leftyTrans[i] = lefties[i].transform;
        }
        positionCalculator = new PositionCalculator(leftyTrans, gameObject.transform);
    }

    void Update()
    {
        positionCalculator.SetCamLocation();    
    }
}

public class PositionCalculator
{
    Transform[] leftyTransforms;
    Transform transform;

    public PositionCalculator(Transform[] _leftyTransforms, Transform _camTrans)
    {
        this.leftyTransforms = _leftyTransforms;
        this.transform = _camTrans;
    }

    public void SetCamLocation()
    {
        transform.position = new Vector3(CalculateXPosition(), transform.position.y, transform.position.z);
    }

    float CalculateXPosition()
    {
        float avgXPos = 0;
        foreach(Transform pos in leftyTransforms)
        {
            avgXPos += pos.position.x;
        }
        return avgXPos /= leftyTransforms.Length;
    }
}
