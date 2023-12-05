using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerarbeitungTest
{
    internal class TestController
    {
        private double calibrationOffset;
        private QuestionController questionController;
        private SoundDomeView soundDomeView;
        private Test test;
        public TestController(SoundDomeView view, OscRouter router, bool isCallibration)
        {
            router.AddReceiver(routerCallback, OscRouter.SubscriberType.Test);

        }
        public void startTest()
        {

        }
        public void routerCallback(string data)
        {

        }
        public double getCalibrationOffset()
        {
            return calibrationOffset;
        }

    }
}
