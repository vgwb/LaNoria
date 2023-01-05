using Lean.Touch;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SpawnObjectUI : MonoBehaviour, IPointerDownHandler
{
    #region Var
    public GameObject ProjectPrefab;
    public Transform SpawnPoint;
    #endregion

    #region MonoB
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    #endregion

    #region Functions
    public void SpawnPrefab()
    {
        if (ProjectPrefab != null && SpawnPoint != null) {
            var obj = Instantiate(ProjectPrefab);
            obj.transform.position = SpawnPoint.position;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        SpawnPrefab();
    }
    #endregion
}
