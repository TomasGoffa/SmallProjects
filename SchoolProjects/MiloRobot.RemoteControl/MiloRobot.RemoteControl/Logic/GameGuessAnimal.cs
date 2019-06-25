/// <summary>
/// This file is part of project for remote
/// control of Robokind’s Milo Robot
/// 
/// Author:     Tomas Goffa
/// </summary>

namespace MiloRobot.RemoteControl.Logic
{
    using MiloRobot.RemoteControl.UserInterface;
    using System;
    using System.Linq;
    using System.Threading;
    using System.Windows;

    public class GameGuessAnimal : IDisposable
    {
        private readonly MiloControl robotClient;

        /// <summary>
        /// Sets the order in which images are showed.
        /// </summary>
        private int[] orderOfPictures;

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
        /// Currrently right answer
        /// </summary>
        private string answer = string.Empty;

        /// <summary>
        /// Thread for showing images on Milo's display.
        /// </summary>
        public Thread showingImages;

        private readonly string BEAR = "/./usr/robokind/games/showImage.sh bear.jpg";
        private readonly string CAT = "/./usr/robokind/games/showImage.sh cat.jpg";
        private readonly string DOG = "/./usr/robokind/games/showImage.sh dog.jpg";
        private readonly string ELEPHANT = "/./usr/robokind/games/showImage.sh elephant.jpg";
        private readonly string FOX = "/./usr/robokind/games/showImage.sh fox.jpg";
        private readonly string GOAT = "/./usr/robokind/games/showImage.sh goat.jpg";
        private readonly string HORSE = "/./usr/robokind/games/showImage.sh horse.jpg";
        private readonly string LION = "/./usr/robokind/games/showImage.sh lion.jpg";
        private readonly string PANDA = "/./usr/robokind/games/showImage.sh panda.jpg";
        private readonly string RABBIT = "/./usr/robokind/games/showImage.sh rabbit.jpg";
        private readonly string SHEEP = "/./usr/robokind/games/showImage.sh sheep.jpg";
        private readonly string TIGER = "/./usr/robokind/games/showImage.sh tiger.jpg";

        private readonly string LETS_PLAY_A_GAME = "Let's play a game.";
        private readonly string END_OF_GAME = "This is end of the game. Thank you for playing.";
        private readonly string START_GUESS = "Look at my chest. What animal do you see?";
        private readonly string GOOD_ANSWER = "You answered right. Congratulations.";
        private readonly string BAD_ANSWER = "No, you are wrong. Try again.";
        private readonly string LETS_CONTINUE = "Let's continue.";

        private readonly string CLOSE_IMAGE = "/./usr/robokind/games/closeImage.sh";

        private readonly int NUMBER_OF_ANIMALS = 12;

        private SpeechRecogniserGuessAnimals speechRecogniser;

        public GameGuessAnimal ()
        {
            this.robotClient = MiloControl.GetInstance();
        }

        /// <summary>
        /// Starts the game
        /// </summary>
        public void Start ()
        {
            try
            {
                this.speechRecogniser = new SpeechRecogniserGuessAnimals (this);
                this.iOrder = 0;
                this.robotClient.SendCommand (this.CLOSE_IMAGE);

                int[] myArray = new int[this.NUMBER_OF_ANIMALS];
                for (int i = 0; i < this.NUMBER_OF_ANIMALS; i++)
                {
                    myArray[i] = i;
                }

                // Shuffle array
                Random random = new Random ();
                this.orderOfPictures = myArray.OrderBy (x => random.Next ()).ToArray ();

                this.robotClient.SendCommand ("speak \"" + this.LETS_PLAY_A_GAME + "\"");
                this.showingImages = new Thread (ShowImages);
                this.showingImages.Start ();

                while (this.iOrder < this.NUMBER_OF_ANIMALS)
                {
                    this.LetsPlay ();
                }

                this.robotClient.SendCommand ("speak \"" + this.END_OF_GAME + "\"");
                this.robotClient.SendCommand (this.CLOSE_IMAGE);

                this.showingImages.Abort ();
            }
            catch (Exception ex)
            {
                Logger.LogMessage ("GuessAnimal_Game: Start():\n" + ex.ToString ());
            }
        }

