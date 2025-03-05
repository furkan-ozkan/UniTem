using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class Test_Runtime_InventorySystem
{
    private GameObject inventoryObject;
    private Inventory inventory;

    [SetUp]
    public void SetUp()
    {
        inventoryObject = new GameObject("TestInventory");
        inventory = inventoryObject.AddComponent<Inventory>();
    }

    [TearDown]
    public void TearDown()
    {
        Object.Destroy(inventoryObject);
        Object.Destroy(inventory);
    }

    [UnityTest]
    public IEnumerator Inventory_Add_Item_Works()
    {
        // Arrange
        int testItemId = 1;

        // Act
        bool result = inventory.AddItemById(testItemId);
        
        // Assert
        Assert.IsNotNull(inventory); 
        Assert.IsTrue(result);
        Assert.IsTrue(inventory.ContainsItemById(testItemId));

        yield return null;
    }


    [UnityTest]
    public IEnumerator Inventory_AddMoreThanCapacity()
    {
        int capacity = inventory.GetCapacity();
        
        for (int i = 0; i < capacity + 1; i++)
        {
            inventory.AddItemById(i + 1);
        }

        Assert.AreEqual(capacity, inventory.GetItemsCount());

        yield return null;
    }

    [UnityTest]
    public IEnumerator Inventory_RemoveToNotContainsItem()
    {
        int nonExistentItemId = 99;
        bool isRemoved = inventory.RemoveItemById(nonExistentItemId);

        Assert.IsFalse(isRemoved);

        yield return null;
    }

    [UnityTest]
    public IEnumerator Inventory_CheckContainedItem()
    {
        int testItemId = 1;
        inventory.AddItemById(testItemId);

        Assert.IsTrue(inventory.ContainsItemById(testItemId));

        yield return null;
    }

    [UnityTest]
    public IEnumerator Inventory_CheckNotContainedItem()
    {
        int testItemId = 1;

        Assert.IsFalse(inventory.ContainsItemById(testItemId));

        yield return null;
    }

    [UnityTest]
    public IEnumerator Inventory_ClearInventory()
    {
        inventory.AddItemById(1);
        inventory.AddItemById(2);

        inventory.ClearInventory();

        Assert.AreEqual(0, inventory.GetItemsCount());

        yield return null;
    }

    [UnityTest]
    public IEnumerator Inventory_AddSameItemMultipleTimes()
    {
        int testItemId = 1;

        inventory.AddItemById(testItemId);
        inventory.AddItemById(testItemId);
        inventory.AddItemById(testItemId);

        Assert.AreEqual(3, inventory.GetItemsCount());

        yield return null;
    }

    [UnityTest]
    public IEnumerator Inventory_AddAndRemoveWithClearInventory()
    {
        int testItemId = 1;

        inventory.AddItemById(testItemId);
        inventory.RemoveItemById(testItemId);
        inventory.ClearInventory();

        Assert.AreEqual(0, inventory.GetItemsCount());

        yield return null;
    }

    [UnityTest]
    public IEnumerator Inventory_AddAndRemoveWithFullInventory()
    {
        int capacity = inventory.GetCapacity();

        for (int i = 0; i < capacity; i++)
        {
            inventory.AddItemById(i + 1);
        }
        int extraItemId = 99;

        inventory.AddItemById(extraItemId);
        inventory.RemoveItemById(extraItemId);

        Assert.AreEqual(capacity, inventory.GetItemsCount());

        yield return null;
    }

    [UnityTest]
    public IEnumerator Inventory_AddAndRemoveAndAddBackWithFullInventory()
    {
        int capacity = inventory.GetCapacity();

        for (int i = 0; i < capacity; i++)
        {
            inventory.AddItemById(i + 1);
        }
        int testItemId = 1;

        inventory.AddItemById(testItemId);
        inventory.RemoveItemById(testItemId);
        inventory.AddItemById(testItemId);

        Assert.AreEqual(capacity, inventory.GetItemsCount());

        yield return null;
    }

    [UnityTest]
    public IEnumerator Inventory_AddNullItem()
    {
        bool added = inventory.AddItemById(-1);

        Assert.IsFalse(added);

        yield return null;
    }

    [UnityTest]
    public IEnumerator Inventory_RemoveNullItem()
    {
        bool removed = inventory.RemoveItemById(-1);

        Assert.IsFalse(removed);

        yield return null;
    }
}
