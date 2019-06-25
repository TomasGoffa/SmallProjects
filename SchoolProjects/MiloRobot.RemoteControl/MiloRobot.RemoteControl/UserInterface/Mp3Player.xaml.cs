/// <summary>
/// This file is part of project for remote
/// control of Robokind’s Milo Robot
/// 
/// Author:     Tomas Goffa
/// </summary>

namespace MiloRobot.RemoteControl.UserInterface
{
    using System;
    using System.Threading;
    using System.Windows;
    using System.Windows.Input;

    using MiloRobot.RemoteControl.Logic;

    /// <summary>
    /// Interaction logic for Mp3Player.xaml
    /// </summary>
    public partial class Mp3Player : Window
    {
        /// <summary>
        /// Instance of MainWindow
        /// </summary>
        private readonly MiloControl robotClient;

        /// <summary>
        /// Thread for playing music
        /// </summary>
        private Thread musicThread;

        /// <summary>
        /// Stores the information what song was selected.
        /// </summary>
        private Songs choosedSong;

        private readonly string PATH_TO_SONGS = "mpg123 /usr/robokind/games/songs/";

        private readonly string BINGO_DOG_SONG = "Bingo_Dog_Song.mp3";
        private readonly string FIVE_LITTLE_DUCKS = "Five_Little_Ducks.mp3";
        private readonly string FIVE_LITTLE_MONKEYS = "Five_Little_Monkeys.mp3";
        private readonly string ITSY_BITSY_SPIDER = "Itsy_Bitsy_Spider.mp3";
        private readonly string LET_IT_GO = "Let_it_go.mp3";
        private readonly string LITTLE_SNOWFLAKE = "Little_Snowflake.mp3";
        private readonly string OLD_MACDONALD_HAD_A_FARM = "Old_MacDonald_Had_a_Farm.mp3";
        private readonly string TWINKLE_TWINKLE_LITTLE_STAR = "Twinkle_Twinkle_Little_Star.mp3";

        public Mp3Player ()
        {
            InitializeComponent ();

            this.robotClient = MiloControl.GetInstance ();
            TwinkleLittleStar.IsChecked = true;
            this.musicThread = new Thread (this.PlaySong);
        }

        private void WindowMouseLeftButtonDown (object sender, MouseButtonEventArgs e)
        {
            DragMove ();
        }

        private void PlaySongBtn_Click (object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.robotClient.IsConnected)
                {
                    if (PlaySongBtn.Content.Equals ("Play song"))
                    {
                        if (BingoDogSong.IsChecked.Value)
                        {
                            this.choosedSong = Songs.BingoDogSong;
                        }
                        else if (FiveLittleDucks.IsChecked.Value)
                        {
                            this.choosedSong = Songs.FiveLittleDucks;
                        }
                        else if (FiveLittleMonkeys.IsChecked.Value)
                        {
                            this.choosedSong = Songs.FiveLittleMonkeys;
                        }
                        else if (ItsyBitsySpider.IsChecked.Value)
                        {
                            this.choosedSong = Songs.ItsyBitsySpider;
                        }
                        else if (LittleSnowflake.IsChecked.Value)
                        {
                            this.choosedSong = Songs.LittleSnowflake;
                        }
                        else if (FarmerDonald.IsChecked.Value)
                        {
                            this.choosedSong = Songs.FarmerDonald;
                        }
                        else if (TwinkleLittleStar.IsChecked.Value)
                        {
                            this.choosedSong = Songs.TwinkleLittleStar;
                        }
                        else if (Surprise.IsChecked.Value)
                        {
                            this.choosedSong = Songs.Surprise;
                        }

                        this.musicThread.Start ();
                        PlaySongBtn.Content = "Stop";
                        CloseBtn.IsEnabled = false;
                    }
                    else
                    {
                        PlaySongBtn.Content = "Play song";
                        CloseBtn.IsEnabled = true;
                        this.musicThread.Abort ();
                        this.musicThread = new Thread (this.PlaySong);

                        this.robotClient.SendCommand ("sudo pkill mpg123");
                        this.robotClient.SendCommand ("anim-stop /usr/robokind/etc/gui/anims/AZR25_happy_01.anim.xml");
                        this.robotClient.SendCommand ("anim-play /usr/robokind/etc/gui/anims/AZR25_defaults.anim.xml");
                    }
                }
                else
                {
                    MessageBox.Show (UserMessages.NO_CONNECTION_MESSAGE, UserMessages.NO_CONNECTION_HEADER,
                            MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                Logger.LogMessage ("Mp3Player: \n" + ex.ToString ());
            }
        }

