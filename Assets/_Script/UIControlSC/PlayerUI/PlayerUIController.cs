using UnityEngine;
using UnityEngine.PlayerLoop;

public class PlayerUIController : MonoBehaviour
{
    [Header("UI-Component")]
    public GameObject inventoryWindow;
    public GameObject equipmentWindow;
    public GameObject bossWindow;
    
    public static Inventory myInventory;
    public static EquipmentWindow myEquipmentWindow;
    public static UIBossState myBossWindow;
    public static BuffManager myBuffManager;
    public static ActionBar myKeyboard;

    private void Start()
    {
        InitUI();
        inventoryWindow.SetActive(false);
        equipmentWindow.SetActive(false);
        bossWindow.SetActive(false);
    }

    private void Update()
    {
        HandleWindows();
    }

    void InitUI(){
        myInventory = FindAnyObjectByType<Inventory>();
        myEquipmentWindow = FindAnyObjectByType<EquipmentWindow>();
        myKeyboard = FindAnyObjectByType<ActionBar>();
        myBuffManager = FindAnyObjectByType<BuffManager>();
        myBossWindow = FindAnyObjectByType<UIBossState>();
    }

    public void GetItem(GameObject item){
        myInventory.GetItem(item);
    }

    public void OnBuffItem(ItemData itemData, float duration){
        myBuffManager.OnBuffItem((Consumable)itemData, duration);
    }
    void HandleWindows(){
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (inventoryWindow.activeSelf)
            {
                inventoryWindow.SetActive(false);
            }
            else {
                inventoryWindow.SetActive(true);
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (equipmentWindow.activeSelf)
            {
                equipmentWindow.SetActive(false);
            }
            else
            {
                equipmentWindow.SetActive(true);
            }
        }
    }
    
    public bool CheckSlotSize(){
        return myInventory.CheckSlotSize();
    }

    public void SyncUIData(){
        myInventory.SyncUIData();
    }

 
    public void ShowBossUI(){
        bossWindow.SetActive(true);
    }

    public void CloseBossUI(){
        bossWindow.SetActive(false);
    }
}