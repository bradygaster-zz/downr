---
title: Redirection with Azure Web Sites
slug: redirection-with-windows-azure-web-sites
author: bradygaster
lastModified: 2012-07-20 16:25:29
pubDate: 2012-10-05 18:15:10
categories: Azure
---

<p>Since the release of Azure Web Sites a few months ago, one of the main questions I&#x2019;m asked pertains to users&#x2019; ability to direct traffic to their Azure Web Site using a custom domain name. I&#x2019;ve found a way to achieve this (sort of). In this blog post you&#x2019;ll
  learn how you can use your DNS provider&#x2019;s administration panel to send traffic to your shared-instance Azure Web Site. </p>
<p>Now, I admit this isn&#x2019;t a perfect solution. I also apologize to the network guys out there, because this is kindergarten-level trickery. My SEO friends will cringe, too. The problem I&#x2019;m aiming to solve in this post, though, is in response to a question
  I had from a few people on Twitter and during demonstrations of the new portal&#x2019;s web site features. </p>
<blockquote>
  <p>
    This is great, but I can&#x2019;t use my domain names with it. If I can&#x2019;t do that and I can&#x2019;t do that while I&#x2019;m working on a site for a client, do I have to upgrade to a reserved instance right away or is there something I can do while in shared mode to
      get traffic using a custom domain name?
  </p>
</blockquote>
<p>
  <a href="/Media/Default/Windows-Live-Writer/a42a1a2b53f1_15164/doug-henning-spinning-fire_2.gif">
    <img alt="doug-henning-spinning-fire" src="/posts/redirection-with-windows-azure-web-sites/media/doug-henning-spinning-fire_thumb.gif">
  </a> Ideally, you could set up some sort of redirection (301 or otherwise) to get traffic to your site using your domain management tool of choice, get some traffic, and then make (or obtain) a small investment in upgrading to a reserved instance once things
  are up and running.
  <a href="http://bit.ly/wawsfreeoffer">Given the free opportunity Microsoft is offering</a>, you&#x2019;ve got 10 chances to build one site for free for a year. The chances at least 1/10th&#x2019;s of your web site ideas are going to make you enough to pay for a small reserved instance is pretty good,
  right? If not, or if you&#x2019;re cool with redirection and just have some silly sites or fun little ideas you want to show off, you can use this domain trick as long as you can stand knowing it&#x2019;s all really just a clever domain redirection <strong>illusion</strong> .
  </p>
<p>I&#x2019;m a user [and huge fan] of
  <a href="http://dnsimple.com/">DNSimple.com</a>  for domain management, so I&#x2019;ll be demonstrating this trick using their administration panel. Your domain management provider probably has something similar to DNSimple.com&#x2019;s <em>URL </em> record type (I think it&#x2019;s a 301 redirect under
  the hood, but don&#x2019;t quote me on that). The idea here is this &#x2013; DNSimple will send traffic that comes to <em>YourCustomDomain.com </em> to <em>YourCustomDomain.azurewebsites.net. </em> Let&#x2019;s set this up and get some traffic coming in!</p>
Create Your Own Azure Web Site
<p>First thing is your web site itself. Go to WindowsAzure.com and set up a free account to
  <a href="http://www.windowsazure.com/en-us/pricing/calculator/?scenario=web">give yourself a year of playtime with 10 free sites</a> . Once you&#x2019;ve logged into the portal, create a new web site. One with a database, use the application gallery, whatever you choose. This demo will allow for the creation of a simple site, but you&#x2019;ll
  then switch over to DNSimple&#x2019;s administration panel to set some DNS settings. In a few minutes you&#x2019;ll have a live site, and a live domain name, that directs traffic to your <em>shared-instance </em> Azure Web Site. </p>
<p>Once you&#x2019;re in the portal just select New, then Web Site, then Quick Create, then give it a URL prefix you&#x2019;re comfortable using. In this case I have a domain named NimbleSquid.com, and I&#x2019;ll use the domain name nimblesquid.azurewebsites.net when I create
  it in the portal.</p>
