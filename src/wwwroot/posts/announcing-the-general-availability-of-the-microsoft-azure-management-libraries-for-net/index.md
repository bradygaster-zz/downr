---
title: Announcing the General Availability of the Microsoft Azure Management Libraries for .NET
slug: announcing-the-general-availability-of-the-microsoft-azure-management-libraries-for-net
author: 
lastModified: 2015-08-27 06:32:12
pubDate: 2014-04-09 07:10:28
categories: Azure
---

<p>I&#x2019;d like to <em>officially </em> introduce you to the 1.0 release of the Microsoft Azure Management Libraries. The official announcement of the libraries came out a few days ago on the
  <a href="http://azure.microsoft.com/en-us/updates/management-libraries-for-net-release-announcement/">Microsoft Azure Service Updates</a>  blog. <strong>Update: </strong> Jeff Wilcox wrote up am
  <a href="http://www.jeff.wilcox.name/2014/04/wamlmaml/">excellent piece introducing the Management Libraries</a>, in which he covers a lot of ground.&#xA0; </p>
<p>As I was busy travelling and presenting at the //build conference in San Francisco and enjoying my son Gabriel&#x2019;s 6th birthday, I was a little tied up and unable to get this post out, but it gave me time to shore up a few little things, publish the code,
  and prepare a surprise for you that I&#x2019;ll describe below in this post. Let&#x2019;s just say I wanted to make it as easy as possible for you to get up and running with the 1.0 bits, since I&#x2019;m so proud of all the work our teams have put into it. This week at
  the //build/ 2014 conference I presented a session with my buddy
  <a href="https://twitter.com/Jodoglevy">Joe Levy</a>  on many new automation <em>stack </em> we&#x2019;ve added to Microsoft Azure. You can watch
  <a href="http://channel9.msdn.com/Events/Build/2014/3-621">our //build/ session on Channel 9</a>, which covers all of the topics from the slide image below. Joe and I talked about the Automation Stack in Microsoft Azure, from the
  <a href="http://www.nuget.org/packages/Microsoft.WindowsAzure.Common/">SDK Common NuGet package</a>  up through the
  <a href="http://www.nuget.org/packages?q=Microsoft.WindowsAzure.Management">Microsoft Azure Management Libraries for .NET</a>  and into
  <a href="http://azure.microsoft.com/en-us/documentation/articles/install-configure-powershell/">PowerShell</a>  and how the new Microsoft Azure Automation Service sits atop all of it for true PowerShell-as-a-service automation that you can use for just about anything. </p>
<p>
  <a href="http://www.bradygaster.com/posts/files/41dc6715-7d5b-4341-8393-cb0b756bda77.png">
    <img alt="automation-stack" src="/posts/announcing-the-general-availability-of-the-microsoft-azure-management-libraries-for-net/media/42e299df-d526-409a-a98c-8e0a016ccc11.png">
  </a> 
</p>

Demonstrating the Management Libraries
<p>My part of the session was primarily focused on how developers can make use of the Management Libraries (<em>MAML,</em>  for short) for various scenarios. I&#x2019;ve created 2 GitHub projects where the code for these demos, and I also have another surprise I&#x2019;ll
  discuss later in the post. First, the demos!</p>
<h3>Integration Testing</h3>
<p>One scenario in which I think MAML has a lot to offer is to enable integration testing. Imagine having a Web Site that talks to a Storage Account to display data to the user in HTML. Pretty common scenario that can have all sorts of problems. Connection
  string incorrectness, a dead back-end, misconfiguration &#x2013; you never know what could happen. Good integration tests offer more confidence that &#x201C;at least the environment is right and everything is configured properly.&#x201D; This scenario is represented by
  the code in the
  <a href="https://github.com/bradygaster/build2014-MAML-IntegrationTesting">MAML Integration Testing code repository</a> . Using
  <a href="https://github.com/xunit/xunit">xunit</a>  tests and MAML together, I was able to automate the entire process of:</p>
