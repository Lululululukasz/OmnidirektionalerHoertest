﻿using Microsoft.VisualBasic.FileIO;
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
            testController = new TestController(soundDomeView.askQuestion, router, calibrationOffset);
        }

        void cancelTest()
        {

        }


        public SystemController()
        {
            router = new OscRouter();
           // router.AddReceiver(SystemController.callback(click), OscRouter.SubscriberType.System);
            router.AddReceiver((message) =>
            {
                if (message == "click:1") cancelTest();
            }, SubscriberType.System);
        }
    }

}
