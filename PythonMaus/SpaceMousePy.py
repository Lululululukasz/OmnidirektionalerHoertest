import pyspacemouse
import time
import math
from pythonosc import udp_client

class RawData:
    def __init__(self):
        self.pitch = 0.0
        self.yaw = 0.0
        self.roll = 0.0
        self.x = 0.0
        self.y = 0.0
        self.z = 0.0
        self.state = object

class Data:
    def __init__(self):
        self.pitch = 0.0
        self.roll = 0.0
        self.x = 0.0
        self.y = 0.0
    def isEqual(self,data):
        if self.pitch == data.pitch and self.roll == data.roll and self.x == data.x and self.y == data.y:
            return True
        else:
            return False

class Sampler:
    def __init__(self):
        self.reader = Reader()
        self.click1 = 0
        self.click2 = 0
        self.lastT = 0
        self.interpreter = Interpreter()
    def run(self):
        while(True):
            data = self.reader.getData()
            if data.pitch < -0.3 or data.pitch > 0.3 or data.roll < -0.3 or data.roll > 0.3 and (data.state.t - lastT) > 0.4:
                lastT = data.state.t
                self.interpreter.interpret(data)
            if data.state.buttons[0] == 1 and self.click1 == 0:
                self.client = udp_client.SimpleUDPClient("127.0.0.1", 9000)
                self.client.send_message("/I1-2/mouse/click", 1)
                self.click1 = 1
                print("Clicked button 1")
            elif data.state.buttons[0] == 0:
                self.click1 = 0
            if data.state.buttons[1] == 1 and self.click2 == 0:
                self.client = udp_client.SimpleUDPClient("127.0.0.1", 9000)
                self.client.send_message("/I1-2/mouse/click", 2)
                self.click2 = 1
                print("Clicked button 2")
            elif data.state.buttons[1] == 0:
                self.click2 = 0   
            
            #time.sleep(0.1)

class Reader:
    def __init__(self):
        self.success = pyspacemouse.open()
        if not self.success:
            print("Mouse Error")
    def getData(self):
        if self.success:
            data = RawData()
            state = pyspacemouse.read()
            data.x = state.x
            data.y = state.y
            data.roll = state.roll
            data.pitch = state.pitch
            data.state = state
            return(data)

class Interpreter:
    def __init__(self):
        self.changeFilter = ChangeFilter()
    def interpret(self,normdata):
        
        #Null Fix
        if normdata.pitch == 0:
            normdata.pitch = 0.00001
        if normdata.roll == 0:
            normdata.roll = 0.00001

        alpha = math.degrees(math.atan(normdata.pitch/normdata.roll))
        if normdata.roll < 0:                              #Winkel Sinnig Umformatieren
            alpha = 180 + alpha
        if normdata.roll > 0 and normdata.pitch < 0:
            alpha = 360 + alpha
            
        
        #Null Fix
        if normdata.x == 0:
            normdata.x = 0.00001
        if normdata.y == 0:
            normdata.y = 0.00001
        
        beta = math.degrees(math.atan(normdata.y/normdata.x))
        if normdata.x < 0:
            beta = 180 + beta
        if normdata.x > 0 and normdata.y < 0:
            beta = 360 + beta
        
        alpha = round(alpha, 2)
        beta = round(beta, 2)
        self.changeFilter.checkValue(alpha,beta)
        

class ChangeFilter:
    def __init__(self):
        self.oldalpha = 0
        self.oldbeta = 0
        self.oscsender = OSCSender()
    def checkValue(self,alpha,beta):
        if self.oldalpha != alpha:
            self.oscsender.sendData(alpha,beta)
            self.oldalpha = alpha
        
            

class OSCSender:
    def __init__(self):
        self.client = udp_client.SimpleUDPClient("127.0.0.1", 9000)
    def sendData(self,alpha,beta):
        self.client.send_message("/I1-2/mouse/alpha", alpha)
        print("alpha "+ str(alpha))


#run programm
sampler = Sampler()
sampler.run()
