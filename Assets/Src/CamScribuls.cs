using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamScribuls : MonoBehaviour
{

    PositionCalculator positionCalculator;

    void Start()
    {
        Invoke("InitPositionCalculator", 0.01f);
    }

    void Update()
    {
        if (positionCalculator != null)
            positionCalculator.SetCamLocation();    
    }

    void InitPositionCalculator()
    {
        GameObject[] lefties = GameObject.FindGameObjectsWithTag("Lefty");
        Transform[] leftyTrans = new Transform[lefties.Length];
        for (int i = 0; i < lefties.Length; i++)
        {
            leftyTrans[i] = lefties[i].transform;
        }
        positionCalculator = new PositionCalculator(leftyTrans, gameObject.transform);
    }

    public void RemoveTransform(int transId)
    {
        positionCalculator.DeleteTransfrom(transId);
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

    public void DeleteTransfrom(int transId)
    {
        int index = 0;
        for(int i = 0; i < leftyTransforms.Length; i++)
        {
            if (leftyTransforms[i].GetInstanceID() == transId)
                index = i;
        }
        List<Transform> transList = new List<Transform>(leftyTransforms);
        transList.RemoveAt(index);
        leftyTransforms = transList.ToArray();
    }

    float CalculateXPosition()
    {
        float avgXPos = 0;
        
        foreach (Transform pos in leftyTransforms)
        {
            avgXPos += pos.position.x;
        }
        return avgXPos /= leftyTransforms.Length;
        
       
    }
}
