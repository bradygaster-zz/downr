---
title: Getting Started with the Azure Management Libraries for .NET
slug: getting-started-with-the-windows-azure-management-libraries
author: 
lastModified: 2015-09-25 16:35:01
pubDate: 2013-10-22 04:35:00
categories: Azure
---

<p>The first thing I had the opportunity to work on when I joined the Azure team was something that I&#x2019;m excited to show off today. I demonstrated the early bits of the Azure Management Libraries at the
  <a href="http://channel9.msdn.com/Events/TechEd/Australia/2013/KOS002">TechEd Australia Developer kick-off session</a>, and now that they&#x2019;re out I&#x2019;m really excited to walk you through getting started with their use. This post will sum up what the Azure Management Libraries are and why you should care to take a peek at
  them, and then I&#x2019;ll dive into some code to show you how to get started. </p>
What are these libraries you speak of?
<p>With this release, a broad surface area of the Azure cloud infrastructure can be accessed and automated using the same technology that was previously available only from the Azure PowerShell Cmdlets or directly from the REST API. Today&#x2019;s initial preview
  includes support for hosted Cloud Services, Virtual Machines, Virtual Networks, Web Sites, Storage Accounts, as well as infrastructure components such as affinity groups. </p>
<p>We&#x2019;ve spent a lot of time designing natural .NET Framework APIs that map cleanly to their underlying REST endpoints. It was very important to expose these services using a modern .NET approach that developers will find familiar and easy to use: </p>
<ul>
  <li>Supports Portable Class Library (PCL), which targets apps that are built for .NET Framework 4.5, Windows Phone 8, Windows Store, and Silverlight
    </li><li>Ships as a set of focused NuGet packages with minimal dependencies to simplify versioning
      </li><li>Supports async/await-based task asynchrony (with easy synchronous overloads)
        </li><li>Has a shared infrastructure for common error handling, tracing, configuration, and HTTP pipeline manipulation
          </li><li>Is factored for easy testability and mocking
            </li><li>Is built on top of popular libraries such as HttpClient and Json.NET</li>
</ul>
<p>These packages open up a rich surface area of Azure services, giving you the power to automate, deploy, and test cloud infrastructure with ease. These services support Azure Virtual Machines, Hosted Services, Storage, Virtual Networks, Web Sites and core
  data center infrastructure management. </p>

Getting Started
<p>As with any good SDK, it helps to know how you could get started using it by taking a look at some code. No code should ever be written to solve a problem that doesn&#x2019;t exist, so let&#x2019;s start with a decent, but simple, problem statement:</p>
<blockquote>
  <p>I have this process I run as in Azure as a Worker Role. It runs great, but the process it deals with really only needs to be run a few times a week. It&#x2019;d be great if I could set up a new service, deploy my code to it, then have it run. Once the process
    finishes it&#x2019;d be even better if the service could &#x201C;phone home&#x201D; so it could be deleted automatically. I sometimes forget to turn it off when I&#x2019;m not using it, and that can be expensive. It&#x2019;d be great if I could automate the creation of what I need,
    then let it run, then have it self-destruct. </p>
