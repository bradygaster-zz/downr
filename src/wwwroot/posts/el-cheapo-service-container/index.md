---
title: El Cheapo Service Container
slug: el-cheapo-service-container
author: bradygaster
lastModified: 2012-10-05 18:15:15
pubDate: 2012-10-05 18:15:04
categories: .NET
---

<p>A few days ago
  <a href="http://bojordan.com/log/?p=597" title="DI on the cheap">a collegaue of mine blogged about performing DI on the cheap</a> . His post got me to thinking about the various IoC containers and DI frameworks that have sprung up (no pun intended, sorry for that heinous attempt at geek humor). As I&apos;ve been working
  through the Social Timeline architecture I&apos;ve concluded the importance that&apos;ll reside on being able to snap in time line data providers with ease. Inspired by Bo&apos;s post, I&apos;ve named the project <em>El Cheapo</em> . Though it doesn&apos;t make use of any of
  the popular IoC/DI frameworks out there it does borrow some of the general ideas from their implementations. I was specifically inspired by
  <a href="http://blog.vuscode.com/malovicn/archive/2008/05/18/design-for-testability-microsoft-unity-part-7.aspx" title="Nikola&apos;s Unity Post">Nikola Malovic&apos;s discussion of Unity</a>, namely the way interfaces are resolved to types.&#xA0;</p>
<p>As you&apos;ll see, the only significant difference between a typical service container implementation and this custom version is that this implementation takes into account the concern of having multiple implementations of a given interface. In Social Timeline
  for instance, I&apos;ve listed the various time line data providers as tabs in the GUI layer. In this way a user can see all that they could represent on the timeline below the tabs. I had intended to use the Unity framework in this implementation but decided
  it was a little more lightweight to just build what I needed myself. Below, you&apos;ll see the code for the El Cheapo service container in its [current] entirety.</p>
