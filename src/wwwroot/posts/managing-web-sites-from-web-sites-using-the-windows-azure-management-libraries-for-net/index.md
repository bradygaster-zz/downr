---
title: Managing Web Sites from Web Sites using the Azure Management Libraries for .NET
slug: managing-web-sites-from-web-sites-using-the-windows-azure-management-libraries-for-net
author: 
lastModified: 2015-05-19 14:02:10
pubDate: 2014-01-25 10:22:24
categories: Azure
---

<p>I received an email from Hanselman this week, a forward of an email he received after posting his [much-appreciated and far too kind]
  <a>blog post</a>  on WAML. The email was from a community member experiencing a behavior when trying to use WAML to create web sites (or manage their existing sites) from code running on the server side from another Azure Web Site. I can imagine lots of
  user stories when a Web Site could be used with WAML:</p>
<ul>
  <li>I&#x2019;m a software-as-a-service application business owner. I want to give users a form that, once filled out and submitted, will take user input and create a new web site and copy my application&#x2019;s code into their web site
    </li><li>My web application needs to create a storage account when it first starts up
      </li><li>My web application needs to create a SQL Database when it first starts up</li>
</ul>
<p>Automation isn&#x2019;t limited to the desktop. With WAML you can pick and choose which areas you need to offer and install the appropriate NuGets and get up and running quickly. There are a few caveats, however, mostly deliberate design decisions based on the
  basic ideas of cryptography and data integrity. I spent a few hours this week talking to my good friends in the Web Sites team, along with my own awesome team in Azure Developer Experience, to work through some certificate-loading problems I was seeing
  in Web Sites. The ability to use a management certificate is <strong>pretty important</strong>  when programming against WAML (yes, AAD support is coming soon in WAML). I&#x2019;ve seen a
  <a>few</a> 
  <a>different</a>  forums mention similar issues. Given WAML makes use of certs, and sometimes using certs on the server side in the Web Site can be a little tricky, I thought a walk-through was in order.</p>
<h3>How Meta. A Web Site that Makes Web Sites.</h3>
<p>I&#x2019;ve created a Visual Studio 2013 solution, with an ASP.NET project in the solution, that I&#x2019;ll be using for this blog post.
  <a>The code for this site is on GitHub</a>, so go grab that first. The code in the single MVC controller shows you a list of the sites you have in your subscription. It also gives you the ability to create a new site. The results of this look like the
  code below. </p>
<p>
  <a>
    <img alt="SNAGHTML2c3c0b1" src="/posts/managing-web-sites-from-web-sites-using-the-windows-azure-management-libraries-for-net/media/da307491-8124-41d5-9947-843562295d80.png">
  </a> 
</p>
<p>Here&#x2019;s a snapshot of the code I&#x2019;m using in an MVC controller to talk to the Azure REST API using WAML. </p>
<p>There are a few areas that you&#x2019;ll need to configure, but I&#x2019;ve made all three of them <em>appSettings</em>  so it should be relatively easy to do. The picture below shows all of these variables. Once you edit these and work through the certificate-related
  setup steps below, you&#x2019;ll have your very own web site-spawning web site. You probably already have the first of these variables but if you don&#x2019;t,
  <a>what are you waiting for</a> ?</p>
<p>
  <a>
    <img alt="image" src="/posts/managing-web-sites-from-web-sites-using-the-windows-azure-management-libraries-for-net/media/59dafa5f-07cb-44ff-88c7-e30efd03867e.png">
  </a> 
</p>
<p>Once your Azure subscription ID is pasted in you&#x2019;ll need to do a little magic with certificates. Before we get to all the crypto-magic, here&#x2019;s the method that the controller calls that prepare WAML for usage by setting up an <strong>X509Certificate</strong> .
  </p>
<p>
  <a>
    <img alt="image" src="/posts/managing-web-sites-from-web-sites-using-the-windows-azure-management-libraries-for-net/media/24366bb2-970a-4986-91cc-69037450a947.png">
  </a> 
</p>
<p>I&#x2019;d been using a base 64 encoded string representation of the certificate, but that wouldn&#x2019;t work on top of Web Sites. Web Sites needs a real physical certificate <strong>file</strong> .Which makes sense &#x2013; you want for access to your subscription to be
  a difficult thing to fake, so this configuration you have to go through once to secure the communication? It&#x2019;s worth it. The code below then takes that credential and runs some calls to the WebSiteManagementClient object, which is a client class in
  the
  <a>Web Sites Management Package</a> . </p>
