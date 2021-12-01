using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersManager : MonoBehaviour
{
    public static PlayersManager _instance;
    
    [SerializeField]
    List<PlayerCombat> players;
    List<bool> playersReady;

    int currentPlayer;

    void Awake()
    {
        _instance = this;

        playersReady = new List<bool>();
        for(int i = 0; i < players.Count; ++i) {
            playersReady.Add(false);
        }

        currentPlayer = -1;
    }

    public void InitCombat()
    {
        currentPlayer = -1;
        for (int i = 0; i < players.Count; ++i) {
            playersReady[i] = false;
        }
        for (int i = 0; i < players.Count; ++i) {
            players[i].InitCombat();
        }
        enabled = true;
    }

    public void PlayerReady(PlayerCombat player)
    {
        playersReady[players.IndexOf(player)] = true;

        if (currentPlayer == -1) {
            currentPlayer = players.IndexOf(player);
            players[currentPlayer].OnSelectForAction(true);
        }
    }

    public void OnPlayerDie(PlayerCombat playerCombat)
    {
        int idPlayer = players.IndexOf(playerCombat);

        playersReady[idPlayer] = false;

        if(currentPlayer == idPlayer) {
            SelectNextPlayer(1);
        }
    }

    internal int GetCurrentMana()
    {
        return players[currentPlayer].GetMana();
    }

    private void Update()
    {
        if(currentPlayer > -1) {
            int dir = InputController.downDown ? 1 : InputController.upDown ? -1 : 0;

            if(dir != 0) {
                SelectNextPlayer(dir);
            }

            if(InputController.ineraction) {
                players[currentPlayer].OnOpenActionList();
            }
        }
    }

    void SelectNextPlayer(int dir)
    {
        int newPlayer = mod(currentPlayer + dir, players.Count);

        while (!playersReady[newPlayer] && currentPlayer != newPlayer) {
            newPlayer = mod(newPlayer + dir, players.Count);
        }

        if (currentPlayer != -1) {
            players[currentPlayer].OnSelectForAction(false);
        }

        if(!playersReady[newPlayer]) {
            currentPlayer = -1;
        } else {
            players[newPlayer].OnSelectForAction(true);
            currentPlayer = newPlayer;
        }
    }

    public static int mod(int x, int m)
    {
        return (x % m + m) % m;
    }

    public void OnAskForAction(ActionCombat action, TargetCombat target)
    {
        CombatRequest request = new CombatRequest();
        request.actor = players[currentPlayer];
        request.action = action;
        request.target = target;

        CombatManager._instance.OnReceiveRequest(request);

        playersReady[currentPlayer] = false;
        SelectNextPlayer(1);
    }

    public void OnWin()
    {
        for (int i = 0; i < players.Count; ++i) {
            players[i].OnWin();
        }
    }
}
