---
title: Managing Multiple Azure Web Site Environments using Visual Studio Publishing Profiles
slug: managing-multiple-windows-azure-web-site-environments-using-visual-studio-publishing-profiles
author: bradygaster
lastModified: 2014-12-31 08:38:40
pubDate: 2012-11-23 07:10:36
categories: Azure
---

<p>This is the second post in the
  <a>Real World Problems with Azure Web Sites</a> . The first post summarized how one can
  <a>manage multiple environments (development, staging, production, etc) using a Git repository with a branching strategy</a> . Not everyone wants to use Git, and most would prefer to stay in their favorite IDE &#x2013; Visual Studio 2012 &#x2013; all day to do pretty
  much everything. My buddy
  <a>Sayed Hashimi</a>  told me about Visual Studio profiles a few weeks ago and I&#x2019;d been wanting to write up something on how it could work with Azure Web Sites. This post follows up on the idea of managing multiple Azure Web Sites, but rather than do it
  with Git, I&#x2019;ll show you how to manage multiple sites with only Visual Studio&#x2019;s awesome publishing-with-profiles features. </p>
Set Up the Environments
<p>The first step in the process is to have your multiple sites set up so that you have environmental isolation. In this case, I&#x2019;m being thorough and requiring there are two gates prior to production release. All three of these sites are in the free zone,
  for this demonstration. </p>
<p>
  <a>
    <img alt="01-sites-provisioned" src="/posts/managing-multiple-windows-azure-web-site-environments-using-visual-studio-publishing-profiles/media/01-sites-provisioned_thumb.png">
  </a> 
</p>
<p>If this was fully realistic, the production zone would probably be at <em>least</em>  shared or reserved, so that it had a domain name mapped to it. That&#x2019;s the only site that would cost money, so the development and staging sites would have no impact on
  the cost I&#x2019;ll incur for this setup. </p>
<p>Once the sites have been created I&#x2019;ll go into each site&#x2019;s dashboard to download the site&#x2019;s publish settings profile. The publish settings files will be used from within Visual Studio to inform the IDE how to perform a web deploy up to my Azure Web Site
  environment.</p>
<p>
  <a>
    <img alt="02-download-publish-profile" src="/posts/managing-multiple-windows-azure-web-site-environments-using-visual-studio-publishing-profiles/media/02-download-publish-profile_thumb.png">
  </a> 
</p>
<p>Once I&#x2019;ve downloaded each of these files I&#x2019;ll have them all lined up in my downloads folder. I&#x2019;ll be using these files in a moment once I&#x2019;ve got some code written for my web site. </p>
<p>
  <a>
    <img alt="SNAGHTMLbb9ed58" src="/posts/managing-multiple-windows-azure-web-site-environments-using-visual-studio-publishing-profiles/media/SNAGHTMLbb9ed58_thumb.png">
  </a> 
</p>
<p>Now that I&#x2019;ve got all my environments set up and have the publishing settings downloaded I can get down to business and write a little code. </p>
Setting up the Web Application Project
<p>I know I&#x2019;ll have some environmental variances in the deployment details of this web application. I&#x2019;ll want to use different databases for each environment, so I&#x2019;ll need to have three different connection strings each site will have to be configured to
  use for data persistence. There&#x2019;ll be application settings and <em>details</em>  and stuff, so the first thing I&#x2019;ll do in this simple ASP.NET MVC project is to prepare the different publishing profiles and the respective configuration for those environments.
  </p>
<p>To do this, I&#x2019;ll just right-click my web project and select the <strong>Publish</strong>  menu item. I&#x2019;m not going to publish anything just yet, but this is the super-easiest way of getting to the appropriate dialog. </p>
<p>
  <a>
    <img alt="image" src="/posts/managing-multiple-windows-azure-web-site-environments-using-visual-studio-publishing-profiles/media/image_thumb.png">
  </a> 
</p>
<p>When the publishing dialog opens, I&#x2019;ll click the <strong>Import</strong>  button to grab the first environment&#x2019;s publish settings files. </p>
<p>
  <a>
    <img alt="SNAGHTMLbc3d1f0" src="/posts/managing-multiple-windows-azure-web-site-environments-using-visual-studio-publishing-profiles/media/SNAGHTMLbc3d1f0_thumb.png">
  </a> 
