using UnityEngine;

//Конкретная реализация взаимодействия атаки. Паттерн стратегия
public class AttackingInteraction : IAnimatedAction
{
    public void MakeAnAnimatedAction(Animator animator) => animator.SetTrigger("Attack");
}