namespace TestingWithFakes3
{
    class Foo
    {
        public int DoAThing(IBar bar)
        {
            // Perform the actions,
            // then act on bar.

            return bar.DoTheOtherThing();
        }
    }
}