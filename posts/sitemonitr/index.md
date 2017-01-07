---
title: The SiteMonitR Sample
slug: sitemonitr
author: bradygaster
lastModified: 2014-02-25 05:15:57
pubDate: 2012-10-05 18:15:11
categories: Azure,SignalR
---

<p>The newest sample from the Azure Evangelism Team (WAET) is a real-time, browser-based web site monitor. The SiteMonitR front-end is blocked out and styled using
  <a href="http://twitter.github.com/bootstrap/">Twitter Bootstrap</a>, and
  <a href="http://knockoutjs.com/">Knockout.js</a>  was used to provide MVVM functionality. A cloud service pings sites on an interval (10 seconds by default, configurable in the worker&#x2019;s settings) and notifies the web client of the sites&#x2019; up-or-down statuses via server-side SignalR conversations.
  Those conversations are then bubbled up to the browser using client-side SignalR conversations. The client also fires off SignalR calls to the cloud service to manage the storage functionality for the URL&#x2019;s to be monitored. If you&#x2019;ve been looking for
  a practical way to use SignalR with Azure, this sample could shed some light on what&#x2019;s possible.</p>
Architectural Overview
<p>The diagram below walks through the various method calls exposed by the SiteMonitR SignalR Hub. This Hub is accessed by both the HTML5 client application and by the Cloud Service&#x2019;s Worker Role code. Since SignalR supports both JavaScript and Native .NET
  client connectivity (as well as a series of other platforms and operating systems), both ends of the application can communicate with one another in an asynchronous fashion.</p>
<p>
  <a href="/Media/Default/WindowsLiveWriter/TheSiteMoni_143A5/SiteMonitR-Diagram.png">
    <img alt="SiteMonitR Architectural Diagram" src="media/SiteMonitR-Diagram_thumb.png">
  </a> 
</p>
<p>Each tier makes a simple request, then some work happens. Once the work is complete, the caller can call events that are handled by the opposite end of the communication. As the Cloud Service observes sites go up and down, it sends a message to the web
  site via the Hub indicating the site&#x2019;s status. The moment the messages are received, the Hub turns around and fires events that are handled on the HTML5 layer via the SignalR jQuery plug-in. Given the new signatures and additional methods added in
  <a href="http://blogs.msdn.com/b/webdev/archive/2012/08/22/announcing-the-release-of-signalr-0-5-3.aspx">SignalR 0.5.3</a>, the functionality is not only identical in how it behaves, the syntax to make it happen in both native .NET code and JavaScript are almost identical, as well. The result is a simple GUI offering a real-time view into any number of
    web sites&apos; statuses all right within a web browser. Since the GUI is written using Twitter Bootstrap and HTML5 conventions, it degrades gracefully, performing great on mobile devices.</p>
Where You Can Get SiteMonitR
<p>As with all the other samples released by the WAET, the SiteMonitR source code is available for download on the
  <a href="http://code.msdn.microsoft.com/SiteMonitR-dd4fcf77">MSDN Code Gallery site</a> . You can view or clone the source code in the
  <a href="https://github.com/WindowsAzure-Samples/SiteMonitR">GitHub.com repository</a>  we set up for the SiteMonitR source. Should you find any changes or improvements you&#x2019;d like to see, feel free to submit a pull request, too. Finally, if you find anything wrong with the sample submit an issue via
  <a href="https://github.com/WindowsAzure-Samples/SiteMonitR/issues">GitHub&#x2019;s issue tab</a>, and we&#x2019;ll do what we can to fix the issues reported. The GitHub.com repository contains a
  <a href="https://github.com/WindowsAzure-Samples/SiteMonitR/blob/master/GettingStarted.md">Getting Started document</a>  that walks you through the whole process &#x2013; with screen shots &#x2013; of setting up the SiteMonitR live in your very own Azure subscription (if you don&#x2019;t have one, get a free 90-day trial
  <a href="http://bit.ly/windowsazuretrial">here</a> ).</p>
Demonstration Video
<p>Finally, the video below walks you through the process of downloading, configuring, and deploying the SiteMonitR to Azure. In less than 10 minutes you&#x2019;ll see the entire process, have your very own web site monitoring solution running in the cloud, and
  you&#x2019;ll be confident you&#x2019;ll be the first to know when any of your sites crash since you&#x2019;ll see their statuses change in real-time. If the video doesn&apos;t load properly for you, feel free to
  <a href="http://channel9.msdn.com/posts/SiteMonitR-Sample">head on over to the Channel 9 post containing the video</a> .</p>
<p>
  
    
  
</p>
Updates
<p>A few days after the SiteMonitR sample was released,
  <a href="https://twitter.com/woloski">Matias Wolowski</a>  added in some awesome functionality. Specifically, he added in the ability for users to add
  <a href="http://phantomjs.org/">PhantomJS</a>  scripts that can be executed dynamically when sites statuses are received.
  <a href="https://github.com/woloski/SiteMonitR">Check out his fork of the SiteMonitR repository on GitHub.com</a> . I&apos;ll be reviewing the code changes over the next few days to determine if the changes are low-impact enough that they can be pulled into the main repository, but the changes Matias made
  are awesome and demonstrate how any of the Azure Evangelism Team&apos;s samples can be extended by the community. Great work, Matias!&#xA0;</p>
