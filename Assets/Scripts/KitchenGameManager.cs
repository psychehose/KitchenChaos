using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenGameManager : MonoBehaviour
{
   public static KitchenGameManager Instance { get; private set; }
   private enum State
   {
      WaitingToStart,
      CountdownToStart,
      GamePlaying,
      GameOver,
   }


   public event EventHandler OnStateChagnged;

   private State state;
   private float waitingToStartTimer = 1f;
   private float countdownToStartTimer = 3f;
   // For Test
   private float gamePlayingTimer;
   private float gamePlayingTimerMax = 15f;

   private void Awake()
   {
      Instance = this;
      state = State.WaitingToStart;
   }

   private void Update()
   {
      switch (state)
      {
         case State.WaitingToStart:
            waitingToStartTimer -= Time.deltaTime;

            if (waitingToStartTimer < 0f)
            {
               state = State.CountdownToStart;
               OnStateChagnged?.Invoke(this, EventArgs.Empty);
            }

            break;
         
         case State.CountdownToStart:
            countdownToStartTimer -= Time.deltaTime;

            if (countdownToStartTimer < 0f)
            {
               state = State.GamePlaying;
               gamePlayingTimer = gamePlayingTimerMax;
               OnStateChagnged?.Invoke(this, EventArgs.Empty);
            }

            break;
         
         case State.GamePlaying:
            gamePlayingTimer -= Time.deltaTime;

            if (gamePlayingTimer < 0f)
            {
               state = State.GameOver;
               OnStateChagnged?.Invoke(this, EventArgs.Empty);
            }

            break;
         
         case State.GameOver:
            break;
      }
      
      Debug.Log(state);
   }

   public bool IsGamePlaying() {
      return state == State.GamePlaying;
   }

   public bool IsCountdownToStartActive()
   {
      return state == State.CountdownToStart;
   }

   public float GetCountdownToStartTimer()
   {
      return countdownToStartTimer;
   }


   public bool IsGameOver()
   {
      return state == State.GameOver;
   }

   public float GetGamePlayingTimerNormalized()
   {
      return 1 - (gamePlayingTimer / gamePlayingTimerMax);
   }
   
   
}