<p>
  <a>
    <img alt="image" src="/posts/managing-web-sites-from-web-sites-using-the-windows-azure-management-libraries-for-net/media/830b88be-259a-48f8-8a47-fbd256abb93f.png">
  </a> 
</p>
<p>This next part is all about cryptography, certificates, and moving things around properly. It&#x2019;s not too complicated or deep into the topics, just a few steps you should know just in case you need to do this again. </p>
<blockquote>
  <p>Don&#x2019;t worry. If it were complicated, you wouldn&#x2019;t be reading about it here. </p>
</blockquote>
<h3>Creating a Self-Signed Cert and Using the PFX and CER Files Properly with Web Sites</h3>
<p>I&#x2019;ll run through these steps pretty quickly, with pictures. There are
  <a>many</a> 
  <a>other</a> 
  <a>great</a> 
  <a>resources</a>  online on how to create certificates so I&#x2019;m not going to go into great detail. This section has three goals:</p>
<ol>
  <li>Create a self-signed certificate
    </li><li>Create a *.CER file that I can use to upload to the Azure portal as a management certificate
      </li><li>Use the *.PFX file I created on the way to creating my *.CER file on my web site</li>
</ol>
<p>To create the self-signed cert open up IIS Manager (some would prefer to do this using
  <a>makecert.exe</a> ) and click the <strong>Server Certificates </strong> feature. </p>
<p>
  <a>
    <img alt="SNAGHTML2649c9d" src="/posts/managing-web-sites-from-web-sites-using-the-windows-azure-management-libraries-for-net/media/90c8b240-2a1d-4585-a816-165184c31a42.png">
  </a> 
</p>
<p>Then, click the <strong>Create Self-Signed Certificate</strong>  action link. </p>
<p>
  <a>
    <img alt="SNAGHTML267808a" src="/posts/managing-web-sites-from-web-sites-using-the-windows-azure-management-libraries-for-net/media/73d41b16-344f-41cc-86cb-db29317551cb.png">
  </a> 
</p>
<p>You get to walk through a wizard:</p>
<p>
  <a>
    <img alt="SNAGHTML269d26a" src="/posts/managing-web-sites-from-web-sites-using-the-windows-azure-management-libraries-for-net/media/d5e06328-49ab-4118-bfb8-e1b69ec2f64a.png">
  </a> 
</p>
<p>Then the new certificate will appear in the list:</p>
<p>
  <a>
    <img alt="image" src="/posts/managing-web-sites-from-web-sites-using-the-windows-azure-management-libraries-for-net/media/7270f81f-8412-44fe-89c2-789bf4a2078e.png">
  </a> 
</p>
<p>Select it and click the <strong>Export</strong>  action link:</p>
<p>
  <a>
    <img alt="SNAGHTML26c6682" src="/posts/managing-web-sites-from-web-sites-using-the-windows-azure-management-libraries-for-net/media/c9b809da-7b87-4a96-965f-9007bc633469.png">
  </a> 
</p>
<p>Now that you&#x2019;ve got the PFX file exported, it&#x2019;d be a good time to drop that into the web site. Drop the PFX file into the <strong>App_Data </strong> folder&#x2026;</p>
<p>
  <a>
    <img alt="image" src="/posts/managing-web-sites-from-web-sites-using-the-windows-azure-management-libraries-for-net/media/eed5822c-b88f-4906-b493-150b0734460f.png">
  </a> 
</p>
<p>Once the .PFX is in the App_Data folder, copy it&#x2019;s location into the <strong>Web.Config</strong>  or in the portal&#x2019;s <strong>configure </strong> tab. </p>
<p>
  <a>
    <img alt="image" src="/posts/managing-web-sites-from-web-sites-using-the-windows-azure-management-libraries-for-net/media/2d6c276f-6727-4726-b489-75155202916a.png">
  </a> 
</p>
<p>Double-click the PFX file. Run through the subsequent steps needed to import the PFX into the personal certificate store. Once the wizard completes you&#x2019;ll have the certificate installed, so the final step will be to export it. Open up your user certificates
  tile. I always find mine using the new Modern Tiles method.</p>
