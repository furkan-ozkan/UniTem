using UnityEngine;
using RedAxeGames;

[RequireComponent(typeof(Rigidbody), typeof(Collider)), RequireComponent(typeof(Carryable), typeof(Placeable))]
public class Item : MonoBehaviour
{
    [SerializeField] private ItemDataBase itemDataBase = null;
    public ItemDataBase ItemDataBase => itemDataBase;
    
    
    private PlayerHand localPlayerHand = null;
    private Placeable localPlaceable = null;
    private Carryable localCarryable = null;

    public Placeable LocalPlaceable => localPlaceable;
    public Carryable LocalCarryable => localCarryable;

    protected virtual void Awake()
    {
        localPlayerHand = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerHand>();
        localCarryable = GetComponent<Carryable>();
        localPlaceable = GetComponent<Placeable>();
    }

    private void OnEnable()
    {
        localCarryable.OnCarriedEvent.AddListener(OnCarried);
    }

    private void OnDisable()
    {
        localCarryable.OnCarriedEvent.RemoveListener(OnCarried);
    }

    private void OnCarried()
    {
        localPlayerHand.CarriedItem(this);
    }
}