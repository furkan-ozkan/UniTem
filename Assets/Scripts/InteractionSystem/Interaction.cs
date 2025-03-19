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
    [SerializeField] private GameObject selectedItem;
    [SerializeField] private Material replaceMaterial;
    [SerializeField] private LayerMask layerMask;
    [SerializeField, ReadOnly] private GameObject currentHeldItem;
    [SerializeField, ReadOnly] private GameObject replaceItem;
    [SerializeField, ReadOnly] private Vector3 replaceStartPos;

    [SerializeField, ReadOnly] private bool canInteract = true;
    private BaseInteractable lastHoveredObject = null;
    private Vector2 lastMousePosition;

    private void OnEnable()
    {
        InputProvider.OnInteractPressed += TryInteract;
        InputProvider.OnLookInput += UpdateMousePosition;
        InputProvider.OnInteractPressed += ReplaceItem;
        
        EventManager.OnItemSelected += CreateReplaceItem;
        EventManager.OnClearSelectedItem += ClearReplaceItem;
    }

    private void OnDisable()
    {
        InputProvider.OnInteractPressed -= TryInteract;
        InputProvider.OnLookInput -= UpdateMousePosition;
        InputProvider.OnInteractPressed -= ReplaceItem;
        
        EventManager.OnItemSelected -= CreateReplaceItem;
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
            if (!interactable.RequirementsMet(gameObject))
            {
                Debug.Log("Etkileşim için gerekli şartlar sağlanmadı!");
                return;
            }

            interactable.Interact(gameObject);
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
        this.selectedItem = selectedItem;
        replaceItem = CloneObject(selectedItem);
    }
    
    public GameObject CloneObject(GameObject original)
    {
        if (original == null) return null;

        ItemSO itemData = original.GetComponent<Item>().itemData;

        GameObject clone = new GameObject(original.name + "_Clone");
        clone.SetActive(false);
        
        clone.transform.position = original.transform.position;
        clone.transform.eulerAngles = itemData.ItemReplaceRotation;
        clone.transform.localScale = itemData.ItemReplaceScale;

        CloneMeshAndMaterials(original, clone);

        return clone;
    }

    private void CloneMeshAndMaterials(GameObject original, GameObject clone)
{
    MeshFilter originalMeshFilter = original.GetComponent<MeshFilter>();
    MeshRenderer originalRenderer = original.GetComponent<MeshRenderer>();
    SkinnedMeshRenderer originalSkinnedRenderer = original.GetComponent<SkinnedMeshRenderer>();

    // Eğer SkinnedMeshRenderer varsa, onu MeshFilter ve MeshRenderer'a dönüştür
    if (originalSkinnedRenderer != null)
    {
        MeshFilter cloneMeshFilter = clone.AddComponent<MeshFilter>();
        MeshRenderer cloneRenderer = clone.AddComponent<MeshRenderer>();

        // SkinnedMeshRenderer'den static bir mesh oluştur
        cloneMeshFilter.mesh = originalSkinnedRenderer.sharedMesh;

        // Materyalleri ata
        Material[] originalMaterials = originalSkinnedRenderer.sharedMaterials;
        Material[] newMaterials = new Material[originalMaterials.Length];

        for (int i = 0; i < originalMaterials.Length; i++)
        {
            newMaterials[i] = replaceMaterial;
        }

        cloneRenderer.sharedMaterials = newMaterials;
    }
    else if (originalMeshFilter != null) // Normal MeshFilter ve MeshRenderer kopyalaması
    {
        MeshFilter cloneMeshFilter = clone.AddComponent<MeshFilter>();
        MeshRenderer cloneRenderer = clone.AddComponent<MeshRenderer>();

        cloneMeshFilter.mesh = originalMeshFilter.mesh;

        Material[] originalMaterials = originalRenderer.sharedMaterials;
        Material[] newMaterials = new Material[originalMaterials.Length];

        for (int i = 0; i < originalMaterials.Length; i++)
        {
            newMaterials[i] = replaceMaterial;
        }

        cloneRenderer.sharedMaterials = newMaterials;
    }

    // Çocuk nesneleri de klonla
    foreach (Transform child in original.transform)
    {
        GameObject childClone = new GameObject(child.name);
        childClone.transform.SetParent(clone.transform);
        childClone.transform.localPosition = child.localPosition;
        childClone.transform.localRotation = child.localRotation;
        childClone.transform.localScale = child.localScale;

        CloneMeshAndMaterials(child.gameObject, childClone);
    }
}


    
    #endregion

    private void ClearReplaceItem()
    {
        Destroy(replaceItem);
        replaceItem = null;
    }

    private void ReplaceItem()
    {
        if (lastHoveredObject || !replaceItem || !replaceItem.activeSelf || !selectedItem) return;

        Item item = selectedItem.GetComponent<Item>();
        
        item.UpdateItemPosition(replaceItem.transform.position);
        item.UpdateItemScale(item.itemData.ItemReplaceScale);
        item.UpdateItemRotation(item.itemData.ItemReplaceRotation);

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
            }
        }
        else
        {
            replaceItem.SetActive(false);
        }
    }

    

}