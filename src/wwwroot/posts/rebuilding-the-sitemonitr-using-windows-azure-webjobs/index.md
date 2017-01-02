---
title: Rebuilding the SiteMonitR using Azure WebJobs SDK
slug: rebuilding-the-sitemonitr-using-windows-azure-webjobs
author: 
lastModified: 2014-10-30 13:55:16
pubDate: 2014-03-02 08:37:13
categories: Azure
---

<p>Some time back, I created the
  <a>SiteMonitR</a> 
  <a>sample</a>  to demonstrate how SignalR could be used to tie together a Web Site and a Cloud Service. Since then Azure has evolved quite rapidly, as have the ASP.NET Web API and SignalR areas. One recent addition to the arsenal of tools available to application
  developers in Azure is
  <a>WebJobs</a> . Similar to the traditional Worker Role, a WebJob allows for the continuous or triggered execution of program logic. The main difference between a Worker Role and a WebJob is that the latter runs not in the context of a separate Cloud Service,
  but as a resource of a Web Site. WebJobs also simplify development of these routine middleware programs, too, since the only requirement on the developer is to reference the
  <a>WebJobs NuGet package</a> . Developers can write basic console applications with methods that, when decorated with properties resident in the WebJobs SDK, will execute at appropriate times or on schedules. You can learn more about the basics of WebJobs
  via the
  <a>introductory article</a>, the
  <a>WebJobs SDK documentation</a>, or from
  <a>Hanselman&#x2019;s blog post</a>  on the topic. </p>
<p>In this blog post, I&#x2019;m going to concentrate on how I&#x2019;ve used WebJobs and some of my other favorite technologies together to re-create the SiteMonitR sample. I&#x2019;ve forked the original GitHub repository into my own account to provide you with access to the
  <a>new SiteMonitR code</a> . Once I wrap up this post I&#x2019;ll also update the
  <a>MSDN Code Sample for SiteMonitR</a>, so if you prefer a raw download you&#x2019;ll have that capability. I worked pretty closely with
  <a>Pranav Rastogi</a>,
  <a>Mike Stall</a>  and the rest of the WebJobs team as I worked through this re-engineering process. They&#x2019;ve also recorded
  <a>an episode of Web Camps TV on the topic</a>, so check that out if you&#x2019;re interested in more details. Finally,
  <a>Sayed</a>  and
  <a>Mads</a>  have developed a
  <a>prototype of some tooling features that could make developing and deploying WebJobs</a>  easier. Take a look at this extension and give us all feedback on it, as we&#x2019;re trying to conceptualize the best way to surface WebJobs tooling and we&#x2019;d love to have
  your input on how to make the whole process easier.&#xA0; </p>
Application Overview
<p>SiteMonitR is a very simple application written for a very simple situation. I wanted to know the status of my web sites during a period where my [non-cloud] previous hosting provider wasn&#x2019;t doing so well with keeping my sites live. I wrote this app up
  and had a monitor dedicated to it, and since then the app has served as proving ground for each new wave of technology I&#x2019;d like to learn. This implementation of the application obviously makes use of WebJobs to queue up various points in the work flow
  of site-monitoring and logging. During this workflow the code also updates a SignalR-enabled dashboard to provide constant, real-time visibility into the health of a list of sites. The workflow diagram below represents how messages move throughout SiteMonitR.
  </p>
<p>
  <a>
    <img alt="01" src="/posts/rebuilding-the-sitemonitr-using-windows-azure-webjobs/media/42c46012-2c29-495a-973b-aca3d1805fed.png">
  </a> 
</p>
<p>The goal for the UX was to keep it elegant and simple. Bootstrap made this easy in the last version of the application, and given the newest release of Boostrap, the templates Pranav and his team made available to us in Visual Studio 2013, it seemed like
  a logical choice for this new version of SiteMonitoR. I didn&#x2019;t change much from the last release aside from making it even <em>more </em> simple than before. I&#x2019;ve been listening to the team, and the community, rave about
  <a>AngularJS</a>  but I&#x2019;d not
  <strike>had</strike> made the time to learn it, so this seemed like a great opportunity. I found I really love AngularJS from reworking this app from Knockout in the previous version. I&#x2019;ve tried a lot of JavaScript frameworks, and I&#x2019;m pretty comfortable saying
  that right now I&#x2019;m in love with AngularJS. It really is simple, and fun to use. </p>
