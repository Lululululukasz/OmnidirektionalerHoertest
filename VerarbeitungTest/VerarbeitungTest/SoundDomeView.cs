using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using Rug.Osc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace VerarbeitungTest
{
    internal class SoundDomeView
    {
        public enum FeedbackType
        {
            rise,
            fall,
            beep
        }


        private static Object _lock;
        private static Thread soundThread;
        private List<Question> QuestionQueue;
        private List<FeedbackType> FeedbackQueue;

        public SoundDomeView()
        {
            QuestionQueue = new List<Question>();
            FeedbackQueue = new List<FeedbackType>();
            soundThread = new Thread(new ThreadStart(SoundOSCThread));
            _lock = new Object();
            soundThread.Start();
        }
        public void askQuestion(Question question)
        {
            QuestionQueue.Add(question);
        }
        public void giveFeedback(FeedbackType f)
        {
            FeedbackQueue.Add(f);
        }
        private void SoundOSCThread()
        {
            while (true)
            {
                lock (_lock)
                {
                    if (QuestionQueue.Count != 0)
                    {
                        Question currentQuestion = QuestionQueue[0];
                        QuestionQueue.RemoveAt(0);

                        OscSender sender = new OscSender(IPAddress.Parse("127.0.0.1"), 9000);
                        sender.Connect();
                        sender.Send(new OscMessage("/adm/obj/1/azim", currentQuestion.angle - 180));
                        sender.Send(new OscMessage("/adm/obj/1/elev", 0));
                        double frequency = currentQuestion.pitch;
                        var sine3Seconds = new SignalGenerator()
                        {
                            Gain = 0.2,
                            Frequency = frequency,
                            Type = SignalGeneratorType.Sin
                        }.Take(TimeSpan.FromSeconds(3));
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
                    else if (FeedbackQueue.Count != 0)
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
                                fbck = new SignalGenerator()
                                {
                                    Gain = 0.2,
                                    Frequency = 200, // start frequency of the sweep
                                    FrequencyEnd = 1000,
                                    Type = SignalGeneratorType.Sweep,
                                    SweepLengthSecs = 1
                                }.Take(TimeSpan.FromSeconds(1));
                                break;
                            case FeedbackType.fall:
                                fbck = new SignalGenerator()
                                {
                                    Gain = 0.2,
                                    Frequency = 1000, // start frequency of the sweep
                                    FrequencyEnd = 200,
                                    Type = SignalGeneratorType.Sweep,
                                    SweepLengthSecs = 1
                                }.Take(TimeSpan.FromSeconds(1));
                                break;
                            case FeedbackType.beep:
                                fbck = new SignalGenerator()
                                {
                                    Gain = 0.2,
                                    Frequency = 1000, // start frequency of the sweep
                                    FrequencyEnd = 200,
                                    Type = SignalGeneratorType.Sweep,
                                    SweepLengthSecs = 0.2
                                }.Take(TimeSpan.FromSeconds(1));
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
                }
                Thread.Sleep(100);
            }
        }
    }
}
