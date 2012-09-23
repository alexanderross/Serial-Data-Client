using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCTVClient.Alerts
{
    [Serializable]
    public class Subscription
    {
        int timeToReset, currentTime;// How long before the alert will reset? then Our counter for that metric.

        public String RawParamName;
        public int alertMin, alertMax;
        public List<Subscriber> Subscribers;

        bool triggered = false;

        public Subscription()
        {
            Subscribers = new List<Subscriber>();
        }
        public Subscription(String rawName, int timeToResetIn, List<Subscriber> subs = null)
        {
            Subscribers = (subs == null) ? new List<Subscriber>() : subs;
            this.RawParamName = rawName;
            this.timeToReset = timeToResetIn;
            this.currentTime = 0;
        }

        public bool Check(int value)
        {
            if (triggered)
            {
                currentTime++;
                if (currentTime > timeToReset)
                {
                    currentTime = 0;
                    triggered = false;
                }
            }
            else if (value >= this.alertMin && value <= alertMax)
            {
                triggered = true;
                return true;
            }
            
            return false;
        }
    }
}
