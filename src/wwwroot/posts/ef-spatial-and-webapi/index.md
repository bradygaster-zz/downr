---
title: Build a Location API Using Entity Framework Spatial and Web API, on Azure Web Sites
slug: ef-spatial-and-webapi
author: bradygaster
lastModified: 2014-08-07 07:56:20
pubDate: 2012-10-30 05:39:22
categories: .NET,Azure
---

<p>As
  <a href="http://weblogs.asp.net/scottgu/archive/2012/10/25/net-4-5-now-supported-with-windows-azure-web-sites.aspx">announced by Scott Guthrie recently</a>, Azure Web Sites now supports the .NET Framework 4.5. Some awesome ASP.NET features are now available to web developers who want to host their ASP.NET applications on Azure following the web sites offering getting
  support for .NET 4.5. One feature I&#x2019;m especially excited about is Entity Framework Spatial support. Only available in .NET 4.5, EF Spatial is something that gives developers who want to build location-aware applications the ability to easily save and
  retrieve location data without having to invent crazy solutions using SQL code. I&#x2019;ve implemented the
  <a href="http://en.wikipedia.org/wiki/Haversine_formula">Haversine formula</a>  using a SQL stored procedure in the past, and I can speak from experience when I say that EF Spatial is about 10,000 times easier and more logical. Don&#x2019;t take my word for it, though. Take a look at the sample code I&#x2019;ll show you
  in this blog post, which demonstrates how you can develop a location-aware API using ASP.NET Web API, EF Spatial, and host the whole thing on Azure Web Sites. </p>
Creating the Site in Azure
<p>Before diving into code I&#x2019;ll go out to the Azure portal and create a new web site. For this API example, I create a site with a database, as I&#x2019;ll want to store the data in a Azure SQL Database. The screen shot below shows the first step of creating a
  new site. By simply selecting the new web site option, then selecting &#x201C;with database,&#x201D; I&#x2019;m going to be walked through the process of creating both assets in Azure in a moment. </p>
<p>
  <a href="/Media/Default/Windows-Live-Writer/ac6f2c93769a_B101/1_2.png">
    <img alt="1" src="/posts/ef-spatial-and-webapi/media/1_thumb.png">
  </a> 
</p>
<p>The first thing Azure will need to know is the URL I&#x2019;ll want associated with my site. The
  <a href="http://bit.ly/windowsazuretrial">free Azure Web Sites offer</a>  defaults to <strong>[yoursitename].azurewebsites.net</strong>, so this first step allows me to define the URL prefix associated with my site. </p>
<p>Simultaneously, this first step gives me the opportunity to define the name of the connection string I&#x2019;ll expect to use in my Web.config file later, that will connect the site to the Azure SQL Database I&#x2019;ll create in a moment. </p>
<p>
  <a href="/Media/Default/Windows-Live-Writer/ac6f2c93769a_B101/2_2.png">
    <img alt="2" src="/posts/ef-spatial-and-webapi/media/2_thumb.png">
  </a> 
</p>
<p>The last steps in the site creation process will collect the username and SQL Server information from you. In this example, I&#x2019;m going to create a new database <strong>and </strong> a new SQL Server in the Azure cloud. However, you can select a pre-existing
  SQL Server if you&#x2019;d prefer during your own setup process. </p>
<p>I specifically unchecked the &#x201C;Configure Advanced Database Settings&#x201D; checkbox, as there&#x2019;s not much I&#x2019;ll need to do to the database in the portal. As you&#x2019;ll see in a moment, I&#x2019;ll be doing all my database &#x201C;stuff&#x201D; using EF&#x2019;s Migrations features. </p>
<p>
  <a href="/Media/Default/Windows-Live-Writer/ac6f2c93769a_B101/3_2.png">
    <img alt="3" src="/posts/ef-spatial-and-webapi/media/3_thumb.png">
  </a> 
</p>
<p>Once I&#x2019;ve entered the username I&#x2019;d like to use, the password, and selected (or created) a SQL server, I click the check button to create the site and the SQL database. In just a few seconds, both are created in Azure, and I can get started with the fun
  stuff &#x2013; the <strong>c0d3z</strong> !</p>

