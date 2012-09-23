using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace CCTVClient.Alerts
{
    public class SubscriptionManager
    {
        public Dictionary<String, List<Subscription>> Subscriptions;
        private String SaveFileLocation;

        public SubscriptionManager(String locale)
        {
            SaveFileLocation = locale;
            Subscriptions = new Dictionary<String, List<Subscription>>();
        }

        public void AddSubscription(Subscription newSub)
        {
            if(!Subscriptions.ContainsKey(newSub.RawParamName)){
                Subscriptions.Add(newSub.RawParamName,new List<Subscription>());
            }
            Subscriptions[newSub.RawParamName].Add(newSub);
        }

        public void Read()
        {
            Stream stream = File.Open(SaveFileLocation,FileMode.OpenOrCreate);
            BinaryFormatter binaryWrite = new BinaryFormatter();
            List<Subscription> tempSubList = (List<Subscription>)binaryWrite.Deserialize(stream);
            foreach (Subscription newSub in tempSubList)
            {
                AddSubscription(newSub);
            }
            stream.Close();
        }

        public void Save()
        {
            Stream stream = File.Open(SaveFileLocation, FileMode.Create);
            BinaryFormatter binaryWrite = new BinaryFormatter();
            List<Subscription> tempSubList = new List<Subscription>(); 
            foreach (List<Subscription> sub in Subscriptions.Values)
            {
                tempSubList.AddRange(sub);
            }
            binaryWrite.Serialize(stream,tempSubList);
            stream.Close();
        }
    }
}
