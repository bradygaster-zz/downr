---
title: Azure Web Sites Log Cleaner
slug: windows-azure-web-sites-log-cleaner
author: bradygaster
lastModified: 2012-10-05 18:15:16
pubDate: 2012-10-05 18:15:11
categories: Azure
---

<p>Azure Web Sites customers who run web sites in shared instance mode have a generous amount of storage space to use for multiple web sites. The 1GB limit for free customers is more than enough to get a site up and running with lots of room left over for
  storage. A site&#x2019;s log files contribute to the amount of space being used by a site. If a site has a lot of traffic, the log files could grow and eat into the space available for the important assets for a site &#x2013; the HTML, executables, images, and other
  relevant assets. That&#x2019;s the sole purpose for the Azure
  <a>Web Sites Log Cleaner</a>, a
  <a>NuGet</a>  utility you can grab and use in your site with little impact or work.</p>
Potential Impacts of Not Backing Up Logs
<p>Log analysis is important. It can show all sorts of things, from how much traffic you have to each section of your site, or if you&#x2019;re getting strange requests that indicate some sort of intrusion. If your logs are important to you, there are some important
  things to remember about Azure Web Sites:</p>
<ul>
  <li>Once the logs reach 35mb in size, logging will be disabled</li>
  <li>If a site&#x2019;s logging functions are toggled off, it could take a few minutes for the logging to re-activate (or deactivate). So if logging is turned off because you exceeded 35mb, then you realize it and clean out your log files, it might take a few seconds
    for the service to start logging again, resulting in visitors you don&#x2019;t capture in your logs</li>
  <li>If your site starts to spew unhandled exceptions, the failed request and exception logging logs will reflect extensive information about each request, on a file-per-request basis. If the log count &#x2013; as in <em>number of files &#x2013; </em> exceeds 50 for either
    of these types of logs, the logging service for the type of log exceeding 50 (failed request or exception logging, specifically) will be disabled.</li>
</ul>
<p>&#xA0;</p>
<p>For these reasons, it&#x2019;s very helpful to set up a utility such as this to automatically backup and clean out your log files. This utility makes that a snap, and since you can fire it via the execution of a simple URL, there are countless scheduling options,
  which I introduce later on in this post. For now, let&#x2019;s take a look at how to get started.</p>
How to Set it Up
<p>Given the convenient features made available via the NuGet team, the installation of this package will make a few changes to your Web.config file. An appSetting node will be added to your file, and an HTTP handler will be registered. The handler, which
  by default answers requests at the URL <strong>/CleanTheLogs.axd</strong>, will zip up your log files in a flattened ZIP file, then send that ZIP file into Azure Blob Storage in a conveniently-named file for as long as you&#x2019;ll need it and delete the
  files from your sites&#x2019; storage space. All you need to do is to hit the URL, and magic happens.</p>

<p>All you need to do is to create a storage account using the Azure Web Sites portal and to put the account name and key into the your Web.config file. The NuGet package adds placeholders to the file to point out what you&#x2019;ll need to add on your own. Once
  it&#x2019;s configured, you could set up a scheduled task to hit the URL on intervals, and you back up your logs and keep your sites tidy.</p>
<p>
  <a>
    <img alt="Placeholders in Web.config" src="/posts/windows-azure-web-sites-log-cleaner/media/web-config_thumb.png">
  </a> 
</p>
<p>Sounds too easy? If you have any issues or want a little more detailed explanation of exactly what happens, check out the video below. It&#x2019;ll walk you through the whole process, from creating a site to peeking at the downloaded ZIP file.</p>
<p>
  
    
  
</p>
Scheduling
<p>There are a multitude of methods you could use to schedule the call to the URL and execute the log cleanup process. You could script a Powershell cmdlet to do it, write some C# code, or use a separate Cron Job service to run the task routinely for you.
  Nathan Totten wrote a
  <a>great blog post on how to achieve task scheduling using a Cron Job service</a> . Whatever your choice, one call to a URL does the entire job of saving and clearing your logs.</p>
<p>I can&#x2019;t guide you on how often you should schedule your logs; each site is different. The guarantee is, if you put a site out there, you&#x2019;ll get some traffic even if it&#x2019;s only robot or search engine traffic, and with traffic comes log data. If you&#x2019;re getting
  a lot of traffic it stands to reason you could reach the 35mb limit in very little time. With a scheduled job like the one Nathan demonstrates in his post, you could set the log backup URL to be called as often as every 10 minutes.</p>
Summary
<p>You&#x2019;ve got a lot more room in Azure Storage space than you do in your web site folders, and you can use that space to store your logs permanently without using up your web site space. Hopefully this utility will solve that problem for you. If you have
  any issues setting it up, submit a comment below or
  <a>find me on Twitter</a> .</p>
Update
<p>The Azure Web Sites Log Cleaner source code has been published into a
  <a>GitHub.com public repository</a> . If you want to see how it works, or you have an interest in changing the functionality in some way, feel free to clone the source code repository.&#xA0;</p>
