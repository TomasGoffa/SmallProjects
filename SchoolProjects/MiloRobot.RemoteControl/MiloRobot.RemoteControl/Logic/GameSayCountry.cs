/// <summary>
/// This file is part of project for remote
/// control of Robokind’s Milo Robot
/// 
/// Author:     Tomas Goffa
/// </summary>

namespace MiloRobot.RemoteControl.Logic
{
    using System;
    using System.Linq;
    using System.Threading;

    public class GameSayCountry
    {
        /// <summary>
        /// 
        /// </summary>
        private int iOrder;

        ///// <summary>
        ///// Locker for setting recognisedWord variable from main thread.
        ///// </summary>
        private readonly object locker = new object ();

        ///// <summary>
        ///// Word recognised from Speech recognition engine
        ///// </summary>
        private string recognisedWord = string.Empty;

        /// <summary>
        /// True, if "recognisedWord" and "answer" are equal.
        /// False, otherwise.
        /// </summary>
        private bool isCorrect = false;

        /// <summary>
        /// Insatence of speech recognise engine.
        /// </summary>
        private SpeechRecogniserSayCountry speechRecogniser;

        /// <summary>
        /// Instance of Main window of this application.
        /// </summary>
        private readonly MiloControl robotClient;

        private string[] listOfShuffledLetters;

        #region Constants
        private readonly string[] listOfLetters = {"A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M",
                                           "N", "P", "R", "S", "T", "U", "V"};
        private readonly string LETS_PLAY_A_GAME = "Let's play a game.";
        private readonly string END_OF_GAME = "This is end of the game. Thank you for playing.";
        private readonly string START_GUESS = "Can you tell me a country which name starts with letter ";
        private readonly string GOOD_ANSWER = "You answered right. Congratulations.";
        private readonly string BAD_ANSWER = "No, you are wrong. Try again.";
        private readonly string LETS_CONTINUE = "Let's continue.";

        private readonly int NUMBER_OF_LETTERS = 20;
        #endregion Constants

        public GameSayCountry ()
        {
            this.robotClient = MiloControl.GetInstance ();
        }

        /// <summary>
        /// Starts the game
        /// </summary>
        public void Start ()
        {
            try
            {
                this.iOrder = 0;
                this.speechRecogniser = new SpeechRecogniserSayCountry (this);
                // Shuffle array
                Random random = new Random ();
                this.listOfShuffledLetters = this.listOfLetters.OrderBy (x => random.Next ()).ToArray ();

                // Logging order of letters
                Logger.LogMessage ("Order of letters:");
                for (int i = 0; i < this.listOfShuffledLetters.Length; i++)
                {
                    Logger.LogMessage (this.listOfShuffledLetters[i]);
                }
                Logger.LogMessage (Environment.NewLine);

                this.robotClient.SendCommand ("speak \"" + this.LETS_PLAY_A_GAME + "\"");

                while (this.iOrder < this.NUMBER_OF_LETTERS)
                {
                    this.LetsPlay ();
                }

                this.robotClient.SendCommand ("speak \"" + this.END_OF_GAME + "\"");
            }
            catch (Exception ex)
            {
                Logger.LogMessage ("SayCountry_Game: Start():\n" + ex.ToString ());
            }
        }

        /// <summary>
        /// Main logic of game
        /// </summary>
        private void LetsPlay ()
        {
            try
            {
                this.robotClient.SendCommand ("speak \"" + this.START_GUESS + this.listOfShuffledLetters[this.iOrder] + "?" + "\"");

                // Listening
                SetRecognisedWord (string.Empty);
                this.speechRecogniser.StartRecognition ();

                while (string.IsNullOrEmpty (this.recognisedWord))
                {
                    Thread.Sleep (100);
                }

                this.speechRecogniser.StopRecognition ();
                this.isCorrect = recognisedWord.Substring (0, 1).Equals (this.listOfShuffledLetters[this.iOrder]);


                if (isCorrect)
                {
                    this.robotClient.SendCommand ("anim-play /usr/robokind/etc/gui/anims/AZR25_victoryPose_01.anim.xml");
                    this.robotClient.SendCommand ("speak \"" + this.GOOD_ANSWER + "\"");
                    this.iOrder++;
                    if (this.iOrder < this.NUMBER_OF_LETTERS)
                    {
                        this.robotClient.SendCommand ("speak \"" + this.LETS_CONTINUE + "\"");
                    }
                }
                else
                {
                    this.robotClient.SendCommand ("anim-play /usr/robokind/etc/gui/anims/AZR25_denial_01.anim.xml");
                    this.robotClient.SendCommand ("speak \"" + this.BAD_ANSWER + "\"");
                }
            }
            catch (Exception ex)
            {
                Logger.LogMessage ("SayCountry_Game: LetsPlay():\n" + ex.ToString ());
            }
        }

        /// <summary>
        /// Sets the recognised word - answer
        /// </summary>
        /// <param name="myValue"></param>
        public void SetRecognisedWord (string myValue)
        {
            lock (this.locker)
            {
                this.recognisedWord = myValue;
            }
        }

        public void Dispose ()
        {
            try
            {
                this.speechRecogniser.StopRecognition ();
                this.speechRecogniser = null;
            }
            catch (Exception ex)
            {
                Logger.LogMessage ("SayCountry_Game: Dispose():\n" + ex.ToString ());
            }
        }
    }
}
