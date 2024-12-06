using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State
{

    private int HP;
    private float speed;
    private int defend;
    private int attack;

    public State()
    {
        HP = 100;
        speed = 10f;
        defend = 50;
        attack = 100;
    }

    public void UpdateState()
    {

    }

    public void PrintCurrentState()
    {
        Debug.Log($"HP : {HP}, Speed : {speed}, Defend : {defend}, Attack : {attack}");
    }
}

public class PlayerState : MonoBehaviour
{

}
