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
    [SerializeField, ReadOnly] private Vector3 replaceStartPos;

    [SerializeField, ReadOnly] private bool canInteract = true;
    private BaseInteractable lastHoveredObject = null;
    private Vector2 lastMousePosition;

    private void Start()
    {
        InputProvider.OnInteractPressed += TryInteract;
        InputProvider.OnLookInput += UpdateMousePosition;
        
        EventManager.OnItemSelected += CreateReplaceItem;
        EventManager.OnItemReplaceClicked += ReplaceItem;
        EventManager.OnClearSelectedItem += ClearReplaceItem;
    }

    private void OnDestroy()
    {
        InputProvider.OnInteractPressed -= TryInteract;
        InputProvider.OnLookInput -= UpdateMousePosition;
        
        EventManager.OnItemSelected -= CreateReplaceItem;
        EventManager.OnItemReplaceClicked -= ReplaceItem;
        EventManager.OnClearSelectedItem -= ClearReplaceItem;
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

    #region ReplaceItem

    private void CreateReplaceItem(GameObject selectedItem)
    {
        replaceItem = CreateReplaceItemRecursively(selectedItem);
    }
    
    private GameObject CreateReplaceItemRecursively(GameObject selectedItem)
    {
        GameObject replaceObject = new GameObject(selectedItem.name + "_Replace");
        replaceObject.SetActive(false);
        replaceStartPos = replaceObject.transform.position;
        replaceStartScale = selectedItem.transform.localScale;

        CopyMeshAndRenderer(selectedItem, replaceObject);

        replaceObject.transform.SetPositionAndRotation(selectedItem.transform.position, selectedItem.transform.rotation);
        replaceObject.transform.localScale = selectedItem.transform.localScale;

        foreach (Transform child in selectedItem.transform)
        {
            GameObject childCopy = CreateReplaceItemRecursively(child.gameObject);
            childCopy.transform.SetParent(replaceObject.transform, false);
        }

        return replaceObject;
    }

    private void CopyMeshAndRenderer(GameObject original, GameObject copy)
    {
        if (original.TryGetComponent(out MeshFilter meshFilter))
        {
            copy.AddComponent<MeshFilter>().sharedMesh = meshFilter.sharedMesh;
        }

        if (original.TryGetComponent(out MeshRenderer meshRenderer))
        {
            MeshRenderer newRenderer = copy.AddComponent<MeshRenderer>();

            Material[] newMaterials = new Material[meshRenderer.sharedMaterials.Length];
            for (int i = 0; i < newMaterials.Length; i++)
            {
                newMaterials[i] = replaceMaterial;
            }
            newRenderer.materials = newMaterials;
        }
    }
    
    #endregion

    private void ClearReplaceItem()
    {
        Destroy(replaceItem);
        replaceItem = null;
    }

    private void ReplaceItem(GameObject selectedItem)
    {
        if (lastHoveredObject || !replaceItem.activeSelf) return;

        Item item = selectedItem.GetComponent<Item>();
        
        item.UpdateItemPosition(replaceItem.transform.position);
        item.UpdateItemScale(item.itemData.ItemReplaceScale);

        foreach (var col in selectedItem.GetComponents<Collider>()) 
            col.enabled = true;
        
        ClearReplaceItem();
        
        EventManager.ItemReplaced();
    }

    private void MoveReplaceItem()
    {
        if (!replaceItem) return; 
        
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
                replaceItem.transform.localScale = replaceStartScale;
            }
        }
        else
        {
            replaceItem.SetActive(false);
        }
    }

    

}