using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class InventoryMovement : MonoBehaviour
{
    public List<GameObject> Containers = new List<GameObject>();
    public List<Vector3> ContainerPositions = new List<Vector3>();
    public int currentIndex = 0;
    public bool containerFlag = false;


    public List<int> resolutionWidth = new List<int>();
    public List<int> resolutionHeight = new List<int>();
    private int resolutionIndex = 1;
    private List<GameObject> items = new List<GameObject>();

    private PlayerControls playerControls;
    
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
            Debug.Log(item);
        }
        

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
           

        playerControls.UI.Down.performed += ctx => moveDown();
        playerControls.UI.Up.performed += ctx => moveUp();
        playerControls.UI.Left.performed += ctx => moveLeft();
        playerControls.UI.Right.performed += ctx => moveRight();
        playerControls.UI.Resdown.performed += ctx => resolutionDown();
        playerControls.UI.Resup.performed += ctx => resolutionUp();
        playerControls.UI.Reset_InventoryDelete.performed += ctx => ClearAndSpawn();



        
        
        
        
        
        // if(playerControls.UI.Down.ReadValue<float>() > 0 && this.currentIndex < Containers.Count()){
        //     Debug.Log("2");
        //     this.currentIndex+=6;
        //     transform.position = this.ContainerPositions[this.currentIndex];
        // }
    }

    private void moveDown(){
        if(this.currentIndex < 12){
            Debug.Log("2");
            this.currentIndex+=6;
            transform.position = this.ContainerPositions[this.currentIndex];
        }
    }
    private void moveUp(){
        if(this.currentIndex > 5){
            Debug.Log("3");
            this.currentIndex-=6;
            transform.position = this.ContainerPositions[this.currentIndex];
        }
    }
    private void moveLeft(){
        if(this.currentIndex != 0 && this.currentIndex != 6 && this.currentIndex != 12){
            Debug.Log("4");
            this.currentIndex--;
            transform.position = this.ContainerPositions[this.currentIndex];
        }
    }
    private void moveRight(){
        if(this.currentIndex != 5 && this.currentIndex != 11 && this.currentIndex < 17){
            Debug.Log("5");
            this.currentIndex++;
            transform.position = this.ContainerPositions[this.currentIndex];
        }
    }


    private void resetMenuLocations(){
        this.ContainerPositions = new List<Vector3>();
        foreach(GameObject container in this.Containers){
                this.ContainerPositions.Add(container.transform.position);

            }
        transform.position = this.ContainerPositions[this.currentIndex];
    }

    private void resolutionDown(){
        if (this.resolutionIndex > 0){
            
            this.resolutionIndex--;
            Screen.SetResolution(this.resolutionWidth[this.resolutionIndex], this.resolutionHeight[this.resolutionIndex], true);
            Debug.Log(this.resolutionWidth[this.resolutionIndex]);
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
            Debug.Log(this.resolutionWidth[this.resolutionIndex]);
            

        }
        else{
            this.resolutionIndex = 0;
            Screen.SetResolution(this.resolutionWidth[this.resolutionIndex], this.resolutionHeight[this.resolutionIndex], true);
        }
        this.containerFlag = false;
        //resetMenuLocations();
        
    }

    private void ClearAndSpawn(){
        foreach(GameObject container in this.Containers){
            container.GetComponent<ContainerDetails>().ClearSlate();
        }

        for (int i = 0; i < 5; i++){
            RandomPlacement(this.items[Random.Range(0, this.items.Count())]);
        }
    }
    
    private void RandomPlacement(GameObject item){
        bool placementFlag = false;
        GameObject container;
       
        while (!placementFlag){
            container = this.Containers[Random.Range(0, this.Containers.Count())];
            if(container.GetComponent<ContainerDetails>().GetOccupant() == null){
                Debug.Log(item);
                container.GetComponent<ContainerDetails>().SetOccupant(item);
                placementFlag = true;
            }
        } 

    }

   

    

    
}
