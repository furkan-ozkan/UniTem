using UnityEngine;

public abstract class ActionContext { }

public class Context_Action_Fire : ActionContext
{
    public GameObject bulletPrefab;
    public Vector3 spawnPosition;
    public Vector3 direction;
    public float speed;
    public float range;

    public Context_Action_Fire(GameObject bulletPrefab, Vector3 spawnPosition, Vector3 direction, float speed, float range)
    {
        this.bulletPrefab = bulletPrefab;
        this.spawnPosition = spawnPosition;
        this.direction = direction;
        this.speed = speed;
        this.range = range;
    }
}

public enum Context_Action_Rotate_Axis
{
    all,
    x,
    y,
    z
}
public class Context_Action_Rotate : ActionContext
{
    public GameObject rotateObject;
    public Vector3 rotationAngle;
    public float rotationTime;
    public Context_Action_Rotate_Axis axis;

    public Context_Action_Rotate(GameObject rotateObject, Vector3 rotationAngle, float rotationTime, Context_Action_Rotate_Axis axis = Context_Action_Rotate_Axis.all)
    {
        this.rotateObject = rotateObject;
        this.rotationAngle = rotationAngle;
        this.rotationTime = rotationTime;
        this.axis = axis;
    }
}