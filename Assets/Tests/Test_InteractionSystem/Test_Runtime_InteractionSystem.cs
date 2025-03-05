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
        yield return null;
    }
}
