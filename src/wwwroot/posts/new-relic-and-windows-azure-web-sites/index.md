---
title: New Relic and Azure Web Sites
slug: new-relic-and-windows-azure-web-sites
author: bradygaster
lastModified: 2013-07-12 21:43:02
pubDate: 2013-07-12 21:43:02
categories: Azure
---

<p>This past week I was able to attend the //build/ conference in San Francisco, and whilst at the conference I and some teammates and colleagues were invited to hang out with the awesome dudes from
  <a>New Relic</a> . To correspond with the Web Sites GA announcement this week, New Relic announced their
  <a>support for Azure Web Sites</a> . I wanted to share my experiences getting New Relic set up with my Orchard CMS blog, as it was surprisingly simple. I had it up and running in under 5 minutes, and promptly
  <a>tweeted my gratification</a> . </p>
<p>
  <a>Hanselman</a>  visited New Relic a few months ago and
  <a>blogged about how he instrumented his sites using New Relic</a>  in order to save money on compute resources. Now that I&#x2019;m using their product and really diving in I can&#x2019;t believe the wealth of information available to me, on an existing site, in seconds.</p>
<h3>FTP, Config, Done. </h3>
<p>Basically, it&#x2019;s all FTP and configuration. <em>Seriously</em> . I uploaded a directory, added some configuration settings using the Azure portal, Powershell Cmdlets, or Node.js CLI tools, and partied. There&#x2019;s
  <a>extensive documentation on setting up New Relic with Web Sites</a>  on their site that starts with a Quick Install process. </p>
<blockquote>
  <p>In the spirit of disclosure, when I set up my first MVC site with New Relic I didn&#x2019;t follow the instructions, and it didn&#x2019;t work quite right. One of New Relic&#x2019;s resident ninja,
    <a>Nick Floyd</a>, had given
    <a>Vstrator&#x2019;s</a> 
    <a>Rob Zelt</a>  and myself a demo the night before during the Hackathon. So I emailed Nick and was all <em>dude meet me at your booth</em>  and he was all <em>dude totally </em> so we like got totally together and he hooked me up with the ka-knowledge
    and stuff. I&#x2019;ll &#x2018;splain un momento. The point in my mentioning this? RT#M when you set this up and life will be a lot more pleasant. </p>
</blockquote>
<p>I don&#x2019;t need to go through the whole NuGet-pulling process, since I&#x2019;ve already got an active site running, specifically using
  <a>Orchard CMS</a> . Plus, I&#x2019;d already created a Visual Studio Web Project to follow Nick&#x2019;s instructions so I had the content items that the
  <a>New Relic Web Sites NuGet package</a>  imported when I installed it. </p>
<p>
  <a>
    <img alt="image" src="/posts/new-relic-and-windows-azure-web-sites/media/image_3.png">
  </a> 
</p>
<p>So, I just FTPed those files up to my blog&#x2019;s root directory. The screen shot below shows how I&#x2019;ve got a <strong>newrelic </strong> folder at the root of my site, with all of New Relic&#x2019;s dependencies and configuration files. </p>
<p>They&#x2019;ve made it <strong>so easy</strong>, I didn&#x2019;t even have to change any of the configuration before I uploaded it and the stuff just worked. </p>
<p>
  <a>
    <img alt="SNAGHTML425ffb" src="/posts/new-relic-and-windows-azure-web-sites/media/SNAGHTML425ffb_thumb.png">
  </a> 
</p>
<p>Earlier, I mentioned having had one small issue as a result of not reading the documentation. In spite of the fact that their docs say, pretty explicitly, to either use the portal or the Powershell/Node.js CLI tools, I&#x2019;d just added the settings to my
  Web.config file, as depicted in the screen shot below. </p>
<p>
  <a>
    <img alt="image" src="/posts/new-relic-and-windows-azure-web-sites/media/image_thumb_2.png">
  </a> 
</p>
<p>Since the ninjas at New Relic support non-.NET platforms too, they do expect those application settings to be set at a deeper level than the *.config file. New Relic needs these settings to be at the environment level. Luckily the soothsayer PM&#x2019;s on the
  Azure team predicted this sort of thing would happen, so when you use some other means of configuring your Web Site, Azure persists those settings at that deeper level. So don&#x2019;t do what I did, okay? Do the right thing. </p>
<p>Just to make sure <strong>you</strong>  see the right way. Take a look at this screen shot below, which I lifted from
  <a>the New Relic documentation</a>  tonight. It&#x2019;s the Powershell code you&#x2019;d need to run to automate the configuration of these settings. </p>
<p>
  <a>
    <img alt="image" src="/posts/new-relic-and-windows-azure-web-sites/media/image_thumb_3.png">
  </a> 
</p>
<p>Likewise, you could configure New Relic using the Azure portal. </p>
<p>
  <a>
    <img alt="image" src="/posts/new-relic-and-windows-azure-web-sites/media/image_thumb_4.png">
  </a> 
</p>
<blockquote>
  <p>Bottom line is this:</p>
  <ul>
    <li>If you just use the <em>Web.config</em>, it won&#x2019;t work
      </li><li>Once you light it up in the portal, it works like a champ</li>
  </ul>
</blockquote>
<h3>Deep Diving into Diagnostics</h3>
<p>Once I spent 2 minutes and got the monitoring activated on my site, it worked just fine. I was able to look right into what Orchard&#x2019;s doing all the way back to the database level. Below, you&#x2019;ll see a picture of the most basic monitoring page looks like
  when I log into New Relic. I can see a great snapshot of everything right away. </p>
<p>
  <a>
    <img alt="image" src="/posts/new-relic-and-windows-azure-web-sites/media/image_thumb_5.png">
  </a> 
</p>
<p>Where I&#x2019;m spending some time right now is on the Database tab in the New Relic console. I&#x2019;m walking through the SQL that&#x2019;s getting executed by Orchard against my SQL database, learning all sort of interesting stuff about what&#x2019;s fast, not-as-fast, and
  so on. </p>
<p>
  <a>
    <img alt="image" src="/posts/new-relic-and-windows-azure-web-sites/media/image_thumb_7.png">
  </a> 
</p>
<p>I can&#x2019;t tell you how impressed I was&#xA0; by the New Relic product when I first saw it, and how stoked I am that it&#x2019;s officially unveiled on Azure Web Sites. Now you can get deep visibility and metrics information about your web sites, just like what was
  available for Cloud Services prior to this week&#x2019;s release. </p>
<p>I&#x2019;ll have a few more of these blog posts coming out soon, maybe even a Channel 9 screencast to show part of the process of setting up New Relic. Feel free to sound off if there&#x2019;s anything on which you&#x2019;d like to see me focus. In the meantime, <strong>happy monitoring!</strong> </p>
