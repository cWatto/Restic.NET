namespace Restic
{
    public class ResticClient
    {
        public ResticClient() {
            Communicator = new ResticCommunicator(this);
        }
        public ResticCommunicator Communicator { get; set; }
        
        public string GetVersion()
        {
            return Communicator.Execute<string>(new Commands.GetVersion(), null);
        }
    }
}