Preparing for Deployment
<p>Just so I have a method of deploying the site once I finish the code, I&#x2019;ll select the new site from the Azure portal by clicking on it&#x2019;s name once the site-creation process completes. </p>
<p>
  <a href="/Media/Default/Windows-Live-Writer/ac6f2c93769a_B101/4_2.png">
    <img alt="4" src="/posts/ef-spatial-and-webapi/media/4_thumb.png">
  </a> 
</p>
<p>The site&#x2019;s dashboard will open up in the browser. If I scroll down, the <strong>Quick Glance</strong>  links are visible on the right side of the dashboard page. Clicking the link labeled <strong>Download Publish Profile</strong>  will do just that &#x2013; download
  a publish settings file, which contains some XML defining how
  <a href="http://www.microsoft.com/visualstudio/">Visual Studio</a>  or
  <a href="http://www.microsoft.com/web/webmatrix/">WebMatrix 2</a>  (or the Web Deploy command line) should upload the files to the server. Also contained within the publish settings file is the metadata specific to the database I created for this site. </p>
<p>
  <a href="/Media/Default/Windows-Live-Writer/ac6f2c93769a_B101/5_2.png">
    <img alt="5" src="/posts/ef-spatial-and-webapi/media/5_thumb.png">
  </a> 
</p>
<p>As you&#x2019;ll see in a moment when I start the deployment process, everything I need to know about deploying a site and a database backing that site is outlined in the publish settings file. When I perform the deployment from within Visual Studio 2012, I&#x2019;ll
  be given the option of using Entity Framework Migrations to populate the database live in Azure. Not only will the site files be published, <strong>the database will be created, too. </strong> All of this is possible via the publish settings file&#x2019;s metadata.
  </p>
Building the API in Visual Studio 2012
<p>The code for the location API will be relatively simple to build (thanks to the Entity Framework, ASP.NET, and Visual Studio teams). The first step is to create a new ASP.NET MVC project using Visual Studio 2012, as is shown below. If you&#x2019;d rather just
  grab the code than walk through the coding process, I&#x2019;ve created a
  <a href="https://github.com/bradygaster/EF-Spatial-Web-API-Demo">public GitHub.com repository for the Spatial Demo solution</a>, so clone it from there if you&#x2019;d rather view the completed source code rather than create it from scratch. </p>
<p>Note that I&#x2019;m selecting the <strong>.NET Framework 4.5</strong>  in this dialog. Previous to the 4.5 support in Azure Web Sites, this would always need to be set at 4.0 or my deployment would fail. As well, I would have had compilation issues for anything
  relating to Entity Framework Spatial, as those libraries and namespaces are also only available under .NET 4.5. Now, I can select the 4.5 Framework, satisfy everyone, and keep on trucking. </p>
<p>
  <a href="/Media/Default/Windows-Live-Writer/ac6f2c93769a_B101/new-project_2.png">
    <img alt="new-project" src="/posts/ef-spatial-and-webapi/media/new-project_thumb.png">
  </a> 
</p>
<p>In the second step of the new MVC project process I&#x2019;ll select <strong>Web API</strong>, since my main focus in this application is to create a location-aware API that can be used by multiple clients. </p>
<p>
  <a href="/Media/Default/Windows-Live-Writer/ac6f2c93769a_B101/web-api-project_2.png">
    <img alt="web-api-project" src="/posts/ef-spatial-and-webapi/media/web-api-project_thumb.png">
  </a> 
</p>
<p>By default, the project template comes with a sample controller to demonstrate how to create Web API controllers, called <strong>ValuesController.cs. </strong> Nothing against that file, but I&#x2019;ll delete it right away, since I&#x2019;ll be adding my own functionality
  to this project. </p>
<h3>Domain Entities</h3>
<p>The first classes I&#x2019;ll add to this project will represent the entity domains pertinent to the project&#x2019;s goals. The first of these model classes is the <strong>LocationEntity</strong>  class. This class will be used in my Entity Framework layer to represent
  individual records in the database that are associated with locations on a map. The LocationEntity class is quite simple, and is shown in the gist below. </p>
<p>
  
