using static WholesomeDungeons.Connection.Enums;

namespace WholesomeDungeons.Connection
{
    internal class Request
    {
        public Request(RequestType requestType)
        {
            RequestType = requestType;
        }
        
        public RequestType RequestType;
    }
}
