/// <summary>
/// This file is part of project for remote
/// control of Robokind’s Milo Robot
/// 
/// Author:     Tomas Goffa
/// </summary>

namespace MiloRobot.RemoteControl.Logic
{
    using System;
    using Renci.SshNet;

    public class MiloControl
    {
        private static MiloControl control;
        private SshClient client;
        public event EventHandler ConnectionLost;

        private MiloControl()
        {
        }

        /// <summary>
        /// Chcecks if system is connected to Milo robot.
        /// </summary>
        /// <returns></returns>
        public bool IsConnected
        {
            get
            {
                if (this.client == null)
                {
                    return false;
                }

                return this.client.IsConnected;
            }
        }

        /// <summary>
        /// Gets the instance of MiloControl class.
        /// </summary>
        /// <returns></returns>
        public static MiloControl GetInstance()
        {
            if (control == null)
            {
                control = new MiloControl ();
            }

            return control;
        }

        /// <summary>
        /// Creates a connection with Milo robot
        /// </summary>
        /// <param name="ipAddress">The IP address/</param>
        /// <param name="username">The Username.</param>
        /// <param name="password">The Password.</param>
        /// <returns>TRUE if connection was successful, FALSE otherwise</returns>
        public bool CreateConnection(string ipAddress, string username, string password)
        {
            try
            {
                this.client = new SshClient (ipAddress, username, password);
                this.client.Connect ();
                this.client.ErrorOccurred += this.ErrorOccurredEventHandler;
                return this.client.IsConnected;
            }
            catch (Exception ex)
            {
                Logger.LogMessage (ex.ToString ());
                return false;
            }
        }

        /// <summary>
        /// Disconnects from Milo robot
        /// </summary>
        public void Disconnect()
        {
            try
            {
                this.client.Disconnect ();
                this.client.ErrorOccurred -= this.ErrorOccurredEventHandler;
                this.client.Dispose ();
            }
            catch (Exception ex)
            {
                Logger.LogMessage (ex.ToString ());
            }
        }

        /// <summary>
        /// Send command to the Milo robot
        /// </summary>
        /// <param name="command"></param>
        public void SendCommand(string command)
        {
            if (!this.IsConnected)
            {
                this.ConnectionLost?.Invoke (this, EventArgs.Empty);
                return;
            }

            try
            {
                this.client.RunCommand (command);
            }
            catch (Exception ex)
            {
                Logger.LogMessage ("RUN COMMAND ERROR: \n" + ex.ToString ());
            }
        }

        private void ErrorOccurredEventHandler(object sender, EventArgs args)
        {
            this.ConnectionLost?.Invoke (this, EventArgs.Empty);
        }
    }
}
