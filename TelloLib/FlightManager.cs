using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelloLib
{
    public class FlightManager
    {
        private FlyState FlyState;
        internal CommunicationManager CommunicationManager { get; }
        internal CommandManager CommandManager { get; }

        public FlightManager()
        {
            FlyState = new FlyState();
            CommunicationManager = new CommunicationManager();
            CommandManager = new CommandManager(FlyState, CommunicationManager);
        }

        public void Start()
        {
            CommunicationManager.ConnectAndStartListening();
        }

        public void Stop()
        {
            //TODO isFlying must be updated by CommandManager !!
            if (FlyState.IsFlying)
            {
                CommandManager.Land();
            }
            CommunicationManager.Disconnect();
        }
    }
}
