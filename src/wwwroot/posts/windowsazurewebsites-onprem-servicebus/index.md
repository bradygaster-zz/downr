---
title: Connecting Azure Web Sites to On-Premises Databases Using Azure Service Bus
slug: windowsazurewebsites-onprem-servicebus
author: bradygaster
lastModified: 2015-02-17 17:02:19
pubDate: 2012-11-26 17:21:58
categories: Azure
---

<p>The third post in the
  <a href="http://www.bradygaster.com/solving-real-world-problems-with-windows-azure-web-sites">Solving Real-world Problems with Azure Web Sites blog series</a>  I&#x2019;ll demonstrate one manner in which a web site can be connected to an on-premises enterprise. A common use-case for a web site is to collect data for storage in a database in an enterprise
  environment. Likewise, the first thing most customers want to move into the cloud is their web site. Ironically, the idea of moving a whole enterprise architecture into the cloud can appear to be a daunting task. So, if one wants to host their site
  in the cloud but keep their data in their enterprise, what&#x2019;s the solution? This post will address that question and point out how the Azure Service Bus between a Azure Web Site and an on-premises database can be a great glue between your web site and
  your enterprise.</p>
<blockquote>
  <p>We have this application that&#x2019;d be great as a cloud-based web application, but we&#x2019;re not ready to move the database into the cloud. We have a few other applications that talk to the database into which our web application will need to save data, but
    we&#x2019;re not ready to move everything yet. Is there any way we could get the site running in Azure Web Sites but have it save data back to our enterprise? Or do we have to move everything or nothing works?</p>
</blockquote>
<p>I get this question quite frequently when showing off Azure Web Sites. People know the depth of what&#x2019;s possible with Azure,&#xA0; but they don&#x2019;t want to have to know everything there is to know about Azure just to have a web site online. More importantly,
  most of these folks have learned that Azure Web Sites makes it dirt simple to get their ASP.NET, PHP, Node.js, or Python web site hosted into Azure. Azure Web Sites provides a great starting point for most web applications, but the block on adoption
  comes when the first few options are laid out, similar to these:</p>
<ul>
  <li>It is costing us increased money and time to manage our web sites so we&#x2019;re considering moving them into Azure Web Sites</li>
  <li>It will it cost us a lot of money to move our database into the cloud</li>
  <li>We don&#x2019;t have the money to rework all the applications that use that database if moving it to the cloud means those applications have to change.</li>
  <li>We can&#x2019;t lose any data, and the enterprise data must be as secure as it already was, at the very least.</li>
  <li>Our web sites need to scale and we can&#x2019;t get them what we need unless we go to Azure Web Sites.</li>
</ul>
<p>This is a common plight and question whenever I attend conferences. So, I chose to take one of these conversations as a starting point. I invented a customer situation, but one that emulates the above problem statement and prioritizations associated with
  the project we have on the table.</p>
Solving the Problem using Azure Service Bus
<p>The first thing I needed to think about when I was brought this problem would be the sample scenario. I needed to come up with something realistic, a problem I had seen customers already experiencing. Here&#x2019;s a high-level diagram of the idea in place.
  There&#x2019;s not much to it, really, just a few simple steps.</p>
<p>
  <a href="/Media/Default/Windows-Live-Writer/Connecting-Windows-Azure-Web-Sites-to-On_13D7D/on-prem-data-flow_2.png">
    <img alt="on-prem-data-flow" src="/posts/windowsazurewebsites-onprem-servicebus/media/on-prem-data-flow_thumb.png">
  </a> 
</p>
<p>In this diagram I point out how my web site will send data over to the Service Bus. Below the service bus layer is a Console EXE application that subscribes to the Service Bus Topic the web application will be publishing into the enterprise for persistence.
  That console EXE will then use Entity Framework to persist the incoming objects &#x2013; Customer class instances, in fact &#x2013; into a SQL Server database.</p>
<p>The subscription internal process is also exploded in this diagram. The Console EXE waits for any instance of a <em>customer</em>  being thrown in, and it wakes up any process that&#x2019;s supposed to handle the incoming instance of that object.</p>
<p>The Console EXE that runs allows the program to subscribe to the Service Bus Topic. When customers land on the topics from other applications, the app wakes up and knows what to do to process those customers. In this first case, the handler component
  basically persists the data to a SQL Server installation on their enterprise.</p>
