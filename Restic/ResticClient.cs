namespace Restic
{
    public class ResticClient
    {
        public ResticClient() {
            Communicator = new ResticCommunicator(this);
        }
        public ResticCommunicator Communicator { get; set; }
        
        public async Task<string> GetVersion()
        {
            return await Communicator.ExecuteAsync<string>(new Commands.GetVersion());
        }
    }
}
