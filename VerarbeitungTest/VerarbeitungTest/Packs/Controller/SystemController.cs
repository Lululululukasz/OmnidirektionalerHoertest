﻿using Microsoft.VisualBasic.FileIO;
using Rug.Osc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using VerarbeitungTest.Packages.Model;
using static VerarbeitungTest.OscRouter;

namespace VerarbeitungTest
{
    internal class SystemController
    {
        double calibrationOffset;
        TestController testController;
        CalibrationController calibrationController;
        SoundDomeView soundDomeView;
        OscRouter router;
        private bool testRunning = false;
        private bool startTest = false;
        private bool stopTest = false;
        private bool waitForConfirm = false;
        

        
        public void doTick()
        {
            if (startTest && !testRunning)
            {
                if(calibrationOffset == 0)
                {
                    calibrationController = new CalibrationController(soundDomeView.askQuestion, soundDomeView.giveFeedback, router);
                    calibrationOffset = calibrationController.startCallibration();
                    Thread.Sleep(500);
                    soundDomeView.clearSoundQueue();
                }
                
                testController = new TestController(soundDomeView.askQuestion, soundDomeView.giveFeedback, router, 0);
                testRunning = true;
                Console.WriteLine("Callibration offset is " + calibrationOffset);
                Thread.Sleep(4000);
                testController.startTest();
            }else if (testRunning)
            {
                if (testController.isTestFinished())
                {
                    Test test = testController.getTestResult();
                    Thread.Sleep(500);
                    soundDomeView.clearSoundQueue();
                    double avr = 0;
                    double n = 0;
                    foreach(double d in test.offset)
                    {
                        n++;
                        avr += d;
                        Console.WriteLine($"Scores {d}");
                    }
                    avr = avr / n;
                    SaveTestResult.SaveResult("test.save", avr);
                    testRunning = false;
                    startTest = false;
                }else if (testController.isReadyForNextQuestion())
                {
                    Thread.Sleep(1500);
                    testController.nextQuestion();
                }
            }
            if (stopTest)
            {
                soundDomeView.giveFeedback(SoundDomeView.FeedbackType.test_stop_confirm);
                waitForConfirm = true;
                while (waitForConfirm)
                {
                    Thread.Sleep(100);
                }
                
            }
        }

        public SystemController(string SoundDomeIp)
        {
            
            string ip = SoundDomeIp;
            Console.WriteLine("System is Ready");
            soundDomeView = new SoundDomeView(ip);
            calibrationOffset = 0;
            soundDomeView.giveFeedback(SoundDomeView.FeedbackType.programm_start);
            router = new OscRouter();
            router.AddReceiver(receiveInputs, SubscriberType.System);

        }
        public void receiveInputs(string data)
        {
            if(data == "click:1")
            {
                startTest = true;
                if(waitForConfirm)
                {
                    if (testController != null)
                    {
                        testController.finishTest();
                    }
                    testRunning = false;
                    startTest = false;
                    stopTest = false;
                    waitForConfirm = false ;
                }
            }else if(data == "click:2")
            {
                if (waitForConfirm)
                {
                    waitForConfirm = false;
                    stopTest = false;
                }
                else
                {
                    stopTest = true;
                }
                
                
            }
        }
    }

}
