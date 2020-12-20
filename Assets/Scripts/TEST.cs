using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TEST : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler {
    public Button btn;
    public void OnPointerEnter(PointerEventData eventData)
    {
        btn.gameObject.SetActive(true);
        Debug.Log("进入");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        btn.gameObject.SetActive(false);
        Debug.Log("退出");
    }

    // Use this for initialization
    void Start () {
        this.btn = this.GetComponentInChildren<Button>();
        btn.gameObject.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
