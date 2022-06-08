using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlovesModelChanger : MonoBehaviour
{
    public List<GameObject> glovesModels;

    private void Awake()
    {
        GetAllGlovesModels();
    }

    private void GetAllGlovesModels()
    {
        int childrenGameObject = transform.childCount;

        for (int i = 0; i < childrenGameObject; i++)
        {
            glovesModels.Add(transform.GetChild(i).gameObject);
        }
    }

    public void UnEquipAllGlovesModels()
    {
        foreach (GameObject item in glovesModels)
        {
            item.SetActive(false);
        }
    }

    public void EquipGlovesModelByName(string glovesName)
    {
        for (int i = 0; i < glovesModels.Count; i++)
        {
            if (glovesModels[i].name == glovesName)
            {
                glovesModels[i].SetActive(true);
            }
        }
    }
}
