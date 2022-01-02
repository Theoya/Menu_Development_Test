using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContainerDetails : MonoBehaviour
{

    public GameObject occupant = null;
    public Sprite occupantSprite = null;
    public Sprite blankSprite = null;

    public void SetOccupant(GameObject occupant){
        this.occupant = occupant;
        this.occupantSprite = occupant.GetComponent<ItemHandler>().itemSprite;
        GetComponent<Image>().sprite = this.occupantSprite;
    }

    public GameObject GetOccupant(){
        return this.occupant;
    }

    public GameObject SwapOccupant(GameObject occupant){
        GameObject oldOccupant = this.occupant;
        this.occupant = occupant;
        this.occupantSprite = occupant.GetComponent<ItemHandler>().itemSprite;
        GetComponent<Image>().sprite = this.occupantSprite;
        return oldOccupant;
    }

    public void ClearSlate(){
        this.occupant = null;
        this.occupantSprite = null;
        GetComponent<Image>().sprite = this.blankSprite;
    }

}
