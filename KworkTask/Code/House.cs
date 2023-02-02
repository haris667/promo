using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class House : MonoBehaviour, IInteractable
{
    public Image healthBar;
    public List<Sprite> spritesHouse = new List<Sprite>();// картинки с состоянием здания

    public float maxHealth;
    public bool deathed;
    private float health = 15;
    private SpriteRenderer currentSprite;

    private void Start() => currentSprite = GetComponent<SpriteRenderer>();
    private void Update() => RemoveHealthPerSeconds();
    private void AddHealth(int count) => health += count * 5;

    public void Interaction(Player player) //метод вызываемый игроком
    {
        AddHealth(player.bag.CountItemsByID(0));
        player.bag.RemoveAllItemsByID(0);

        ChangeSpriteHouse(health);
    }

    public IAnimatedAction TypeInteraction()
    {
        return new AttackingInteraction();
    }
    private void RemoveHealthPerSeconds()// понижение жизней относительно времени
    {
        health -= Time.deltaTime / 3;
        healthBar.fillAmount = health / 100;
        ChangeSpriteHouse(health);

        if(health <= 0)  // состояние уничтожение и конец игры
        {
            deathed = true;
            SessionManager.S.AddDeathedHouse();
        }
    }
    private void ChangeSpriteHouse(float health) //переходы состояний здания
    {
        if(health < 20) currentSprite.sprite = spritesHouse[0];
        else if(health >= 20 && health < 40) currentSprite.sprite = spritesHouse[1];
        else if(health >= 40 && health < 60) currentSprite.sprite = spritesHouse[2];
        else if(health >= 60 && health < 80) currentSprite.sprite = spritesHouse[3];
        else if(health >= 80 && health < 100) currentSprite.sprite = spritesHouse[4];
        else currentSprite.sprite = spritesHouse[4];
    }
}
