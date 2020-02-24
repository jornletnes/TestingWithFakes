namespace TestingWithFakes1
{
    class Foo
    {
        public int DoAThing()
        {
            // Perform the actions,
            // then act on bar.

            Bar bar = new Bar();
            return bar.DoTheOtherThing();
        }
    }
}