---
title: The Azure Service Bus Simplifier
slug: service-bus-simplifier
author: bradygaster
lastModified: 2014-03-14 11:31:51
pubDate: 2012-10-05 18:15:09
categories: Azure
---

<p>One of the things that makes enterprise service development of any type difficult is the requirement to learn that ESB&#x2019;s programming model. The Azure has a very simple programming model already, but for those developers getting started with Azure Service
  Bus programming for the first time who mainly want a simple publish/subscribe-style bus architecture limited to a few types of custom messages, I&#x2019;ve created a NuGet package called the <em>ServiceBusSimplifier. </em> </p>
<p>If all you need from the Azure service bus is a simple implementation of the pub/sub model there&#x2019;s no longer a need to learn all the plumbing and deeper detail work. Of course, there&#x2019;s nothing<em> wrong </em> with learning the plumbing and doing so is
  encouraged, but for those just getting started with Azure&#x2019;s Topic-based messaging, the <em>ServiceBusSimplifier </em> package might be just the answer. It provides extremely basic access to publishing and subscribing custom messages through the Azure
  service bus. The video below demonstrates how to use the package, and there is some detailed usage instruction below the video.</p>
<iframe height="315" src="http://www.youtube.com/embed/E_XBHxgxDnE" frameborder="0" width="420" allowfullscreen></iframe>
Usage and Source Code
<p>If you&#x2019;re just trying to use the package to simplify your entry into using Azure&#x2019;s service bus, just pull down the
  <a>package from NuGet</a> . </p>
<p>
  <a>
    <img alt="image" src="/posts/service-bus-simplifier/media/image_thumb_2.png">
  </a> 
  <br>
  <br>If you&#x2019;d like to peruse the source code, which the remaining sections of this post will take a slightly deeper dive into an element at a time or you&#x2019;d like to add functionality to it, the code is available as a
  <a>GitHub repository</a> .</p>
Setting up the Connection
<p>To set up the service bus connection, call the <em>Setup </em> method, passing it an instance of the class the package uses to supply authentication and connectivity information to the service bus, the <em><a>InitializationRequest</a>  </em> class.
  The <em>ServiceBus</em>  abstraction class offers a Fluent interface, so the methods could be chained together if need be. </p>
<p>
  <a>
    <img alt="image" src="/posts/service-bus-simplifier/media/image_thumb_3.png">
  </a> 
</p>
Subscribing
<p>As mentioned earlier, the <em>ServiceBus</em>  abstraction offers very simple usage via self-documenting methods. The code below has been augmented to subscribe to an instance of a custom class. </p>
<p>
  <a>
    <img alt="image" src="/posts/service-bus-simplifier/media/image_thumb_4.png">
  </a> 
</p>
<p>Note, there are no special requirements or inheritance chain necessary for a class to be passed around within this implementation. The class below is the one being used in this example, and in the GitHub repository. </p>
<p>
  <a>
    <img alt="image" src="/posts/service-bus-simplifier/media/image_thumb_5.png">
  </a> 
</p>
<p>Finally, here&#x2019;s the <em>HandleSimpleMessage </em> method from the program class. Note, this could have been passed as an anonymous method rather than a pointer to a class member or static member. The video demonstration above shows such a usage, but it&#x2019;s
  important to note that either a static, instance, or anonymous method would be appropriate being passed to the <em>Subscribe</em>  method. </p>
<p>
  <a>
    <img alt="image" src="/posts/service-bus-simplifier/media/image_thumb_6.png">
  </a> 
</p>
Publishing
<p>The final piece of this demonstration involves publishing messages into the Azure service bus. The code below shows how to publish a message to the bus, using the self-explanatory <em>Publish </em> method</p>
<p>
  <a>
    <img alt="image" src="/posts/service-bus-simplifier/media/image_thumb_7.png">
  </a> 
</p>
<p>Hopefully, the <em>ServiceBusSimplifier </em> package will ease your development experience with the Azure service bus. Even though the Azure service bus is dirt-simple to use, this handy utility library will give your code 1-line access to efficient publish/subscribe
  mechanisms built into the cloud. Happy coding!</p>
