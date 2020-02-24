# Testing with injected fakes

_This article is aimed at beginner to intermediate object-oriented programmers. The examples in the article are pseudo-code. The code examples in the Git repo are C#, with NUnit as the test runner harness and Moq as the faking framework._

## Introduction

Automated tests allow you to verify that your code works, without opening your app and clicking through the workflow. Tests can typically be run from your code editor - while you&#39;re writing the code for the feature. Your tests must be reliable - they must behave the same every time you run them. When a test breaks, it will reveal where the bug is, and what you must do to fix it - it cuts down on analysis time. Automated tests will save you from accidentally deploying a broken version of your app. If you&#39;re careful with your wording, an automated test describes a capability (or feature) your app provides.

A unit test is tests for the smallest testable unit; typically a class or a function. An integration test is a test for compound behaviors between several testable units; typically how several classes or functions cooperate. A unit test is simpler to understand and faster to run; an integration test is harder to understand and slower to run. When writing tests, you should state as many capabilities as possible as unit tests.

The following lesson takes an integration test using two testable units, and re-states it as a unit test that tests one unit in isolation from the other.

## The initial feature and its code

Imagine you have the following classes.
```
1. **class**  Foo
2. {
3.      **public**   **int**  DoAThing()
4.     {
5.         // Perform the actions,
6.         // then act on bar.
7.
8.         Bar bar =  **new**  Bar();
9.          **return**  bar.DoTheOtherThing();
10.     }
11. }
```

```
1. **class**  Bar
2. {
3.      **public**   **int**  DoTheOtherThing()
4.     {
5.         // This is where we fake some heavy lifting
6.         Thread.Sleep(1000);
7.
8.          **return**  42;
9.     }
10. }
```

Foo.DoAThing() does a few actions, then news up a Bar and calls bar.DoTheOtherThing().

Bar.DoTheOtherThing() does some pretty expensive computations, or fires off a query over the internet, or flushes a cache, or does any other non-performant or side-effecting thing we shouldn&#39;t be doing in a test.

The test code for Foo looks something like below.

```
1. [Test]
2. **public**   **void**  TestDoAThing()
3. {
4.     //Arrange
5.     Foo foo =  **new**  Foo();
6.      **int**  expected = 42;
7.
8.     //Act
9.      **int**  result = foo.DoAThing();
10.
11.     //Assert
12.     Assert.That(result, Is.EqualTo(expected));
13. }
```

The problem with this test is that we assert on the behavior of another class - in order to test Foo, we must perform the expensive actions of Bar. Foo.TestDoAThing() will break when Bar.DoTheOtherThing() breaks. When analyzing the broken test, we would need to fully understand bar.DoTheOtherThing(). This is time consuming and cognitively taxing.

Notice that the function DoAThing() news up the Bar class, so there is no way we can substitute in a test fake.

## The same feature, but a concretion is injected

The solution is to pass in a Bar into the function. This way, we can construct a Bar in the test code. We call this injection.

```
1. **public**   **int**  DoAThing(Bar bar)
2. {
3.     // Perform the actions,
4.     // then act on bar.
5.
6.      **return**  bar.DoTheOtherThing();
7. }
```

```
1. [Test]
2. **public**   **void**  TestDoAThing()
3. {
4.     //Arrange
5.     Foo foo =  **new**  Foo();
6.     Bar bar =  **new**  Bar();
7.      **int**  expected = 42;
8.
9.     //Act
10.      **int**  result = foo.DoAThing(bar);
11.
12.     //Assert
13.     Assert.That(result, Is.EqualTo(expected));
14. }
```

Injecting bar makes it better, but we are still asserting on the behavior of the Bar class. And the Bar class is still a concrete type. We could provide a fake version in the test by manually writing a sub class of Bar, make sure its DoTheOtherThing() implementation is cheap and safe, and pass this new subclass in. But there is a better way.

## The same feature, but an abstraction is injected

The solution is to make Bar inherit from an Interface, then pass this interface into the function. In other words - rely on an abstraction rather than a concretion. By injecting an abstraction, we are making it easy to swap out the heavy concrete class with a fake one. We can mock the Interface; which means we will construct a fake version of the real type, set up the mock to return the same value as the production code, but completely skip the performance hit.

```
1. **public**   **int**  DoAThing(IBar bar)
2. {
3.     // Perform the actions,
4.     // then act on bar.
5.
6.      **return**  bar.DoTheOtherThing();
7. }
```

```
1. [Test]
2. **public**   **void**  TestDoAThing()
3. {
4.     //Arrange
5.     Foo foo =  **new**  Foo();
6.      **int**  expected = 42;
7.     Mock\&lt;IBar\&gt; fakeBar =  **new**  Mock\&lt;IBar\&gt;();
8.     fakeBar.Setup(f =\&gt; f.DoTheOtherThing()).Returns(expected);
9.
10.     //Act
11.      **int**  result = foo.DoAThing(fakeBar.Object);
12.
13.     //Assert
14.     Assert.That(result, Is.EqualTo(expected));
15. }
```

We are now able to test Foo, without relying on an actual Bar. We don&#39;t need to invoke Bars heavy DoTheOtherThing(), and we can be confident there are no side effects. We can test Foo and Bar independently, in complete isolation from one another.

Bottom line is **always inject abstractions, because they are easy to fake**.

If you have comments or questions, hit me up at [jorn.letnes@gmail.com](mailto:jorn.letnes@gmail.com)