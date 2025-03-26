using UnityEngine;

public class PlayerInventoryInput : MonoBehaviour
{
    public event System.Action<int> OnInventorySlotChange = null;

    private void Update()
    {
        for (int i = 0; i <= 9; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha0 + i))
            {
                int index = i - 1;
                int calculatedIndex = Mathf.Sign(index).Equals(-1) ? 10 : index;
                OnInventorySlotChange?.Invoke(calculatedIndex);
            }
        }
    }
}