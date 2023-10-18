
import time
import random
import time
import math

from pythonosc import udp_client


client = udp_client.SimpleUDPClient("127.0.0.1", 9001)
i = 0.01
while 1:
    alpha = math.sin(i)*180+180
    beta = math.cos(i)*180+180
    x = math.sin(i)
    y = math.cos(i)
    roll = math.sin(i)
    pitch = math.cos(i)
    yaw = math.tan(i)
    z = random.randrange(-1,1)
    client.send_message("/I1-2/mouse/deg", alpha)
    client.send_message("/I1-2/mouse/degSchieben", beta)
    client.send_message("/I1-2/mouse/rawx", x)
    client.send_message("/I1-2/mouse/rawy", y)
    client.send_message("/I1-2/mouse/rawz", z)
    client.send_message("/I1-2/mouse/rawroll", roll)
    client.send_message("/I1-2/mouse/rawpitch", pitch)
    client.send_message("/I1-2/mouse/rawyaw", yaw)
    i += 0.5
    time.sleep(0.1)