<p>
  <a>
    <img alt="image" src="/posts/managing-web-sites-from-web-sites-using-the-windows-azure-management-libraries-for-net/media/bfdfc0b3-b4a8-4c9d-9d89-b1273e5daca8.png">
  </a> 
</p>
<p>Open up the file in the user certificate manager, and select the new certificate just created. Select the <strong>Export</strong>  context menu.</p>
<p>
  <a>
    <img alt="image" src="/posts/managing-web-sites-from-web-sites-using-the-windows-azure-management-libraries-for-net/media/705c6685-4c0a-433b-a544-e8b8a8f6ffdf.png">
  </a> 
</p>
<p>Select the DER option. This is when you&#x2019;ll output a CER file that can be used as your management certificate in the next step. </p>
<p>
  <a>
    <img alt="SNAGHTML28b0253" src="/posts/managing-web-sites-from-web-sites-using-the-windows-azure-management-libraries-for-net/media/63b5d8d3-438f-4c43-8db5-414288353c6d.png">
  </a> 
</p>
<p>Save the output *.CER file on your desktop. With the PFX file set up in the web site and this file created, we&#x2019;re almost finished.</p>
<h3>Uploading the Management Cert to the Portal</h3>
<p>With the CER file ready, all one needs to do to upload it is to go to the Management Portal. So long as the web site you&#x2019;re running WAML <strong>in</strong>  is trying to access resources in the same subscription, everything should work. Go to the management
  portal, select <strong>Settings </strong> from the navigation bar, and then select the <strong>Management Certificates</strong>  navigation bar.Click the Upload button to upload the <strong>*.CER file only. <em>NOT </em> the PFX, yet!</strong> </p>
<p>
  <a>
    <img alt="image" src="/posts/managing-web-sites-from-web-sites-using-the-windows-azure-management-libraries-for-net/media/18ca6bc6-006c-443a-a130-858d4cfe85e0.png">
  </a> 
</p>
<p>Once the CER is uploaded it&#x2019;ll appear in the list of management certificates.</p>
<p>
  <a>
    <img alt="image" src="/posts/managing-web-sites-from-web-sites-using-the-windows-azure-management-libraries-for-net/media/2014cab4-8e75-4e6c-a791-0f42d5adb2b1.png">
  </a> 
</p>
<p>With those configuration changes in place, I can finish the configuration by adding the password for the PFX to the Web.Config file. This part isn&#x2019;t perfect, but it&#x2019;s just to get you started with the difficult connecting-of-the-dots that can occur when
  embarking on a new feature or prototype. </p>
<p>
  <a>
    <img alt="image" src="/posts/managing-web-sites-from-web-sites-using-the-windows-azure-management-libraries-for-net/media/b99a0f0b-d111-4c82-bb20-5b4a4b3b51f5.png">
  </a> 
</p>
<h3>Deploying the Site</h3>
<p>The last step, following the configuration and certificates being set up, is to deploy the site. I can do that from right within Visual Studio using the publish web features. Here, I&#x2019;m just creating a new site. </p>
<p>
  <a>
    <img alt="SNAGHTML2a45cae" src="/posts/managing-web-sites-from-web-sites-using-the-windows-azure-management-libraries-for-net/media/82078571-9c9a-439b-a7fc-c7a95a0bdf62.png">
  </a> 
</p>
<p>Once the site deploys and loads up in a browser, you can see what capabilities it&#x2019;ll offer &#x2013; the simple creation of other Azure Web Sites. </p>
<p>
  <a>
    <img alt="SNAGHTML2c01155" src="/posts/managing-web-sites-from-web-sites-using-the-windows-azure-management-libraries-for-net/media/b1da4ac4-6a13-42d3-bcdd-fd2d3efcdc05.png">
  </a> 
</p>
<h3></h3>
<h3>Summary</h3>
<p>This article covers more how to prepare a web site with the proper certificate setup and contains code that explains the actual functional code. I&#x2019;d welcome you to take a look at the repository, submit questions in the comments below, or even fork the
  repository and come up with a better way, or to add features, whatever you think of. Have fun, and happy coding!</p>
