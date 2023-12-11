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
        SoundDomeView soundDomeView;
        OscRouter router;

        void startTest()
        {
            testController.startTest();
        }

        void cancelTest() {
            testController.finishTest();
        }

        void startCalibration()
        {
            testController.startCallibration();
        }

        public SystemController()
        {
            testController = new TestController(soundDomeView.askQuestion, router, calibrationOffset);
            router = new OscRouter();
           // router.AddReceiver(SystemController.callback(click), OscRouter.SubscriberType.System);
            router.AddReceiver((message) =>
            {
                if (message == "click:1") startTest();
                else if (message == "click:2") startCalibration();
                else if (message == "click:3") cancelTest();
            }, SubscriberType.System);
        }
    }

}
