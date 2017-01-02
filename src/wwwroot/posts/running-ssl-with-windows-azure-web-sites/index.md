---
title: Running SSL with Azure Web Sites Today
slug: running-ssl-with-windows-azure-web-sites
author: bradygaster
lastModified: 2015-09-23 19:17:32
pubDate: 2013-04-04 06:09:34
categories: Azure
---

<p>If you&#x2019;re a web developer working with ASP.NET, Node.js, PHP, Python, or you have plans on building your site in C++, Azure Web Sites is the best thing since sliced bread. With support for virtually every method of deployment and with support for most
  of the major web development models you can&#x2019;t beat it. Until recently, SSL was the only question mark for a lot of web site owners, as WAWS doesn&#x2019;t yet support SSL out of the box (trust me, it&#x2019;s coming, I promise). The good news is that there&#x2019;s a method
  of achieving SSL-secured sites <strong>now</strong> . In this blog post I&#x2019;ll introduce the idea of a workaround my engineering friends in the Web Sites team call the SSL Forwarder, and to demonstrate how you can get up and running with an SSL-protected
  Azure-hosted web site in just a few minutes&#x2019; work.</p>
Disclaimer
<p>First, I&#x2019;d like to point out one very important point about the SSL Forwarder solution. This solution works, and we have a handful of community members actively using this solution to provide an SSL front-end for their web sites. So feel comfortable using
  it, but understand that this isn&#x2019;t something you&#x2019;ll have to do forever, as SSL is indeed coming as an in-the-box feature for Web Sites. If you love the idea of Azure Web Sites but the lack of in-the-box SSL support is a deal-breaker for you and your
  organization, this is a viable option to get you up and running <em>now</em> . However, the SSL Forwarder isn&#x2019;t an officially supported solution, in spite of one being actively used by numerous customers. So if you set this up and you experience anything
  weird, feel free to contact me directly via the comment form below, or on Twitter, or by email (and I&#x2019;ll give you my email address on Twitter if you need it). All that being said, I&#x2019;ve heard from quite a few in the community who are using this solution
  that it has mitigated their concern and they appear to be running quite well with this in place.</p>
Architectural Overview
<blockquote>
  <p>Don&#x2019;t panic when you see this solution. <em><strong>Do</strong> </em>  read the introduction, once you see grok how it all works, the SSL Forwarding solution is a whole lot less intimidating. I admit to having freaked out with fear when I first saw this.
    I&#x2019;m no expert at most of the things involved in this exercise, but the Web Sites team literally put together a &#x201C;starter project&#x201D; for me to use, and it took me 1 hour to get it working. If I can do this, <strong>you can do this</strong> .</p>
</blockquote>
<p>
  <a>
    <img alt="SSL-Forwarder-Diagram" src="/posts/running-ssl-with-windows-azure-web-sites/media/SSL-Forwarder-Diagram_thumb_1.png">
  </a> The idea of the SSL Forwarder is pretty simple. You set up a
  <a>Cloud Service</a>  using the Azure portal that redirects traffic to your Azure Web Site. You can use all the niceties of Web Sites (like Git deployment,
  <a>DropBox integration</a>, and publishing directly to your site using
  <a>Visual Studio</a>  or
  <a>WebMatrix</a> ) to actually build your web site, but the requests actually resolve to your Cloud Service endpoint, which then proxies HTTP traffic into your Web Site.</p>
<p>The diagram to the right shows how this solution works, at a high level. The paragraph below explains it in pretty simple terms. I think you&#x2019;ll agree that it isn&#x2019;t that complicated and that the magic that occurs works because of tried-and-true IIS URL
  Rewrite functionality. In order to obtain the 99.9% uptime as outlined in the
  <a>Azure SLA</a>, you&#x2019;ll need to deploy at least 2 instances of the Cloud Service, so the diagram shows 2 instances running. As well, the code provided with this blog post as a starting point is defaulted to start 2 instances. You can back this off or
  increase it however you want, but the 99.9% uptime is only guaranteed if you deploy the Cloud Service in 2 instances or more (and there&#x2019;s no SLA in place yet for Web Sites, since it&#x2019;s still in preview at the time of this blog post&#x2019;s release, so you
  can host your Web Site on as many or as few instances as you like).</p>
<p>You map your domain name to your Cloud Service. Traffic resolves to the Cloud Service, and is then reverse-proxied back to your Web Site. The Cloud Service has 1 Web Role in it, and the Web Role consists of a single file, the Web.config file. The Web.config
  in the Web Role contains some hefty IISRewrite rules that direct traffic to the Web Site in which your content is hosted. In this way, all traffic &#x2013; be it HTTP or HTTPS traffic &#x2013; comes through the Cloud Service and resolves onto the Web Site you want
  to serve. Since Cloud Services support the use of custom SSL certificates, you can place a certificate into the Cloud Service, and serve up content via an HTTPS connection.</p>
Setup
<p>To go along with this blog post, there&#x2019;s a
  <a>GitHub.com</a> 
  <a>repository</a>  containing a Visual Studio 2012 solution you can use to get started. This solution contains three projects:</p>
<ul>
  <li>A Azure Cloud Project</li>
  <li>A web site that&#x2019;s used as a Web Role for the Cloud Project</li>
  <li>A web site that&#x2019;s deployed to Azure Web Sites (you&#x2019;ll want to replace this one with the project you&#x2019;re deploying or just remove it, it&#x2019;s just there as part of the sample)</li>
</ul>
<h3>Create the Cloud Service and Web Site</h3>
<p>First thing is, I&#x2019;ll need to create a Web Site to host the site&#x2019;s code. Below is a screen shot of me creating a simple web site myself using the Azure portal.</p>
<p>
  <a>
    <img alt="1" src="/posts/running-ssl-with-windows-azure-web-sites/media/1_thumb.png">
  </a> 
</p>
<p>Obviously, I&#x2019;ll need to create a Azure Cloud Service, too. In this demo, I&#x2019;ll be using a new Cloud Service called <em>SSLForwarder</em>, since I&#x2019;m not too good at coming up with funky names for things that don&#x2019;t end in a capital R (and when I do,
  <a>Phil teases me</a>, so I&#x2019;ll spare him the ammunition). Below is another screen shot of the Azure portal, with the new Cloud Service being created.</p>
<p>
  <a>
    <img alt="2" src="/posts/running-ssl-with-windows-azure-web-sites/media/2_thumb.png">
  </a> 
</p>
<p>If you&#x2019;re following along at home  work, leave your browser open when you perform the next step, if you even <strong>need</strong>  to perform the next step, as it is an optional one.</p>
<h3>Create a Self-signed Certificate</h3>
<p>This next step is optional, and only required if you don&#x2019;t already have an SSL certificate in mind that you&#x2019;d like to use. I&#x2019;ll use the IIS Manager to create my own self-signed certificate. In the IIS Manager I&#x2019;ll click the <em>Server Certificates</em>   applet, as shown below.</p>
<blockquote>
  <p>When I browse this site secured with this certificate, there&#x2019;ll be an error message in the browser informing me that this cert isn&#x2019;t supposed to be used by the domain name from where it&#x2019;s being served. Since <em>you&#x2019;ll</em>  be using a <em>real </em> SSL
    certificate, you shouldn&#x2019;t have to worry about that error when you go through this process (and I trust you&#x2019;ll forgive a later screen shot where the error is visible).</p>
</blockquote>
<p>
  <a>
    <img alt="4" src="/posts/running-ssl-with-windows-azure-web-sites/media/4_thumb.png">
  </a> 
</p>
<p>Once that applet loads up in the manager, I&#x2019;ll click the link in the actions pane labeled <em>Create Self-Signed Certificate</em> .</p>
<p>
  <a>
    <img alt="5" src="/posts/running-ssl-with-windows-azure-web-sites/media/5_thumb_1.png">
  </a> 
</p>
<p>I&#x2019;ll name my certificate <em>SSLForwarderTesting</em>, and then it appears in the list of certificates I have installed on my local development machine. I select that certificate from the list and click the link in the Actions pane labeled <em>Export</em>   to save the cert somewhere as a file.</p>
<p>
  <a>
    <img alt="6" src="/posts/running-ssl-with-windows-azure-web-sites/media/6_thumb.png">
  </a> 
</p>
<p>Then I find the location where I&#x2019;ll save the file and provide it with a password (which I&#x2019;ll need to remember for the next step).</p>
<p>
  <a>
    <img alt="7" src="/posts/running-ssl-with-windows-azure-web-sites/media/7_thumb.png">
  </a> 
</p>
<p>Now that this [optional] step is complete I have a <em>*.PFX</em>  file I can use to install my certificate in the Cloud Service.</p>
<h3>Install the SSL Certificate into a Cloud Service</h3>
<p>To activate SSL on the Cloud Service I&#x2019;ll need to install an SSL certificate into the service using the Azure portal. Don&#x2019;t panic, this is easier than it sounds. Promise. Five minutes, tops.</p>
<p>Back in my browser, on the Azure portal page, I&#x2019;ll click the Cloud Service that&#x2019;ll be answering HTTP/S requests for my site. The service&#x2019;s dashboard page will open up.</p>
<p>
  <a>
    <img alt="8" src="/posts/running-ssl-with-windows-azure-web-sites/media/8_thumb_1.png">
  </a> 
</p>
<p>I&#x2019;ll click the <em>Certificates<strong> </strong> </em> tab in the navigation bar.</p>
<p>
  <a>
    <img alt="9" src="/posts/running-ssl-with-windows-azure-web-sites/media/9_thumb.png">
  </a> 
</p>
<p>I&#x2019;m going to want to upload my certificate, so this next step should be self-explanatory.</p>
<p>
  <a>
    <img alt="10" src="/posts/running-ssl-with-windows-azure-web-sites/media/10_thumb.png">
  </a> 
</p>
<p>The next dialog gives me a pretty hard-to-screw-up dialog. Unless I forgot that password.</p>
<blockquote>
  <p>( cue the sound of hands ruffling through hundreds of post-its)</p>
</blockquote>
<p>
  <a>
    <img alt="11" src="/posts/running-ssl-with-windows-azure-web-sites/media/11_thumb_1.png">
  </a> 
</p>
<p>Once the certificate is uploaded, I&#x2019;ll click the new cert and copy the thumbprint to my clipboard, maybe paste it into Notepad just for the moment&#x2026;</p>
<p>
  <a>
    <img alt="13" src="/posts/running-ssl-with-windows-azure-web-sites/media/13_thumb.png">
  </a> 
</p>
<h3>Configuring the Cloud Service&#x2019;s SSL Certificate</h3>
<p>With the SSL cert installed and the thumbprint copied, I&#x2019;ll open up the <em>ServiceConfiguration.cloud.cscfg</em>  file in Visual Studio 2012 and set the thumbprint&#x2019;s configuration. I could also do this using the built-in Azure tools in Visual Studio,
  but since I&#x2019;ve got the thumbprint copied this is just as easy to do directly editing the files. Plus, the Web Sites team made it pretty obvious where to put the thumbprint, as you&#x2019;ll see from the screen shot below.</p>
<p>
  <a>
    <img alt="14" src="/posts/running-ssl-with-windows-azure-web-sites/media/14_thumb.png">
  </a> 
</p>
<h3>Configuring the URL Rewrite Rules in the Web Role</h3>
<p>Remember the architectural overview from earlier. The main thing this Cloud Service does is to answer HTTP/S requests and then reverse-proxy that traffic back to the Web Site I&#x2019;m happily hosting in Azure. Setting up this proxy configuration isn&#x2019;t too
  bad, especially when I&#x2019;ve got the code the team handed me. I just look for all the places in the Web.config file from the Web Role project that mentions <em>foo</em> .com or <em>foo</em> .azurewebsites.net or <em>foo&#x2026;</em> well, you get the idea.</p>
<p>Here&#x2019;s the Web.config file from the Web Role project before I edit it to comply with the Web Site and Cloud Service I created to demonstrate this solution open in Visual Studio 2012. I&#x2019;ve marked all the spots you&#x2019;ll need to change in the screen shot.</p>
<p>
  <a>
    <img alt="15" src="/posts/running-ssl-with-windows-azure-web-sites/media/15_thumb.png">
  </a> 
</p>
<p>Here&#x2019;s the file after being edited. Again, I&#x2019;ll indicate the places where I made changes.</p>
<p>
  <a>
    <img alt="16" src="/posts/running-ssl-with-windows-azure-web-sites/media/16_thumb.png">
  </a> 
</p>
<p>Now that the Cloud Service and the Web.config file of the Web Role project&#x2019;s been configured to redirect traffic to another destination, the proxy is ready for deployment. The solution&#x2019;s Cloud project is defaulted to run at 2 instances, so that&#x2019;s something
  you&#x2019;ll want to remember &#x2013; you&#x2019;ll be paying for 2 instances of the Cloud Service you&#x2019;ll be using to forward HTTP/S traffic to your Web Site.</p>
<h3></h3>
<h3>Publish the Cloud Service</h3>
<p>Within Visual Studio 2012, I right-click the Cloud project and select the <em>Publish</em>  context menu item.</p>
<p>
  <a>
    <img alt="22" src="/posts/running-ssl-with-windows-azure-web-sites/media/22_thumb.png">
  </a> 
</p>
<p>A dew dialogs will walk me through the process of publishing the SSLForwarder service into Azure. It may take a few minutes to complete, but once it publishes the Cloud Service will be running in your subscription and ready to respond to HTTP/S requests.</p>
<p>To verify everything&#x2019;s working, try hitting the Cloud Service URL &#x2013; in my case <em>sslforwarder.cloudapp.net, </em> to see if it&#x2019;s answering requests or spewing errors about unreachable hosts, either of which wouldn&#x2019;t be surprising &#x2013; we&#x2019;ve redirected the
  Cloud Service&#x2019;s Web Role to a Web Site. That web site probably isn&#x2019;t set up yet, so you could see some unpredictable results.</p>
<p>If you actually pre-configured your <em>SSLForwarder</em>  Cloud Service to direct traffic to a *.azurewebsites.net you&#x2019;re already running you&#x2019;re pretty much finished, and you&#x2019;re probably running behind HTTPS without any problems right now. If not, and
  the idea of publishing Web Sites from Visual Studio is new to you, you&#x2019;ll have a chance to use that technique here.</p>
<h3>Publish a Azure Web Site</h3>
<p>I&#x2019;ll go back into the Azure portal and go specifically to the <em>SSLForwarder</em>  Web Site I created earlier on in the post.</p>
<p>
  <a>
    <img alt="17" src="/posts/running-ssl-with-windows-azure-web-sites/media/17_thumb.png">
  </a> 
</p>
<p>Once the site&#x2019;s dashboard opens up, I&#x2019;ll find the link labeled <em>download publishing profile</em> . This file will be used by Visual Studio during publishing to make the process very simple.</p>
<p>
  <a>
    <img alt="18" src="/posts/running-ssl-with-windows-azure-web-sites/media/18_thumb.png">
  </a> 
</p>
<h3>Publishing and Browsing an SSL-encrypted Web Site</h3>
<p>Once the publish settings file has been downloaded, it&#x2019;s easy to push the site to Web Sites using Visual Studio 2012 or WebMatrix. With the sample project provided I&#x2019;ll open up the Web Application project I want to publish to Web Sites. Then, I&#x2019;ll right-click
  the web site project and select the <em>Publish</em>  menu item.</p>
<p>
  <a>
    <img alt="19" src="/posts/running-ssl-with-windows-azure-web-sites/media/19_thumb.png">
  </a> 
</p>
<p>Then, the publish process will make the remainder of the publishing experience pretty simple.</p>
<blockquote>
  <p>Remember to thank
    <a>Sayed Hashimi</a>  if you need him out and about, he loves to hear thoughts on publishing and uses suggestions to make the experience an improved one for you. He also has a stupendous team of people working with him to execute great publishing experiences,
    who love feedback.</p>
</blockquote>
<p>The publish process dialogs will walk you through the simple act of publishing your site up to Azure. Once it completes (which usually takes 30-60 seconds for a larger site) the site will open up in a web browser.</p>
<p>
  <a>
    <img alt="21" src="/posts/running-ssl-with-windows-azure-web-sites/media/21_thumb.png">
  </a> 
</p>
<p>Note the URL still shows HTTP, and it also shows the URL of the Azure Web Site you created. You&#x2019;ll need to manually enter in the URL for the Cloud Service you created.</p>
<p>For me, that&#x2019;s
  <a>www.sslforwarder.com</a> . So long as the domain name you enter <em>resolves to </em> Cloud Service you should be alright. You can also opt for the *.cloudapp.net approach too, as the domain name of the site you want to put out. Whatever your preference
  for solving this particular issue.</p>
<p>I&#x2019;m going to go ahead and change the domain name <em>and </em> the protocol, so our hit to the site being hosted in Web Sites will respond to it receiving an SSL-encrypted request, then load the site in the browser.</p>
<blockquote>
  <p>Note &#x2013; this is when you&#x2019;ll need to forgive me for showing you something that causes a warning in the browser. It&#x2019;s just that way since I used a self-signed cert in a Azure service, so we should expect to see an error here. It&#x2019;s right there in the browser&#x2019;s
    address bar, where it says &#x201C;Certificate Error.&#x201D; If I&#x2019;d used a real SSL cert, from a real authority, the error wouldn&#x2019;t be there.</p>
</blockquote>
<p>
  <a>
    <img alt="23" src="/posts/running-ssl-with-windows-azure-web-sites/media/23_thumb.png">
  </a> 
</p>

Summary
<p>So for many months I&#x2019;ve heard users request SSL on Web Sites, saying <em>everything about it is awesome. </em> Then they stare at me and wait about 3 seconds and usually follow it up with &#x201C;<em>but you&#x2019;ve gotta get me SSL man, I&#x2019;m over here and I gotta have my SSL&#x201D;</em> .
  I understand their desire for us to support it, and luckily, the Web Sites team and our engineering organization is so willing to share their solutions publicly. This is a great solution, but it won&#x2019;t work in every situation and isn&#x2019;t as good as what
  the Web Sites teams have in plans for the future. The SSL Forwarder solution is a good stop-gap, a good temporary solution to a problem we&#x2019;ve had a lot of requests about.</p>
<p>Hopefully this helps in your decision to give Azure Web Sites a shot. If SSL has been your sole reason for not wanting to give it a try, now you have a great workaround in place that you can facilitate to get started right now.</p>
