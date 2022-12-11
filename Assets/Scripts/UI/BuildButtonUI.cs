using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildButtonUI : MonoBehaviour
{
    public TextMeshProUGUI Title;
    private BuildingSO BuildingSO;

    private void Awake()
    {
        this.GetComponent<Button>().onClick.AddListener(() => OnClick());
    }
    public void OnClick()
    {
        GameManager.Instance.SetSelectedBuilding(BuildingSO);
        UIManager.Instance.DeselectBarrack();

    }

    public void SetBuild(BuildingSO buildingSO) 
    {
        BuildingSO= buildingSO;
        Title.text = BuildingSO.BuildName + "\n" + "HP : " + BuildingSO.BuildHP;
    }
}
