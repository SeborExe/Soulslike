using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PantsModelChanger : MonoBehaviour
{
    public List<GameObject> pantsModels;

    private void Awake()
    {
        GetAllPantsModels();
    }

    private void GetAllPantsModels()
    {
        int childrenGameObject = transform.childCount;

        for (int i = 0; i < childrenGameObject; i++)
        {
            pantsModels.Add(transform.GetChild(i).gameObject);
        }
    }

    public void UnEquipAllPantsModels()
    {
        foreach (GameObject item in pantsModels)
        {
            item.SetActive(false);
        }
    }

    public void EquipPantsModelByName(string pantsName)
    {
        for (int i = 0; i < pantsModels.Count; i++)
        {
            if (pantsModels[i].name == pantsName)
            {
                pantsModels[i].SetActive(true);
            }
        }
    }
}
