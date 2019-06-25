/// <summary>
/// This file is part of project for remote
/// control of Robokind’s Milo Robot
/// 
/// Author:     Tomas Goffa
/// </summary>

namespace MiloRobot.RemoteControl.Logic
{
    using System;
    using System.Speech.Recognition;

    public class SpeechRecogniserGuessAnimals
    {
        private SpeechRecognitionEngine recEngine = new SpeechRecognitionEngine ();
        private readonly GameGuessAnimal guessAnimal;
        public SpeechRecogniserGuessAnimals (GameGuessAnimal animalGame)
        {
            this.guessAnimal = animalGame;
            Choices commands = new Choices ();
            commands.Add (new string[] { "bear", "cat", "dog", "elephant", "fox", "goat",
                                                "horse", "lion", "panda", "rabbit", "sheep", "tiger"});
            GrammarBuilder gBuilder = new GrammarBuilder ();
            gBuilder.Append (commands);
            Grammar grammar = new Grammar (gBuilder);

            recEngine.LoadGrammarAsync (grammar);

        }

        private void RecEngine_SpeechRecognized (object sender, SpeechRecognizedEventArgs e)
        {
            string recognisedWord = e.Result.Text;
            float classifierConfidence = e.Result.Confidence;
            if (classifierConfidence < 0.92)
            {
                return;
            }

            this.guessAnimal.SetRecognisedWord (recognisedWord);
        }

        public void StartRecognition ()
        {
            recEngine.SetInputToDefaultAudioDevice ();
            recEngine.SpeechRecognized += RecEngine_SpeechRecognized;
            recEngine.RecognizeAsync (RecognizeMode.Multiple);
        }

        public void StopRecognition ()
        {
            recEngine.RecognizeAsyncStop ();
        }
    }
}
