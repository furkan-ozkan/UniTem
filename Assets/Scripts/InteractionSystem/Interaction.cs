using UnityEngine;
using Cysharp.Threading.Tasks;

public class Interaction : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float interactionDistance = 3f;
    [SerializeField] private float interactionCooldown = 0.2f; 
    [SerializeField] private bool _isMouseLocked;
    private IInputHandler inputHandler;
    private bool canInteract = true;

    public void Initialize(IInputHandler handler)
    {
        inputHandler = handler;
    }

    private void Awake()
    {
        if (inputHandler == null) 
            inputHandler = new NewInputHandler();
    }

    private void Update()
    {
        if (inputHandler.IsInteractPressed() && canInteract) 
        {
            TryInteract();
        }
    }

    public void TryInteract()
    {
        Ray ray = GenerateRay();
        if (PerformRaycast(ray, out RaycastHit hit))
        {
            HandleInteraction(hit.collider.gameObject);
            InteractionCooldown().Forget(); 
        }
    }

    private Ray GenerateRay()
    {
        return _isMouseLocked 
            ? new Ray(playerCamera.transform.position, playerCamera.transform.forward)
            : playerCamera.ScreenPointToRay(inputHandler.GetLookInput());
    }

    private bool PerformRaycast(Ray ray, out RaycastHit hit)
    {
        return Physics.Raycast(ray, out hit, interactionDistance);
    }

    public void HandleInteraction(GameObject target)
    {
        if (target.TryGetComponent(out BaseInteractable interactable))
        {
            ActionContext context = new ActionContext
            {
                        // Etkileşime girilen nesne
            };

            if (interactable.CanInteract(context))
            {
                interactable.Interact(context);
            }
            else
            {
                Debug.Log("Etkileşim için gerekli şartlar sağlanmadı!");
            }
        }
    }


    private async UniTaskVoid InteractionCooldown()
    {
        canInteract = false;
        Debug.LogWarning("Interaction On Cooldown");
        await UniTask.Delay((int)(interactionCooldown * 1000)); 
        canInteract = true;
    }
}