<p>
  <a>
    <img alt="06" src="/posts/rebuilding-the-sitemonitr-using-windows-azure-webjobs/media/3f37cc46-2e17-4886-8149-e10e0b7a2d80.png">
  </a> 
</p>
<p>The entire solution is comprised of 4 projects. Two of these projects are basic console applications I use as program logic for the WebJobs. One is [duh] a Web Application, and the last is a Common project that has things like helper methods, constants,
  and configuration code. Pretty basic structure, that when augmented with a few NuGet packages and some elbow grease, makes for a relatively small set of code to have to understand. </p>
<p>
  <a>
    <img alt="02" src="/posts/rebuilding-the-sitemonitr-using-windows-azure-webjobs/media/e417ca6a-3c1f-4599-bbb3-eb95cc7539d8.png">
  </a> 
</p>
<p>With a quick high-level introduction to the application out of the way, I&#x2019;ll introduce the WebJob methods, and walk through the code and how it works. </p>
Code for Harnessing WebJobs
<p>One of the goals for WebJobs was to provide ASP.NET developers a method of reaching in and doing things with Azure&#x2019;s storage features without requiring developers to learn much about how Azure storage actually works. The team&#x2019;s architects thought (rightfully)
  that via the provision of convenience attributes that referred to abstract use-cases facilitated in many common Azure scenarios, more developers would have the basic functionality they need to use storage without actually needing to learn how to use
  storage. Sometimes the requirement of having to learn a new API to use a feature mitigates that feature&#x2019;s usefulness (I know, it sounds crazy, right?). </p>
<p>So, Mike and Pranav and the rest of the team came up with a series of attributes that are explained pretty effectively in their SDK documentation. I&#x2019;m going to teach via demonstration here, so let&#x2019;s just dive in and look at the first method. This method,
  <strong>CheckSitesFunction</strong>, lives in the executable code for a WebJob that will be executed <strong>on a schedule. </strong> Whenever the scheduler service wakes up this particular job, the method below will execute with two parameters being
  passed in. The first parameter references a table of <strong>SiteRecord</strong>  objects, the second is a storage queue into which the code will send messages. </p>
<p>
  <a>
    <img alt="07" src="/posts/rebuilding-the-sitemonitr-using-windows-azure-webjobs/media/fe129dc9-1f97-472a-95a1-3a1ee1a83fa6.png">
  </a> 
</p>
<p>You could&#x2019;ve probably guessed what I&#x2019;m going to do next. Iterate over all the records in the table, grab the URL of the site that needs to be pinged, then ping it and send the results to a storage queue. The <strong>out </strong> parameter in this method
  is actually a queue itself. So the variable <strong>resultList</strong>  below is literally going to represent the list of messages I&#x2019;m planning on sending into that storage queue. </p>
<p>Now, if you&#x2019;re obsessive like me you&#x2019;ll probably have that extra monitor set up just to keep tabs on all your sites, but that&#x2019;s not the point of this WebJob. As the code executes, I&#x2019;m also going to call out to the Web API controller in the web site via
  the <strong>UpdateDashboard</strong>  method. I&#x2019;ll cover that in more detail later, but that&#x2019;s mainly to provide the user with real-time visibility into the health of the sites being checked. Realistically all that <em>really matters</em>  is the log
  data for the site health, which is why I&#x2019;m sending it to a queue to be processed. I don&#x2019;t want to slow down the iterative processing by needing to wait for the whole process so I queue it up and let some other process handle it. </p>
<p>
  <a>
    <img alt="08" src="/posts/rebuilding-the-sitemonitr-using-windows-azure-webjobs/media/8c846a60-8093-4990-9fdb-3edf252199fb.png">
  </a> 