        /// <summary>
        /// Main logic of game
        /// </summary>
        private void LetsPlay ()
        {
            try
            {
                this.robotClient.SendCommand (this.CLOSE_IMAGE);

                switch (this.orderOfPictures[this.iOrder])
                {
                    case 0:
                        this.answer = "bear";
                        break;
                    case 1:
                        this.answer = "cat";
                        break;
                    case 2:
                        this.answer = "dog";
                        break;
                    case 3:
                        this.answer = "elephant";
                        break;
                    case 4:
                        this.answer = "fox";
                        break;
                    case 5:
                        this.answer = "goat";
                        break;
                    case 6:
                        this.answer = "horse";
                        break;
                    case 7:
                        this.answer = "lion";
                        break;
                    case 8:
                        this.answer = "panda";
                        break;
                    case 9:
                        this.answer = "rabbit";
                        break;
                    case 10:
                        this.answer = "sheep";
                        break;
                    case 11:
                        this.answer = "tiger";
                        break;
                }
                this.robotClient.SendCommand ("speak \"" + this.START_GUESS + "\"");

                // Listening
                SetRecognisedWord (string.Empty);
                this.speechRecogniser.StartRecognition ();

                while (string.IsNullOrEmpty (this.recognisedWord))
                {
                    Thread.Sleep (100);
                }

                this.speechRecogniser.StopRecognition ();
                this.isCorrect = recognisedWord.Equals (answer);

                if (isCorrect)
                {
                    this.robotClient.SendCommand ("anim-play /usr/robokind/etc/gui/anims/AZR25_victoryPose_01.anim.xml");
                    this.robotClient.SendCommand ("speak \"" + this.GOOD_ANSWER + "\"");
                    this.iOrder++;
                    if (this.iOrder < 12)
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
                Logger.LogMessage ("GuessAnimal_Game: LetsPlay():\n" + ex.ToString ());
            }
        }

        /// <summary>
        /// Shows images of animals on Milo's display
        /// </summary>
        private void ShowImages ()
        {
            try
            {
                while (true)
                {
                    switch (this.orderOfPictures[this.iOrder])
                    {
                        case 0:
                            this.robotClient.SendCommand (this.BEAR);
                            break;
                        case 1:
                            this.robotClient.SendCommand (this.CAT);
                            break;
                        case 2:
                            this.robotClient.SendCommand (this.DOG);
                            break;
                        case 3:
                            this.robotClient.SendCommand (this.ELEPHANT);
                            break;
                        case 4:
                            this.robotClient.SendCommand (this.FOX);
                            break;
                        case 5:
                            this.robotClient.SendCommand (this.GOAT);
                            break;
                        case 6:
                            this.robotClient.SendCommand (this.HORSE);
                            break;
                        case 7:
                            this.robotClient.SendCommand (this.LION);
                            break;
                        case 8:
                            this.robotClient.SendCommand (this.PANDA);
                            break;
                        case 9:
                            this.robotClient.SendCommand (this.RABBIT);
                            break;
                        case 10:
                            this.robotClient.SendCommand (this.SHEEP);
                            break;
                        case 11:
                            this.robotClient.SendCommand (this.TIGER);
                            break;
                    }

                    Thread.Sleep (1500);
                }
            }
            catch (Exception ex)
            {
                Logger.LogMessage ("GuessAnimal_Game: ShowImages():\n" + ex.ToString ());
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
                this.robotClient.SendCommand (this.CLOSE_IMAGE);
            }
            catch (Exception ex)
            {
                Logger.LogMessage ("GuessAnimal_Game: Dispose():\n" + ex.ToString ());
            }
        }
    }
}
