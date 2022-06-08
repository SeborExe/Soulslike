using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelmetModelChanger : MonoBehaviour
{
    public List<GameObject> helmetModels;

    private void Awake()
    {
        GetAllHelmetModels();
    }

    private void GetAllHelmetModels()
    {
        int childrenGameObject = transform.childCount;

        for (int i = 0; i < childrenGameObject; i++)
        {
            helmetModels.Add(transform.GetChild(i).gameObject);
        }
    }

    public void UnEquipAllHelmetModels()
    {
        foreach (GameObject item in helmetModels)
        {
            item.SetActive(false);
        }
    }

    public void EquipHelmetModelByName(string helmetName)
    {
        for (int i = 0; i < helmetModels.Count; i++)
        {
            if (helmetModels[i].name == helmetName)
            {
                helmetModels[i].SetActive(true);
            }
        }
    }
}
