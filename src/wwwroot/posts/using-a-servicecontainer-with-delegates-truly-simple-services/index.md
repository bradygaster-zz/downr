---
title: Using a ServiceContainer with Delegates - Truly Simple Services
slug: using-a-servicecontainer-with-delegates-truly-simple-services
author: bradygaster
lastModified: 2011-05-21 02:19:31
pubDate: 2012-10-05 18:15:05
categories: .NET
---

<p>While working with a colleague on a project I got into a debate about event-handling versus delegates stuffed into a service container. It sounded weird to me at the time; I&apos;ve always been a huge fan of event-driven programming so I tend to lean that
  way when I need to make my objects responsive to one another. The way my colleague looks at it, &quot;everything&apos;s a service, so if you just add your methods that you need to run to your service container you can call them when you need them.&quot; The idea seemed
  truly nutty to me so I just had to code it up. It is important to note here that our system already made use of the ServiceContainer approach. We have a custom service container that our applications use; in this way we can add any service to the container
  at run-time and provide our whole application (and all of it&apos;s components) with access to everything via the service container. If you&apos;re not familliar with that approach this may seem a little off-kilter.&#xA0;</p>
<p>First thing I did was to create a custom ServiceContainer implementation. The code for my GenericServiceContainer is below. Note the extra method I&apos;ve added which makes use of generic types.&#xA0;</p>
<p>&#xA0;</p>
<pre>public class GenericServiceContainer : System.ComponentModel.Design.ServiceContainer
{
&#xA0; public virtual T GetService() where T : class
&#xA0; {
&#xA0;&#xA0;&#xA0; return this.GetService(typeof(T)) as T;
&#xA0; }
}</pre>
<p>Next, I&apos;ll create a few delegate types. In this way, any class that has knowledge of these delegate types can request the GSC send&apos;em out.</p>
<pre>public delegate void MessageDelegate(string message);
public delegate int AdditionDelegate(int x, int y);</pre>
<p>&#xA0;</p>
<p>Though the functionality abstracted in the form of delegates I still need to create a class that can &quot;do the work&quot; for the application. To accomplish this I&apos;ve written a simple WorkerClass, the code for which is below.</p>
<pre>public class WorkerClass
{
&#xA0; public static void ShowMessage(string message)
&#xA0; {
&#xA0;&#xA0;&#xA0; Console.WriteLine(message);
&#xA0; }
&#xA0; 
&#xA0; public static int Add(int x, int y)
&#xA0; {
&#xA0;&#xA0;&#xA0; return (x + y);
&#xA0; }
}</pre>
<p>
  <br> Finally, some tests will prove the theory. &#xA0;</p>
<pre>[TestFixture]
public class TestServiceContainer
{
&#xA0; GenericServiceContainer _container;
&#xA0; 
&#xA0; [SetUp]
&#xA0; public void Setup()
&#xA0; {
&#xA0;&#xA0;&#xA0; _container = new GenericServiceContainer();
&#xA0;&#xA0;&#xA0; _container.AddService(typeof(MessageDelegate),
&#xA0;&#xA0;&#xA0;&#xA0;&#xA0; new MessageDelegate(WorkerClass.ShowMessage));
&#xA0;&#xA0;&#xA0;&#xA0; &#xA0;
&#xA0;&#xA0;&#xA0; _container.AddService(typeof(AdditionDelegate),
&#xA0;&#xA0;&#xA0;&#xA0;&#xA0; new AdditionDelegate(WorkerClass.Add));
&#xA0; }
&#xA0; 
&#xA0; [TearDown]
&#xA0; public void TearDown()
&#xA0; {
&#xA0;&#xA0;&#xA0; _container.Dispose();
&#xA0; }
&#xA0; 
&#xA0; [Test]
&#xA0; public void TestMessageDelegate()
&#xA0; {
&#xA0;&#xA0;&#xA0; MessageDelegate del = _container.GetService();
&#xA0;&#xA0;&#xA0; Assert.IsNotNull(del);
&#xA0;&#xA0;&#xA0; del.Invoke(&quot;Testing&quot;);
&#xA0; }
&#xA0; 
&#xA0; [Test]
&#xA0; public void TestAdditionDelegate()
&#xA0; {
&#xA0;&#xA0;&#xA0; AdditionDelegate del = _container.GetService();
&#xA0;&#xA0;&#xA0; Assert.IsNotNull(del);
&#xA0;&#xA0;&#xA0; int result = del.Invoke(2, 2);
&#xA0;&#xA0;&#xA0; Assert.AreEqual(result, (2 + 2));
&#xA0; }
}</pre>
<p>&#xA0;</p>
<p>So that&apos;s it! With this code I&apos;ve defined the structure of the methods that will be doing the work and allowed redirection to a worker class to perform the work. Now, any class within the application being augmented by my custom ServiceContainer can have
  easy access to centralized functionality.</p>
<p>Happy Coding!&#xA0;</p>
