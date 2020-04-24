using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericPopup : MonoBehaviour
{
    public void ClosePopup()
    {
        this.gameObject.SetActive(false);
    }
    
    public void OpenPopup()
    {
        this.gameObject.SetActive(true);
    }

}
