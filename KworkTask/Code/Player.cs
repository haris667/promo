using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Creature
{
    public float speed;
    public FixedJoystick joystick;
    private Rigidbody2D rigidbody2D;

    private Vector3 direction; // направление движения
    private Vector3 rayDirection; // направления луча
    public Bag bag = new Bag();

    private void Start() 
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        contactFilter = LayerMask.GetMask("InteractableObject"); // луч может взаимодейстовать только с нужными еам объектами
    } 
    private void Update()
    {
        ChangeRayDirection();
        Move();

        ChangeAnimation();
    }

    public override void Move() //задаем значения в вектор и передаем его как физическую силу
    {
        direction.x = joystick.Horizontal;
        direction.y = joystick.Vertical;
        rigidbody2D.velocity = direction * speed;

        ChangeAnimation();
    }
    public void Action()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDirection * 3f, 2f, contactFilter); // инициализация самого луча

        if(hit && hit.transform.TryGetComponent(out ICollected collectedObject)) //проверка на касания
            GetItem(collectedObject); //если предмет можно собирать то собираем его
        if(hit && hit.transform.TryGetComponent(out IInteractable interactableObject))
            InteractionWithObject(interactableObject);
    }

    //Действие
    private void ChangeRayDirection() =>  rayDirection = direction.sqrMagnitude > 0 ? direction : rayDirection;
     //смотрим менялось ли направление движения, если менялось - меняем направление луча

    //вспомогательные методы
    private void GetItem(ICollected collectedObject) => bag.AddItem(collectedObject.item);
    private void InteractionWithObject(IInteractable interactableObject) 
    {
        interactableObject.Interaction(this);

        actionView = interactableObject.TypeInteraction();
        actionView.MakeAnAnimatedAction(animator);
    }
    
    private void ChangeAnimation() 
    {
        if(direction.sqrMagnitude > 0)
        {
            animator.SetFloat("X", direction.x);
            animator.SetFloat("Y", direction.y);
        }
        
        animator.SetFloat("Speed", direction.sqrMagnitude);
    }
}
