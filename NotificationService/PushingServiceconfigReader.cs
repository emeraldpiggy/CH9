using System;
using System.Xml.Linq;

namespace TowersPerrin.RiskAgility.Framework.PushingServices
{
    public static class PushingServiceconfigReader
    {
        public static string GetPushingServiceUrl(string pushingServiceConfig)
        {
            var xDoc = XDocument.Parse(pushingServiceConfig);
            if (xDoc.Root == null)
            {
                throw new InvalidOperationException("Failed to find pushing service's configurations.");
            }

            var address = xDoc.Root.Attribute("Address").Value;
            if (string.IsNullOrEmpty(address))
            {
                throw new InvalidOperationException("Failed to find pushing service's address.");
            }

            return address;
        }
    }
}