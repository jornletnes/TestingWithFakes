using NUnit.Framework;

namespace TestingWithFakes1
{
    class FooTest
    {
        [Test]
        public void TestDoAThing()
        {
            //Arrange
            Foo foo = new Foo();
            int expected = 42;

            //Act
            int result = foo.DoAThing();

            //Assert
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}