</p>
<p>Some of the metadata associated with a
  <a href="http://msdn.microsoft.com/en-us/library/system.data.spatial.dbgeography.aspx">DbGeography</a>  object isn&#x2019;t easily or predictably serialized, so to minimize variableness (okay, I&#x2019;m a control freak when it comes to serialization) I&#x2019;ve also created a class to represent a Location object on the wire. This class, the <strong>Location</strong>   class, is visible in the following gist. Take note, though, it&#x2019;s not that much different from the typical LocationEntity class aside from one thing. I&#x2019;m adding the explicit Latitude and Longitude properties to this class.
  <a href="http://msdn.microsoft.com/en-us/library/system.data.spatial.dbgeography.aspx">DbGeography</a>  instances offer a good deal more functionality, but I won&#x2019;t need those in this particular API example. Since all I need is latitude and longitude in the API side, I&#x2019;ll just work up some code in the API controller I&#x2019;ll create later to
  convert the entity class to the API class. </p>
<p>
  
</p>
<p>Essentially, I&#x2019;ve created a data transfer object and a view model object. Nothing really new here aside from the Entity Framework Spatial additions of functionality from what I&#x2019;ve done in previous API implementations which required the database entity
  be loosely coupled away from the class the API or GUI will use to display (or transmit) the data. </p>
<h3>Data Context, Configuring Migrations, and Database Seeding</h3>
<p>Now that the models are complete I need to work in the Entity Framework &#x201C;plumbing&#x201D; that gives the controller access to the database via EF&#x2019;s magic. The first step in this process is to work up the Data Context class that provides the abstraction layer
  between the entity models and the database layer. The data context class, shown below, is quite simple, as I&#x2019;ve really only got a single entity in this example implementation. </p>
<p>
  