        private void CloseBtn_Click (object sender, RoutedEventArgs e)
        {
            try
            {
                this.Close ();
            }
            catch (Exception ex)
            {
                Logger.LogMessage ("Mp3Player: \n" + ex.ToString ());
            }
        }

        /// <summary>
        /// Plays song on Milo.
        /// This method runs in separate thread.
        /// </summary>
        private void PlaySong ()
        {
            this.robotClient.SendCommand ("anim-loop /usr/robokind/etc/gui/anims/AZR25_happy_01.anim.xml");
            this.robotClient.SendCommand ("speak \"" + "Come and sing with me" + "\"");

            switch (this.choosedSong)
            {
                case Songs.BingoDogSong:
                    this.robotClient.SendCommand (PATH_TO_SONGS + BINGO_DOG_SONG);
                    break;
                case Songs.FiveLittleDucks:
                    this.robotClient.SendCommand (PATH_TO_SONGS + FIVE_LITTLE_DUCKS);
                    break;
                case Songs.FiveLittleMonkeys:
                    this.robotClient.SendCommand (PATH_TO_SONGS + FIVE_LITTLE_MONKEYS);
                    break;
                case Songs.ItsyBitsySpider:
                    this.robotClient.SendCommand (PATH_TO_SONGS + ITSY_BITSY_SPIDER);
                    break;
                case Songs.LittleSnowflake:
                    this.robotClient.SendCommand (PATH_TO_SONGS + LITTLE_SNOWFLAKE);
                    break;
                case Songs.FarmerDonald:
                    this.robotClient.SendCommand (PATH_TO_SONGS + OLD_MACDONALD_HAD_A_FARM);
                    break;
                case Songs.TwinkleLittleStar:
                    this.robotClient.SendCommand (PATH_TO_SONGS + TWINKLE_TWINKLE_LITTLE_STAR);
                    break;
                case Songs.Surprise:
                    this.robotClient.SendCommand (PATH_TO_SONGS + LET_IT_GO);
                    break;
            }

            this.robotClient.SendCommand ("anim-stop /usr/robokind/etc/gui/anims/AZR25_happy_01.anim.xml");
            this.robotClient.SendCommand ("anim-play /usr/robokind/etc/gui/anims/AZR25_defaults.anim.xml");
        }
    }

    /// <summary>
    /// Enum for chosing the song.
    /// </summary>
    public enum Songs
    {
        /// <summary>
        /// Bingo dog song
        /// </summary>
        BingoDogSong,

        /// <summary>
        /// Five Little Ducks
        /// </summary>
        FiveLittleDucks,

        /// <summary>
        /// Five little monkeys
        /// </summary>
        FiveLittleMonkeys,

        /// <summary>
        /// Itsy bitsy spider
        /// </summary>
        ItsyBitsySpider,

        /// <summary>
        /// Little snowflake
        /// </summary>
        LittleSnowflake,

        /// <summary>
        /// Old MacDonald had a farm
        /// </summary>
        FarmerDonald,

        /// <summary>
        /// Twinkle twinkle little star
        /// </summary>
        TwinkleLittleStar,

        /// <summary>
        /// Surprise song
        /// </summary>
        Surprise
    }
}
