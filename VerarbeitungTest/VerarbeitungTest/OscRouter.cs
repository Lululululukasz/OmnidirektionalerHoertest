using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace VerarbeitungTest
{
    

    internal class OscRouter
    {
        public enum SubscriberType
        {
            System,
            Test,
            Question
        }

        private List<Action<string>> systemSubs;
        private List<Action<string>> testSubs;
        private List<Action<string>> questionSubs;
        private OscReceiver receiver;

        public OscRouter() 
        {
            systemSubs = new List<Action<string>>();
            testSubs = new List<Action<string>>();
            questionSubs = new List<Action<string>>();
            receiver = new OscReceiver(Route,9000);
        }
        public OscRouter(OscReceiver _receiver) //constructor if receiver is already created
        {
            systemSubs = new List<Action<string>>();
            testSubs = new List<Action<string>>();
            questionSubs = new List<Action<string>>();
            receiver = _receiver;
        }
        /// <summary>
        /// Adds an Subscriber to the subscriber list.
        /// _callback -> Function of the Target Subscriber f(string data)
        /// type -> Enum of the SubscriberType is used for sorting Packages
        /// </summary>
        public void AddReceiver(Action<string> _callback, SubscriberType type)
        {
            switch (type)//Add the object to the assigned list
            {
                case SubscriberType.System:
                    systemSubs.Add(_callback);
                    break;
                case SubscriberType.Test:
                    testSubs.Add(_callback);
                    break;
                case SubscriberType.Question:
                    questionSubs.Add(_callback);
                    break;
            }
        }
        /// <summary>
        /// Callback for NetworkThread. Relays Incomming Data to the Predefined Function 
        /// </summary>
        public void Route(string data)
        {
            string adress = data.Split(",")[0];
            string value = data.Split(",")[1];

            if(adress.Split("/")[1].CompareTo("I1-2") == 0) //own group data
            {
                switch (adress.Split("/")[3])
                {
                    case ("click"):
                        foreach (Action<string> a in systemSubs)
                        {
                            string oscdata = "click:"+ value.Trim();
                            a(oscdata);
                        }
                        break;
                    case ("alpha"):
                        foreach(Action<string> a in testSubs)
                        {
                            string oscdata = "alpha:" + value.Trim();
                            a(oscdata);
                        }
                        break;
                    case ("beta"):
                        foreach (Action<string> a in testSubs)
                        {
                            string oscdata = "beta:" + value.Trim();
                            a(oscdata);
                        }
                        break;
                    default:
                        Console.WriteLine("Unkown Command -> " + adress.Split("/")[3]);
                        break;
                }
            }
            else //data from other groups
            {
                foreach (Action<string> a in questionSubs)
                {
                    string oscdata = adress.Split("/")[3].Trim() + ":" + value.Trim();
                    a(oscdata);
                }
            }


        }
        /// <summary>
        /// Manually Close the NetworkSocket
        /// </summary>
        public void Close()
        {
            receiver.Close();
        }
    }
}
