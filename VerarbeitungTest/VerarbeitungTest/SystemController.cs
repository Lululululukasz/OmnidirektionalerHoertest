using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Linq;
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
            calibrationController = new CalibrationController(soundDomeView.askQuestion, router);
            calibrationOffset = calibrationController.startCallibration();
            testController = new TestController(soundDomeView.askQuestion, router, calibrationOffset);
            testController.startTest();
        }

        void cancelTest() {
            testController.finishTest();
        }

        public SystemController()
        {
            //in case someone cancels a test before starting it
            testController = new TestController(soundDomeView.askQuestion, router, calibrationOffset);
            router = new OscRouter();
           // router.AddReceiver(SystemController.callback(click), OscRouter.SubscriberType.System);
            router.AddReceiver((message) =>
            {
                if (message == "click:1") startTest();
                else if (message == "click:2") cancelTest();
            }, SubscriberType.System);
        }
    }

}
