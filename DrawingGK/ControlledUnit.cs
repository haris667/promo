using UnityEngine;

public class ControlledUnit : Unit
{
    public ControlledUnit(IMovable movableBehaviour) : base(movableBehaviour) {}

    public override void Move() => movableBehaviour.Move(target, transform);

    private void Update() => Move();
}