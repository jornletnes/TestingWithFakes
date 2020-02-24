using System.Threading;

namespace TestingWithFakes1
{
    class Bar
    {
        public int DoTheOtherThing()
        {
            // This is where we fake some heavy lifting
            Thread.Sleep(1000);

            return 42;
        }
    }
}