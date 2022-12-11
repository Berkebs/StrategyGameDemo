using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    private List<InformationEventButton> BarrackPanelButtons;
    public TextMeshProUGUI SelectedObjectTitle;
    public Image SelectedObjectIcon;
    public GameObject SoldierPanelContainer,InformationPanel;


    
    private List<BuildingSO> BuildingList;
    public Transform BuildListContainer;

    private void Awake()
    {
        Instance = this;
        BarrackPanelButtons = new List<InformationEventButton>();
        for (int i = 0; i < SoldierPanelContainer.transform.childCount; i++)
        {
            BarrackPanelButtons.Add(SoldierPanelContainer.transform.GetChild(i).GetComponent<InformationEventButton>());
        }
        InformationPanel.SetActive(false);
        BuildingList=GameManager.Instance.GetBuildingList();
        CreateBuildListUI();
    }

    void CreateBuildListUI() 
    {
        for (int multiplier = 0; multiplier < 10; multiplier++)
        {
            for (int i = 0; i < BuildingList.Count; i++)
            {
                GameObject BuildButton = Instantiate(BuildingList[i].BuildUIPrefab, BuildListContainer);
                BuildButton.GetComponent<BuildButtonUI>().SetBuild(BuildingList[i]);
            }
        }

    }



    public void SelectBarrack(BuildObject SelectedObject)
    {
        InformationPanel.SetActive(true);
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
        InformationPanel.SetActive(false);
    }
}
