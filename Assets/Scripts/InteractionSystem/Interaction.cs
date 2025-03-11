using System;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;

public class Interaction : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float interactionDistance = 3f;
    [SerializeField] private float interactionCooldown = 0.2f; 
    [SerializeField] private bool _isMouseLocked;
    
    [Header("Replace Settings")]
    [SerializeField] private Material replaceMaterial;
    [SerializeField] private LayerMask layerMask;
    [SerializeField, ReadOnly] private Vector3 replaceStartScale;
    [SerializeField, ReadOnly] private GameObject currentHeldItem;
    [SerializeField, ReadOnly] private GameObject replaceItem;

    [SerializeField, ReadOnly] private bool canInteract = true;
    private BaseInteractable lastHoveredObject = null;
    private Vector2 lastMousePosition;

    private void Start()
    {
        InputProvider.OnInteractPressed += TryInteract;
        InputProvider.OnInteractPressed += ReplaceItem;
        InputProvider.OnEscPressed += EndReplace;
        InputProvider.OnLookInput += UpdateMousePosition;
        
        EventManager.OnStartItemHold += StartReplace;
        EventManager.OnEndItemHold += EndReplace;
    }

    private void OnDestroy()
    {
        InputProvider.OnInteractPressed -= TryInteract;
        InputProvider.OnInteractPressed -= ReplaceItem;
        InputProvider.OnEscPressed -= EndReplace;
        InputProvider.OnLookInput -= UpdateMousePosition;
        
        EventManager.OnStartItemHold -= StartReplace;
        EventManager.OnEndItemHold -= EndReplace;
    }

    private void Update()
    {
        MoveReplaceItem();
    }

    private void UpdateMousePosition(Vector2 mousePosition)
    {
        lastMousePosition = mousePosition;
        TryHover(mousePosition);
    }

    private void TryHover(Vector2 mousePosition)
    {
        if (PerformRaycast(mousePosition, out RaycastHit hit))
        {
            BaseInteractable currentHoveredObject = hit.transform.GetComponent<BaseInteractable>();
            if (currentHoveredObject != null && currentHoveredObject != lastHoveredObject)
            {
                lastHoveredObject?.EndHover();
                currentHoveredObject.StartHover();
                lastHoveredObject = currentHoveredObject;
            }
            else if(!currentHoveredObject)
            {
                lastHoveredObject?.EndHover();
                lastHoveredObject = null;
            }
        }
        else
        {
            lastHoveredObject?.EndHover();
            lastHoveredObject = null;
        }
    }

    private void TryInteract()
    {
        if (!canInteract) return;

        if (PerformRaycast(lastMousePosition, out RaycastHit hit))
        {
            HandleInteraction(hit.collider.gameObject);
        }
    }

    private bool PerformRaycast(Vector2 mousePosition, out RaycastHit hit)
    {
        Ray ray = GenerateRay(mousePosition);
        return Physics.Raycast(ray, out hit, interactionDistance);
    }

    private Ray GenerateRay(Vector2 mousePosition)
    {
        return _isMouseLocked
            ? new Ray(playerCamera.transform.position, playerCamera.transform.forward)
            : playerCamera.ScreenPointToRay(mousePosition);
    }

    private void HandleInteraction(GameObject target)
    {
        if (target.TryGetComponent(out BaseInteractable interactable))
        {
            if (!interactable.Interact(gameObject))
            {
                Debug.Log("Etkileşim için gerekli şartlar sağlanmadı!");
            }
            InteractionCooldown().Forget();
        }
    }

    private async UniTaskVoid InteractionCooldown()
    {
        canInteract = false;
        await UniTask.Delay((int)(interactionCooldown * 1000)); 
        canInteract = true;
    }
    
    /// <summary>
    /// Replace
    /// </summary>

    private GameObject CreateReplaceItem(GameObject originalItem)
{
    GameObject replaceItem = new GameObject(originalItem.name + "_Replace");

    CopyMeshAndRenderer(originalItem, replaceItem);

    if (originalItem.TryGetComponent(out Item itemComponent))
    {
        replaceItem.transform.localScale = itemComponent.itemData.ItemReplaceScale; 
    }
    else
    {
        replaceItem.transform.localScale = originalItem.transform.localScale; 
    }

    replaceItem.transform.SetPositionAndRotation(originalItem.transform.position, originalItem.transform.rotation);

    foreach (Transform child in originalItem.transform)
    {
        GameObject childCopy = CreateReplaceItem(child.gameObject); 
        childCopy.transform.SetParent(replaceItem.transform, false); 
    }

    return replaceItem;
    }
    
    private void CopyMeshAndRenderer(GameObject original, GameObject copy)
    {
        if (original.TryGetComponent(out MeshFilter meshFilter))
        {
            copy.AddComponent<MeshFilter>().sharedMesh = meshFilter.sharedMesh;
        }
        MeshRenderer newRenderer = copy.AddComponent<MeshRenderer>();
        newRenderer.material = replaceMaterial;
    }

    public void StartReplace(GameObject heldItem)
    {
        EndReplace();
        currentHeldItem = heldItem;
        replaceItem = CreateReplaceItem(heldItem);
    }
    public void EndReplace()
    {
        if (!currentHeldItem)
            return;
        
        currentHeldItem = null;
        Destroy(replaceItem);
        replaceItem = null;
    }
    public void ReplaceItem()
    {
        if (canInteract && currentHeldItem && lastHoveredObject == null && replaceItem.activeSelf)
        {
            Item heldItem = currentHeldItem.GetComponent<Item>();
            Vector3 position = replaceItem.transform.position;
            EventManager.ItemReplaced(currentHeldItem);
            
            heldItem.UpdateItemPosition(position);
            heldItem.UpdateItemScale(heldItem.itemData.ItemReplaceScale);
            
            foreach (var col in heldItem.GetComponents<Collider>())
            {
                col.enabled = true;
            }

            EndReplace();
        }
    }

    public void MoveReplaceItem()
    {
        if (replaceItem == null) return; 
        
        Ray ray = GenerateRay(lastMousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, interactionDistance, layerMask))
        {
            replaceItem.transform.position = hit.point;
            if (hit.collider.GetComponent<BaseInteractable>())
            {
                replaceItem.SetActive(false); 
            }
            else
            {
                replaceItem.SetActive(true);
                replaceItem.transform.localScale = Vector3.one;
            }
        }
        else
        {
            replaceItem.SetActive(false);
        }
    }

    

}