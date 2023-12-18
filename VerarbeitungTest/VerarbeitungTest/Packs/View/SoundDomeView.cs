using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using Rug.Osc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;
using VerarbeitungTest.Packages.Model;

namespace VerarbeitungTest
{
    internal class SoundDomeView
    {
        public enum FeedbackType
        {
            rise,
            fall,
            beep,
            start_test,
            test_passed,
            test_not_passed,
            start_callibration,
            callibration_passed,
            correct,
            wrong,
            programm_start,
            test_stop_confirm
        }


        private static Object _lock;
        private static Thread soundThread;
        private List<Question> QuestionQueue;
        private List<FeedbackType> FeedbackQueue;
        private Dictionary<FeedbackType,Mp3FileReader> mp3Dictionary;
        private string ip;
        private OscSender sender;
        public SoundDomeView(string soundDomeIp = "127.0.0.1")
        {
            sender = new OscSender(IPAddress.Parse(soundDomeIp), 9001);
            ip = soundDomeIp;
            QuestionQueue = new List<Question>();
            FeedbackQueue = new List<FeedbackType>();
            mp3Dictionary = new Dictionary<FeedbackType, Mp3FileReader>();
            //Load mp3 files
            mp3Dictionary[FeedbackType.start_test] = new Mp3FileReader("audio/start-test.mp3");
            mp3Dictionary[FeedbackType.start_callibration] = new Mp3FileReader("audio/start-callibration.mp3");
            mp3Dictionary[FeedbackType.test_passed] = new Mp3FileReader("audio/Test-Passed.mp3");
            mp3Dictionary[FeedbackType.test_not_passed] = new Mp3FileReader("audio/test-not-passed.mp3");
            mp3Dictionary[FeedbackType.callibration_passed] = new Mp3FileReader("audio/callibration-passed.mp3");
            mp3Dictionary[FeedbackType.correct] = new Mp3FileReader("audio/correct.mp3");
            mp3Dictionary[FeedbackType.wrong] = new Mp3FileReader("audio/wrong.mp3");
            mp3Dictionary[FeedbackType.programm_start] = new Mp3FileReader("audio/program_start.mp3");
            mp3Dictionary[FeedbackType.test_stop_confirm] = new Mp3FileReader("audio/test_stop_confirm.mp3");
            soundThread = new Thread(new ThreadStart(SoundOSCThread));
            _lock = new Object();
            soundThread.Start();
        }
        public void askQuestion(Question question)
        {
            sender.Send(new OscMessage("/adm/obj/1/azim", (float)(question.angle - 180)));
            sender.Send(new OscMessage("/adm/obj/1/elev", (float)0.0));
            sender.Send(new OscMessage("/adm/obj/1/dist", (float)0.8));
            sender.Send(new OscMessage("/adm/obj/2/azim", (float)(question.angle - 180)));
            sender.Send(new OscMessage("/adm/obj/2/elev", (float)0.0));
            sender.Send(new OscMessage("/adm/obj/2/dist", (float)0.8));
            QuestionQueue.Add(question);
        }
        public void giveFeedback(FeedbackType f)
        {
            FeedbackQueue.Add(f);
        }
        public Dictionary<FeedbackType,Mp3FileReader> getMp3Dictionary()
        {
            return mp3Dictionary;
        }
        public void clearSoundQueue()
        {
            FeedbackQueue.Clear();
            QuestionQueue.Clear();
        }
        private void SoundOSCThread()
        {
            
            double lastAngle = 0;
            sender.Connect();
            while (true)
            {
                lock (_lock)
                {
                    if (FeedbackQueue.Count != 0)
                    {

                        FeedbackType currentFeedback = FeedbackQueue[0];
                        FeedbackQueue.RemoveAt(0);


                        var fbck = new SignalGenerator()
                        {
                            Gain = 0.2,
                            Frequency = 30,
                            Type = SignalGeneratorType.Sin
                        }.Take(TimeSpan.FromSeconds(1));
                        switch (currentFeedback)
                        {
                            case FeedbackType.rise:
                                fbck = new SignalGenerator(44100, 1)
                                {
                                    Gain = 0.2,
                                    Frequency = 200, // start frequency of the sweep
                                    FrequencyEnd = 1000,
                                    Type = SignalGeneratorType.Sweep,
                                    SweepLengthSecs = 1
                                }.Take(TimeSpan.FromSeconds(1));
                                break;
                            case FeedbackType.fall:
                                fbck = new SignalGenerator(44100, 1)
                                {
                                    Gain = 0.2,
                                    Frequency = 1000, // start frequency of the sweep
                                    FrequencyEnd = 200,
                                    Type = SignalGeneratorType.Sweep,
                                    SweepLengthSecs = 1
                                }.Take(TimeSpan.FromSeconds(1));
                                break;
                            case FeedbackType.beep:
                                fbck = new SignalGenerator(44100, 1)
                                {
                                    
                                    Gain = 0.2,
                                    Frequency = 1000, // start frequency of the sweep
                                    FrequencyEnd = 200,
                                    Type = SignalGeneratorType.Sweep,
                                    SweepLengthSecs = 0.2
                                }.Take(TimeSpan.FromSeconds(1));
                                break;
                            default:
                                if (mp3Dictionary.ContainsKey(currentFeedback))
                                {
                                    mp3Dictionary[currentFeedback].CurrentTime = TimeSpan.Zero;
                                    fbck = mp3Dictionary[currentFeedback].ToSampleProvider();

                                }
                                
                                break;
                        }
                        using (var wo = new WaveOutEvent())
                        {
                            wo.Init(fbck);
                            wo.Play();
                            while (wo.PlaybackState == PlaybackState.Playing)
                            {
                                Thread.Sleep(100);
                            }
                        }

                    }
                    else if (QuestionQueue.Count != 0)
                    {
                        Question currentQuestion = QuestionQueue[0];
                        QuestionQueue.RemoveAt(0);
                        lastAngle = currentQuestion.angle;
                        Console.WriteLine("setAngle " + (lastAngle));
                        
                        double frequency = currentQuestion.pitch;
                        Console.WriteLine("Freq: " + frequency + "hz");
                        var sine3Seconds = new SignalGenerator(44100,1)
                        {
                            Gain = 0.2,
                            Frequency = frequency,
                            Type = SignalGeneratorType.Sin
                        }.Take(TimeSpan.FromSeconds(1));
                        using (var wo = new WaveOutEvent())
                        {
                            wo.Init(sine3Seconds);
                            wo.Play();
                            while (wo.PlaybackState == PlaybackState.Playing)
                            {
                                Thread.Sleep(100);
                            }
                        }
                    }
                }
                
                Thread.Sleep(5);
            }
        }
    }
}
