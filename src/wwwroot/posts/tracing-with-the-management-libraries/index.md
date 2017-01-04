---
title: Tracing with the Management Libraries
slug: tracing-with-the-management-libraries
author: 
lastModified: 2013-11-13 20:49:16
pubDate: 2013-11-13 20:49:16
categories: Azure
---

<p>When we built out the
  <a href="http://www.nuget.org/packages?q=Microsoft.WindowsAzure.Management&amp;prerelease=true&amp;sortOrder=relevance">Azure Management Libraries</a>, one of the things we knew we needed to do was to make it easy for developers to trace API conversations however they want or need, to whatever medium they&#x2019;d like to save the trace logs. The deeper the trace information
  available during conversations with the Azure REST API, the easier it is to figure out what&#x2019;s going on. To make tracing easier and more flexible, we added an interface to the SDK common package called the <strong>ICloudTracingInterceptor. </strong> You
  can implement this method however you want to drop the trace messages from the API conversations. This post will walk through a simple example of using the tracing interception functionality in the management libraries in a WPF application. </p>
Building a Custom Interceptor
<p>First, a quick inspection of the tracing interceptor interface is in order. The object browser window below shows the interface and all of the methods it offers.</p>
<p>
  <a href="http://www.bradygaster.com/posts/files/ac8f3dd8-3e1c-4d47-b4cf-ab0ac9bedb2a.png">
    <img alt="Object Browser" src="/posts/tracing-with-the-management-libraries/media/f493ed63-cec3-4bad-851b-41bba18a95be.png">
  </a> 
</p>
<p>An obvious implementation of the interceptor logic would be to dump trace messages to a Console window or to a debug log. Given I&#x2019;m building a desktop application I&#x2019;d like to see that trace output dropped into a multi-line textbox. Rather than pass the
  constructor of my implementation a reference to a WPF TextBox control I&#x2019;ll take a reference to an <strong>Action</strong>, as I may need to be proactive in handling threading issues associated with the API calls running in an asynchronous manner.
  Better safe than sorry, right? Below is the beginning of the code for my custom tracing interceptor. </p>
<p>
  <a href="http://www.bradygaster.com/posts/files/b357ea33-dc89-4fd6-bb53-28afe0aa1876.png">
    <img alt="Custom tracing interceptor implementation" src="/posts/tracing-with-the-management-libraries/media/b6e10e64-a36b-4d81-87b3-871e16f105ae.png">
  </a> 
</p>
<p>With the custom implementation built, I&#x2019;m ready to use it in an application. </p>
Hooking the Interceptor to the Cloud Context
<p>Also provided within our common package is another helpful object that abstracts the very idea of the cloud&#x2019;s context and the conversations occurring with the REST API. This abstraction, appropriately named <strong>CloudContext</strong>, is visible in
  the object browser below. </p>
<p>
  <a href="http://www.bradygaster.com/posts/files/5cc5503f-4941-4c04-8e37-bd36cd8fa496.png">
    <img alt="Object Browser" src="/posts/tracing-with-the-management-libraries/media/8ac719fc-3c9f-4096-be5e-9457842371a2.png">
  </a> 
</p>
<p>The Cloud Context abstraction makes it simple to dial in a custom tracing interceptor. In my WPF application code, shown below, I simply use the <strong>Configuration.Tracing.AddTracingInterceptor</strong>  method to add a new instance of my interceptor
  to the context. </p>
<p>
  <a href="http://www.bradygaster.com/posts/files/4ce1fa01-314b-48fc-87e6-3d84d70fa8df.png">
    <img alt="Hooking in the tracing interceptor" src="/posts/tracing-with-the-management-libraries/media/d473b6e6-64df-4145-9ebb-d87a6adb8d06.png">
  </a> 
</p>
<p>Now, each call I make to the Azure API via the management libraries will be dumped to the multi-line text box in my WPF form. The screen shot below is the WPF application running. In this example, I&#x2019;ve made a call to the management API&#x2019;s list locations
  method, which returns a listing of all of the regions supported by the Azure fabric. </p>
<p>
  <a href="http://www.bradygaster.com/posts/files/0f14869a-be54-42a6-9724-f26707d47d0b.png">
    <img alt="Example code running" src="/posts/tracing-with-the-management-libraries/media/5693e963-ecdf-499a-a737-fd1c9b9f9199.png">
  </a> 
</p>
<p>Since the tracing interceptor is bound to the cloud context, <em>any </em> calls made through the management libraries will be traced and dropped into the text box (or whatever you specify in your own implementation). We think this adds a nice, one-stop
  method of tracing your API conversations. Keep watching this space for more blog posts about the management libraries. My next post, which will be released this week, will cover the code in this WPF application responsible for loading up a list of subscriptions
  from a publish settings file, and how the information in the publish settings file can be used to hydrate an X509 Certificate at run-time (note the &#x201C;Select Publish Settings File&#x201D; UX elements in this example). </p>
