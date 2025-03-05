using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class Test_Runtime_InteractionSystem
{
    private GameObject player;
    private GameObject interactable;

    [SetUp]
    public void SetUp()
    {
        player = new GameObject("Player");
        interactable = new GameObject("Interactable");
    }

    [TearDown]
    public void TearDown()
    {
        
    }

    [UnityTest]
    public IEnumerator Interactable_JustInteraction()
    {
        // Arrange
        Test_Interaction_Door door = interactable.AddComponent<Test_Interaction_Door>();
        ActionInvoker actionInvoker = player.AddComponent<ActionInvoker>();
        door._openedRotation = new Vector3(0, 90, 0);
        door._closedRotation = new Vector3(0, 0, 0);
        door._doorRotate = ScriptableObject.CreateInstance<Test_Action_Door_Rotate>();
        
        yield return null;
    }
}