Code Summary and Walk-through
<p>This example code consists of three projects, and is
  <a href="https://github.com/bradygaster/WebSitesAndOnPremWithServiceBus">all available for your perusal as a GitHub.com repository</a> . The first of these projects is a simple MVC web site, the second is console application. The final project is a core project that gives these two projects a common language via a domain
  object and a few helper classes. Realistically, the solution could be divided into 4 projects; the core project could be divided into 2 projects, one being the service bus utilities and the other being the on-premises data access code. For the purposes
  of this demonstration, though, the common core project approach was good enough. The diagram below shows how these projects are organized and how they depend on one another.</p>
<p>
  <a href="/Media/Default/Windows-Live-Writer/Connecting-Windows-Azure-Web-Sites-to-On_13D7D/on-prem-project-structure_2.png">
    <img alt="on-prem-project-structure" src="/posts/windowsazurewebsites-onprem-servicebus/media/on-prem-project-structure_thumb.png">
  </a> 
</p>
<p>The overall idea is simple -&#xA0; build a web site that collects customer information in a form hosted in a Azure Web Site, then ship that data off to the on-premises database via the Azure Service Bus for storage. The first thing I created was a domain object
  to represent the customers the site will need to save. This domain object serves as a contract between both sides of the application, and will be used by an Entity Framework context class structure in the on-premises environment to save data to a SQL
  Server database.</p>
<p></p>
<p>With the domain language represented by the <em>Customer</em>  class I&#x2019;ll need some Entity Framework classes running in my enterprise to save customer instances to the SQL database. The classes that perform this functionality are below. They&#x2019;re not too
  rich in terms of functionality or business logic implementation, they&#x2019;re just in the application&#x2019;s architecture to perform CRUD operations via Entity Framework, given a particular domain object (the <em>Customer</em>  class, in this case).</p>
<p></p>
<p>This next class is sort of the most complicated spot in the application if you&#x2019;ve never done much with the Azure Service Bus. The good thing is, if you don&#x2019;t <em>want </em> to learn how to do a lot with the internals of Service Bus, this class could be
  reused in your own application code to provide a quick-and-dirty first step towards using Service Bus.</p>
<p>The <em>ServiceBusHelper</em>  class below basically provides a utilitarian method of allowing for both the publishing and subscribing features of Service Bus. I&#x2019;ll use this class on the web site side to publish Customer instances into the Service Bus,
  and I&#x2019;ll also use it in my enterprise application code to subscribe to and read messages from the Service Bus whenever they come in. The code in this utility class is <em>far from perfect</em>, but it should give me a good starting point for publishing
  and subscribing to the Service Bus to connect the dots.</p>