</p>
<p>In addition to a scheduled WebJob, there&#x2019;s another one that will run on events. Specifically, these WebJobs will wake up whenever messages land into specific queues being observed by this WebJob. The method signatures with appropriately-decorated attributes
  specifying which queues to watch and tables to process, are shown in the code below. </p>
<p>
  <a>
    <img alt="09" src="/posts/rebuilding-the-sitemonitr-using-windows-azure-webjobs/media/cafcdd3c-1059-48c5-b9e8-5a63130817ae.png">
  </a> 
</p>
<p>One method in particular, <strong>AddSite, </strong> runs whenever a user event sends a message into a queue used to receive requests to add sites to the list of sites being watched. The user facilitates this use-case via the SiteMonitR dashboard, a message
  is sent to a queue containing a URL, and then, this method just <strong>wakes up and executes</strong> . Whenever a user sends a message containing a string URL value for the site they&#x2019;d like to monitor, the message is then saved to the storage table
  provided in the second parameter. As you can see from the method below <strong>there&#x2019;s no code that makes explicit use of the storage API or SDK</strong>, but rather, it&#x2019;s just an instance of an <strong>IDictionary</strong>  implementer to which I&#x2019;m
  just adding items. </p>
<p>
  <a>
    <img alt="10" src="/posts/rebuilding-the-sitemonitr-using-windows-azure-webjobs/media/58f576f9-0c21-4823-b494-eb903d52891e.png">
  </a> 
</p>
<p>The <strong>SaveSiteLogEntry</strong>  method below is similar to the <strong>AddSite</strong>  method. It has a pair of parameters. One of these parameters represents the incoming queue watched by this method, the second represents a table into which data
  will be stored. In this example, however, the first parameter isn&#x2019;t a primitive type, but rather a custom type I wrote within the SiteMonitR code. This variation shows the richness of the WebJob API; when methods land on this queue they can be deserialized
  into object instances of type <strong>SiteResult</strong>  that are then handled by this method. This is a lot easier than needing to write my own polling mechanism to sit between my code and the storage queue. The WebJob service takes care of that for
  me, and all I need to worry about is <strong>how I handle incoming messages</strong> . That removes a good bit of the ceremony of working with the storage SDK; of course, the trade-off is that I have little no no control over the inner workings of the
  storage functionality. </p>
<p>That&#x2019;s the beauty of it. In a lot of application code, the plumbing doesn&#x2019;t really matter in the beginning. All that matters is that all the pieces work together. </p>
<p>
  <a>
    <img alt="12" src="/posts/rebuilding-the-sitemonitr-using-windows-azure-webjobs/media/d452518b-7635-47ba-a981-246333e7ca05.png">
  </a> 
</p>
<p>Finally, there&#x2019;s one more function that deletes sites. This function, like the others, takes a first parameter decorated by the <strong>QueueInput</strong>  attribute to represent a queue that&#x2019;s being watched by the program. The final parameters in the
  method represent <strong>two different tables from which data will be deleted</strong> . First, the site record is deleted, then the logs for that site that&#x2019;ve been stored up are deleted. </p>
<p>
  <a>
    <img alt="11" src="/posts/rebuilding-the-sitemonitr-using-windows-azure-webjobs/media/0d7bdfc0-b763-4a5c-9678-031c182ec5d5.png">
  </a> 
</p>
The SiteMonitR Dashboard
<p>The UX of SiteMonitR is built using Web API, SignalR, and AngularJS. This section will walk through this part of the code and provide some visibility into how the real-time updates work, as well as how the CRUD functionality is exposed via a Web API Controller.
  This controller&#x2019;s add, delete, and list methods are shown below. Note, in this part of the code I&#x2019;ll actually be using the Storage SDK via a utility class resident in the Web Project. </p>
<p>
  <a>
    <img alt="14" src="/posts/rebuilding-the-sitemonitr-using-windows-azure-webjobs/media/06a3680b-1b90-4bc4-97be-4f9c3062b565.png">
  </a> 