<p>&#xA0;</p>
<pre>public class FunctionalityBasket
{
&#xA0;&#xA0;&#xA0; Dictionary&gt; _innerBasket;
&#xA0;&#xA0;&#xA0; public FunctionalityBasket()
&#xA0;&#xA0;&#xA0; {
&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0; _innerBasket = new Dictionary&gt;();
&#xA0;&#xA0;&#xA0; }
&#xA0;&#xA0;&#xA0; public void Register<i>(I implementation)
&#xA0;&#xA0;&#xA0; {
&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0; Register<i>(implementation, implementation.GetType().Name);
&#xA0;&#xA0;&#xA0; }
&#xA0;&#xA0;&#xA0; public void Register<i>(I implementation, string name)
&#xA0;&#xA0;&#xA0; {
&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0; Type t = typeof(I);
&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0; if (!_innerBasket.ContainsKey(t))
&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0; _innerBasket[t] = new Dictionary();
&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0; _innerBasket[t].Add(
&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0; name,
&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0; implementation
&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0; );
&#xA0;&#xA0;&#xA0; }
&#xA0;&#xA0;&#xA0; public Dictionary GetImplementations()
&#xA0;&#xA0;&#xA0; {
&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0; Type t = typeof(TInterface);
&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0; Dictionary ret = new Dictionary();
&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0; Dictionary.Enumerator enm = _innerBasket[t].GetEnumerator();
&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0; while (enm.MoveNext())
&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0; ret.Add(enm.Current.Key, (TInterface)enm.Current.Value);
&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0; return ret;
&#xA0;&#xA0;&#xA0; }
&#xA0;&#xA0;&#xA0; public TInterface GetImplementation(string name)
&#xA0;&#xA0;&#xA0; {
&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0; return GetImplementations()[name];
&#xA0;&#xA0;&#xA0; }
}
</i></i></i></pre>
<p>&#xA0;</p>
<p>I feel the best way to exemplify usage of the FunBasket (another bad pun) is via a series of unit tests, which are below.</p>
<p>&#xA0;</p>
<pre>[TestFixture]
public class ElCheapoUnitTests
{
&#xA0;&#xA0;&#xA0; [TestFixtureSetUp]
&#xA0;&#xA0;&#xA0; public void SetupTestFixture()
&#xA0;&#xA0;&#xA0; {
&#xA0;&#xA0;&#xA0; }
&#xA0;&#xA0;&#xA0; [TestFixtureTearDown]
&#xA0;&#xA0;&#xA0; public void TearDownTestFixture()
&#xA0;&#xA0;&#xA0; {
&#xA0;&#xA0;&#xA0; }
&#xA0;&#xA0;&#xA0; [Test]
&#xA0;&#xA0;&#xA0; public void InterfacesAndImplementationsCanBeAddedToBasket()
&#xA0;&#xA0;&#xA0; {
&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0; FunctionalityBasket basket = new FunctionalityBasket();
&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0; basket.Register(new NullLogger());
&#xA0;&#xA0;&#xA0; }
&#xA0;&#xA0;&#xA0; [Test]
&#xA0;&#xA0;&#xA0; public void InterfacesAndImplementationsCanBeAddedAndListOfInterfacesRetrieved()
&#xA0;&#xA0;&#xA0; {
&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0; FunctionalityBasket basket = new FunctionalityBasket();
&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0; basket.Register(new NullLogger(), NullLogger.Name);
&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0; basket.Register(new LameLogger(), LameLogger.Name);
&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0; Dictionary loggers = basket.GetImplementations();
&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0; Assert.That(loggers.Count &gt; 0, &quot;Implementations can be added and retrieved by interface type.&quot;);
&#xA0;&#xA0;&#xA0; }
&#xA0;&#xA0;&#xA0; [Test]
&#xA0;&#xA0;&#xA0; public void ReturnedImplementationsAreUsefulBasedOnInterface()
&#xA0;&#xA0;&#xA0; {
&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0; FunctionalityBasket basket = new FunctionalityBasket();
&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0; basket.Register(new NullLogger(), NullLogger.Name);
&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0; basket.Register(new LameLogger(), LameLogger.Name);
&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0; Dictionary loggers = basket.GetImplementations();
&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0; Assert.That(loggers.Count &gt; 0, &quot;Implementations can be added and retrieved by interface type.&quot;);
&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0; foreach (ILogger logger in loggers.Values)
&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0; logger.Log();
&#xA0;&#xA0;&#xA0; }
&#xA0;&#xA0;&#xA0; [Test]
&#xA0;&#xA0;&#xA0; public void ImplementationsCanBeRetrievedByName()
&#xA0;&#xA0;&#xA0; {
&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0; FunctionalityBasket basket = new FunctionalityBasket();
&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0; basket.Register(new NullLogger(), NullLogger.Name);
&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0; basket.Register(new LameLogger(), LameLogger.Name);
&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0; Assert.IsNotNull(basket.GetImplementation(NullLogger.Name), &quot;Null Logger was added and is retrievable&quot;);
&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0; Assert.IsNotNull(basket.GetImplementation(LameLogger.Name), &quot;Lame Logger was added and is retrievable&quot;);
&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0; basket.GetImplementation(NullLogger.Name).Log();
&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0; basket.GetImplementation(LameLogger.Name).Log();
&#xA0;&#xA0;&#xA0; }
}
// -------------------------------------------------------
// test implementations
// -------------------------------------------------------
interface ILogger
{
&#xA0;&#xA0;&#xA0; void Log();
}
class NullLogger : ILogger
{
&#xA0;&#xA0;&#xA0; public static string Name = &quot;NullLogger&quot;;
&#xA0;&#xA0;&#xA0; public void Log()
&#xA0;&#xA0;&#xA0; {
&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0; Console.WriteLine(&quot;This isn&apos;t happening&quot;);
&#xA0;&#xA0;&#xA0; }
}
class LameLogger : ILogger
{
&#xA0;&#xA0;&#xA0; public static string Name = &quot;LameLogger&quot;;
&#xA0;&#xA0;&#xA0; public void Log()
&#xA0;&#xA0;&#xA0; {
&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0; Console.WriteLine(&quot;I&apos;m so lame&quot;);
&#xA0;&#xA0;&#xA0; }
}
</pre>
<p>&#xA0;</p>
<p>Happy Coding! And GO Bulldogs and Panthers this weekend!</p>
