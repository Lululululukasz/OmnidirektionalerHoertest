using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Rug.Osc;

namespace VerarbeitungTest
{
    internal class SoundDomeSender
    {
        private OscSender sender;
        private IPAddress ip;
        private int port;
        public SoundDomeSender(string ip,int port) 
        { 
            this.ip = IPAddress.Parse(ip);
            this.port = port;
            sender = new OscSender(this.ip, this.port);
            sender.Connect();
        }

        public void sendeDeg(int ObjectID,float Azimut,float Elevation,float Distance) //Sendet an den Sounddome Deg daten im passenden format
        {
            Console.WriteLine("SENDEOSC Daten: "+Azimut+" "+Elevation);
            sender.Send(new OscMessage("/adm/obj/"+ ObjectID +"/azim", Azimut));
            sender.Send(new OscMessage("/adm/obj/" + ObjectID + "/elev", Elevation));
            sender.Send(new OscMessage("/adm/obj/" + ObjectID + "/dist", Distance));
        }
        public void sendePos(int ObjectID, float x,float y,float z) //Sendet kordinaten im passenden format
        {
            Console.WriteLine("SENDEOSC Daten: " + x + " " + y);
            sender.Send(new OscMessage("/adm/obj/" + ObjectID + "/x", x));
            sender.Send(new OscMessage("/adm/obj/" + ObjectID + "/y", y));
            sender.Send(new OscMessage("/adm/obj/" + ObjectID + "/z", z));
        }
        public void reconnect() //falls aus irgentwelchen gründen die verbindung sich aufhängt
        {
            sender.Close();
            sender.Connect();
        }
        public void disconnect() //sollte vielleicht mal von nutzen sein
        {
            sender.Close();
        }
    }
}