</p>
<p>I&#x2019;ll grab the first publish settings file I find in my downloads folder, for the site&#x2019;s development environment. </p>
<p>
  <a>
    <img alt="SNAGHTMLbc4eff3" src="/posts/managing-multiple-windows-azure-web-site-environments-using-visual-studio-publishing-profiles/media/SNAGHTMLbc4eff3_thumb.png">
  </a> 
</p>
<p>Once I click <strong>Open</strong>, the wizard will presume I&#x2019;m done and advance to the next screen. I&#x2019;ll click the <strong>Profile</strong>  link in the navigation bar at this point one more time, to go back to the first step in the wizard.
  
</p>
<blockquote>
  <p>
    If, at any point during this process you&#x2019;re asked if you want to saved the profile, click <strong>yes</strong> . 
  </p>
</blockquote>
<p>
  <a>
    <img alt="SNAGHTMLbc7a83d" src="/posts/managing-multiple-windows-azure-web-site-environments-using-visual-studio-publishing-profiles/media/SNAGHTMLbc7a83d_thumb.png">
  </a> 
</p>
<p>I&#x2019;ll repeat the import process for the staging and production files. The idea here is, to get all of the publish settings files imported as separate profiles for the same Visual Studio web application project. Once I&#x2019;ve imported all those files I&#x2019;ll click
  the <strong>Manage Profiles</strong>  button. The dialog below should open up, which will show me all of the profiles I&#x2019;ve imported. </p>
<p>
  <a>
    <img alt="SNAGHTMLbcad50b" src="/posts/managing-multiple-windows-azure-web-site-environments-using-visual-studio-publishing-profiles/media/SNAGHTMLbcad50b_thumb.png">
  </a> 
</p>
<p>This part isn&#x2019;t a requirement for you or a recommendation, but I don&#x2019;t typically need the FTP profile so I&#x2019;ll go through and delete all of the *FTP profiles that were imported. Again, not a requirement, just a preference, but once I&#x2019;m done with it I&#x2019;ll
  have all the web deploy profiles left in my dialog. </p>
<p>
  <a>
    <img alt="SNAGHTMLbcc102b" src="/posts/managing-multiple-windows-azure-web-site-environments-using-visual-studio-publishing-profiles/media/SNAGHTMLbcc102b_thumb.png">
  </a> 
</p>
<p>I&#x2019;ll just click <strong>Close</strong>  now that I&#x2019;ve got the profiles set up. Now that the profiles are setup they&#x2019;ll be visible under the <strong>Properties/PublishProfiles</strong>  project node in Visual Studio. This folder is where the XML files containing
  publishing details are stored. </p>
<p>
  <a>
    <img alt="image" src="/posts/managing-multiple-windows-azure-web-site-environments-using-visual-studio-publishing-profiles/media/image_thumb_1.png">
  </a> 
</p>
<p>With the profile setup complete, I&#x2019;m going to go ahead and set up the configuration specifics for each environment. By right-clicking on each <em>*.pubxml</em>  file and selecting the <strong>Add Config Transform</strong>  menu item, a separate <em>*.config</em>   will be created in the project. </p>
<p>
  <a>
    <img alt="image" src="/posts/managing-multiple-windows-azure-web-site-environments-using-visual-studio-publishing-profiles/media/image_thumb_2.png">
  </a> 
</p>
<p>Each file represents the transformations I&#x2019;ll want to do as I&#x2019;m deploying the web site to the individual environment sites. Once I&#x2019;ve added a configuration transformation for each profile, there&#x2019;ll be a few nodes under the Web.config file I&#x2019;ll have the
  opportunity of configuring specific details for each site. </p>
<p>
  <a>
    <img alt="image" src="/posts/managing-multiple-windows-azure-web-site-environments-using-visual-studio-publishing-profiles/media/image_thumb_3.png">
  </a> 
</p>
<p>Now that I&#x2019;ve got the publish profiles and their respective configuration transformation files set up for each profile, I&#x2019;ll write some code to make use of an application setting so I can check to make sure the per-profile deployment does what I think
  it&#x2019;ll do. </p>
<blockquote>
  <p>Now, if you&#x2019;re thinking to yourself this isn&#x2019;t very practical, since I couldn&#x2019;t allow my developers to have the ability of deploying to production and you&#x2019;re compelled to blow off the rest of this post since you feel I&#x2019;ve completely jumped the shark
    at this point, keep on reading. I bring it back down to Earth and even talk a little release-management process later on. </p>
</blockquote>
Environmental Configuration via Profiles
<p>Now I&#x2019;ll go into the Web.config file and add an <em>appSetting</em>  to the file that will reflect the message I want users to see whenever they browse to the home page. This setting will be specific per environment, so I&#x2019;ll use the transformation files
  in a moment to make sure each environment has its very own welcome message.</p>
