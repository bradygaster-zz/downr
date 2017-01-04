---
title: Testing SignalR Connections with NUnit
slug: testing-signalr-connections-with-nunit
author: bradygaster
lastModified: 2012-10-05 18:15:12
pubDate: 2012-10-05 18:15:09
categories: SignalR
---

<p>The
  <a title="SignalR GitHub" href="https://github.com/SignalR/SignalR"> SignalR GitHub</a>  site has an example wherein a SignalR <em><a title="The source code for the PersistentConnection class" href="https://github.com/SignalR/SignalR/blob/master/SignalR/PersistentConnection.cs">PersistentConnection</a> </em>   instance is used from a non-HTML client. The idea of being able to use SignalR connections in applications other than those that run in a web browser raises some interesting challenges. Likewise, there aren&#x2019;t too many examples on how to use SignalR
  connections. This post will demonstrate asynchronously testing a SignalR connection in an end-to-end scenario using NUnit.</p>
Creating Custom SignalR Connections
<p>The first step is to create a custom SignalR connection. For the purposes of this example a simple connection implementation will do just fine. Borrowing from the example on the SignalR GitHub site is the <em>EchoConnection </em> class below.</p>
<p>The implementation of the <em>EchoConnection </em> should be relatively self-explanatory. Whenever the connection receives a message from a client, it turns around and sends that message out to all the connected clients using the <em>Broadcast </em> method.</p>
<p>
  <img alt="image" src="/posts/testing-signalr-connections-with-nunit/media/1.png">
</p>
<p>Unlike SignalR Hubs, which are automatically wired up and routed, SignalR Connection classes aren&#x2019;t (at least, I don&#x2019;t know that they are). There&#x2019;s an additional step with SignalR connections; they must be routed similarly to how MVC routes or
  <a title="WCF Web API home page" href="http://wcf.codeplex.com/wikipage?title=WCF%20HTTP">WCF Web API</a>  routes are set up. The code below is from an MVC 3.0 application&#x2019;s <em>Global.asax.cs </em> file, but you could accomplish the same sort of thing using a custom module.</p>
<p>
  <img src="/posts/testing-signalr-connections-with-nunit/media/2.png">
</p>
<p>The point here is to define a route that will provide HTTP access to this connection class. Next, the client code needs to be written. That&#x2019;ll be accomplished using an
  <a title="NUnit is my favorite unit testing framework" href="http://nunit.org/">NUnit</a>  test (it&#x2019;s really an integration test, but that&#x2019;s semantic sugar).</p>
Using the SignalR.Client NuGet Package
<p>The example source code contains a simple class library project. This project references the
  <a title="SignalR.Client NuGet project page" href="http://nuget.org/List/Packages/SignalR.Client">SignalR.Client</a>  NuGet package to provide .NET client communication functionality to an application written in managed code. You can use the SignalR.Client project in your desktop applications or in back-end services or Azure Worker Roles. The class
  library project is shown below, so you can get an idea of the minimal dependencies necessary to support .NET client SignalR support.</p>
<p>
  <img src="/posts/testing-signalr-connections-with-nunit/media/3.png">
</p>
<p>SignalR.Client makes use of
  <a title="Newtonsoft.JSON NuGet project page" href="http://nuget.org/List/Packages/Newtonsoft.JSON">Newtonsoft.JSON</a>, so that project gets pulled in automatically when you update your NuGet package references. <strong>Caveat/Gotcha:</strong>  I had to explicitly redirect this class library project&#x2019;s <em>App.config </em>  file to the newest version
  of the Newtonsoft.JSON assembly. You might not need to do that, but just in case, here&#x2019;s how.</p>
<p>
  <img src="/posts/testing-signalr-connections-with-nunit/media/4.png">
</p>
Writing the Test
<p>At this point, the project is properly set up and everything should be ready for setting up the unit (or integration, depending on your stance) test to communicate with the SignalR connection.</p>
<p>It is important to point out that this test makes use of the <em>AutoResetEvent </em> class to wait for the asynchronous process to complete. This is done because SignalR&#x2019;s communication via either the connection or hub approach implies that the communication
  is asynchronous.</p>
<p>
  <img src="/posts/testing-signalr-connections-with-nunit/media/5.png">
</p>
<p>If this test is run when the web server is turned off, it will obviously fail, as the test output below from NUnit demonstrates.</p>
<p>
  <a href="/Media/Default/SignalRConnectionNUnitTest/6.png">
    <img alt="Failing!!!" src="/posts/testing-signalr-connections-with-nunit/media/6.png">
  </a> 
</p>
<p>Once the web server is started and the test re-run, the client receives a message almost as soon as it finishes sending it, and the test passes.</p>
<p>
  <a href="/Media/Default/SignalRConnectionNUnitTest/7.png">
    <img alt="Passing!!!" src="/posts/testing-signalr-connections-with-nunit/media/7.png">
  </a> 
</p>

Summary
<p>SignalR is an asynchronous messaging library designed to provide push-like functionality and continuous connectivity for web clients. However, it isn&#x2019;t <em>just </em>  for the web; SignalR connections can be used from within .NET code to provide instant
  communication and constant connectivity between multiple clients.</p>
<p>Happy coding!</p>
