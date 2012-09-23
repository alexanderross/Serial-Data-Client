using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using CCTVClient.Data;
using CCTVClient.Alerts;

namespace CCTVClient
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainPage());
            /*SMS Test
            SMSAlert mail = new SMSAlert("mail.oneseventyfour.com",26,"mailer+oneseventyfour.com","password",false,"smartCCTV");
            mail.AddSubscriber("2069486945@vtext.com");
            mail.SendMessageToSubscribed("TEST!");
             
            Subscriber person = new Subscriber("Alexander", "2069486945", "vtext.com");
            Subscriber person1 = new Subscriber("Jeezy", "2069486922", "vtext.com");
            Subscription subscript = new Subscription("GARAGE", 100);
            Subscription subscript2 = new Subscription("MAILBOX", 100);
            subscript.Subscribers.Add(person);
            subscript.Subscribers.Add(person1);
            subscript2.Subscribers.Add(person);
            subscript2.Subscribers.Add(person1);
            SubscriptionManager subMan = new SubscriptionManager("SUBDATA.bin");
            subMan.AddSubscription(subscript);
            subMan.AddSubscription(subscript2);
            subMan.Save();

            SubscriptionManager subRet = new SubscriptionManager("SUBDATA.bin");
            subRet.Read();
            Console.WriteLine("Done Testing");
            */
        }
    }
}
