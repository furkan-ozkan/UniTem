using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Collider), typeof(MeshRenderer))]
public class PlacePreviewItem : MonoBehaviour
{
    [SerializeField] private LayerMask _collideLayer = 0;
    [SerializeField] private Material canPlaceMaterial = null, canNotPlaceMaterial = null;

    private BoxCollider _boxCollider = null;
    private MeshRenderer _meshRenderer = null;

    private bool isCollide = false;
    private Collider[] _overlapResults = new Collider[10];

    private void Awake()
    {
        _boxCollider = GetComponent<BoxCollider>();
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    public void Initialize<T>(T Item) where T : Item
    {
        MeshFilter itemMeshFilter = Item.GetComponent<MeshFilter>();
        GetComponent<MeshFilter>().mesh = itemMeshFilter.mesh;

        transform.localScale = Item.transform.localScale;
        _boxCollider.size = Item.GetComponent<BoxCollider>().size;
        _boxCollider.center = Item.GetComponent<BoxCollider>().center;
    }

    private void Update()
    {
        CheckOverlap();
        _meshRenderer.material = isCollide ? canNotPlaceMaterial : canPlaceMaterial;
    }

    private void CheckOverlap()
    {
        Vector3 scaledSize = Vector3.Scale(_boxCollider.size, transform.localScale) / 2.0f;
        int count = Physics.OverlapBoxNonAlloc(transform.position + _boxCollider.center, scaledSize, _overlapResults, transform.rotation, _collideLayer);
        isCollide = count > 0;
    }

    private void OnDrawGizmos()
    {
        if (_boxCollider == null) 
            return;
        
        Gizmos.color = isCollide ? Color.red : Color.green;
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
        Vector3 scaledSize = Vector3.Scale(_boxCollider.size, transform.localScale);
        Gizmos.DrawWireCube(_boxCollider.center, scaledSize);
    }

    public void SetVisibleState(bool state)
    {
        if (gameObject.activeSelf.Equals(state))
            return;

        gameObject.SetActive(state);
    }

    public bool CanPlace() => !isCollide;
}