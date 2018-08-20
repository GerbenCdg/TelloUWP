using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelloLib;
using TelloLib.CustomEventArgs;

namespace TelloLib
{
    public class Tello
    {
        public CommandManager CommandManager { get => FlightManager.CommandManager; }
        public CommunicationManager CommunicationManager { get => FlightManager.CommunicationManager; }
        public FlightManager FlightManager { get; }
        // public FlyState FlyState { get; }

        public Tello()
        {
            //FlyState = new FlyState();
            //CommunicationManager = new CommunicationManager();
            //CommandManager = new CommandManager(CommunicationManager);
            FlightManager = new FlightManager();

            CommunicationManager.ConnectionStatusHandler += ConnectionStatusHandler;
            CommunicationManager.VideoStreamHandler += StreamHandler;
            CommunicationManager.DroneInfoHandler += DroneInfoHandler;
        }

        private void DroneInfoHandler(object sender, CustomEventArgs.DroneInfoEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void StreamHandler(object sender, CustomEventArgs.VideoStreamEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void ConnectionStatusHandler(object sender, ConnectionStatusEventArgs e)
        {
            //switch (e.ConnectionStatus)
            //{             
            //    case ConnectionStatus.Disconnected:
            //        break;
            //    case ConnectionStatus.Connecting:                    
            //        break;
            //    case ConnectionStatus.Connected:
            //        break;
            //    default:
            //        break;
            //}
        }


    }
}
