import pyspacemouse
import time
import math
from pythonosc import udp_client


#run programm

click1 = 0
click2 = 0
lastT = 0
client = udp_client.SimpleUDPClient("127.0.0.1", 9000)
success = pyspacemouse.open()
while True:
    
    state = pyspacemouse.read()
    if state.buttons[0] == 1 and click1 == 0:
        client.send_message("/I1-2/mouse/click", 1)
        click1 = 1
        print("Clicked button 1")
    elif state.buttons[0] == 0:
        click1 = 0
    if state.buttons[1] == 1 and click2 == 0:
        client = udp_client.SimpleUDPClient("127.0.0.1", 9000)
        client.send_message("/I1-2/mouse/click", 2)
        click2 = 1
        print("Clicked button 2")
    elif state.buttons[1] == 0:
        click2 = 0   
    if state.t - lastT > 0.1:
        lastT = state.t
        if state.pitch < -0.3 or state.pitch > 0.3 or state.roll < -0.3 or state.roll > 0.3:
            ##Mache alpha berrechnung
            
            pitch = state.pitch
            roll = state.roll
            #Null Fix
            if pitch == 0:
                pitch = 0.00001
            if roll == 0:
                roll = 0.00001
            alpha = math.degrees(math.atan(pitch/roll))
            if roll < 0:                              #Winkel Sinnig Umformatieren
                alpha = 180 + alpha
            if roll > 0 and pitch < 0:
                alpha = 360 + alpha
            client.send_message("/I1-2/mouse/alpha", alpha)
            print("alpha  "+str(alpha))
        if state.x < -0.5 or state.x > 0.5 or state.y < -0.5 or state.y > 0.5:
            ##mache beta berrechnung
            
            #Null Fix
            if x == 0:
                x = 0.00001
            if y == 0:
                y = 0.00001
            beta = math.degrees(math.atan(y/x))
            if x < 0:
                beta = 180 + beta
            if x > 0 and y < 0:
                beta = 360 + beta
            client.send_message("/I1-2/mouse/beta", beta)
            print("beta  "+str(beta))