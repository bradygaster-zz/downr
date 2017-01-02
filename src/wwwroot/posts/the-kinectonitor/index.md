---
title: The Kinectonitor
slug: the-kinectonitor
author: bradygaster
lastModified: 2012-10-05 18:15:12
pubDate: 2012-10-05 18:15:09
categories: Azure,Kinect,SignalR
---

<p>Suppose you had a some scary-looking hoodlum walking around your house when you were out? You&#x2019;d want to know about it, wouldn&#x2019;t you? Take one Kinect, mix in a little Azure Service Bus, sprinkle in some SignalR, and mix it all together with some elbow
  grease, and you could watch in near-real-time as sinewy folks romp through your living room. Here&#x2019;s how.</p>
<p>You might not be there (or want to be there) when some maniac breaks in, but it&#x2019;d be great to have a series of photographs with the dude&#x2019;s face to aid the authorities in their search for your home stereo equipment. The video below is a demonstration of
  the code this post will dive into in more detail. I figured it&#x2019;d give some context to the problem this article will be trying to solve.</p>
<p>
  <iframe height="315" src="http://www.youtube.com/embed/UfGOR1Eg_qQ" frameborder="0" width="560" allowfullscreen></iframe>
</p>
<blockquote>
  <p>I&#x2019;d really like to have a web page where I could go to see what&#x2019;s going on in my living room when I&#x2019;m not there. I know that fancy Kinect I picked up my kids for their XBox can do that sort of thing and I know how to code some .NET. Is it possible to
    make something at home that&#x2019;d give me this sort of thing?</p>
</blockquote>
<p>Good news! It isn&#x2019;t that difficult. To start with, take a look at the Kinectonitor Visual Studio solution below.</p>
<p>
  <a>
    <img alt="image" src="/posts/the-kinectonitor/media/image_thumb_1.png">
  </a> 
</p>
<p>At a high level it&#x2019;ll provide the following high-level functions. The monitor application will watch over a room. When a skeleton is detected in the viewing range, a photograph will be taken using the Kinect camera. The image will be published to the
  Azure Service Bus, using a Topic publish/subscribe approach. An MVC web site will subscribe to the Azure Service Bus topic, and whenever the subscriber receives a ping from the service bus with a new image taken by the Kinect, it will use a SignalR
  hub to update an HTML client with the new photo. Here&apos;s a high-level architectural diagram of how the whole thing works, end-to-end.&#xA0;</p>
<p>
  <img src="/posts/the-kinectonitor/media/SignalR-Kinect-Azure-Security-System-Diagram.png" alt="">
