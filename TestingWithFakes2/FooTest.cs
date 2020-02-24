using NUnit.Framework;

namespace TestingWithFakes2
{
    class FooTest
    {
        [Test]
        public void TestDoAThing()
        {
            //Arrange
            Foo foo = new Foo();
            Bar bar = new Bar();
            int expected = 42;

            //Act
            int result = foo.DoAThing(bar);

            //Assert
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}