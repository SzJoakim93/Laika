using UnityEngine;
using UnityEngine.UI;

public enum Item {
    empty,
    keyCard,
    petrol,
    oil
}

public class Inventory : MonoBehaviour
{
    const int none = -1;
    static Color green = new Color(0.0f, 1.0f, 0.0f);
    static Color red = new Color(1.0f, 0.0f, 0.0f);

    [SerializeField]
    Image [] Slots;
    Item [] items;
    int selectedItem;
    // Start is called before the first frame update
    void Start()
    {
        selectedItem = none;
        items = new Item[5];
        for (int i = 0; i < items.Length; i++)
            items[i] = Item.empty;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Item SelectedItem {
        get { return (selectedItem == none ? Item.empty : items[selectedItem]); }
    }

    public void DropSelectedItem() {
        if (selectedItem != none)
            items[selectedItem] = Item.empty;
    }

    public void Select(int x)
    {
        if (selectedItem == x)
        {
            Slots[x].color = green;
            selectedItem = none;
        }
        else
        {
            if (selectedItem != none)
                Slots[selectedItem].color = green;
            Slots[x].color = red;
            selectedItem = x;
        }
    }

    void OnTriggerEnter(Collider coll) {
        if (coll.CompareTag("petrol")) {
            Debug.Log("Item found");
            for (int i = 0; i < items.Length; i++) {
                if (items[i] == Item.empty) {
                    items[i] = Item.petrol;
                    Debug.Log(i);
                    Image itemImg = Slots[i].transform.GetChild(0).GetComponent<Image>();
                    itemImg.gameObject.SetActive(true);
                    itemImg.sprite = Resources.Load<Sprite>("Sprites/PetrolUI");
                    Destroy(coll.gameObject);
                    break;
                }
            }
        }
    }

}
