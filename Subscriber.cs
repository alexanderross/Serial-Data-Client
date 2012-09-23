using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCTVClient.Alerts
{
    [Serializable]
    public class Subscriber
    {
        public String Name, ContactNumber, ProviderGateway, email;

        public Subscriber(){
        }

        public Subscriber(String nameIn, String contactNumIn, String providerIn)
        {
            Name = nameIn;
            ContactNumber = contactNumIn;
            ProviderGateway = providerIn;
        }

        public Subscriber(String nameIn,String contactNumIn,String providerIn,String mailIn){
            Name = nameIn;
            ContactNumber = contactNumIn;
            ProviderGateway = providerIn;
        }
    }
}
