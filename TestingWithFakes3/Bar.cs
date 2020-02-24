using System.Threading;

namespace TestingWithFakes3
{
    class Bar : IBar
    {
        public virtual int DoTheOtherThing()
        {
            // This is where we fake some heavy lifting
            Thread.Sleep(1000);

            return 42;
        }
    }
}