</p>
<p>Remember the scheduled WebJob from the discussion earlier? During that section I mentioned the Web API side of the equation and how it would be used with SignalR to provide real-time updates to the dashboard <em>from </em> the WebJob code running in a
  process external to the web site itself. In essence, the WebJob programs simply make HTTP GET/POST calls over to the Web API side to the methods below. Both of these methods are pretty simple; they just hand off the information they obtained during
  the HTTP call from the WebJob up to the UX via bubbling up events through SignalR. </p>
<p>
  <a>
    <img alt="15" src="/posts/rebuilding-the-sitemonitr-using-windows-azure-webjobs/media/f1def7ba-7428-40fa-aac0-b54264271e4c.png">
  </a> 
</p>
<p>The SignalR Hub being called actually has no code in it. It&#x2019;s just a Hub the Web API uses to bubble events up to the UX in the browser. </p>
<p>
  <a>
    <img alt="16" src="/posts/rebuilding-the-sitemonitr-using-windows-azure-webjobs/media/17602576-fe90-4301-bf9a-19b6ca4038ec.png">
  </a> 
</p>
<p>The code the WebJobs use to call the Web API make use of a client I&#x2019;ll be investigating in more detail in the next few weeks. I&#x2019;ve been so busy in my own project work I&#x2019;ve had little time to keep up with some of the awesome updates coming from the Web
  API team recently. So I was excited to have time to tinker with the
  <a>Web API Client NuGet&#x2019;s latest release</a>, which makes it dirt simple to call out to a Web API from client code. In this case, my client code is running in the scheduled WebJob. The utility code the WebJob calls that then calls to the Web API Controller
  is below. </p>
<p>
  <a>
    <img alt="13" src="/posts/rebuilding-the-sitemonitr-using-windows-azure-webjobs/media/d4c3d5f1-289c-4083-a4bc-fe72036d19f9.png">
  </a> 
</p>
<p>As I mentioned, I&#x2019;m using AngularJS in the dashboard&#x2019;s HTML code. I love how similar AngularJS&#x2019;s templating is to Handlebars. I didn&#x2019;t have to learn a whole lot here, aside from how to use the <strong>ng-class</strong>  attribute with potential multiple
  values. The data-binding logic on that line defines the color of the box indicating the site&#x2019;s status. I love this syntax and how easy it is to update UX elements when models change using AngularJS. I don&#x2019;t think I&#x2019;ve ever had it so easy, and have really
  enjoyed the logical nature of AngularJS and how everything just seemed to work. </p>
<p>
  <a>
    <img alt="17" src="/posts/rebuilding-the-sitemonitr-using-windows-azure-webjobs/media/50388f7a-694d-4136-9579-0b96806cf331.png">
  </a> 
</p>
<p>Another thing that is nice with AngularJS is well-explained by
  <a>Ravi on his blog on a Better Way of Using ASP.NET SignalR with AngularJS</a> . Basically, since services in AngularJS are Singletons and since AngularJS has a few great ways it does injection, one can easily dial in a service that connects to the SignalR
  Hub. Then, this wrapper service can fire JavaScript methods that can be handled by Angular controllers on the client. This approach felt very much like DI/IoC acts I&#x2019;ve taken for granted in C# but never really used too much in JavaScript. The nature
  of service orientation in JavaScript and how things like abstracting a SignalR connection are really elegant when performed with AngularJS. The code below shows the service I inject into my Angular controller that does just this: it handles SignalR
  events and bubbles them up in the JavaScript code on the client. </p>
<p>
  <a>
    <img alt="18" src="/posts/rebuilding-the-sitemonitr-using-windows-azure-webjobs/media/8a62be04-80e6-4d63-9b4d-88df9638d247.png">
  </a> 
</p>
<p>Speaking of my AngularJS controller, here it is. This first code is how the Angular controller makes HTTP calls to the Web API running on the server side. Pretty simple. </p>
<p>
  <a>
    <img alt="19" src="/posts/rebuilding-the-sitemonitr-using-windows-azure-webjobs/media/84e64a76-9e28-463f-9952-b3be3df9f3fb.png">
  </a> 
