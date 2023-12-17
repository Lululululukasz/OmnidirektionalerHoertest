
import time
import random
import time
import math

from pythonosc import udp_client


client = udp_client.SimpleUDPClient("127.0.0.1", 9000)
client.send_message("/I1-2/mouse/click", 1)
