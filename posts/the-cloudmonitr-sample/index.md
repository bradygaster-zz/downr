---
title: The CloudMonitR Sample
slug: the-cloudmonitr-sample
author: bradygaster
lastModified: 2012-10-01 23:01:27
pubDate: 2012-10-05 18:15:11
categories: Azure,SignalR
---

<p>The next Azure code sample released by the Azure Evangelism Team is the CloudMonitR sample. This code sample demonstrates how Azure Cloud Services can be instrumented and their performance analyzed in real time using a
  <a href="http://signalr.net/">SignalR</a>  Hub residing in a web site hosted with Azure Web Sites. Using
  <a href="http://twitter.github.com/bootstrap/">Twitter Bootstrap</a>  and
  <a href="http://highcharts.com/">Highcharts</a>  JavaScript charting components, the web site provides a real-time view of performance counter data and trace output. This blog post will introduce the CloudMonitR sample and give you some links to obtain it. </p>
Overview
<p>Last week I had the pleasure of travelling to Stockholm, Sweden to speak at a great community-run conference,
  <a href="http://www.azureug.se/CloudBurst2012/">CloudBurst 2012</a>  (as well as a few other events, which will be covered in a future post <em>very very soon</em> ). I decided to release a new Azure code sample at the conference, and to use the opportunity to walk through the architecture and implementation
  of the sample with the participants. As promised during that event, this is the blog post discussing the CloudMonitR sample, which you can obtain either as a ZIP file download from the
  <a href="http://code.msdn.microsoft.com/CloudMonitR-6e224501">MSDN Code Gallery</a>  or directly from its
  <a href="https://github.com/WindowsAzure-Samples/CloudMonitR">GitHub.com repository</a> . </p>
<p>Below, you&#x2019;ll see a screen shot of CloudMonitR in action, charting and tracing away on a running Azure Worker Role. </p>
<p>
  <img alt="35[1]" src="media/35%5B1%5D_3.png"> </p>
<p>The architecture of the CloudMonitR sample is similar to a previous sample I recently blogged about, the
  <a href="http://www.bradygaster.com/sitemonitr">SiteMonitR</a>  sample. Both samples demonstrate how
  <a href="http://signalr.net/">SignalR</a>  can be used to connect Azure Cloud Services to web sites (and back again), and both sites use
  <a href="http://twitter.github.com/bootstrap/">Twitter Bootstrap</a>  on the client to make the GUI simple to develop and customizable via CSS. </p>
<p>The point of CloudMonitR, however, is to allow for simplistic performance analysis of single- or multiple-instance Cloud Services. The slide below is from the
  <a href="http://bradystorage.blob.core.windows.net/decks/CloudMonitR.pptx">CloudBurst presentation deck</a>, and shows a very high-level overview of the architecture. </p>
<p>
  <a href="/Media/Default/WindowsLiveWriter/TheCloudMonitRSample_DF44/CloudMonitR_4.png">
    <img alt="CloudMonitR" src="media/CloudMonitR_thumb_1.png">
  </a> 
</p>
<p>As each instance of the Worker (or Web) Role you wish to analyze comes online, it makes an outbound connection to the SignalR Hub running in a Azure Web Site. Roles communicate with the Hub to send up tracing information and performance counter data to
  be charted using the
  <a href="http://highcharts.com/">Highcharts</a>  JavaScript API. Likewise, user interaction initiated on the Azure Web Sites-hosted dashboard to do things like add additional performance counters to observe (or to delete ones no longer needed on the dashboard) is communicated back to
  SignalR Hub. Performance counters selected for observation are stored in a Azure Table Storage table, and retrieved as the dashboard is loaded into a browser. </p>
Available via NuGet, Too!
<p>The
  <a href="https://nuget.org/packages/CloudMonitR">CloudMonitR</a>  solution is also available as a pair of NuGet packages. The first of these packages, the simply-named CloudMonitR package, is the one you&#x2019;d want to pull down to reference from a Web or Worker Role for which you need the metrics and trace
  reporting functionality. Referencing this package will give you everything you need to start reporting the performance counter and tracing data from within your Roles. </p>
<p>The
  <a href="https://nuget.org/packages/CloudMonitR.Web">CloudMonitR.Web</a>  package, on the other hand, won&#x2019;t bring down a ton of binaries, but will instead provide you with the CSS, HTML, JavaScript, and a few image files required to run the CloudMonitR dashboard in any ASP.NET web site. </p>
