using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    [SerializeField]public Vector3 offSet;
    Box gameObjectWhichIhave;
    [SerializeField] bool isLocked;
    [SerializeField] GameObject _lock;


    public bool Unlock()
    {
        if (isLocked)
        {
            isLocked = false;
            Start();
            return true;
        }
        return false;
    }

    private void Start()
    {
        if (!isLocked)
        {
            _lock.SetActive(false);
        }
    }
    public void removeBecauseImFull()
    {
        this.gameObjectWhichIhave = null;
    }


    public void AssignGameObject(Box go)
    {
        gameObjectWhichIhave = go;
    }

    public Box CheckGameObject()
    {
        return gameObjectWhichIhave;
    }


    public bool isLockedForVideoAd()
    {
        return isLocked;
    }
    public void OnMouseDown()
    {
        bool panelLock = GameManager.instance.isAlreadyOpenAnyPanel;
        if (isLocked && !panelLock)
        {
            isLocked = false;
            _lock.SetActive(false);
        }
    }
}