<ol>
  <li>Creating a web site
    </li><li>Publishing the web site&#x2019;s code
      </li><li>Creating a storage account
        </li><li>Getting that storage account&#x2019;s connection string
          </li><li>Saving data to the storage account that I intend on displaying on the web site
            </li><li>Configuring the web site&#x2019;s connection string so that it can find the storage account and pull the data for display
              </li><li>Hit the web site and verify it displays the correct information
                </li><li>Delete the storage account
                  </li><li>Delete the web site</li>
</ol>
<p>If this sounds like a common practice for your Microsoft Azure web apps, you might get some value from this demo, as it could streamline your entire process of integration testing. Here&#x2019;s the best part &#x2013; if you&#x2019;re not really an Azure storage person, and
  your typical scenario involves a non-cloud-hosted ASP.NET web site that talks to SQL Server, you could still make use of MAML for your own integration tests. Simply use the
  <a href="http://www.nuget.org/packages/Microsoft.WindowsAzure.Management.Sql">SQL Management Client</a>  to fire up a Microsoft Azure SQL Database, insert a few records, and do basically the rest of the integration testing &#x201C;stuff&#x201D; but set up your page to read from the database instead of the storage account. Then, whether you&#x2019;re
  deploying your production site to Microsoft Azure or not, you can make sure it all works using a scorched-earth testing environment. </p>
<h3>Enabling SaaS</h3>
<p>Cloud is a great place for software-as-a-service vendors. In typical SaaS situations, a customer can hit a web site, provide some information, and <em>voila&#x2019;</em>, their newly-customized web site is all ready. The final demonstration I did during the
  //build/ session was geared towards these sorts of scenarios. In my demo at //build/, I demonstrated this sort of scenario by creating an MVC application I called <em>MiniBlogger</em>, for it generates live MiniBlog sites running in Microsoft Azure.
  When the user clicks the button, a Web API controller is invoked using JavaScript. The controller code makes a few calls out to the Microsoft Azure REST API using MAML. It first verifies the site name is available and if not, the user is provided a
  subtle visual cue that their requested site name isn&#x2019;t available:</p>
<p>
  <a href="http://www.bradygaster.com/posts/files/dc46aea1-619c-446e-95b4-d907b03ccf34.png">
    <img alt="image" src="/posts/announcing-the-general-availability-of-the-microsoft-azure-management-libraries-for-net/media/2726d4f8-b6fc-424d-9e4f-c8b228864675.png">
  </a> 
</p>
<p>When the user finds a name they like that&#x2019;s also not already in use, they can create the site. As the API controller iterates over each step of the process it sends messages to a SignalR Hub (yes, I can work SignalR in <em>anywhere</em> ), and the user
  is provided real-time status on the process of the site being created and deployed. </p>
<p>
  <a href="http://www.bradygaster.com/posts/files/02d13340-7681-4c9d-8961-a5cf01c51093.png">
    <img alt="image" src="/posts/announcing-the-general-availability-of-the-microsoft-azure-management-libraries-for-net/media/6301c0fd-14ec-4089-8117-7fff23427050.png">
  </a> 
</p>
<p>Once the deployment is complete, the site pops up in a new browser, all ready for use.
  <a href="https://github.com/bradygaster/build2014-MAML-EnablingSaaS">The code for this demo is also on GitHub, so fork it and party</a> . </p>
Get Your Very Own MAML Project Template (the surprise)
<p>In this session I made use of
  <a href="https://twitter.com/sayedihashimi">Sayed</a>  and
  <a href="https://twitter.com/mkristensen">Mads&#x2019;</a>  work on
  <a href="http://www.sidewaffle.com/">SideWaffle</a>  and
  <a href="https://github.com/ligershark/template-builder">Template Builder</a>  to create a Visual Studio Extension that makes it easy to get up and running with MAML. Sayed and Mads have long thought SideWaffle would be great for coming up with canned presentations, and this was my first attempt at delivering
  on their goal. I asked them both tons of questions throughout the process, so first and foremost, thanks to them for SideWaffle and their patience as I fumbled through aspects of getting the hang of using it.</p>
