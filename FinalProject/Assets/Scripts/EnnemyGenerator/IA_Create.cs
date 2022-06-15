using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IA_Create : MonoBehaviour
{
    //public int nbBots;
    public Target ennemy;
    public PlayerController player;
    public Text nb_round_affichage;

    public int nbBots = 1;

    public float init_damage = 3f;
    public float init_health = 10f;
    public float attack_distance = 2f;
    public float increase_health = 1f;
    public float increase_attack = 0.5f;

    private List<Target> bot;
    private int round;
    private bool restarting;

    Vector3 spawnPoints(){
        List<Vector3> my_list = new List<Vector3>();
        my_list.Add(new Vector3(-2, 4, 115));
        my_list.Add(new Vector3(-2, 4, 145));
        my_list.Add(new Vector3(-7, 6, 140));
        my_list.Add(new Vector3(-26, 7, 136));
        my_list.Add(new Vector3(-26, 5, 115));
        my_list.Add(new Vector3(5, 3, 115));
        my_list.Add(new Vector3(10, 3, 95));
        my_list.Add(new Vector3(31, 3, 87));
        my_list.Add(new Vector3(28, 3, 118));
        my_list.Add(new Vector3(-27, 3, 92));
        my_list.Add(new Vector3(-41, 5, 107));

        return my_list[Random.Range(0, my_list.Count- 1)];
    }

    // Start is called before the first frame update
    void Start()
    {
        round = 0;
        SetupBots();
        UpdateRoundNumber();
        restarting = false;
    }

    void SetupBots(){
        bot = new List<Target>();
        for (int i = 0; i < nbBots; i++){
            Target new_bot = Instantiate (ennemy, spawnPoints(), Quaternion.identity);
            new_bot.Setup(init_health + round * increase_health, init_damage + round * init_damage, attack_distance, player);
            bot.Add(new_bot);
        }
    }

    void UpdateRoundNumber(){
        nb_round_affichage.text = "Round NB : " + round.ToString() + " Ghost left : " + nbBots;
    }

    void Restart(){
        for (int i = 0; i < nbBots; i++){
            Target old_bot = bot[i];
            Destroy(old_bot);
        }
        nbBots = 1;
        round = 0;
        SetupBots();
        UpdateRoundNumber();
        restarting = false;
        player.Respawn();
    }

    void Update(){
        if (restarting == false){
            if (nbBots <= 0){
                round += 1;
                nbBots = (int)(round * 1.2f) + 1;
                UpdateRoundNumber();
                SetupBots();
            }
            for (int i = 0; i < nbBots; i++){
                if (bot[i].IsAlive() == false){
                    kill(i);
                    UpdateRoundNumber();
                }
            }
            if (player.life == 0){
                nb_round_affichage.text = "You died in round " + round.ToString();
                restarting = true;
                Invoke("Restart", 5f);
            }
        }
    }

    /*public void Respawn(int reference){
        Target old_bot = bot[reference];
        Target new_bot = Instantiate (ennemy, spawnPoints(), Quaternion.identity);
        new_bot.Setup(old_bot.getHealth() + increase_health, old_bot.getDamage() + increase_attack, attack_distance, reference, player);
        Destroy(old_bot);
        bot[reference] = new_bot;
    }*/

    public void kill(int reference){
        Target old_bot = bot[reference];
        Destroy(old_bot);
        bot.RemoveAt(reference);
        nbBots -= 1;
        player.IncreasePower();
    }
}