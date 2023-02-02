using UnityEngine;

//базовый класс для всех существ
[RequireComponent(typeof(Animator))]
public abstract class Creature : MonoBehaviour //класс абстрактный => не может иметь экземпляра
{
    public IAnimatedAction actionView; // отображение действия. Паттерн стратегия
    protected LayerMask contactFilter;
    protected Animator animator;

    protected void Awake() => animator = GetComponent<Animator>();
    public abstract void Move();

    //используемые паттерны:
    //стратегия - замена поведения схожих сущностей через интерфейсы и классы с конкретной реализацией поведения
    //синглтон - задает глобальную точку доступа к 1 экземпляру класса, более 1 экземпляра такой класс иметь не может. Стоит быть аккуратнее с этим паттерном, т.к. легко превратить его в антипаттерн "класс Бога", где класс отвечает за слишком многое
}
