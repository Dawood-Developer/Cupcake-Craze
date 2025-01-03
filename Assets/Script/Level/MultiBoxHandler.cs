using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiBoxHandler : MonoBehaviour
{
    List<Box> m_Boxes = new List<Box>();
    [SerializeField] TextMesh m_TextMesh;
    [SerializeField] Transform boxesParent;

    private void Start()
    {
        GetRefrences();
        SetText();
    }

    public void GetRefrences()
    {
        for (int i = 0; i < boxesParent.transform.childCount; i++)
        {
            m_Boxes.Add(boxesParent.transform.GetChild(i).gameObject.GetComponent<Box>());
            if (i != 0)
            {
                m_Boxes[i].gameObject.SetActive(false);
            }
        }
    }


    public void EnableNextBox()
    {
        m_Boxes?.RemoveAt(0);
        if (m_Boxes.Count > 0)
        {
            m_Boxes[0]?.gameObject?.SetActive(true);
        }
        SetText();
    }

    public void SetText()
    {
        m_TextMesh.text = m_Boxes.Count.ToString();
    }
}