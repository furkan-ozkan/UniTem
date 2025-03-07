using System;
using UnityEngine;

/// <summary>
/// Oyun içindeki item etkileşimlerini merkezi bir şekilde yöneten event yöneticisi.
/// 
/// Bu sınıf, item tıklama, envanterde seçim ve item kullanım gibi etkileşim durumlarına
/// karşılık gelen event'leri içerir. Kodun dokümantasyonu sayesinde ekip içindeki diğer geliştiriciler
/// de hangi event'in ne işe yaradığını kolayca anlayabilir.
/// </summary>
public static class EventManager
{
    /// <summary>
    /// Oyun dünyasında bir item tıklandığında tetiklenir.
    /// </summary>
    public static Action<GameObject, Item> OnItemClicked;
    
    /// <summary>
    /// Envanterde bir item seçildiğinde tetiklenir.
    /// </summary>
    public static Action<GameObject, Item> OnItemSelectedInInventory;
    
    /// <summary>
    /// Bir item kullanıldığında (örneğin; tüketildiğinde veya aktif hale getirildiğinde) tetiklenir.
    /// </summary>
    public static Action<GameObject, Item> OnItemUsed;

    /// <summary>
    /// Belirtilen oyuncu ve item bilgilerini kullanarak item tıklama event'ini tetikler.
    /// </summary>
    public static void ItemClicked(GameObject player, Item clickedItem) => OnItemClicked?.Invoke(player, clickedItem);
    
    /// <summary>
    /// Belirtilen oyuncu ve item bilgilerini kullanarak envanterde item seçimi event'ini tetikler.
    /// </summary>
    public static void ItemSelectedInInventory(GameObject player, Item clickedItem) => OnItemSelectedInInventory?.Invoke(player, clickedItem);
    
    /// <summary>
    /// Belirtilen oyuncu ve item bilgilerini kullanarak item kullanımı event'ini tetikler.
    /// </summary>
    public static void ItemUsed(GameObject player, Item usedItem) => OnItemUsed?.Invoke(player, usedItem);
}