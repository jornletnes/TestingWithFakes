namespace TestingWithFakes2
{
    class Foo
    {
        public int DoAThing(Bar bar)
        {
            // Perform the actions,
            // then act on bar.

            return bar.DoTheOtherThing();
        }
    }
}