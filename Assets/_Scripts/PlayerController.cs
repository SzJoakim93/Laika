using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public class PlayerController : MonoBehaviour
{//ennek szerintem singletonnak kell lennie

    #region instances
    public PopupText DialogText;
    public static PlayerController Instance;
    Camera mainCamera;
    EventSystem eventSystem;
    Animator myAnimator;

    [SerializeField]
    protected NavMeshAgent agent;
    #endregion
   
    // if ui is not included in this, then the raycast goes tru it and ui click triggers movement
    [Tooltip("All the layers, we dont want to ignore raycast(ground, interactable + ui)")]
    [SerializeField]
    protected LayerMask movementMask;
    public bool canInteract = false;
    private bool isControllableByMouseClick;
    public bool IsControllableByMouseClick {

        get { return isControllableByMouseClick; }
        set { isControllableByMouseClick = value; }
    }

    protected bool gameHasStarted = true;

    /// <summary>
    /// Ennek a gameobjectnek vagyok benne a boxColliderjaban. Majd errol fogom probalni leszedni az eppen szukseges interfaceket, 
    /// es kezdeni veluk valamit az Itemek Use() fgveben,etc
    /// </summary>
    GameObject currentlyCollidingWith;

    #region player settings
    [SerializeField]
    protected float walkSpeed;

    #endregion
    
    #region cache variables

    protected Ray movementControlScreenToWorlRay;
    protected RaycastHit movementControlScreenToWorldHit;

    #endregion

    public static Action<Vector3> OnNavigationClick = delegate { };

    private void Awake()
    {
        #region singleton
        if (Instance == null) {
            Instance = this;
        } else if (Instance != this) {

            Destroy(this);
            Debug.LogError("Singleton broken");
        }
        #endregion

        isControllableByMouseClick = true;
        mainCamera = Camera.main;
        agent = GetComponent<NavMeshAgent>();
        myAnimator = GetComponentInChildren<Animator>();
        eventSystem = EventSystem.current;
    }

    void Start()  {
        walkSpeed = 10f;
    }

    void Update() {
        SetAnimatorParameters();
        GetScreenClickToNavigate();
    }

    private void GetScreenClickToNavigate() {
        //ezt szerintem az inputmanagerbol kene olvasni pl baseInputmanager.navigationClicked
        if (Input.GetMouseButtonDown(0) && isControllableByMouseClick && !eventSystem.IsPointerOverGameObject()) {
            movementControlScreenToWorlRay = mainCamera.ScreenPointToRay(Input.mousePosition);//ez erintokepernyon is mukodik

            if (Physics.Raycast(movementControlScreenToWorlRay, out movementControlScreenToWorldHit, 100, movementMask.value)) {
                OnNavigationClick(this.transform.position);
                agent.SetDestination(movementControlScreenToWorldHit.point);
                agent.speed = walkSpeed;
            }
        }
    }

    private void SetAnimatorParameters() {
        if (agent.velocity.sqrMagnitude > 0f)
            myAnimator.SetBool("isWalking", true);
        else if (agent.velocity.sqrMagnitude == 0)
            myAnimator.SetBool("isWalking", false);
    }

    void OnTriggerEnter(Collider coll) {
        if (coll.CompareTag("camera_place"))
            mainCamera.transform.position = coll.transform.position;
        else if (coll.gameObject.tag.Contains("interactable")) {
            coll.GetComponent<ActionManager>().Invoke();
        }
    }
}

