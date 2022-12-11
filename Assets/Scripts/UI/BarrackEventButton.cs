using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BarrackEventButton : MonoBehaviour
{
    private BuildObject buildObject;
    private ObjectTypes CreateObjectType;
    public Image ButtonIcon;
    public TextMeshProUGUI Title;
    private void Awake()
    {
        this.GetComponent<Button>().onClick.AddListener(()=>OnClick());
    }
    public void OnClick() 
    {
        buildObject.CreateObject(CreateObjectType);
        UIManager.Instance.DeselectBarrack();

    }
    public void SetBarrack(BuildObject _buildObject, Soldier _CreateObject) 
    {
        ButtonIcon.sprite=_CreateObject.GetComponent<SpriteRenderer>().sprite;
        buildObject= _buildObject;
        CreateObjectType= _CreateObject.ObjectType;
        Title.text = CreateObjectType.ToString();

    }
}
