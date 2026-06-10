using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowReprter : MonoBehaviour
{
    public GameObject Reporter;
    
    private int _clickCount = 0;
    private GameObject _reporterInstance;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnShow);
    }

    private void OnShow()
    {
        if (_reporterInstance != null) return;
        
        _clickCount++;
        
        if (_clickCount >= 6)
        {
            _reporterInstance = Instantiate(Reporter);
        }
    }
}