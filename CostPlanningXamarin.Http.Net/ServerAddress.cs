using System.Text;

namespace CostPlanningXamarin.Http.Net
{
    public class ServerAddress
    {
        public ServerAddress(string address, ushort port, bool isHttpSecure)
        {
            Address = address;
            Port = port;
            IsHttpSecure = isHttpSecure;
        }

        public bool IsHttpSecure { get; }
        public string Address { get; }
        public ushort Port { get; }
        public string Root { get; set; } = "api";

        public virtual string Format(string domain = null)
        {
            StringBuilder sb = new StringBuilder();
            if (IsHttpSecure)
            {
                sb.Append(@"https://");
            }
            else
            {
                sb.Append(@"http://");
            }
            sb.Append(Address);
            if (Port > 0)
            {
                sb.Append(":").Append(Port);
            }
            sb.Append(@"/");
            if (!string.IsNullOrEmpty(Root))
            {
                sb.Append(Root).Append(@"/");
            }
            if (!string.IsNullOrEmpty(domain))
            {
                sb.Append(domain).Append(@"/");
            }

            return sb.ToString();
        }
    }
}
