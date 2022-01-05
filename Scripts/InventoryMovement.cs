using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class InventoryMovement : MonoBehaviour
{
    public List<GameObject> containers = new List<GameObject>();
    public List<Vector3> containerPositions = new List<Vector3>();
    public GameObject currentContainer = null;
    public int currentIndex = 0;
    public bool containerFlag = false;


    public List<int> resolutionWidth = new List<int>();
    public List<int> resolutionHeight = new List<int>();
    public GameObject itemLabel = null;

    public GameObject pickupStorage = null;
    public Sprite blankSprite = null;

    private int resolutionIndex = 1;
    private List<GameObject> items = new List<GameObject>();
    private PlayerControls playerControls;

    private GameObject heldObject;

    
    
    
    // Start is called before the first frame update
    void Awake()
    {
        this.playerControls = new PlayerControls();
        
        this.resolutionWidth.Add(1280);
        this.resolutionWidth.Add(1920);
        this.resolutionWidth.Add(3860);
        this.resolutionHeight.Add(720);
        this.resolutionHeight.Add(1080);
        this.resolutionHeight.Add(2160);

        Object[] itemsObjects = Resources.LoadAll("ItemPrefabs", typeof(GameObject));
        foreach (GameObject item in itemsObjects) 
        {    
            this.items.Add(item);
            
        }

        this.currentContainer = this.containers[currentIndex];
        Debug.Log(this.currentContainer);
        ClearAndSpawn();
        SetItemLabel();
    }



   /// 
   /// This function is called when the object becomes enabled and active.
   /// 
    void OnEnable()
    {
        this.playerControls.UI.Enable();
    }

    /// 
    /// This function is called when the object becomes enabled and active.
    /// 
    void OnDisable()
    {
        this.playerControls.UI.Disable();
    }

    

    // Update is called once per frame
    void Update()
    {


        
        resetMenuLocations();
           
        //Joysticks/D-Pad
        playerControls.UI.Down.performed += ctx => moveDown();
        playerControls.UI.Up.performed += ctx => moveUp();
        playerControls.UI.Left.performed += ctx => moveLeft();
        playerControls.UI.Right.performed += ctx => moveRight();

        //Shoulder buttons
        playerControls.UI.Resdown.performed += ctx => resolutionDown();
        playerControls.UI.Resup.performed += ctx => resolutionUp();

        //Y Button
        playerControls.UI.Reset_InventoryDelete.performed += ctx => ClearAndSpawn();

        //A button
        playerControls.UI.PickupPutdown.performed += ctx => Grab();


        
        
        
        
        
        // if(playerControls.UI.Down.ReadValue<float>() > 0 && this.currentIndex < Containers.Count()){
        //     Debug.Log("2");
        //     this.currentIndex+=6;
        //     transform.position = this.ContainerPositions[this.currentIndex];
        // }
    }

    private void SetItemLabel(){
        if (this.currentContainer.GetComponent<ContainerDetails>().occupant != null){
            this.itemLabel.GetComponent<Text>().text = this.currentContainer.GetComponent<ContainerDetails>().occupant.GetComponent<ItemHandler>().itemName;
            this.itemLabel.transform.GetChild(0).gameObject.GetComponent<Text>().text = this.currentContainer.GetComponent<ContainerDetails>().occupant.GetComponent<ItemHandler>().itemName;
        }
        else{
            this.itemLabel.GetComponent<Text>().text = "";
            this.itemLabel.transform.GetChild(0).gameObject.GetComponent<Text>().text = "";
        }
    }
    
    private void PositionSetter(){
        transform.position = this.containerPositions[this.currentIndex];
        this.currentContainer = this.containers[currentIndex];
        SetItemLabel();
    }

    private void moveDown(){
        if(this.currentIndex < 12){
            this.currentIndex+=6;
            PositionSetter();
        }
    }

    private void moveUp(){
        if(this.currentIndex > 5){
            this.currentIndex-=6;
            PositionSetter();
        }
    }

    private void moveLeft(){
        if(this.currentIndex != 0 && this.currentIndex != 6 && this.currentIndex != 12){
            this.currentIndex--;
            PositionSetter();
        }
    }

    private void moveRight(){
        if(this.currentIndex != 5 && this.currentIndex != 11 && this.currentIndex < 17){
            this.currentIndex++;
            PositionSetter();
        }
    }


    private void resetMenuLocations(){
        this.containerPositions = new List<Vector3>();
        foreach(GameObject container in this.containers){
                this.containerPositions.Add(container.transform.position);

            }
        transform.position = this.containerPositions[this.currentIndex];
    }

    private void resolutionDown(){
        if (this.resolutionIndex > 0){
            
            this.resolutionIndex--;
            Screen.SetResolution(this.resolutionWidth[this.resolutionIndex], this.resolutionHeight[this.resolutionIndex], true);
            
        }
        else{
            this.resolutionIndex = this.resolutionWidth.Count()-1;
            Screen.SetResolution(this.resolutionWidth[this.resolutionIndex], this.resolutionHeight[this.resolutionIndex], true);
        }
        this.containerFlag = false;
        //resetMenuLocations();
        
    }

    

    private void resolutionUp(){
        
        if (this.resolutionIndex < this.resolutionWidth.Count()-1){
            
            this.resolutionIndex++;
            Screen.SetResolution(this.resolutionWidth[this.resolutionIndex], this.resolutionHeight[this.resolutionIndex], true);
            
            

        }
        else{
            this.resolutionIndex = 0;
            Screen.SetResolution(this.resolutionWidth[this.resolutionIndex], this.resolutionHeight[this.resolutionIndex], true);
        }
        this.containerFlag = false;
        //resetMenuLocations();
        
    }

    private void ClearAndSpawn(){
        foreach(GameObject container in this.containers){
            container.GetComponent<ContainerDetails>().ClearSlate();
        }

        for (int i = 0; i < 5; i++){
            RandomPlacement(this.items[Random.Range(0, this.items.Count())]);
        }
        SetItemLabel();
    }
    
    private void RandomPlacement(GameObject item){
        bool placementFlag = false;
        GameObject container;
       
        while (!placementFlag){
            
            container = this.containers[Random.Range(0, this.containers.Count())];
            if(container.GetComponent<ContainerDetails>().GetOccupant() == null){
                
                container.GetComponent<ContainerDetails>().SetOccupant(item);
                placementFlag = true;
            }
        } 

    }

    private void pickupStorageHandler(){
        if (this.heldObject != null){
            this.pickupStorage.GetComponent<Image>().sprite = heldObject.GetComponent<ItemHandler>().itemSprite;
        }
        else{
            this.pickupStorage.GetComponent<Image>().sprite = this.blankSprite;
        }

    }

    private void Grab(){
        ContainerDetails containerDet = this.currentContainer.GetComponent<ContainerDetails>();
        if (this.heldObject == null){
            
            if (containerDet.occupant != null){
                this.heldObject = containerDet.occupant;
                containerDet.occupant = null;
                containerDet.updateSprite();
            }
            
        }
        else {
            
            if (containerDet.occupant != null){
                GameObject temp = this.heldObject;
                this.heldObject = containerDet.occupant;
                containerDet.occupant = temp;
                containerDet.updateSprite();
            }
            else{
                containerDet.occupant = this.heldObject;
                this.heldObject = null;
                containerDet.updateSprite();
            }

        }
        pickupStorageHandler();




    }

   

    

    
}