</blockquote>
<p>Until this preview release of the Azure Management Libraries (WAML for short hereafter, though this is <em>not </em> an official acronym, I&#x2019;m just being lazy), this wasn&#x2019;t very easy. There&#x2019;ve been some great open-source contributions to answering the .NET
  layer in managing Azure services and their automation, but nothing comprehensive that delivers C# wrappers for nearly all of the Azure Management REST APIs. If you needed to use these API to generate your own &#x201C;stuff&#x201D; in Azure, you pretty much had to
  write your own HTTP/XML code to communicate with the REST API. Not fun. Repetitive. Boring, maybe, after you do a few dozen out of hundreds of API methods. </p>
<h3>Getting the Management Libraries</h3>
<p>I decided to do this work in a simple WPF application I&#x2019;ll run on my desktop for the time being. I&#x2019;ll want to run it as long-running app or service later, but for now this will work just fine. Since I&#x2019;ve got a Azure Cloud Service with a Worker Role I&#x2019;ll
  want to run in the cloud, I&#x2019;ve just added all three projects to a single solution, which you&#x2019;ll see below. </p>
<p>
  <img>
</p>
<p>You probably noticed that I&#x2019;m preparing to add some NuGet packages to the WPF application. That&#x2019;s because all of the Azure Management Libraries are available as individual NuGet packages. I&#x2019;m going to select the
  <a href="http://www.nuget.org/packages/Microsoft.WindowsAzure.Management.Libraries">Microsoft.WindowsAzure.Management.Libraries</a>  package, as that one will pull everything in the Management Libraries into my project. If I wanted to manage one aspect of Azure rather than all of it, I&#x2019;d reference one of the more specific packages,
  like
  <a href="http://www.nuget.org/packages/Microsoft.WindowsAzure.Management.WebSites">Microsoft.WindowsAzure.Management.WebSites</a>, which provides management functionality specific only to the Azure Web Sites component. </p>
<p>
  <img>
</p>
<p>Once I&#x2019;ve referenced the NuGet packages, I&#x2019;m ready to set up client authentication between my WPF application and the Azure REST APIs. </p>
<h3>Authenticating</h3>
<p>The first implementation we&#x2019;ve built out for authenticating users who are using WAML and Azure is a familiar one &#x2013; using X509 Certificates. Integrated sign-in was added recently in SDK 2.2 to Visual Studio and to PowerShell, and we&#x2019;re working on a solution
  for this in WAML, too. With this first preview release we&#x2019;re shipping certificate authentication, but stay tuned, we&#x2019;re doing our best to add in additional functionality. </p>
<blockquote>
  <p>Don&#x2019;t panic. We&#x2019;ve made this so easy even I can do it. </p>
</blockquote>
<p>I&#x2019;m not going to go deep into a discussion of using certificate-based authentication in this post. In fact, I&#x2019;m going to be as brute-force as possible just to move into the functional areas of this tutorial. I&#x2019;ll need two pieces of information to be able
  to log into the Azure API:</p>
<ul>
  <li>A subscription ID
    </li><li>A management certificate</li>
</ul>
<p>I obtained these values from one of my publish settings files. The XML for this file is below. </p>
<p>
  <img>
</p>
<p>With the key and the subscription ID in my code later on, I can call the <strong>GetCredentials </strong> method below that returns an instance of the abstract class, <strong>SubscriptionCloudCredentials</strong>, we&#x2019;re using to represent a credential
  instance in the Management Library code. That way, if I add single-sign on later it&#x2019;ll be easy for me to replace the certificate authentication with something else. The code the the <strong>CertificateAuthenticationHelper</strong>  class from my sample
  code is below:</p>
<p>
  <img>
</p>
<p>Now I&#x2019;ll write a controller class that&#x2019;ll do the work between my WPF application and the Management Libraries &#x2013; a convenience layer, in a sense. </p>
<h3>Management Convenience Layer</h3>
<p>To map out all the various parameters I&#x2019;ll have in my workflow I&#x2019;ve created the <strong>ManagementControllerParameters</strong>  class shown below. This class will summarize all of the pieces of data I&#x2019;ll need to create my services and deploy my code.</p>
<p>
  <img>
</p>
<p>Then, I&#x2019;ll create a class that will provide convenience functionality between the UX code and the Management Library layer. This code will make for cleaner code in the UX layer later on. Note the constructor of the code below. In it, two clients are being
  created. One, the <strong>StorageManagementClient</strong>, will provide the ability for me to manage storage accounts. The other, the <strong>ComputeManagementClient</strong>, provides the ability for me to work with most of the Azure compute landscape
  &#x2013; hosted services, locations, virtual machines, and so on. </p>
<p>For the purposes of explaining these steps individually, I&apos;ve created a partial class named <strong>ManagementController </strong> that&apos;s spread across multiple files. This just breaks up the code into functional units to make it easier to explain in this
  post, and to provide for you as a
  <a href="https://gist.github.com/bradygaster/4822fc0beebb7226af22">public Gist</a>  so that you can clone all the files and use them in your own code. </p>
<p>
  <img>
</p>
<p>Now, let&#x2019;s wire up some management clients and do some work. </p>
<h3>Create a New Storage Account using the Storage Management Client</h3>
<p>The first thing I&#x2019;ll need in my deployment strategy is a storage account. I&#x2019;ll be uploading the .cspkg file I packaged up from a Cloud project in Visual Studio into a Azure blob. Before I can do that, I&#x2019;ll need to create an account into which that package
  file can be uploaded. The code below will create a new storage account in a specified region. </p>
<p>
  <img>
</p>
<p>Once the storage account has finished creating, I&apos;m ready to use it. Given that I&apos;ll need a connection string to connect my application (and my soon-to-be-created cloud service) to the storage account, I&apos;ll create a method that will reach out to the Azure
  REST APIs to get the storage account&apos;s connection keys. Then, I&apos;ll build the connection string and hand it back to the calling code. </p>
<p>
  <img>
</p>
<p>Now that the storage account has been created I&apos;ll create my cloud service and publish my package up to Azure.</p>
<h3>Create and Deploy a new Cloud Service using the Compute Management Client</h3>
<p>The call to create a cloud service is surprisingly simple. All I need to do is to provide the name of the cloud service I intend on creating and the region in which I&apos;d like it to be created. </p>
<p>
  <img>
</p>
<p>Finally, all I need to do to deploy the cloud service is to upload the cloud service package file I created in Visual Studio to a blob, then call the REST API. That call will consist of the blob URI of the package I uploaded to my storage account, and
  the XML data from the cloud project&apos;s configuration file. This code will make use of the
  <a href="http://www.nuget.org/packages/windowsazure.storage">Azure Storage SDK, which is also available as a NuGet package</a> . </p>
<p>
  <img>
</p>
<p>Now that all the code&apos;s written to create my Azure application, I&apos;ll write some code to destroy it once it wraps up all of the work it was designed to do. </p>
<h3>Deleting Assets from Azure</h3>
<p>Deleting assets using the Azure Management Libraries is as easy as creating the assets. The code below cleans up the storage account I created. Then, it deletes the cloud service deployment and the cloud service altogether. </p>
<p>
  <img>
</p>
<p>With all the convenience code written at this point, the user experience code should be relatively painless to write next. </p>
<h3>The User Experience</h3>
<p>The UX for this application is relatively simplistic. I&apos;m just providing a pair of buttons on a WPF form. One will create the assets I need in Azure and perform the deployment. The other will delete the assets from Azure. XAML code for this UX is below.
  It isn&apos;t much to look at but the idea here is to keep this simple.</p>
<p>
  <img>
</p>
<p>The codebehind for the UX is also just as easy. In the Create button-click event, I create a new <strong>ManagementController</strong>  instance, providing it all of the parameters I&apos;ll need to create the application&apos;s components in the Azure fabric. Then
  I call all of the methods to created everything. </p>
<p>I also handle the Delete button-click by cleaning up everything I just created. </p>
<p>
  <img>
</p>
<p>I could modify this code to use the Windows Storage SDK to watch a storage queue on the client side. When the cloud service is finished doing its job, it could send a message into that queue in the cloud. The message would then be caught by the client,
  which would in turn call the Cleanup method and delete the entire application. </p>
Endless Automation Possibilities
<p>The Azure Management Libraries provide a great automation layer between your code and Azure. You can use these libraries, which are in their preview release as of this week, to automate your entire Azure creation and destruction processes. In this first
  preview release, we&apos;re providing these management libraries for our compute and storage stacks, as well as for Azure Web Sites. In time, we&apos;ll be adding more functionality to the libraries. The goal is to give you automation capabilities for everything
  in Azure. </p>
<p>We&apos;re also excited about your feedback and look forward to suggestions during this preview phase. Please try out the Management Libraries, use them in your own experimentation, and let us know what you&apos;re using them to facilitate. If you have ideas or
  questions about the design, we&apos;re open to that too. The code for the libraries, like many other things in the Azure stack, are open source. We encourage you to take a look at the code in
  <a href="https://github.com/WindowsAzure/azure-sdk-for-net">our GitHub repository</a> . </p>
<h3>This Team is Astounding. I am Not Worthy.</h3>
<p>
  <a href="http://www.jeff.wilcox.name/">Jeff Wilcox&#x2019;s</a>  team of amazing developers have put in a lot of time on the Management Libraries and today we&#x2019;re excited to share them with you via
  <a href="http://www.nuget.org/packages?q=microsoft.windowsazure.management&amp;prerelease=true&amp;sortOrder=relevance">NuGet</a> . Jeff&#x2019;s build script and NuGet wizardry have been a lot of fun to watch. The pride this team takes in what it does and the awesomeness of what they&#x2019;ve produced is evident in how easy the Management Libraries are to use. We think you&#x2019;ll agree,
  and welcome your feedback and stories of how you&#x2019;re finding ways to use them. </p>
