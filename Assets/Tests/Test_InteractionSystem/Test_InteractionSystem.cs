using NUnit.Framework;
using UnityEngine;

public class Test_InteractionSystem
{
    [Test]
    public void Interaction_ShouldCallInteract_WhenPressed()
    {
        // Arrange
        var interaction = new GameObject().AddComponent<Interaction>();

        var interactableObject = new GameObject();
        interactableObject.AddComponent<TestInteractable>();

        // Act
        interaction.HandleInteraction(interactableObject); 

        // Assert
        Assert.IsTrue(interactableObject.GetComponent<TestInteractable>().WasInteracted); 
    }


}