</p>
<p>Here&#x2019;s how the Angular controller handles the events bubbled up from the Angular service that abstracts the SignalR connection back to the server. Whenever events fire from within that service they&#x2019;re handled by the controller methods, which re-bind the
  HTML UX and keep the user up-to-date in real-time on what&#x2019;s happening with their sites. </p>
<p>
  <a>
    <img alt="20" src="/posts/rebuilding-the-sitemonitr-using-windows-azure-webjobs/media/7a0b56be-ec1d-431d-9d2c-c15cdaf8a8eb.png">
  </a> 
</p>
<p>With that terse examination of the various moving parts of the SiteMonitR application code out of the way the time has come to learning how it can be deployed. </p>
Deploying SiteMonitR
<p>There are three steps in the deployment process for getting the site and the WebJobs running in Azure, and the whole set of code can be configured in one final step. The first step should be self-explanatory; the site needs to be published to Azure Web
  Sites. I won&#x2019;t go through the details of publishing a web site in this post. There are literally hundreds of resources out there on the topic, from Web Camps TV episodes to Sayed&#x2019;s blog, as well as demo videos on AzureConf, //build/ keynotes, and so
  on. Save it to say that <strong>publishing and remote debugging ASP.NET sites in Azure Web Sites is pretty simple</strong> . Just right-click, select Publish, and follow the steps. </p>
<p>
  <a>
    <img alt="21" src="/posts/rebuilding-the-sitemonitr-using-windows-azure-webjobs/media/1109e3ce-18ed-435e-a041-e28e1ea75885.png">
  </a> 
</p>
<p>Once the code is published the two WebJob executables need to be zipped up and deployed. First, I&#x2019;ll pop out of Visual Studio to the bin/Release folder of my event-driven WebJob. Then I&#x2019;ll select all the required files, right-click them, and zip them
  up into a single zip file. </p>
<p>
  <a>
    <img alt="25" src="/posts/rebuilding-the-sitemonitr-using-windows-azure-webjobs/media/93be4eec-aaac-4abd-8521-9b18582291c3.png">
  </a> 
</p>
<p>Then in the portal&#x2019;s widget for the SiteMonitR web site I&#x2019;ll click the WebJobs tab in the navigation bar to get started creating a new WebJob. </p>
<p>
  <a>
    <img alt="26" src="/posts/rebuilding-the-sitemonitr-using-windows-azure-webjobs/media/41df8d32-50cf-4b4e-88dc-16428dab7bc1.png">
  </a> 
</p>
<p>I&#x2019;ll give the WebJob a name, then select the zip file and specify how the WebJob should run. I&#x2019;ll select <strong>Run Continuously</strong>  for the event-driven WebJob. It has code in it that will halt the process in wait for incoming queue messages, so
  this selection will be adequate. </p>
<p>
  <a>
    <img alt="27" src="/posts/rebuilding-the-sitemonitr-using-windows-azure-webjobs/media/0f2f0c8f-32a3-4131-8c0f-1d9f6ea894c0.png">
  </a> 
</p>
<p>Next I&#x2019;ll zip up the output of the scheduled WebJob&#x2019;s code. </p>
<p>
  <a>
    <img alt="24" src="/posts/rebuilding-the-sitemonitr-using-windows-azure-webjobs/media/58cb7ed5-27f3-4fae-a77c-aff28cf4cdff.png">
  </a> 
</p>
<p>This time, when I&#x2019;m uploading the zip file containing the WebJob, I&#x2019;ll select the <strong>Run on a Schedule</strong>  option from the <strong>How to Run</strong>  drop-down. </p>
<p>
  <a>
    <img alt="29" src="/posts/rebuilding-the-sitemonitr-using-windows-azure-webjobs/media/1173947b-2e5a-4505-9815-b0e54b9b9154.png">
  </a> 
</p>
<p>Then I&#x2019;ll set up the schedule for my WebJob. In this example I&#x2019;m going to be a little obsessive and run the process to check my sites every fifteen minutes. So, every fifteen minutes the scheduled task, which is responsible for checking the sites, will
  wake up, check all the sites, and enqueue the results of each check so that the results can be logged. If a user were sitting on the dashboard they&#x2019;d observe this check happen every 15 minutes for each site. </p>
