---
title: Running Nancy on Azure Web Sites
slug: running-nancy-on-windows-azure-web-sites
author: bradygaster
lastModified: 2012-10-05 18:15:14
pubDate: 2012-10-05 18:15:11
categories: Azure
---

<p>I&#x2019;m a huge fan of finding cleaner ways to build web applications. Simplicity is a really good thing.
  <a href="https://github.com/NancyFx/Nancy">Nancy</a>  is a simple little web framework inspired by Ruby&#x2019;s
  <a href="http://www.sinatrarb.com/">Sinatra</a> . Nancy is open source, hosted on GitHub.com, and distributed like any other awesome .NET open source library &#x2013; via
  <a href="http://nuget.org/packages?q=nancy">NuGet</a>  packages. When a coding challenge landed on my desk this week that I&#x2019;ll be responsible for prototyping, Nancy seemed like a good option, so I&#x2019;ve taken the time to tinker with it a little. I&#x2019;ll spare you the details of the coding assignment
  for this post. Instead, I&#x2019;ll focus on the process of getting Nancy working in your own Azure Web Site. </p>
<p>To start with, I&#x2019;ve created an empty web site using the Azure portal. This site won&#x2019;t use a database, so I just logged into the portal and selected <strong>New &#x2013;&gt; Web Site &#x2013;&gt; Quick Create</strong>  and gave it a pretty logical name. </p>
<p>
  <a href="/Media/Default/Windows-Live-Writer/Running-NancyFx-on-Windows-Azure-Web-Sit_A703/1_2.png">
    <img alt="1" src="/posts/running-nancy-on-windows-azure-web-sites/media/1_thumb.png">
  </a> 
</p>
<p>&#xA0;</p>
<p>Once the site was finished cooking, I grab the publish profile settings from the site&#x2019;s dashboard. </p>
<p>
  <a href="/Media/Default/Windows-Live-Writer/Running-NancyFx-on-Windows-Azure-Web-Sit_A703/2_2.png">
    <img alt="2" src="/posts/running-nancy-on-windows-azure-web-sites/media/2_thumb.png">
  </a> 
</p>
Getting Started with Nancy
<p>First, I create an empty ASP.NET web application. To make sure everything&#x2019;s as clean as it could be, I remove a good portion of the files and folders from the site. Since I&#x2019;ll be creating the most basic level functionality in this example, I don&#x2019;t need
  a whole lot of extraneous resources and functionality in the site. The project below shows what I have left over once I ransacked the project&#x2019;s structure. Take note of the <em>Uninstall-Package</em>  command I made in the Package Manager Console. I ran
  similar commands until I had as bare-bones a project structure as possible. Then I removed some stuff from the Web.config until it literally was quite minimalistic.</p>
<p>
  <a href="/Media/Default/Windows-Live-Writer/Running-NancyFx-on-Windows-Azure-Web-Sit_A703/3_2.png">
    <img alt="3" src="/posts/running-nancy-on-windows-azure-web-sites/media/3_thumb.png">
  </a> 
</p>
<p>&#xA0;</p>
<p>To run Nancy on Azure Web Sites, I&#x2019;ll need to install 2 packages from NuGet. The first of these is the
  <a href="http://nuget.org/packages/Nancy">Nancy core package</a> . The second package I&#x2019;ll need is the package that enables
  <a href="http://nuget.org/packages/Nancy.Hosting.Aspnet">Nancy hosting on ASP.NET</a> . </p>
<p>
  <a href="/Media/Default/Windows-Live-Writer/Running-NancyFx-on-Windows-Azure-Web-Sit_A703/4_2.png">
    <img alt="4" src="/posts/running-nancy-on-windows-azure-web-sites/media/4_thumb.png">
  </a> 
</p>
<p>&#xA0;</p>
<p>In my web project I create a new folder called NancyStuff and add a new class called <em>HelloWorldModule</em> . </p>
<p>
  <a href="/Media/Default/Windows-Live-Writer/Running-NancyFx-on-Windows-Azure-Web-Sit_A703/5_4.png">
    <img alt="5" src="/posts/running-nancy-on-windows-azure-web-sites/media/5_thumb_1.png">
  </a> 
</p>
<p>&#xA0;</p>
<p>This class is a very basic Nancy Module, and I learned how to create it by perusing the
  <a href="https://github.com/NancyFx/Nancy/wiki/Exploring-the-nancy-module">Nancy project&#x2019;s GitHub.com Wiki</a> . I basically want to have a route that, when called, will just say hello to the user. You don&#x2019;t get much simpler than this for assigning functionality to a given route. When the user requests the <strong>/hello</strong>   route using an HTTP GET request, the message <strong>Hello World</strong>  is rendered to the response stream. The <em>HelloWorldModule </em> class will extend <em>NancyModule. </em> In Nancy, routes are handled by classes which inherit from <em>NancyModule. </em> According
  to the Nancy documentation, <em><a href="https://github.com/NancyFx/Nancy/wiki/Exploring-the-nancy-module">Modules are the lynchpin of any given Nancy application</a> . </em> &#xA0;</p>

<p>&#xA0;</p>
<p>At this point, everything should work, so I&#x2019;ll go ahead and deploy the web site up to Azure. To do this, I select the <em>Publish </em> context menu item from within Visual Studio 2012. </p>
<p>
  <a href="/Media/Default/Windows-Live-Writer/Running-NancyFx-on-Windows-Azure-Web-Sit_A703/6_2.png">
    <img alt="6" src="/posts/running-nancy-on-windows-azure-web-sites/media/6_thumb.png">
  </a> 