</p>
The Kinectonitor Core Project
<p>Within the core project will exist a few common areas of functionality. The idea behind the core project is to provide a domain structure, functional abstraction, and initial implementation of the image publication concept. For all intents and purposes,
  the Kinect will be publishing messages containing image data to the Azure Service Bus, and allow subscribers (which we&#x2019;ll get to in a moment) their own autonomy. The <em>ImageMessage</em>  class below illustrates the message that&#x2019;ll be transmitted through
  the Azure Service Bus.</p>
<p>
  <a>
    <img alt="image" src="/posts/the-kinectonitor/media/image_thumb_2.png">
  </a> 
</p>
<p>A high-level abstraction will be needed to represent consumption of image messages coming from the Azure cloud. The purpose of the <em>IImageMessageProcessor </em> service is to receive messages from the cloud and to then notify it&#x2019;s own listeners that
  an image has been received.</p>
<p>
  <a>
    <img alt="image" src="/posts/the-kinectonitor/media/image_thumb_3.png">
  </a> 
</p>
<p>A simple implementation is needed to receive image messages and to notify observers when they&#x2019;re received. This implementation will allow the SignalR hub, which we&#x2019;ll look at next, to get updates from the service bus; this abstraction and implementation
  are the custom glue that binds the service bus subscriber to the web site.</p>
<p>
  <a>
    <img alt="image" src="/posts/the-kinectonitor/media/image_thumb_4.png">
  </a> 
</p>
<p>Next up is the MVC web site, in which the SignalR hub is hosted and served up to users in an HTML client.</p>

Wiring Up a SignalR Hub
<p>Just before we dive into the MVC code itself, take a quick look at the solution again and note the <em>ServiceBusSimplifier </em> project. This is a super na&#xEF;ve, demo-class wrapper around the Azure Service Bus that was inspired by the
  <a>far-more-complete implementation Joe Feser shares on GitHub</a> . I used Joe&#x2019;s library to get started with Azure Service Bus and really liked some of his hooks, but his implementation was overkill for my needs so I borrowed some of his ideas in a tinier
  implementation. If you&#x2019;re deep into Azure Service Bus, though, you should totally look into Joe&#x2019;s code.</p>
<p>
  <a>
    <img alt="image" src="/posts/the-kinectonitor/media/image_thumb_5.png">
  </a> 
</p>
<p>Within the <em>ServiceBusSimplifier </em> project is a class that provides a Fluent wrapper abstraction around the most basic Azure Service Bus publish/subscribe concepts. The <em>ServiceBus </em> class (which could probably stand be renamed) is below,
  but collapsed. The idea is <em>just to get the idea </em> of how this abstraction is going to simplify things from here on out. I&#x2019;ll post a link to download the source code for this article in a later section. For now, just understand that the projects
  will be using this abstraction to streamline development and form a convention around the Azure Service Bus usage.</p>
<p>
  <a>
    <img alt="image" src="/posts/the-kinectonitor/media/image_thumb_6.png">
  </a> 
</p>
<p>A few calls are going to be made in calls to the <em>ServiceBus.Setup </em> method, specifically to provide Azure Service Bus authentication details. The classes that represent this sort of thing are below.</p>
<p>
  <a>
    <img alt="image" src="/posts/the-kinectonitor/media/image_thumb_7.png">
  </a> 
</p>
<p>Now that we&#x2019;ve covered the shared code that the MVC site and WPF/Kinect app will use to communicate via the Azure Service Bus, let&#x2019;s keep rolling and see how the MVC site is connected to the cloud using this library.</p>
<p>In this prototype code, the Global.asax.cs file is edited. A property is added to the web application to expose an instance of the <em>MockImageMessageProcessor</em>, (a more complete implementation would probably make use of an IoC container to store
  an instance of the service) for use later on in the SignalR Hub. Once the service instance is created, the Azure Service Bus wrapper is created and the <em>ImageMessage</em>  messages are subscribed to by the site <em>MessageProcessor </em> instance&#x2019;s
  <em>Process </em> method.</p>
<p>
  <a>
    <img alt="image" src="/posts/the-kinectonitor/media/image_thumb_8.png">
  </a> 
</p>
<p>When the application starts up, the instance is created and shared with the web site&#x2019;s server-side code. The SignalR Hub, then, can make use of that service implementation. The SignalR Hub listens for <em>ImageReceived </em> events coming from the service.
  Whenever the Hub handles the event, it turns around and notifies the clients connected to it that a new photo has arrived.</p>
<p>
  <a>
    <img alt="image" src="/posts/the-kinectonitor/media/image_thumb_9.png">
  </a> 
</p>
<p>With the Hub created, a simple Index view (and controller action) will provide the user-facing side of the Kinectonitor. The HTML/JQuery code below demonstrates how the client responds when messages arrive. There isn&#x2019;t much to this part, really. The code
  just changes the <em>src </em> attribute of an <em>img </em> element in the HTML document, then fades the image in using JQuery sugar.</p>
<p>
  <a>
    <img alt="image" src="/posts/the-kinectonitor/media/image_thumb_10.png">
  </a> 
</p>
<p>Now that the web client code has been created, we&#x2019;ll take a quick look at the Kinect code that captures the images and transmits them to the service bus.</p>
The Kinectonitor Monitor WPF Client
<p>Most of the Kinect-interfacing code comes straight from the samples available with the
  <a>Kinect SDK download</a> . The main points to be looked at in the examination of the WPF client is to see how it publishes the image messages into the Azure cloud.</p>
<p>The XAML code for the main form of the WPF app is about as dirt-simple as you could get. It just needs a way to display the image being taken by the Kinect and the skeletal diagram (the code available from the Kinect SDK samples). The XAML for this sample
  client app is below.</p>
<p>
  <a>
    <img alt="image" src="/posts/the-kinectonitor/media/image_thumb_11.png">
  </a> 
</p>
<p>When the WPF client opens, the first step is, of course, to connect to the Kinect device and the Azure Service Bus. The <em>OnLoad </em> event handler below is how this work is done. Note that this code also instantiates a <em>Timer</em>  instance. That
  timer will be used to control the delay between photographs, and will be looked at in a moment.</p>
<p>
  <a>
    <img alt="image" src="/posts/the-kinectonitor/media/image_thumb_13.png">
  </a> 
</p>
<p>Whenever image data is collected from the camera it&#x2019;ll be displayed in the WPF Image control shown earlier. The <em>OnKinectVideoReady </em> handler method below is where the image processing/display takes place. Take note of the highlighted area; this
  code sets an instance of a <em>BitmapSource </em> object, which will be used to persist the image data to disk later.</p>
<p>
  <a>
    <img alt="image" src="/posts/the-kinectonitor/media/image_thumb_14.png">
  </a> 
</p>
<p>Each time the Kinect video image is processed a new <em>BitmapSource </em> instance is created. Remember the <em>Timer </em> instance from the class earlier? That timer&#x2019;s handler method is where the image data is saved to disk and transmitted to the cloud.
  Note the check being performed on the <em>AreSkeletonsBeingTracked </em> property. That&#x2019;s the last thing that&#x2019;ll be looked at next, that&#x2019;ll tie the WPF functionality together.</p>
<p>
  <a>
    <img alt="image" src="/posts/the-kinectonitor/media/image_thumb_15.png">
  </a> 
</p>
<p>If the Kinectonitor WPF client just continually took snapshots and sent them into the Azure cloud the eventual price would probably be prohibitive. The idea behind a monitor like this, really, though, is to show only when people enter during unexpected
  times. So, the WPF client will watch for skeletons using the built-in Kinect skeleton tracking functionality (and code from the Kinect SDK samples). If a skeleton is being tracked, we know someone&#x2019;s in the room and that a photo should be taken. Given
  that the skeleton might be continually tracked for a few seconds (or minutes, or longer), the Kinect will continue to photograph while a skeleton is being tracked. As soon as the tracking stops, a final photo is taken, too. The code that sets the <em>AreSkeletonsBeingTracked </em> property
  value during the skeleton-ready event handler is below.</p>
<p>
  <a>
    <img alt="image" src="/posts/the-kinectonitor/media/image_thumb_16.png">
  </a> 
</p>
<p>Some logic occurs during the setter of the <em>AreSkeletonsBeingTracked </em> method, just to sound the alarms whenever a skeleton is tracked, without having to wait the typical few seconds until the next timer tick.</p>
<p>
  <a>
    <img alt="image" src="/posts/the-kinectonitor/media/image_thumb_17.png">
  </a> 
</p>
<p>That&#x2019;s it for the code! One more note &#x2013; it helps if the Kinect is up high or in front of a room (or both). During development of this article I just placed mine on top of the fridge, which is next to the kitchen bar where I do a lot of work. It could
  see the whole room pretty well and picked up skeletons rather quickly. Just a note for your environment testing phase.</p>
<p>
  <a>
    <img alt="IMAG0356" src="/posts/the-kinectonitor/media/IMAG0356_thumb.jpg">
  </a> 
</p>

Summary
<p>This article took a few of the more recent techniques and tools to be released to .NET developers. With a little creativity and some time, it&#x2019;s not difficult to use those components to make something pretty neat at home in the physical computing world.
  Pairing these disciplines up to create something new (or something old someway different) is great fodder for larger projects using the same technologies later.</p>
<p>
  <a>If you&#x2019;d like to view the Kinectonitor GitHub source code, it&#x2019;s right here.</a> 
</p>