<p>
  <a>
    <img alt="30" src="/posts/rebuilding-the-sitemonitr-using-windows-azure-webjobs/media/9dfc343e-6110-4731-831b-684dff105df5.png">
  </a> 
</p>
<p>The two WebJobs are listed in the portal once they&#x2019;re created. Controls for executing manual or scheduled WebJobs as well as stopping those currently or continuously running appear at the bottom of the portal. </p>
<p>
  <a>
    <img alt="31" src="/posts/rebuilding-the-sitemonitr-using-windows-azure-webjobs/media/db2d5570-e033-4074-a0d0-455db20a21eb.png">
  </a> 
</p>
Configuration
<p>The final step in deployment (like always) is configuration. SiteMonitR was built for Azure, so it can be entirely configured from within the portal. The first step in configuring SiteMonitR is to create a Storage Account for it to use for storing data
  in tables and for the queue being used to send and receive messages. An existing storage account could be used, and each object created in the storage account is prefixed with the <strong>sitemonitr</strong>  phrase. That said, it could be better for
  some to have an isolated storage account. If so, create a storage account in the portal. </p>
<p>
  <a>
    <img alt="28" src="/posts/rebuilding-the-sitemonitr-using-windows-azure-webjobs/media/295fc7ec-dd69-4a31-bc8c-17281d933d23.png">
  </a> 
</p>
<p>Once the storage account has been created, copy the name and either primary or secondary keys so that you can build a storage account connection string. That connection string needs to be used, as does the URL of my site (in this case,
  <a>sitemonitr.azurewebsites.net</a>, which is in fact a live instance of this code) as connection strings and an appSetting (respectively). See below on how to configure these items right from within the Azure portal. </p>
<p>
  <a>
    <img alt="image" src="/posts/rebuilding-the-sitemonitr-using-windows-azure-webjobs/media/06a941cc-b688-48e3-9f7e-db590fb35830.png">
  </a> 
</p>
<p>Once these values are configured, the site should be ready to run. </p>
Running SiteMonitR
<p>To test it out, open the site in another browser instance, then go to the WebJobs tab of the site and select the scheduled task. Then, run it from within the portal and you should see the HTML UX reacting as the sites are checked and their status sent
  back to the dashboard from the WebJob code. </p>
<p>
  <a>
    <img alt="33" src="/posts/rebuilding-the-sitemonitr-using-windows-azure-webjobs/media/4985fdee-557d-4f51-99e8-e42ff9bf2ae3.png">
  </a> 
</p>
<p>Speaking of dashboards, the WebJobs feature itself has a nice dashboard I can use to check on the status of my WebJobs and their history. I&#x2019;ll click the <strong>Logs</strong>  link in the WebJobs tab to get to the dashboard. </p>
<p>
  <a>
    <img alt="35" src="/posts/rebuilding-the-sitemonitr-using-windows-azure-webjobs/media/b11ccc17-5155-4a09-9695-5959e4986ca4.png">
  </a> 
</p>
<p>The WebJobs Dashboard shows all of my jobs running, or the history for those that&#x2019;ve already executed. </p>
<p>
  <a>
    <img alt="34" src="/posts/rebuilding-the-sitemonitr-using-windows-azure-webjobs/media/9bb7092b-5d03-49e4-90fc-0361cc6b0b86.png">
  </a> 
</p>
Summary
<p>I&#x2019;m really enjoying my time spent with WebJobs up to this point. At the original time of this post, WebJobs was in the first Alpha release stage. so they&#x2019;re still pretty preview. I&#x2019;m seeing huge potential for WebJobs, where customers who have a small
  middle-tier or scheduled <em>thing </em> that needs to happen. In those cases a Worker Role could be quite overkill, so WebJobs is a great middle-of-the-road approach to giving web application owners and developers these sorts of scheduled or event-driven
  programs that enable a second or multiple tiers running. I&#x2019;ve really enjoyed playing with WebJobs, learning the SDK, and look forward to more interesting things coming out in this space. </p>