</p>
<p>&#xA0;</p>
<p>Then, I import the publish settings file using the <em>Import </em> feature on the publishing dialog. </p>
<p>
  <a href="/Media/Default/Windows-Live-Writer/Running-NancyFx-on-Windows-Azure-Web-Sit_A703/7_2.png">
    <img alt="7" src="/posts/running-nancy-on-windows-azure-web-sites/media/7_thumb.png">
  </a> 
</p>
<p>&#xA0;</p>
<p>Once I find the .<em>publishsettings </em> file I downloaded from the Azure portal and import it, I&#x2019;m ready to publish. Clicking the publish button in the dialog at this point will result in the site being deployed up to my Azure Web Site. </p>
<p>
  <a href="/Media/Default/Windows-Live-Writer/Running-NancyFx-on-Windows-Azure-Web-Sit_A703/8_2.png">
    <img alt="8" src="/posts/running-nancy-on-windows-azure-web-sites/media/8_thumb.png">
  </a> 
</p>
<p>&#xA0;</p>
<p>The site will open once the deployment has completed and will <em>probably </em> present me with a 404 error, indicating there&#x2019;s no route configured to answer requests to the root of the site. Changing the URL to hit <strong>/hello</strong>  will result
  with the module answering the request and doing what I expected it to do:</p>
<p>
  <a href="/Media/Default/Windows-Live-Writer/Running-NancyFx-on-Windows-Azure-Web-Sit_A703/9_2.png">
    <img alt="9" src="/posts/running-nancy-on-windows-azure-web-sites/media/9_thumb.png">
  </a> 
</p>
<p>&#xA0;</p>
Enabling Static File Browsing
<p>With this first module functioning properly I want to create a static file from which the Nancy module could be called using jQuery on the client. The idea is, now that I have this Nancy module working, I might want to make calls to it using some AJAX
  method to display the message it returns to the user. So I add a new static HTML page to my solution. </p>
<p>
  <a href="/Media/Default/Windows-Live-Writer/Running-NancyFx-on-Windows-Azure-Web-Sit_A703/10_4.png">
    <img alt="10" src="/posts/running-nancy-on-windows-azure-web-sites/media/10_thumb_1.png">
  </a> 
</p>
<p>&#xA0;</p>
<p>The problem here is that, since Nancy&#x2019;s handing requests to my site and using the modules I create to respond to those requests, the ability to browse to static files is&#x2026; well&#x2026; sort of <em>turned off. </em> So attempts to hit a seemingly innocent-enough
  page results with a heinous &#x2013; yet adorable in a self-loathing sort of way - response. </p>
<p>
  <a href="/Media/Default/Windows-Live-Writer/Running-NancyFx-on-Windows-Azure-Web-Sit_A703/11_2.png">
    <img alt="11" src="/posts/running-nancy-on-windows-azure-web-sites/media/11_thumb.png">
  </a> 
</p>
<p>&#xA0;</p>
<p>Thanks to the Nancy documentation, it was pretty easy to find a solution to this behavior. See, Nancy basically starts intercepting requests to my site, and it takes precedence over the default behavior of &#x201C;serving up static files when they&#x2019;re requested.&#x201D;
  Nancy&#x2019;s routing engine listens for requests and if one&#x2019;s made to the site for which no module has been created and routed, the site has no idea how to handle the request. </p>
<p>To solve this problem, I need to create another class that extends the <em>DefaultNancyBootstrapper </em> class. This class, explained pretty thoroughly in the
  <a href="https://github.com/NancyFx/Nancy/wiki/Managing-static-content">GitHub.com Nancy Wiki&#x2019;s article on managing static content</a>, is what I&#x2019;ll need to use to instruct Nancy on how to route to individual static files. For now I&#x2019;m only in need of a class to handle this one particular static file, but setting up a bootstrapper
  to allow static browsing in a directory is possible. Other options exist too, such as routes that use regular expressions, but that&#x2019;s something I&#x2019;ll look at in a later episode of this series. For now, I just want to tell Nancy to serve up the page <em>Default.html </em> whenever
  a request is made to <em>/Default.html</em> . I&#x2019;m also enabling static file browsing out of the <strong>/scripts </strong> folder of the site. The main thing to look at here is the call to the <em>StaticContentsConventions.Add</em>  method, into which
  I pass the name of the file and the route on which it should be served up. </p>

<p>&#xA0;</p>
<p>Now, I&#x2019;ll add some jQuery code to the static page that calls the <em>HelloWorldModule</em>  and displays whatever it responds with in an HTML element on the page. </p>

<p>&#xA0;</p>
<p>When the static file loads up in the browser, the jQuery code makes an AJAX request back to the server to the <strong>/hello</strong>  route, and then drops the response right into the page. When I deploy the site again to Azure Web Sites and hit the <em>Default.html</em>   file, the behavior is just what I&#x2019;d expected it would be; the page loads, then the message is obtained via AJAX and displayed. </p>
<p>
  <a href="/Media/Default/Windows-Live-Writer/Running-NancyFx-on-Windows-Azure-Web-Sit_A703/12_2.png">
    <img alt="12" src="/posts/running-nancy-on-windows-azure-web-sites/media/12_thumb.png">
  </a> 
</p>
<p>&#xA0;</p>
<p>Hopefully this introduction has demonstrated the support Azure Web Sites has for Nancy. Since Nancy can be hosted under ASP.NET, and since Azure Web Sites support the ASP.NET pipeline, everything works. I&#x2019;ll continue to tinker with Nancy from here as
  I work on the coding project for which I chose it to be a puzzle piece. Hopefully during that development work I&#x2019;ll learn more about Nancy, have the time to demonstrate what I learn, and ease someone&#x2019;s day when they decide to move their Nancy site over
  to Azure Web Sites. </p>
<p>Happy coding!</p>
