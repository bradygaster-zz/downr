---
title: Complex Type Messaging with ActiveMQ and .NET
slug: complex-type-messaging-with-activemq-and-.net
author: bradygaster
lastModified: 2015-06-22 14:41:05
pubDate: 2012-10-05 18:15:02
categories: .NET
---

<p>
  <a>Mark Bloodworth</a>  wrote a few blogs on the art of using ActiveMQ within .NET. Those posts are the origination for my ability to produce this post, so thanks, Mark. I&#x2019;d highly advise going to read Mark&#x2019;s
  <a>introductory post on the topic</a>  first. I&#x2019;ll paraphrase a good deal of Mark&#x2019;s post in this one so look to him for the details and deeper explanation, he&#x2019;s the man.</p>
<p>Mark&#x2019;s post shows at a high level how to pass simple string messages via ActiveMQ. This post will answer the question of passing more complicated types via ActiveMQ so that you can use strongly-typed instances as your messages.</p>
<h3>
  <strong>Getting Started</strong> 
</h3>
<p>It sounds a bit scary, especially if you&#x2019;re not akin to using non-MSMQ messaging engines. Don&#x2019;t freak out, ActiveMQ actually quite easy. I&#x2019;m a Java troglodyte, so if I can do this, <em>you can do this. </em> </p>
<ol>
  <li>
    <a>Download ActiveMQ</a>  (as of this post the current release was 5.5.0)
    <br>It&#x2019;ll come as a ZIP file. I just unzipped mine to my root drive, so C:\ActiveMQ</li>
  <li>
    <a>Download Spring.NET</a> 
    <br>I placed mine again, in my system root, at C:\SpringNet</li>
  <li>In the ActiveMQ\conf you&#x2019;ll find the file activemq.xml, and in that file you&#x2019;ll need to make sure you set the file as displayed in the screenshot below. This is explained in the comments of Mark&#x2019;s post in more detail, but for now, just trust me.
    <br>
    <br>
    <img alt="image" src="/posts/complex-type-messaging-with-activemq-and-.net/media/image_3.png">
    <br>
    <br>
  </li>
  <li>Go to a dos prompt, cd into the ActiveMQ\bin directory, and type &#x201C;activemq&#x201D; (without the quotes). You&#x2019;ll see a window like that below open and you&#x2019;ll need to find a line similar in nature to the one highlighted. Again, there&#x2019;s some detail in Mark&#x2019;s
    post on this we won&#x2019;t delve into at this point.
    <br>
    <br>
    <img alt="image" src="/posts/complex-type-messaging-with-activemq-and-.net/media/image_6.png">
  </li>
  <li>That&#x2019;s it! We&#x2019;re ready to code! Most of the code is again, inspired &#x2013; okay, mimics &#x2013; Mark&#x2019;s examples. Our point here is to pass complex message types via ActiveMQ, so we&#x2019;ll do a few things slightly differently. I&#x2019;ll also take a few na&#xEF;ve steps and use
    generic implementations for publishing and subscribing to messages. </li>
</ol>
<h3>Ontology</h3>
<p>Both sides of the conversation will need to have context. I&#x2019;ll also throw in a strings class to mitigate fat-fingering [and potentially configuration of some sort later on]. The message class and utility class are below.</p>
<p>
  <img alt="image" src="/posts/complex-type-messaging-with-activemq-and-.net/media/image_9.png">
</p>
<h3>Subscription</h3>
<p>With ActiveMQ running we&#x2019;re now ready to subscribe to a queue in which messages of the <em>SampleRequest </em> type will be sent. As mentioned earlier the first thing needed is a listener. The main difference in this example and in Mark&#x2019;s post is that
  this code expects for the messages to be Object Messages and not Simple messages, as Mark&#x2019;s example was for primarily passing string messages. Below is the generic listener, which basically expects the body of a particular message to be of the type
  specified in the generic argument.</p>
<p>
  <img alt="image" src="/posts/complex-type-messaging-with-activemq-and-.net/media/image_14.png">
</p>
<p>Next is the program that creates an instance of the listener (and the queue if it doesn&#x2019;t already exist in the ActiveMQ installation). The listener is created and bound to a ActiveMQ connection. Again, more detail in other places, for now the idea is,
  we&#x2019;re going to listen for messages. The program code below begins watching the ActiveMQ specified in the <em>Strings.Destination</em>  property at the URL specified in the <em>Strings.Url </em> property.</p>
<p>
  <img alt="image" src="/posts/complex-type-messaging-with-activemq-and-.net/media/image_15.png">
</p>
<h3>Publication</h3>
<p>With the subscriber listening for messages within ActiveMQ the next application will need to send messages into the queue. As with the subscription side the publication side will rely on a generic approach to publishing messages into ActiveMQ queues.</p>
<p>
  <img alt="image" src="/posts/complex-type-messaging-with-activemq-and-.net/media/image_18.png">
</p>
<p>The program code will take input from the user. Then it will use that input as the value of the <em>SampleRequest.Message </em> property and will set the <em>SampleRequest.ClientMachine </em> property to the client computer&#x2019;s machine name.</p>
<p>
  <img alt="image" src="/posts/complex-type-messaging-with-activemq-and-.net/media/image_21.png">
</p>
<p>When both the projects included in the accompanying download are executed the results are instantaneous.</p>
<p>
  <img alt="image" src="/posts/complex-type-messaging-with-activemq-and-.net/media/image_28.png">
</p>
<h3>Durability</h3>
<p>To prove the durability of the ActiveMQ layer during debug mode, stop the subscription application and use the publication client to send a few more messages.</p>
<p>
  <img alt="image" src="/posts/complex-type-messaging-with-activemq-and-.net/media/image_29.png">
</p>
<p>Then, stop the publication client. Once the subscription client is re-started, the messages sent into the ActiveMQ while the subscription application was shut down are collected and processed. The screenshot below shows how the later messages sent when
  the subscription app wasn&#x2019;t running are processed as soon as it is re-started. Then, the publication app sends in two more messages and the new instance of the subscription app continues processing them as they arrive.</p>
<p>
  <img alt="image" src="/posts/complex-type-messaging-with-activemq-and-.net/media/image_30.png">
</p>
<p>Happy coding!</p>
