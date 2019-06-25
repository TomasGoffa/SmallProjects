﻿/// <summary>
/// This file is part of project for remote
/// control of Robokind’s Milo Robot
/// 
/// Author:     Tomas Goffa
/// </summary>

namespace MiloRobot.RemoteControl.Logic
{
    using System;
    using System.Speech.Recognition;

    public class SpeechRecogniserSayCountry
    {
        private SpeechRecognitionEngine recEngine;
        private readonly GameSayCountry sayCountry;
        public SpeechRecogniserSayCountry (GameSayCountry countryGame)
        {
            this.recEngine = new SpeechRecognitionEngine (new System.Globalization.CultureInfo ("en-US"));

            string[] words = { "Afghanistan", "Albania", "Algeria", "Andorra", "Angola", "Antigua and Barbuda",
                            "Argentina", "Armenia", "Australia", "Austria", "Azerbaijan", "Bahamas","Bahrain",
                            "Bangladesh", "Barbados", "Belarus", "Belgium", "Belize", "Benin", "Bhutan", "Bolivia",
                            "Bosnia and Herzegovina", "Botswana", "Brazil", "Brunei", "Bulgaria", "Burkina Faso",
                            "Burundi", "Cabo Verde", "Cambodia", "Cameroon", "Canada", "Central African Republic",
                            "Chad", "Chile", "China", "Colombia", "Comoros", "Congo", "Costa Rica", "Cote d'Ivoire",
                            "Croatia", "Cuba", "Cyprus", "Czech Republic", "Denmark", "Djibouti", "Dominica",
                            "Dominican Republic", "Ecuador", "Egypt", "El Salvador", "Equatorial Guinea",
                            "Eritrea", "Estonia", "Ethiopia", "Fiji", "Finland", "France", "Gabon", "Gambia", "Georgia",
                            "Germany", "Ghana", "Greece", "Grenada", "Guatemala", "Guinea", "Guinea-Bissau", "Guyana",
                            "Haiti", "Honduras", "Hungary", "Iceland", "India", "Indonesia", "Iran", "Iraq", "Ireland",
                            "Israel", "Italy", "Jamaica", "Japan", "Jordan", "Kazakhstan", "Kenya", "Kiribati", "Kosovo",
                            "Kuwait", "Kyrgyzstan", "Laos", "Latvia", "Lebanon", "Lesotho", "Liberia", "Libya", "Liechtenstein",
                            "Lithuania", "Luxembourg", "Macedonia", "Madagascar", "Malawi", "Malaysia", "Maldives", "Mali",
                            "Malta", "Marshall Islands", "Mauritania", "Mauritius", "Mexico", "Micronesia", "Moldova",
                            "Monaco", "Mongolia", "Montenegro", "Morocco", "Mozambique", "Myanmar", "Namibia", "Nauru",
                            "Nepal", "Netherlands", "New Zealand", "Nicaragua", "Niger", "Nigeria", "North Korea",
                            "Norway", "Oman", "Pakistan", "Palau", "Palestine", "Panama", "Papua New Guinea", "Paraguay",
                            "Peru", "Philippines", "Poland", "Portugal", "Qatar", "Romania", "Russia", "Rwanda",
                            "Saint Kitts and Nevis", "Saint Lucia", "Saint Vincent and the Grenadines", "Samoa",
                            "San Marino", "Sao Tome and Principe", "Saudi Arabia", "Senegal", "Serbia", "Seychelles",
                            "Sierra Leone", "Singapore", "Slovakia", "Slovenia", "Solomon Islands", "Somalia",
                            "South Africa", "South Korea", "South Sudan", "Spain", "Sri Lanka", "Sudan", "Suriname",
                            "Swaziland", "Sweden", "Switzerland", "Syria", "Taiwan", "Tajikistan", "Tanzania",
                            "Thailand", "Timor-Leste", "Togo", "Tonga", "Trinidad and Tobago", "Tunisia", "Turkey",
                            "Turkmenistan", "Tuvalu", "Uganda", "Ukraine", "United Arab Emirates", "United Kingdom",
                            "United States of America", "Uruguay", "Uzbekistan", "Vanuatu", "Vatican", "Venezuela",
                            "Vietnam", "Yemen", "Zambia", "Zimbabwe"};
            this.sayCountry = countryGame;
            Choices commands = new Choices ();
            commands.Add (words);
            GrammarBuilder gBuilder = new GrammarBuilder ();
            gBuilder.Append (commands);
            Grammar grammar = new Grammar (gBuilder);

            recEngine.LoadGrammarAsync (grammar);
            recEngine.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs> (RecEngine_SpeechRecognized);
            recEngine.SetInputToDefaultAudioDevice ();
        }

        private void RecEngine_SpeechRecognized (object sender, SpeechRecognizedEventArgs e)
        {
            try
            {
                string recognisedWord = e.Result.Text;
                float classifierConfidence = e.Result.Confidence;
                if (classifierConfidence < 0.85)
                {
                    return;
                }

                this.sayCountry.SetRecognisedWord (recognisedWord);
            }
            catch (Exception ex)
            {
                Logger.LogMessage ("SpeechRecogniser_SayCountry: StartRecognition():\n" + ex.ToString ());
            }
        }

        public void StartRecognition ()
        {
            try
            {
                recEngine.RecognizeAsync (RecognizeMode.Multiple);
            }
            catch (Exception ex)
            {
                Logger.LogMessage ("SpeechRecogniser_SayCountry: StartRecognition():\n" + ex.ToString ());
            }
        }

        public void StopRecognition ()
        {
            try
            {
                recEngine.RecognizeAsyncStop ();
            }
            catch (Exception ex)
            {
                Logger.LogMessage ("SpeechRecogniser_SayCountry: StartRecognition():\n" + ex.ToString ());
            }
        }
    }
}
