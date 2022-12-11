using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public List<BarrackEventButton> BarrackPanelButtons;
    public TextMeshProUGUI SelectedObjectTitle;
    public Image SelectedObjectIcon;
    public GameObject BarrackPanelContainer,BarrackPanel;

    private void Awake()
    {
        Instance = this;
        BarrackPanelButtons = new List<BarrackEventButton>();
        for (int i = 0; i < BarrackPanelContainer.transform.childCount; i++)
        {
            BarrackPanelButtons.Add(BarrackPanelContainer.transform.GetChild(i).GetComponent<BarrackEventButton>());

        }
        BarrackPanel.SetActive(false);

    }
    public void SelectBarrack(BuildObject SelectedObject)
    {
        BarrackPanel.SetActive(true);
        int InstSoldierCount = SelectedObject.BuildObjectSO.InstantiableSoldiers.Count;
        for (int i = 0; i < BarrackPanelButtons.Count; i++)
        {
            if (i<InstSoldierCount)
            {
                BarrackPanelButtons[i].gameObject.SetActive(true);
                BarrackPanelButtons[i].SetBarrack(SelectedObject, SelectedObject.BuildObjectSO.InstantiableSoldiers[i]);
            }
            else
            {
                BarrackPanelButtons[i].gameObject.SetActive(false);
            }
        }
        SelectedObjectIcon.sprite=SelectedObject.gameObject.GetComponent<SpriteRenderer>().sprite;
        SelectedObjectTitle.text=SelectedObject.BuildObjectSO.ObjectType.ToString();
        
    }

    public void DeselectBarrack() 
    {
        BarrackPanel.SetActive(false);
    }
}
