
import time
import random
import time
import math

from pythonosc import udp_client


client = udp_client.SimpleUDPClient("127.0.0.1", 9006)
client.send_message("/adm/obj/1/azim", 60)
client.send_message("/adm/obj/1/elev", 0)
client.send_message("/adm/obj/1/dist", 0.8)