<p>
  <a>
    <img alt="image" src="/posts/managing-multiple-windows-azure-web-site-environments-using-visual-studio-publishing-profiles/media/image_thumb_5.png">
  </a> 
</p>
<p>This is the message that would be displayed to a user if they were to hit the home page of the site. I need to add some code to my controller and view to display this message. It isn&#x2019;t very exciting code, but I&#x2019;ve posted it below for reference. </p>
<p>First, the controller code that reads from configuration and injects the message into the view. </p>
<p>
  <a>
    <img alt="image" src="/posts/managing-multiple-windows-azure-web-site-environments-using-visual-studio-publishing-profiles/media/image_thumb_6.png">
  </a> 
</p>
<p>Then I&#x2019;ll add some code to the view to display the message in the browser. </p>
<p>
  <a>
    <img alt="image" src="/posts/managing-multiple-windows-azure-web-site-environments-using-visual-studio-publishing-profiles/media/image_thumb_7.png">
  </a> 
</p>
<p>When I browse the site I&#x2019;ll get the obvious result, a simple hello message rendered from the configuration file on my local machine. </p>
<p>
  <a>
    <img alt="SNAGHTMLd3eb220" src="/posts/managing-multiple-windows-azure-web-site-environments-using-visual-studio-publishing-profiles/media/SNAGHTMLd3eb220_thumb.png">
  </a> 
</p>
<p>I&#x2019;ll go into the development configuration profile file and make a few changes &#x2013; I strip out the comments and stuff I don&#x2019;t need, and then I add the <em>message</em>  <em>appSetting</em>  variable to the file and set the transformation to perform a replace
  when the publish happens. This basically replaces everything in the <em>Web.config</em>  file with everything in the <em>Web.MySite-Dev - Web Deploy.config</em>  file that has a <em>xdt:Transform </em> attribute set to <em>Replace</em> . </p>
<p>
  <a>
    <img alt="image" src="/posts/managing-multiple-windows-azure-web-site-environments-using-visual-studio-publishing-profiles/media/image_thumb_8.png">
  </a> 
</p>
<p>I do the same thing for the staging profile&#x2019;s configuration file&#x2026;</p>
<p>
  <a>
    <img alt="image" src="/posts/managing-multiple-windows-azure-web-site-environments-using-visual-studio-publishing-profiles/media/image_thumb_9.png">
  </a> 
</p>
<p>&#x2026; and then for the production profile&#x2019;s configuration file. </p>
<p>
  <a>
    <img alt="image" src="/posts/managing-multiple-windows-azure-web-site-environments-using-visual-studio-publishing-profiles/media/image_thumb_10.png">
  </a> 
</p>
<p>With the environmentally-specific configuration attributes set up in the profile transformations and the publish profiles set up, everything should work whenever I need to do a deployment to any of the environments. Speaking of which, let&#x2019;s wrap this
  up with a few deployments to our new environments!</p>
Deployment
<p>The final step will be to deploy the code for the site into each environment to make sure the profile configuration is correct. This will be easy, since I&#x2019;ve already imported all of my environments&#x2019; configuration files. I&#x2019;ll deploy development first by
  right-clicking the project and again selecting the <strong>Publish</strong>  context menu item. When the publish wizard opens up I need to select the development environment&#x2019;s profile from the menu. </p>
<p>
  <a>
    <img alt="SNAGHTMLd55a0ea" src="/posts/managing-multiple-windows-azure-web-site-environments-using-visual-studio-publishing-profiles/media/SNAGHTMLd55a0ea_thumb.png">
  </a> 
</p>
<p>Once the publish process completes the site will open up in my browser and I can see that the appropriate message is being displayed, indicating the configuration transformation occurred properly based on the publish profile I&#x2019;d selected to deploy. </p>
<p>
  <a>
    <img alt="SNAGHTMLd572671" src="/posts/managing-multiple-windows-azure-web-site-environments-using-visual-studio-publishing-profiles/media/SNAGHTMLd572671_thumb.png">
  </a> 
</p>
<p>Next, I right-click the project and select Publish again, this time selecting the staging environment. </p>
<p>
  <a>
    <img alt="SNAGHTMLd585116" src="/posts/managing-multiple-windows-azure-web-site-environments-using-visual-studio-publishing-profiles/media/SNAGHTMLd585116_thumb.png">
  </a> 