<p>You can get the
  <a href="http://visualstudiogallery.msdn.microsoft.com/07c3e25f-970f-4bce-ba62-28b6e876188c">Microsoft Azure Management Libraries</a>  extension now in the Visual Studio Extensions Gallery. I&#x2019;ve also created
  <a href="https://www.youtube.com/watch?v=hG6a8oyxynA&amp;feature=youtu.be">a little YouTube video demonstrating its usage</a> . In five minutes, you can have a running Console Application that creates Virtual Machines in Microsoft Azure. </p>
<p>This Visual Studio Extension I created contains a few elements. First, it has a project template that references all of the MAML NuGet packages and the
  <a href="http://www.nuget.org/packages/Microsoft.IdentityModel.Clients.ActiveDirectory/">Active Directory Authentication Library NuGet package</a>, which are dependencies for the demonstration. When you install the extension you&#x2019;ll get a new project template like the one highlighted below.</p>
<p>
  <a href="http://www.bradygaster.com/posts/files/f02ff431-72bc-41b0-b461-f9521b938ac2.png">
    <img alt="SNAGHTMLae8a6c3" src="/posts/announcing-the-general-availability-of-the-microsoft-azure-management-libraries-for-net/media/8d03bf9c-9e82-4dd2-9341-af3fa9588e69.png">
  </a> 
</p>
<p>The project is a basic Console Application, but with all the MAML/ADAL NuGets referenced. Also contained within the extension are five item templates and 6 code snippets that walk you through the process of authoring code that will result in the following
  workflow:</p>
<ol>
  <li>Authenticate to Microsoft Azure using Azure Active Directory
    </li><li>Retrieve a list of Microsoft Azure subscriptions that the authenticated user can access
      </li><li>Find a specific subscription and associate the AAD token with that subscription
        </li><li>Create a new Cloud Service in the subscription
          </li><li>Create a new Storage Account in the subscription
            </li><li>Get the list of Virtual Machine images containing a filter (this is provided in the snippets as a parameter)
              </li><li>Create a Virtual Machine running in the newly-created Cloud Service container, using the VHD of the image selected earlier
                </li><li>Deploy the Virtual Machine and start it up</li>
</ol>
<p>The screen shot below is from my own instance of Visual Studio testing out the item templates. I&#x2019;m on step 2 in this screen shot, about to add the class that facilitates the subscription-selection process described above. </p>
<p>
  <a href="http://www.bradygaster.com/posts/files/04a35ea8-541c-4701-a286-4528d373742e.png">
    <img alt="SNAGHTMLaf38d3a" src="/posts/announcing-the-general-availability-of-the-microsoft-azure-management-libraries-for-net/media/9f108f7a-f91b-458b-a7fe-4e3cb906f9b3.png">
  </a> 
</p>
<p>Likewise, here&#x2019;s my code being edited during step 2. Note how the snippet is really doing the work, and the comments provide guidance. </p>
<p>
  <a href="http://www.bradygaster.com/posts/files/14ab51a7-8966-4ab9-bcf0-38991e8cd2d1.png">
    <img alt="image" src="/posts/announcing-the-general-availability-of-the-microsoft-azure-management-libraries-for-net/media/dedaa8e1-9952-4655-9c7b-8bc41ddc9f93.png">
  </a> 
</p>
<p>Each step of the process is pretty-well documented. I tried really hard to think of the easiest way to help the Microsoft Azure community get up and running with MAML following our 1.0 release, and this extension seemed to be the best answer I could come
  up with. I hope you find it as helpful as I think you&#x2019;ll find it, but I welcome any feedback you may have on the extension and how it could be improved. Same thing for MAML &#x2013; we&#x2019;re all about taking feedback, so let us know what we can do to make the
  future better for you as you automate everything in Microsoft Azure. </p>
Congrats to the Team
<p>I&#x2019;d definitely like to congratulate my team, and <strong>all the teams </strong> in Microsoft Azure who brought their awesome in full force this year in preparation for //build/. We had some great releases, amazing announcements, and heard great things
  from the community. Happy coding!</p>