<p>
  <a href="/Media/Default/Windows-Live-Writer/a42a1a2b53f1_15164/image_2.png">
    <img alt="image" src="/posts/redirection-with-windows-azure-web-sites/media/image_thumb.png">
  </a> 
</p>
<p>&#xA0;</p>
<p>Once the site is finished creating it will appear in the list of web sites you have hosted in Azure. </p>
<p>
  <a href="/Media/Default/Windows-Live-Writer/a42a1a2b53f1_15164/2_2.png">
    <img alt="2" src="/posts/redirection-with-windows-azure-web-sites/media/2_thumb.png">
  </a> 
</p>
<p>&#xA0;</p>
<p>When you click on that site to select it and click the browse button you&#x2019;ll see the Azure Web Sites domain name, with the custom prefix you provided to create the site. </p>
<p>
  <a href="/Media/Default/Windows-Live-Writer/a42a1a2b53f1_15164/3_2.png">
    <img alt="3" src="/posts/redirection-with-windows-azure-web-sites/media/3_thumb.png">
  </a> 
</p>
<p>&#xA0;</p>
Setting up a URL Record using DNSimple.com
<p>I don&#x2019;t know if other DNS providers call their redirection records &#x201C;URL&#x201D; the way DNSimple.com does, but if not, your provider probably has something like a 301 redirect. That&#x2019;s sort of the idea here, we&#x2019;re just going to redirect traffic <em>permanently </em> to
  a *.azurewebsites.net domain whenever a request is made to the real domain name. For NimbleSquid.com, no records exist, so the DNSimple.com advanced editor for this domain has no entries. </p>
<p>
  <a href="/Media/Default/Windows-Live-Writer/a42a1a2b53f1_15164/4_2.png">
    <img alt="4" src="/posts/redirection-with-windows-azure-web-sites/media/4_thumb.png">
  </a> 
</p>
<p>&#xA0;</p>
<p>Likewise, if I try to browse to the custom domain name, I&#x2019;ll get an error page. That&#x2019;s pretty much expected behavior at this point; DNSimple.com&#x2019;s DNS servers basically don&#x2019;t know where to send the request. </p>
<p>
  <a href="/Media/Default/Windows-Live-Writer/a42a1a2b53f1_15164/5_2.png">
    <img alt="5" src="/posts/redirection-with-windows-azure-web-sites/media/5_thumb.png">
  </a> 
</p>
<p>&#xA0;</p>
<p>Click on the <strong>Add Records</strong>  button, then select the <strong>URL </strong> option from the context menu. You&#x2019;ll then see the screen below. You can choose to put in a CNAME prefix here, or just leave it blank. In the case of the screenshot
  below, any requests made to any CNAME of nimblesquid.com will be directed to the nimblesquid.azurewebsites.net domain. </p>
<p>
  <a href="/Media/Default/Windows-Live-Writer/a42a1a2b53f1_15164/7_2.png">
    <img alt="7" src="/posts/redirection-with-windows-azure-web-sites/media/7_thumb.png">
  </a> 
</p>
<p>&#xA0;</p>
<p>Setting the TTL menu to 1 minute will result in the domain name resolving (or redirecting) to your Azure Web Site just a moment or two after you click the <strong>Add Record </strong> button. Now, when users make a request to your custom domain name, they&#x2019;ll
  land on your Azure Web Site. Granted, this is a trick, as it just does a redirection, but if you&#x2019;ve got a site on Azure Web Sites and you&#x2019;ve got a custom domain you want to use with that site, and you aren&#x2019;t ready or can&#x2019;t yet afford to upgrade to a
  reserved instance, this could get you through in the meantime. You can get your site up and running, set up the redirection, and start taking orders or showing off your skills on your blog. </p>