<p></p>
<p>Now that the Service Bus helper is there to deal with both ends of the conversation I can tie the two ends together pretty easily. The web site&#x2019;s code won&#x2019;t be too complicated. I&#x2019;ll create a view that site users can use to input their customer data. Obviously,
  this is a short form for demonstration purposes, but it could be any shape or size you want (within reason, of course, but you&#x2019;ve got ample room if you&#x2019;re just working with serialized objects).</p>
<p></p>
<p>If I&#x2019;ve got an MVC view, chances are I&#x2019;ll need an MVC action method to drive that view. The code for the <em>HomeController</em>  class is below. Note, the <em>Index</em>  action is repeated &#x2013; one to display the form to the user, the second to handle the
  form&#x2019;s posting. The data is collected via the second <em>Index</em>  action method, and then passed into the Service Bus.</p>
<p></p>
<p>The final piece of code to get this all working is to write the console application that runs in my enterprise environment. The code below is all that&#x2019;s needed to do this; when the application starts up it subscribes to the Service Bus topic and starts
  listening for incoming objects. Whenever they come in, the code then makes use of the Entity Framework classes to persist the <em>Customer </em> instance to the SQL Server database.</p>
<p></p>
<p>Now that the code&#x2019;s all written, I&#x2019;ll walk you through the process of creating your Service Bus topic using the Azure portal. A few configuration changes will need to be made to the web site and the on-premise console application, too, but the hard part
  is definitely over.</p>
Create the Service Bus Topic
<p>Creating a Service Bus topic using the Azure portal is relatively painless. The first step is to use the <strong>New</strong>  menu in the portal to create the actual Service Bus topic. The screen shot below, from the portal, demonstrates the single step
  I need to take to create my own namespace in the Azure Service Bus.</p>
<p>
  <a href="/Media/Default/Windows-Live-Writer/Connecting-Windows-Azure-Web-Sites-to-On_13D7D/creating-service-bus-topic_2.png">
    <img alt="creating-service-bus-topic" src="/posts/windowsazurewebsites-onprem-servicebus/media/creating-service-bus-topic_thumb.png">
  </a> 
</p>
<p>Once I click the <strong>Create a New Topic</strong>  button, the Azure portal will run off and create my very own area within the Service Bus. The process won&#x2019;t take long, but while the Service Bus namespace is being created, the portal will make sure
  I know it hasn&#x2019;t forgotten about me.</p>
<p>
  <a href="/Media/Default/Windows-Live-Writer/Connecting-Windows-Azure-Web-Sites-to-On_13D7D/service-%20bus-getting-created_2.png">
    <img alt="service- bus-getting-created" src="/posts/windowsazurewebsites-onprem-servicebus/media/service-%20bus-getting-created_thumb.png">
  </a> 
</p>
<p>After a few seconds, the namespace will be visible in the Azure portal. If I select the new namespace and click the button at the bottom of the portal labeled <strong>Access Key</strong>, a dialog will open that shows me the connection string I&#x2019;ll need
  to use to connect to the Service Bus.</p>
<p>
  <a href="/Media/Default/Windows-Live-Writer/Connecting-Windows-Azure-Web-Sites-to-On_13D7D/image_2.png">
    <img alt="image" src="/posts/windowsazurewebsites-onprem-servicebus/media/image_thumb.png">
  </a> 
</p>
<p>I&#x2019;ll copy that connection string out of the dialog. Then, I&#x2019;ll paste that connection string into the appropriate place in the <em>Web.config</em>  file of the web application. The screen shot below shows the <em>Web.config </em> file from the project, with
  the appropriate <em>appSettings</em>  node highlighted.</p>
<p>
  <a href="/Media/Default/Windows-Live-Writer/Connecting-Windows-Azure-Web-Sites-to-On_13D7D/image_4.png">
    <img alt="image" src="/posts/windowsazurewebsites-onprem-servicebus/media/image_thumb_1.png">
  </a> 
</p>
<p>A similar node also needs to be configured in the console application&#x2019;s <em>App.config </em> file, as shown below.</p>
<p>
  <a href="/Media/Default/Windows-Live-Writer/Connecting-Windows-Azure-Web-Sites-to-On_13D7D/image_6.png">
    <img alt="image" src="/posts/windowsazurewebsites-onprem-servicebus/media/image_thumb_2.png">
  </a> 
</p>
<p>In all, there are only two <em>*.config</em>  files that need to be edited in the solution to get this working &#x2013; the console application&#x2019;s <em>App.config</em>  file and the web application&#x2019;s <em>Web.config</em>  file. Both of these files are highlighted
  in the solution explorer view of the solution included with this blog post.</p>
<p>
  <a href="/Media/Default/Windows-Live-Writer/Connecting-Windows-Azure-Web-Sites-to-On_13D7D/image_8.png">
    <img alt="image" src="/posts/windowsazurewebsites-onprem-servicebus/media/image_thumb_3.png">
  </a> 
</p>
<p>With the applications configured properly and the service bus namespace created, I can now run the application to see how things work.</p>
Running the Code
<p>Since I&#x2019;ll be using Entity Framework to scaffold the SQL Server database on the fly, all I&#x2019;ll need to do to set up my local enterprise environment is to create a new database. The screen shot below shows my new SQL database before running the console
  application on my machine. Note, there are no tables or objects in the database yet.</p>
<p>
  <a href="/Media/Default/Windows-Live-Writer/Connecting-Windows-Azure-Web-Sites-to-On_13D7D/image_12.png">
    <img alt="image" src="/posts/windowsazurewebsites-onprem-servicebus/media/image_thumb_5.png">
  </a> 
</p>
<p>The first thing I&#x2019;ll do to get running is to debug the console application in Visual Studio. I could just hit F5, but I&#x2019;ll be running the web application in debug mode next. The idea here is to go ahead and fire up the console application so that it can
  create the database objects and prepare my enterprise for incoming messages.</p>
<p>
  <a href="/Media/Default/Windows-Live-Writer/Connecting-Windows-Azure-Web-Sites-to-On_13D7D/image_14.png">
    <img alt="image" src="/posts/windowsazurewebsites-onprem-servicebus/media/image_thumb_6.png">
  </a> 
</p>
<p>The&#xA0; console application will open up, but will display no messages until it begins processing Customer objects that land on the Service Bus. To send it some messages, I&#x2019;ll now debug the web application, while leaving the console application running locally.</p>
<p>
  <a href="/Media/Default/Windows-Live-Writer/Connecting-Windows-Azure-Web-Sites-to-On_13D7D/image_16.png">
    <img alt="image" src="/posts/windowsazurewebsites-onprem-servicebus/media/image_thumb_7.png">
  </a> 
</p>
<p>When the web site fires up and opens in my web browser, I&#x2019;ll be presented the simple form used to collect customer data. If I fill out that form and click the <strong>Save</strong>  button, the data will be sent into the Service Bus.</p>
<p>
  <a href="/Media/Default/Windows-Live-Writer/Connecting-Windows-Azure-Web-Sites-to-On_13D7D/image_18.png">
    <img alt="image" src="/posts/windowsazurewebsites-onprem-servicebus/media/image_thumb_8.png">
  </a> 
</p>
<p>By leaving the console application running as I submit the form, I can see the data coming into my enterprise environment.</p>
<p>
  <a href="/Media/Default/Windows-Live-Writer/Connecting-Windows-Azure-Web-Sites-to-On_13D7D/SNAGHTMLcceb1f2.png">
    <img alt="SNAGHTMLcceb1f2" src="/posts/windowsazurewebsites-onprem-servicebus/media/SNAGHTMLcceb1f2_thumb.png">
  </a> 
</p>
<p>Going back into SQL Server Management Studio and refreshing the list of tables I can see that the Entity Framework migrations ran&#xA0; perfectly, and created the table into which the data will be saved. If I select the data out of that table using a SQL query,
  I can verify that indeed, the data was persisted into my on-premises database.</p>
<p>
  <a href="/Media/Default/Windows-Live-Writer/Connecting-Windows-Azure-Web-Sites-to-On_13D7D/SNAGHTMLcd1045e.png">
    <img alt="SNAGHTMLcd1045e" src="/posts/windowsazurewebsites-onprem-servicebus/media/SNAGHTMLcd1045e_thumb.png">
  </a> 
</p>
<p>At this point, I&#x2019;ve successfully pushed data from my Azure Web Site up into the Service Bus, and then back down again into my local enterprise database.</p>
Summary
<p>One of the big questions I&#x2019;ve gotten from the community since the introduction of Azure Web Sites to the Azure platform is on how to connect these sites to an enterprise environment. Many customers aren&#x2019;t ready to move their whole enterprise into Azure,
  but they want to take some steps towards getting their applications into the cloud. This sort of <em>hybrid cloud</em>  setup is one that&#x2019;s perfect for Service Bus. As you&#x2019;ve seen in this demonstration, the process of connecting a Azure Web Site to your
  on-premises enterprise isn&#x2019;t difficult, and it allows you the option of moving individual pieces as you&#x2019;re ready. Getting started is easy, cheap, and will allow for infinite scaling opportunities. Find out how easy Azure can be for Web Sites, mobile
  applications, or hybrid situations such as this by getting a
  <a href="http://bit.ly/windowsazuretrial">free trial account today</a> . I&#x2019;m sure you&#x2019;ll see that pretty much anything&#x2019;s possible with Azure.</p>