</p>
<p>When the publish completes, the staging welcome message is displayed. </p>
<p>
  <a>
    <img alt="SNAGHTMLd59b886" src="/posts/managing-multiple-windows-azure-web-site-environments-using-visual-studio-publishing-profiles/media/SNAGHTMLd59b886_thumb.png">
  </a> 
</p>
<p>If I repeat the same steps for production, the appropriate message is displayed there, too. </p>
<p>
  <a>
    <img alt="SNAGHTMLd5aeeb4" src="/posts/managing-multiple-windows-azure-web-site-environments-using-visual-studio-publishing-profiles/media/SNAGHTMLd5aeeb4_thumb.png">
  </a> 
</p>
<p>In a few short steps, I&#x2019;m able to set up a series of environments and publish profiles, that work together to allow me separate deployment environments, with little extra work or overhead. Since the profiles are linked to the configuration transformations
  explicitly, it all just works when I deploy the site. </p>
Release Management
<p>As promised earlier in that <em>blockquote</em>  up there, I want to stay with the &#x201C;these are real world scenarios as much as possible based on my real-world experiences and questions I&#x2019;ve been asked&#x201D; mantra, I feel it&#x2019;s necessary to get into the idea
  of release management insomuch as how it&#x2019;d apply here. In the previous example I was using Git branches to gate releases. In this example, I&#x2019;m not using any centralized build solution, but rather assuming there&#x2019;s a source control environment in between
  the team members &#x2013; developers, testers, release management, and so on &#x2013; but that the whole team just chooses to use the Web Deploy awesomesauce built into Visual Studio. </p>
<p>Think of a company with aggressive timelines but who still take care to gate releases but choose not (for whatever reason) to set up a centralized build system. This company still feels strongly about managing the release process and about maintaining
  separate chains of testing and signoff responsibility as code is moved through the environments on the way to a production release, but they love using Visual Studio and Web Deploy to get things into the environments as quickly as possible.&#xA0; </p>
<p>The diagram below demonstrates one potential release cycle that could make use of the publish profile method of gating deployments through a series of environmental gates.</p>
<p>
  <a>
    <img alt="deployment-strategy" src="/posts/managing-multiple-windows-azure-web-site-environments-using-visual-studio-publishing-profiles/media/deployment-strategy_thumb.png">
  </a> 
</p>
<p>Assume the team has come to a few conclusions and agreements on how their release cycle will execute. </p>
<ul>
  <li>All the team members are relatively technical and comfortable using Visual Studio with web application projects </li>
  <li>The team uses a source control method to share source code and to distribute it internally between team members </li>
  <li>The web application project checked into source control has with it the publish profile for deploying the site into the development Azure Web Site </li>
  <li>Testers maintain copies of the staging publish profile setting, are regarded as the owners of the staging environment, and are the only team members who can deploy code to the staging Azure Web Site </li>
  <li>Release managers maintain copies of the production publish settings files, are regarded as the owners of the production releases, and are the only team members who can deploy code to the production environment </li>
  <li>As developers, testers, and RM&#x2019;s complete their respective testing phases in the environments they own and are ready to sign off, they escalate the deployment process to the next level </li>
  <li>Following escalation, the first general step is to test the previous environment for verification purposes, then to deploy to the next environment and to begin testing of the deployment in that environment
    <br>
  </li>
</ul>
<p>Luckily, this sort of situation is quite possible using publish profiles and free Azure Web Sites used as environmental weigh stations on the way to be deployed to a production site that&#x2019;s deployed to multiple large reserved instances (for instance).
  </p>
Summary
<p>The convenient partnership between web publishing and Azure Web Sites shouldn&#x2019;t be regarded as an indicator of it creating the potential for cowboy coding, but more considered a tool that when coupled with a responsible release cycle and effective deployment
  gating can streamline and simplify the entire SDLC when your business is web sites. </p>
<p>I hope this post has introduced you to a method of controlling your deployment environments, while also allowing you to do the whole thing from within Visual Studio. Later on, I&#x2019;ll follow up this post with an example of doing this sort of thing using
  Team Foundation Services. </p>
<p>Hopefully, you have enough ammunition to
  <a>get started with your very own Azure Web Site account today, for free</a>, and you feel confident you&#x2019;ll be able to follow your very own release management process, without the process or architecture slowing you down. If you have any questions about
  this approach or the idea in general, feel free to use the comments form below. </p>
<p>Happy coding!</p>
