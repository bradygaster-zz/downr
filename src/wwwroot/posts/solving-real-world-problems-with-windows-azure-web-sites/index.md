---
title: Solving Real-world Problems with Azure Web Sites
slug: solving-real-world-problems-with-windows-azure-web-sites
author: bradygaster
lastModified: 2014-10-09 22:33:40
pubDate: 2013-02-25 16:53:48
categories: Azure
---

<p>I&#x2019;ve been asked a lot of great questions about Azure Web Sites since the feature launched in June. Things like on-premise integration, connecting to service bus, and having multiple environments (like staging, production, etc), are all great questions
  that arise on a pretty regular cadence. With this post, I&#x2019;m going to kick off a series on solving real-world problems for web site and PaaS owners that will try to address a lot of these questions and concerns. I&#x2019;ve got a few blog posts in the hopper
  that will address some of these questions, rather than just cover how certain things are done. Those posts are great and all (and a lot of fun to write), but they don&#x2019;t answer some real-world, practical questions I&#x2019;ve been asked this year. Stay tuned
  to this area of my site, as I&#x2019;ll be posting these articles over the next few weeks and probably into the new year. As I post each of these solutions I&#x2019;ll update this post so you have a one-stop shop to go to when you need to solve one of these problems.</p>
Posts in this Series
<p>
  <a>Multiple Environments with Azure Web Sites</a> 
  <br>In this post I demonstrate how to have production and staging sites set up for your web site so that you can test your changes in a sandbox site before pushing your production site and potentially causing damage to it (and your reputation). If you&#x2019;ve
  wondered how to gate your deployments using Azure Web Sites, this is a good place to start. You&#x2019;ll learn how to use Azure Web Sites with a GitHub.com repository and some creative branching strategies to maintain multiple environments for a site.</p>
<p>
  <a>Managing Multiple Azure Web Site Environments using Visual Studio Publishing Profiles</a> 
  <br>This post takes the same sort of scenario as presented in the first article. Rather than use GitHub.com as a means to executing a series of gated environment deployments it focuses on the awesome features within Visual Studio for web publishing. Specifically,
  you&#x2019;ll see how to use publishing profiles to deploy to multiple Azure Web Sites, so that a team of peers responsible for releasing a site can do so without ever needing to leave Visual Studio.</p>
<p>This post also takes a look at the idea of release management and how this solution answers the question of doing proper release management with a cloud-hosted web site. If you&#x2019;ve wondered how your SDLC could fit in the idea of continuously maintaining
  a series of environments for gating your releases using Visual Studio&#x2019;s super-simple publishing features, this is a great place to start.</p>
<p>
  <a>Connecting Azure Web Sites to On-Premises Databases Using Azure Service Bus</a> 
  <br>This post introduces the idea creating a hybrid cloud setup using a Azure Web Site and the Azure Service Bus, to demonstrate how a web site hosted in Azure can connect to your on-premises enterprise database. If you&#x2019;ve been wondering how to save data
  from your Azure Web Site into your local database but didn&#x2019;t know how to do it, or if you&#x2019;re thinking of taking baby steps in your move toward cloud computing, this post could provide some good guidance on how to get started.</p>
<p>
  <a>Continuous Delivery to Azure Web Sites using TeamCity</a> 
  <br>My good friend Magnus put together a very extensive and informative blog post on using Azure Web Sites with
  <a>TeamCity</a> . If you&apos;re a CI user, or a TeamCity user, you&apos;ll want to check this out, as it is a great recipe for implementing your CI builds against Azure Web Sites. Magnus worked really hard on this blog post and started working on it for his Windows
  AzureConf talk, and I&apos;m proud to see how it came together for him.</p>
<p>Jim O&apos;Neil&apos;s Blog Series on Integrating Azure Web Sites and Notifications
  <br>Jim&apos;s blog series -
  <a>part 1</a>,
  <a>part 2</a>, and
  <a>part 3</a>  - are great if you&apos;re looking into implementing instant messaging with your Azure Web Site. This series is great, and I feel it shows off some amazing potential and adds a whole new dimension to what&apos;s possible on the platform. Take a look
  at these posts, they&apos;re quite inspiring.</p>
<p>
  <a>G. Andrew Duthie&#x2019;s Series on Back-end Data Services with Windows 8 Applications</a> 
  <br>The
  <a>@devhammer</a>  himself has created an awesome series on putting your data in the cloud, so your Windows 8 applications have a common place from which to pull the data. In this series, he discusses how to use Azure Web Sites to create an API that can
  be called by a Windows 8 application. If you&#x2019;re delving into Windows 8 development and have been wondering how you could use Web Sites to create the APIs you&#x2019;ll be calling, this series is a must-read.</p>
<p>
  <a>Maarten Balliauw on Configuring IIS Methods for ASP.NET Web API with Azure Web Sites</a> 
  <br>My good friend and dedicated MVP Maarten just wrote up a great post on configuring IIS within Azure Web Sites to support additional HTTP methods like HEAD and PATCH using configuration files. If you&#x2019;re trying to do some deeper Web API functionality
  and have struggled with getting these types of HTTP methods supported, this could solve your problem.</p>
<p>
  <a>Branches, Team Foundation Services, and Azure Web Sites</a> 
  <br>Wouter de Kort covers how to achieve the multiple-environment approach using Team Foundation Services online. If you&apos;re using TFS and you want to set up multiple branches that deploy to a variety of web sites, this article will help you get going. This
  is a great introduction to using TFS for multiple branches with Azure Web Sites.&#xA0;</p>
