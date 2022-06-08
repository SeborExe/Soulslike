using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BootsModelChanger : MonoBehaviour
{
    public List<GameObject> bootsModels;

    private void Awake()
    {
        GetAllBootsModels();
    }

    private void GetAllBootsModels()
    {
        int childrenGameObject = transform.childCount;

        for (int i = 0; i < childrenGameObject; i++)
        {
            bootsModels.Add(transform.GetChild(i).gameObject);
        }
    }

    public void UnEquipAllBootsModels()
    {
        foreach (GameObject item in bootsModels)
        {
            item.SetActive(false);
        }
    }

    public void EquipBootsModelByName(string bootsName)
    {
        for (int i = 0; i < bootsModels.Count; i++)
        {
            if (bootsModels[i].name == bootsName)
            {
                bootsModels[i].SetActive(true);
            }
        }
    }
}
