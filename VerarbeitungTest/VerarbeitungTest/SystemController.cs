using Microsoft.VisualBasic.FileIO;
using Rug.Osc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
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

        void startTest()
        {
            //calibrationController = new CalibrationController(soundDomeView.askQuestion, soundDomeView.giveFeedback, router);
            //calibrationOffset = calibrationController.startCallibration();
            testController = new TestController(soundDomeView.askQuestion,soundDomeView.giveFeedback, router, 0);
            testController.startTest();
        }

        void cancelTest() {
            Console.WriteLine(2);
            testController.finishTest();
        }

        public SystemController()
        {
            soundDomeView = new SoundDomeView("127.0.0.1");
            calibrationOffset = 0;
            router = new OscRouter();
            router.AddReceiver((message) =>
            {
                if (message == "click:1") startTest();
                else if (message == "click:2" && testController != null)
                {
                    cancelTest();
                }
            }, SubscriberType.System);

            //in case someone cancels a test before starting it
            testController = new TestController(soundDomeView.askQuestion, soundDomeView.giveFeedback, router, calibrationOffset);

        }
    }

}