</p>
<p>Take note of the constructor, which is overridden from the
  <a href="http://msdn.microsoft.com/en-us/library/gg679467(v=vs.103).aspx">base&#x2019;s constructor</a> . This requires me to make a change in the web.config file created by the project template. By default, the web.config file is generated with a single connection string, the name of which is <strong>DefaultConnection</strong> .
  I need to either create a secondary connection string with the right name, change the default one (which I&#x2019;ve done in this example), or use Visual Studio&#x2019;s MVC-generation tools to create an EF-infused controller, which will add a new connection string
  to the web.config automatically. Since I&#x2019;m coding up this data context class manually, I just need to go into the Web.config and change the <strong>DefaultConnection</strong>  connection string&#x2019;s name attribute to match the one I&#x2019;ve added in this constructor
  override, <strong>SpatialDemoConnectionString</strong> . Once that&#x2019;s done, this EF data context class will use the connection string identified in the configuration file with that name. </p>
<blockquote>
  <p>During deployment, this becomes a very <strong>nifty</strong>  facet of developing ASP.NET sites that are deployed to Azure Web Sites using the Visual Studio 2012 publishing functionality. We&#x2019;ll get to that in a moment, though&#x2026;</p>
</blockquote>
<p>EF has this awesome feature called Migrations that gives EF the ability of setting up and/or tearing down database schema objects, like tables and columns and all indexes (oh my!).&#xA0; So the next step for me during this development cycle is to set up the
  EF Migrations for this project.
  <a href="http://channel9.msdn.com/Shows/Web+Camps+TV/Rowan-Miller-Demonstrates-Entity-Framework-5-Using-ASPNET-MVC-4">Rowan Miller does a great job of describing how EF Migrations work in this Web Camps TV episode</a>, and Robert Green&#x2019;s
  <a href="http://channel9.msdn.com/Shows/Visual-Studio-Toolbox">Visual Studio Toolbox</a>  show has a ton of
  <a href="http://channel9.msdn.com/Shows/Visual-Studio-Toolbox/Visual-Studio-Toolbox-Entity-Framework-Part-1">great</a> 
  <a href="http://channel9.msdn.com/Shows/Visual-Studio-Toolbox/Visual-Studio-Toolbox-Entity-Framework-Part-2">content</a>  on EF, too, so check out those resources for more information on EF Migrations&#x2019; awesomeness. The general idea behind Migrations, though, is simple &#x2013; it&#x2019;s a way of allowing EF to scaffold database components up and down, so I won&#x2019;t have to
  do those items using SQL code. </p>
<p>What&#x2019;s even better than the fact the EF has Migrations is that I don&#x2019;t need to memorize how to do it because the NuGet/PowerShell/Visual Studio gods have made that pretty easy for me.&#xA0; To turn Migrations on for my project, which contains a class that
  derives from EF&#x2019;s data context class (the one I just finished creating in the previous step), I simply need to type the command <strong>enable-migrations</strong>  into the NuGet package management console window. </p>
<p>Once I enable migrations, a new class will be added to my project. This class will be added to a new Migrations folder, and is usually called <strong>Configuration.cs</strong> . Within that file is contained a constructor and a method I can implement however
  I want called &#x2013; appropriately &#x2013; <strong>Seed.</strong>  In this particular use-case, I enable automatic migrations and add some seed data to the database. </p>
<p>
  
</p>
<p>Enabling automatic migrations basically assumes any changes I make will automatically be reflected in the database later on (again, this is super-nifty once we do the deployment, so stay tuned!). </p>
<blockquote>
  <p>Quick background on what types of locations we&#x2019;ll be saving&#x2026; My wife and I moved from the Southeast US to the Pacific Northwest region recently. Much to our chagrin, there are far fewer places to pick up great chicken wings than there were in the Southeast.
    So, I decided I needed to use our every-Sunday-during-football snack of chicken wings as a good use-case for a location-based app. What a better example than to give you a list of good chicken wing restaurants listed in order of proximity? Anyway,
    that&#x2019;s the inspiration for the demo. Dietary recommendation is not implied, BTW.</p>
</blockquote>
The API Controller Class
<p>With all the EF plumbing and domain models complete, the last step in the API layer is to create the API controller itself. I simply add a new Web API controller to the <strong>Controllers</strong>  folder, and change the code to make use of the plumbing
  work I&#x2019;ve completed up to now. The dialog below shows the first step, when I create a new <strong>LocationController</strong>  Web API controller.</p>
<p>
  <a href="/Media/Default/Windows-Live-Writer/ac6f2c93769a_B101/8_2.png">
    <img alt="8" src="/posts/ef-spatial-and-webapi/media/8_thumb.png">
  </a> 
</p>
<p>This controller has one method, that takes the latitude and longitude from a client. Those values are then used in conjunction with EF Spatial&#x2019;s
  <a href="http://msdn.microsoft.com/en-us/library/system.data.spatial.dbgeography.distance.aspx">DbGeography.Distance</a>  method to sort the records from closest in proximity, then the first five records are returned. The result of this call is that the closest five locations are returned with a client provides its latitude and longitude coordinates
  to the API method. The
  <a href="http://msdn.microsoft.com/en-us/library/system.data.spatial.dbgeography.distance.aspx">Distance method</a>  is used again to determine how far away each location is from the provided coordinates. The results are then returned using the API-specific class rather than the EF-specific class (thereby separating the two layers and easing some
  of the potential serialization issues that could arise), and the whole output is formatted to either XML or JSON and sent down the wire via HTTP. </p>
<p>
  
</p>
<p>At this point, the API is complete and can be deployed to Azure directly from within Visual Studio 2012 using the great publishing features created by the Visual Studio publishing team (my buddy
  <a href="http://twitter.com/sayedihashimi">Sayed Hashimi</a>  loves to talk about this stuff, so ping him on Twitter if you have any questions or suggestions on this awesome feature-set). </p>
<h3>Calling the Location API using an HTML 5 Client</h3>
<p>In order to make this a more comprehensive sample, I&#x2019;ve added some HTML 5 client code and
  <a href="http://knockoutjs.com/">Knockout.js</a> -infused JavaScript code to the Home/Index.cshtml view that gets created by default with the ASP.NET MVC project template. This code makes use of the HTML 5 geospatial capabilities to read the user&#x2019;s current position. The latitude and
  longitude are then used to call directly the location API, and the results are rendered in the HTML client using a basic table layout. </p>
<p>
  
</p>
<p>The final step is to deploy the whole thing up to Azure Web Sites. This is something I wasn&#x2019;t able to do until last week, so I&#x2019;m super-stoked to be able to do it now and to share it with you on a demo site, the URL of which I&#x2019;ll hand out at the end of
  this post. </p>
<h3>One Last NuGet to Include</h3>
<p>Entity Framework Spatial has some new data types that add support for things like&#x2026; well... latitude and longitude, in this particular case. By default, these types aren&#x2019;t installed into a Azure instance, as they&#x2019;re part of the database SDK. Most times,
  those assemblies aren&#x2019;t needed on a web server, so by default you won&#x2019;t have them when you deploy. To work around this problem and to make Entity Framework Spatial work on the first try following your deployment to Azure, install the
  <a href="http://nuget.org/packages/microsoft.sqlserver.types">Microsoft.SqlServer.Types NuGet package</a>  into your project by typing <strong>install-package Microsoft.SqlServer.Types</strong>  in the Package Manager Console or by manually finding the package in the &#x201C;Manage NuGet References&#x201D; dialog. </p>
<blockquote>
  <p>Thanks to Scott Hunter for this extremely valuable piece of information, which I lacked the first time I tried to do this. This solution was so obvious I hid in my car with embarrassment after realizing how simple it was and that I even had to ask.
    NuGet, again, to the rescue!</p>
</blockquote>
<p>Once this package is installed, deploying the project to Azure will trigger automatic retrieval of that package, and the support for the location data types in SQL Server will be added to your site. </p>
Publishing from Visual Studio 2012 is a Breeze
<p>You&#x2019;ve probably seen a ton of demonstrations on how to do deployment from within Visual Studio 2012, but it never ceases to amaze me just how quick and easy the team has made it to deploy sites &#x2013; with databases &#x2013; directly up to Azure in so few, simple
  steps. To deploy to a site from within Visual Studio 2012, I just right-click the site and select &#x2013; get this &#x2013; <strong>Publish. </strong> The first dialog that opens gives me the option to import a publish settings file, which I downloaded earlier just
  after having created the site in the Azure portal. </p>
<p>
  <a href="/Media/Default/Windows-Live-Writer/ac6f2c93769a_B101/publish-1_4.png">
    <img alt="publish-1" src="/posts/ef-spatial-and-webapi/media/publish-1_thumb_1.png">
  </a> 
</p>
<p>Once the file is imported, I&#x2019;m shown the details so I have the chance to verify everything is correct, which I&#x2019;ve never seen it <em>not be, </em> quite frankly. I just click Next here to move on. </p>
<p>
  <a href="/Media/Default/Windows-Live-Writer/ac6f2c93769a_B101/publish-2_4.png">
    <img alt="publish-2" src="/posts/ef-spatial-and-webapi/media/publish-2_thumb_1.png">
  </a> 
</p>
<p>This next step is where all the magic happens that I&#x2019;ve been promising you&#x2019;d see. This screen, specifically the last checkbox (highlighted for enthusiasm), points to the database I created earlier in the first step when I initially created the &#x201C;site with
  database&#x201D; in the Azure portal. If I check that box, when I deploy the web site, <em>the database schema will be automatically created for me, and the seed data will be inserted and be there when the first request to the site is made</em> . All that,
  just by publishing the site! </p>
<p>
  <a href="/Media/Default/Windows-Live-Writer/ac6f2c93769a_B101/publish-3_2.png">
    <img alt="publish-3" src="/posts/ef-spatial-and-webapi/media/publish-3_thumb.png">
  </a> 
</p>
<p>Can you imagine anything more convenient? I mean seriously. I publish my site and the database is automatically created, seeded, and everything wired up for me using Entity Framework, with a minimal amount of code. Pretty much magic, right?</p>
Have at it!
<p>Now that the .NET 4.5 Framework is supported by Azure Web Sites, you can make use of these and other new features, many of which are discussed or demonstrated on at
  <a href="http://www.asp.net/vnext">www.asp.net&#x2019;s page set aside just on the topic of ASP.NET 4.5 awesomeness</a> . If you want to get started building your own location API&#x2019;s built on top of Entity Framework Spatial,
  <a href="http://bit.ly/windowsazuretrial">grab your very own Azure account here</a>, that offers all kinds of awesomeness for free. You can take the sample code for this blog, or copy the gists and tweak them however you want. </p>
<p>Happy Coding!</p>
