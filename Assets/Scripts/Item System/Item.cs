using UnityEngine;

/// <summary>
/// Oyun dünyasındaki bir item ile etkileşimi yöneten sınıf.
/// BaseInteractable sınıfından kalıtım alarak temel etkileşim mantığını kullanır.
/// </summary>
public class Item : BaseInteractable
{
    public ItemSO itemData;

    /// <summary>
    /// Oyuncu item ile etkileşime geçtiğinde tetiklenir.
    /// Bu metod event yöneticisi üzerinden ilgili event'i çağırır.
    /// </summary>
    /// <param name="player">Etkileşimde bulunan oyuncu nesnesi.</param>
    /// <returns>İşlemin başarılı olup olmadığını belirtir.</returns>
    public override bool Interact(GameObject player)
    {
        EventManager.ItemClicked(player, this);
        return true;
    }
    
    /// <summary>
    /// Item kullanıldığında tetiklenecek işlemleri gerçekleştirir.
    /// Örneğin, item tüketme veya aktif etme işlemleri burada ele alınabilir.
    /// </summary>
    /// <param name="player">Item'ı kullanacak oyuncu nesnesi.</param>
    public void UseItem(GameObject player)
    {
        EventManager.ItemUsed(player, this);
    }
}