using MiloRobot.RemoteControl.Logic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MiloRobot.RemoteControl.UserInterface
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Thread for Say Country game.
        /// </summary>
        private Thread thread_SayCountry_Game;

        /// <summary>
        /// Thread for Guess Animal game.
        /// </summary>
        private Thread thread_GuessAnimal_Game;

        /// <summary>
        /// Thread for presentation method.
        /// </summary>
        private Thread thread_Presentation;

        /// <summary>
        /// Instance of GuessAnimal game.
        /// </summary>
        private GameGuessAnimal guessAnimal;

        /// <summary>
        /// Instance of SayCountry game.
        /// </summary>
        private GameSayCountry sayCountry;

        /// <summary>
        /// SSH client
        /// .
        /// </summary>
        private MiloControl robotClient;

        private string[] countries = { "Afghanistan", "Albania", "Algeria", "Andorra", "Angola", "Antigua and Barbuda",
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

        private readonly string CLOSE_IMAGE = "/./usr/robokind/games/closeImage.sh";

        public MainWindow ()
        {
            InitializeComponent ();

            this.robotClient = MiloControl.GetInstance ();
            this.robotClient.ConnectionLost += this.ConnectionLostEventHandler;

            ConnectionStatusLabel.Content = "Disconnected";
            ConnectionStatusLabel.Foreground = System.Windows.Media.Brushes.Red;

            this.UpdateMiloButtonsStatus (false);
            this.FillComboBox ();

            IPAddressTextBox.Text = "192.168.0.110";
            UsernameTextBox.Text = "variscite";
            PasswordPasswordBox.Password = "password";

            this.guessAnimal = new GameGuessAnimal ();
            this.sayCountry = new GameSayCountry ();

            this.thread_GuessAnimal_Game = new Thread (this.StartGuessAnimalGame);
            this.thread_SayCountry_Game = new Thread (this.StartSayCountryGame);
            this.thread_Presentation = new Thread (this.Presentation);
        }

        #region EmotionEventHandlers
        private void AngryBtn_Click (object sender, RoutedEventArgs e)
        {
            this.robotClient.SendCommand ("anim-play /usr/robokind/etc/gui/anims/AZR25_angry_01.anim.xml");
        }

        private void DenialBtn_Click (object sender, RoutedEventArgs e)
        {
            this.robotClient.SendCommand ("anim-play /usr/robokind/etc/gui/anims/AZR25_denial_01.anim.xml");
        }

        private void EnragedBtn_Click (object sender, RoutedEventArgs e)
        {
            this.robotClient.SendCommand ("anim-play /usr/robokind/etc/gui/anims/AZR25_enraged_01.anim.xml");
        }

        private void FearBtn_Click (object sender, RoutedEventArgs e)
        {
            this.robotClient.SendCommand ("anim-play /usr/robokind/etc/gui/anims/AZR25_fear_01.anim.xml");
        }

        private void HappyBtn_Click (object sender, RoutedEventArgs e)
        {
            this.robotClient.SendCommand ("anim-play /usr/robokind/etc/gui/anims/AZR25_happy_01.anim.xml");
        }

        private void PanicBtn_Click (object sender, RoutedEventArgs e)
        {
            this.robotClient.SendCommand ("anim-play /usr/robokind/etc/gui/anims/AZR25_panic_01.anim.xml");
        }

        private void SadBtn_Click (object sender, RoutedEventArgs e)
        {
            this.robotClient.SendCommand ("anim-play /usr/robokind/etc/gui/anims/AZR25_sad_01.anim.xml");
        }

        private void ScaredBtn_Click (object sender, RoutedEventArgs e)
        {
            this.robotClient.SendCommand ("anim-play /usr/robokind/etc/gui/anims/AZR25_scared_02_4000.anim.xml");
        }

        private void SurpriseBtn_Click (object sender, RoutedEventArgs e)
        {
            this.robotClient.SendCommand ("anim-play /usr/robokind/etc/gui/anims/AZR25_surprise_01.anim.xml");
        }

        private void YawnBtn_Click (object sender, RoutedEventArgs e)
        {
            this.robotClient.SendCommand ("anim-play /usr/robokind/etc/gui/anims/AZR25_yawn_01.anim.xml");
        }

        private void DefaultBtn_Click (object sender, RoutedEventArgs e)
        {
            this.robotClient.SendCommand ("anim-play /usr/robokind/etc/gui/anims/AZR25_defaults.anim.xml");
        }
        #endregion EmotionEventHandlers
        #region Events
        private void ConnectButton_Click (object sender, RoutedEventArgs e)
        {
            if (ConnectButton.Content.Equals ("Connect"))
            {

                string ipAdr = IPAddressTextBox.Text;
                string username = UsernameTextBox.Text;
                string password = PasswordPasswordBox.Password.ToString ();

                if (!string.IsNullOrEmpty (ipAdr) && !string.IsNullOrEmpty (username) && !string.IsNullOrEmpty (password))
                {
                    // Connect
                    var connected = this.robotClient.CreateConnection (ipAdr, username, password);

                    if (!connected)
                    {
                        MessageBox.Show (UserMessages.CONNECTION_ERROR_MESSAGE, UserMessages.CONNECTION_ERROR_HEADER,
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                    if (this.robotClient.IsConnected)
                    {
                        ConnectButton.Content = "Disconnect";
                        this.UpdateMiloButtonsStatus (true);
                        ConnectionStatusLabel.Content = "Connected";
                        ConnectionStatusLabel.Foreground = System.Windows.Media.Brushes.Green;
                        MessageBox.Show (UserMessages.CONNECTION_SUCCESSFUL_MESSAGE + ipAdr, UserMessages.CONNECTION_SUCCESSFUL_HEADER,
                            MessageBoxButton.OK, MessageBoxImage.Information);
                        Logger.LogMessage ("CONNECTION SUCCESSFUL: \nIPAddress:\t" + ipAdr + "\nUsername:\t" + username + "\nPassword:\t" + password);
                    }
                }
                else
                {
                    MessageBox.Show (UserMessages.CONNECTION_EMPTY_VALUES_MESSAGE, UserMessages.CONNECTION_EMPTY_VALUES_HEADER,
                            MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                // If PC is connected to the client (robot), disconnect from it.
                this.robotClient.Disconnect ();
                ConnectButton.Content = "Connect";
                this.UpdateMiloButtonsStatus (false);
                ConnectionStatusLabel.Content = "Disconnected";
                ConnectionStatusLabel.Foreground = System.Windows.Media.Brushes.Red;
                MessageBox.Show (UserMessages.DISCONNECTION_SUCCESSFUL_MESSAGE, UserMessages.DISCONNECTION_SUCCESSFUL_HEADER,
                        MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void SpeakBtn_Click (object sender, RoutedEventArgs e)
        {
            this.robotClient.SendCommand ("speak \"" + SpeakTextBox.Text + "\"");
        }

        private void Movementbtn_Click (object sender, RoutedEventArgs e)
        {
            switch (AlternativeCommandComboBox.Text)
            {
                case "ROM":
                    this.robotClient.SendCommand ("anim-play /usr/robokind/etc/gui/anims/AZR25_ROM_01.anim.xml");
                    break;

                case "ArmSwing":
                    this.robotClient.SendCommand ("anim-play /usr/robokind/etc/gui/anims/AZR25_armSwing_01.anim.xml");
                    break;

                case "Dance":
                    this.robotClient.SendCommand ("anim-play /usr/robokind/etc/gui/anims/AZR25_dance_01.anim.xml");
                    break;

                case "Courage":
                    this.robotClient.SendCommand ("anim-play /usr/robokind/etc/gui/anims/AZR25_courage_01.anim.xml");
                    break;

                case "GrabLeft":
                    this.robotClient.SendCommand ("anim-play /usr/robokind/etc/gui/anims/AZR25_grabLeft_01.anim.xml");
                    break;

                case "GrabRight":
                    this.robotClient.SendCommand ("anim-play /usr/robokind/etc/gui/anims/AZR25_grabRight_01.anim.xml");
                    break;

                case "HandWave":
                    this.robotClient.SendCommand ("anim-play /usr/robokind/etc/gui/anims/AZR25_handWave_01_5000.anim.xml");
                    break;

                case "Haughty":
                    this.robotClient.SendCommand ("anim-play /usr/robokind/etc/gui/anims/AZR25_haughty_01.anim.xml");
                    break;

                case "HeadShake":
                    this.robotClient.SendCommand ("anim-play /usr/robokind/etc/gui/anims/AZR25_headShake_01.anim.xml");
                    break;

                case "Idle":
                    this.robotClient.SendCommand ("anim-play /usr/robokind/etc/gui/anims/AZR25_idle_01.anim.xml");
                    break;

                case "Inebriated":
                    this.robotClient.SendCommand ("anim-play /usr/robokind/etc/gui/anims/AZR25_inebriated_01.anim.xml");
                    break;

                case "LookingAround":
                    this.robotClient.SendCommand ("anim-play /usr/robokind/etc/gui/anims/AZR25_lookingAround_01.anim.xml");
                    break;

                case "LookLeftShoulder":
                    this.robotClient.SendCommand ("anim-play /usr/robokind/etc/gui/anims/AZR25_LookLeftShoulder_01.anim.xml");
                    break;

                case "LookRightShoulder":
                    this.robotClient.SendCommand ("anim-play /usr/robokind/etc/gui/anims/AZR25_LookRightShoulder_01.anim.xml");
                    break;

                case "GoodJob":
                    this.robotClient.SendCommand ("anim-play /usr/robokind/etc/gui/anims/AZR25_M1L2_Good_Job_01.anim.xml");
                    break;

                case "PointLeft":
                    this.robotClient.SendCommand ("anim-play /usr/robokind/etc/gui/anims/AZR25_pointLeft_01.anim.xml");
                    break;

                case "PointRight":
                    this.robotClient.SendCommand ("anim-play /usr/robokind/etc/gui/anims/AZR25_pointRight_01.anim.xml");
                    break;

                case "Shakespeare":
                    this.robotClient.SendCommand ("anim-play /usr/robokind/etc/gui/anims/AZR25_Shakespeare_02.anim.xml");
                    break;

                case "Shrug":
                    this.robotClient.SendCommand ("anim-play /usr/robokind/etc/gui/anims/AZR25_shrug_01.anim.xml");
                    break;

                case "TheMonkeyDance":
                    this.robotClient.SendCommand ("anim-play /usr/robokind/etc/gui/anims/AZR25_TheMonkeyDance_01.anim.xml");
                    break;

                case "VictoryPose":
                    this.robotClient.SendCommand ("anim-play /usr/robokind/etc/gui/anims/AZR25_victoryPose_01.anim.xml");
                    break;

                case "WalkBackward":
                    this.robotClient.SendCommand ("anim-play /usr/robokind/etc/gui/anims/AZR25_walkBackward_7s_01.anim.xml");
                    break;
            }
        }

        private void GuessAnimalBtn_Click (object sender, RoutedEventArgs e)
        {
            try
            {
                if (GuessAnimalBtn.Content.Equals ("Guess Animal"))
                {
                    // AllocConsole();
                    this.thread_GuessAnimal_Game.Start ();
                    GuessAnimalBtn.Content = "Stop";
                    this.UpdateMiloButtonsStatus (false);
                    GuessAnimalBtn.IsEnabled = true;
                    GuessAnimalAnswerBtn.IsEnabled = true;
                }
                else
                {
                    this.thread_GuessAnimal_Game.Abort ();
                    GuessAnimalBtn.Content = "Guess Animal";
                    // FreeConsole();
                    this.guessAnimal.showingImages.Abort ();
                    this.thread_GuessAnimal_Game = new Thread (this.StartGuessAnimalGame);
                    this.guessAnimal.Dispose ();
                    this.robotClient.SendCommand (CLOSE_IMAGE);

                    if (this.robotClient.IsConnected)
                    {
                        this.UpdateMiloButtonsStatus (true);
                    }
                    else
                    {
                        GuessAnimalBtn.IsEnabled = false;
                        GuessAnimalAnswerBtn.IsEnabled = false;
                        ConnectButton.Content = "Connect";
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogMessage ("MainWindow: GuessAnimalBtn_Click():\n" + ex.ToString ());
            }
        }

        private void SayCountryBtn_Click (object sender, RoutedEventArgs e)
        {
            try
            {
                if (SayCountryBtn.Content.Equals ("Say Country"))
                {
                    // AllocConsole();
                    this.thread_SayCountry_Game.Start ();
                    SayCountryBtn.Content = "Stop";
                    this.UpdateMiloButtonsStatus (false);
                    SayCountryBtn.IsEnabled = true;
                    SayCountryAnswerBtn.IsEnabled = true;
                }
                else
                {
                    SayCountryBtn.Content = "Say Country";
                    this.sayCountry.Dispose ();
                    // FreeConsole();

                    if (this.robotClient.IsConnected)
                    {
                        this.UpdateMiloButtonsStatus (true);
                    }
                    else
                    {
                        SayCountryBtn.IsEnabled = false;
                        SayCountryAnswerBtn.IsEnabled = false;
                        ConnectButton.Content = "Connect";
                    }
                    this.thread_SayCountry_Game.Abort ();
                    this.thread_SayCountry_Game = new Thread (this.StartSayCountryGame);
                }
            }
            catch (Exception ex)
            {
                Logger.LogMessage ("MainWindow: SayCountryBtn_Click():\n" + ex.ToString ());
            }
        }

        private void GuessAnimalAnswerBtn_Click (object sender, RoutedEventArgs e)
        {
            if (GuessAnimalBtn.IsEnabled)
            {
                try
                {
                    AnswerGuessAnimal answerGuessAnimal = new AnswerGuessAnimal (this);
                    answerGuessAnimal.ShowDialog ();
                }
                catch (Exception ex)
                {
                    Logger.LogMessage ("MainWindow: GuessAnimalAnswerBtn_Click():\n" + ex.ToString ());
                }
            }
        }

        private void SayCountryAnswerBtn_Click (object sender, RoutedEventArgs e)
        {
            if (SayCountryBtn.IsEnabled)
            {
                try
                {
                    AnswerSayCountry answerSayCountry = new AnswerSayCountry (this);
                    answerSayCountry.ShowDialog ();
                }
                catch (Exception ex)
                {
                    Logger.LogMessage ("MainWindow: SayCountryAnswerBtn_Click():\n" + ex.ToString ());
                }
            }
        }

        private void MusicPlayerBtn_Click (object sender, RoutedEventArgs e)
        {
            try
            {
                Mp3Player mp3Player = new Mp3Player ();
                mp3Player.ShowDialog ();
            }
            catch (Exception ex)
            {
                Logger.LogMessage ("MainWindow: MusicPlayerBtn_Click():\n" + ex.ToString ());
            }
        }

        private void PresentationCycleBtn_Click (object sender, RoutedEventArgs e)
        {
            try
            {
                if (PresentationCycleBtn.Content.Equals ("Presentation"))
                {
                    this.thread_Presentation.Start ();
                    PresentationCycleBtn.Content = "Stop";
                    this.UpdateMiloButtonsStatus (false);
                    PresentationCycleBtn.IsEnabled = true;
                }
                else
                {
                    PresentationCycleBtn.Content = "Presentation";

                    if (this.robotClient.IsConnected)
                    {
                        this.UpdateMiloButtonsStatus (true);
                    }
                    else
                    {
                        PresentationCycleBtn.IsEnabled = false;
                        ConnectButton.Content = "Connect";
                    }
                    this.thread_Presentation.Abort ();
                    this.thread_Presentation = new Thread (this.Presentation);
                }
            }
            catch (Exception ex)
            {
                Logger.LogMessage ("MainWindow: PresentationCycleBtn_Click():\n" + ex.ToString ());
            }
        }

        private void ShutdownBtn_Click (object sender, MouseButtonEventArgs e)
        {
            //try
            //{
            //    if (this.client.IsConnected)
            //    {
            //        this.robotClient.SendCommand("sudo poweroff");
            //    }
            //}
            //catch (Exception ex)
            //{
            //    LogMessage("SENDING SHUTDOWN COMMAND ERROR: \n" + ex.ToString());
            //}
        }

        private void RebootBtn_Click (object sender, MouseButtonEventArgs e)
        {
            //try
            //{
            //    if (this.client.IsConnected)
            //    {
            //        this.robotClient.SendCommand("sudo reboot");
            //    }
            //}
            //catch (Exception ex)
            //{
            //    LogMessage("SENDING REBOOT COMMAND ERROR: \n" + ex.ToString());
            //}
        }

        private void CloseBtn_Click (object sender, RoutedEventArgs e)
        {
            try
            {
                robotClient.SendCommand (CLOSE_IMAGE);
                this.robotClient.Disconnect ();
                // FreeConsole();
            }
            catch
            {
                // Nothing to catch here.
            }

            finally
            {
                this.Close ();
                Environment.Exit (0);
            }
        }

        private void MainWindow_CLosing (object sender, CancelEventArgs e)
        {
            try
            {
                robotClient.SendCommand (CLOSE_IMAGE);
                this.robotClient.Disconnect ();
                // FreeConsole();
            }
            catch
            {
                // Nothing to catch here.
            }
        }

        private void ConnectionLostEventHandler(object sender, EventArgs e)
        {
            if (this.robotClient == null || !this.robotClient.IsConnected)
            {
                this.UpdateMiloButtonsStatus (false);
                MessageBox.Show ("Connection Lost.");
            }
        }

        #endregion Events

        #region Methods

        /// <summary>
        /// Converts Bitmap object to BitmapSource
        /// </summary>
        /// <param name="source">Bitmap object to be converted</param>
        /// <returns>Output object of type BitmapSource</returns>
        public static BitmapSource FromBitmapToBitmapSource (Bitmap source)
        {
            return System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap (
                          source.GetHbitmap (),
                          IntPtr.Zero,
                          Int32Rect.Empty,
                          BitmapSizeOptions.FromEmptyOptions ());
        }

        /// <summary>
        /// Updates status of buttons which send commands to Milo
        /// </summary>
        /// <param name="enabled">The boolean if buttons are enabled or not.</param>
        private void UpdateMiloButtonsStatus (bool enabled)
        {
            AngryBtn.IsEnabled = enabled;
            DenialBtn.IsEnabled = enabled;
            EnragedBtn.IsEnabled = enabled;
            FearBtn.IsEnabled = enabled;
            HappyBtn.IsEnabled = enabled;
            PanicBtn.IsEnabled = enabled;
            SadBtn.IsEnabled = enabled;
            ScaredBtn.IsEnabled = enabled;
            SurpriseBtn.IsEnabled = enabled;
            YawnBtn.IsEnabled = enabled;
            DefaultBtn.IsEnabled = enabled;
            SpeakBtn.IsEnabled = enabled;
            Movementbtn.IsEnabled = enabled;
            GuessAnimalBtn.IsEnabled = enabled;
            GuessAnimalAnswerBtn.IsEnabled = enabled;
            SayCountryBtn.IsEnabled = enabled;
            SayCountryAnswerBtn.IsEnabled = enabled;
            MusicPlayerBtn.IsEnabled = enabled;
            PresentationCycleBtn.IsEnabled = enabled;
        }

        /// <summary>
        /// Fills the combobox.
        /// </summary>
        private void FillComboBox ()
        {
            AlternativeCommandComboBox.Items.Add ("ROM");
            AlternativeCommandComboBox.Items.Add ("ArmSwing");
            AlternativeCommandComboBox.Items.Add ("Dance");
            AlternativeCommandComboBox.Items.Add ("Courage");
            AlternativeCommandComboBox.Items.Add ("GrabLeft");
            AlternativeCommandComboBox.Items.Add ("GrabRight");
            AlternativeCommandComboBox.Items.Add ("HandWave");
            AlternativeCommandComboBox.Items.Add ("Haughty");
            AlternativeCommandComboBox.Items.Add ("HeadShake");
            AlternativeCommandComboBox.Items.Add ("Idle");
            AlternativeCommandComboBox.Items.Add ("Inebriated");
            AlternativeCommandComboBox.Items.Add ("LookingAround");
            AlternativeCommandComboBox.Items.Add ("LookLeftShoulder");
            AlternativeCommandComboBox.Items.Add ("LookRightShoulder");
            AlternativeCommandComboBox.Items.Add ("GoodJob");
            AlternativeCommandComboBox.Items.Add ("PointLeft");
            AlternativeCommandComboBox.Items.Add ("PointRight");
            AlternativeCommandComboBox.Items.Add ("Shakespeare");
            AlternativeCommandComboBox.Items.Add ("Shrug");
            AlternativeCommandComboBox.Items.Add ("TheMonkeyDance");
            AlternativeCommandComboBox.Items.Add ("VictoryPose");
            AlternativeCommandComboBox.Items.Add ("WalkBackward");
        }

        /// <summary>
        /// Starts the Guess animal game.
        /// </summary>
        private void StartGuessAnimalGame ()
        {
            this.guessAnimal.Start ();
        }

        /// <summary>
        /// Starts the Say country game.
        /// </summary>
        private void StartSayCountryGame ()
        {
            this.sayCountry.Start ();
        }

        /// <summary>
        /// Sends the answer to the Guess animal game from UI.
        /// </summary>
        /// <param name="answer">The answer.</param>
        public void SendAnswerToGuessAnimal (string answer)
        {
            this.guessAnimal.SetRecognisedWord (answer.ToLower ());
        }

        /// <summary>
        /// Sends the answer to the Say country game from UI.
        /// </summary>
        /// <param name="answer">The answer.</param>
        public void SendAnswerToSayCountry (string answer)
        {
            answer = answer.ToUpper ();
            bool contain = false;
            for (int i = 0; i < this.countries.Length; i++)
            {
                if (this.countries[i].ToUpper ().Equals (answer))
                {
                    contain = true;
                    break;
                }
            }

            if (!contain)
            {
                answer = "orphan";
            }

            this.sayCountry.SetRecognisedWord (answer);
        }

        /// <summary>
        /// Milo is showing his capabilities.
        /// This method was used on Educo 2017.
        /// </summary>
        private void Presentation ()
        {
            try
            {
                while (true)
                {
                    this.robotClient.SendCommand ("speak \"" + "Hi. I am Milo and I live at Technical University." + "\"");
                    Thread.Sleep (7000);
                    this.robotClient.SendCommand ("anim-play /usr/robokind/etc/gui/anims/AZR25_angry_01.anim.xml");
                    Thread.Sleep (7000);
                    this.robotClient.SendCommand ("anim-play /usr/robokind/etc/gui/anims/AZR25_denial_01.anim.xml");
                    Thread.Sleep (7000);
                    this.robotClient.SendCommand ("anim-play /usr/robokind/etc/gui/anims/AZR25_enraged_01.anim.xml");
                    Thread.Sleep (7000);
                    this.robotClient.SendCommand ("anim-play /usr/robokind/etc/gui/anims/AZR25_courage_01.anim.xml");
                    Thread.Sleep (7000);
                    this.robotClient.SendCommand ("anim-play /usr/robokind/etc/gui/anims/AZR25_fear_01.anim.xml");
                    Thread.Sleep (7000);
                    this.robotClient.SendCommand ("anim-play /usr/robokind/etc/gui/anims/AZR25_inebriated_01.anim.xml");
                    Thread.Sleep (7000);
                    this.robotClient.SendCommand ("anim-play /usr/robokind/etc/gui/anims/AZR25_happy_01.anim.xml");
                    Thread.Sleep (7000);
                    this.robotClient.SendCommand ("anim-play /usr/robokind/etc/gui/anims/AZR25_panic_01.anim.xml");
                    Thread.Sleep (7000);
                    this.robotClient.SendCommand ("anim-play /usr/robokind/etc/gui/anims/AZR25_sad_01.anim.xml");
                    Thread.Sleep (7000);
                    this.robotClient.SendCommand ("anim-play /usr/robokind/etc/gui/anims/AZR25_scared_02_4000.anim.xml");
                    Thread.Sleep (7000);
                    this.robotClient.SendCommand ("anim-play /usr/robokind/etc/gui/anims/AZR25_victoryPose_01.anim.xml");
                    Thread.Sleep (7000);
                    this.robotClient.SendCommand ("anim-play /usr/robokind/etc/gui/anims/AZR25_surprise_01.anim.xml");
                    Thread.Sleep (7000);
                    this.robotClient.SendCommand ("anim-play /usr/robokind/etc/gui/anims/AZR25_yawn_01.anim.xml");
                    Thread.Sleep (7000);
                    this.robotClient.SendCommand ("anim-play /usr/robokind/etc/gui/anims/AZR25_defaults.anim.xml");
                    Thread.Sleep (7000);
                    this.robotClient.SendCommand ("anim-play /usr/robokind/etc/gui/anims/AZR25_TheMonkeyDance_01.anim.xml");
                    Thread.Sleep (18000);
                }
            }
            catch (Exception ex)
            {
                Logger.LogMessage ("MainWindow: Presentation():\n" + ex.ToString ());
            }

        }

        /// <summary>
        /// Opens console.
        /// </summary>
        [DllImport ("Kernel32")]
        public static extern void AllocConsole ();

        /// <summary>
        /// Closes console.
        /// </summary>
        [DllImport ("Kernel32")]
        public static extern void FreeConsole ();

        #endregion Methods
    }
}
