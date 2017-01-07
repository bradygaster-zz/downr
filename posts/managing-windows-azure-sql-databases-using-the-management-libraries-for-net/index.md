---
title: Managing Azure SQL Databases Using the Management Libraries for .NET
slug: managing-windows-azure-sql-databases-using-the-management-libraries-for-net
author: 
lastModified: 2014-11-06 08:22:50
pubDate: 2013-12-11 17:07:31
categories: Azure
---

<p>The Azure SDK team has been working hard to finish up more of the Azure Management Libraries so you can use .NET code to manage your Azure resources, and today&#x2019;s blog will highlight one of the newest additions to the WAML stack. Today I&#x2019;ll introduce you
  to the
  <a href="http://www.nuget.org/packages/Microsoft.WindowsAzure.Management.Sql">Azure SQL Database Management Library</a> . We released the SQL Management Library this week, and it&#x2019;s loaded with features for managing your Azure SQL Databases. Like the rest of the management libraries, the SQL library provides most of the functionality
  you&#x2019;d previously only been able to do using the Azure portal, so you&#x2019;ll be able to write .NET code to do pretty much any level of automation against your SQL databases and servers. </p>
<p>Here&#x2019;s a list of some of the features supported by the new SQL library:</p>
<ul>
  <li>Create, update, delete, and list your Azure SQL Servers
    </li><li>Create, update, delete, and list your Azure SQL Databases
      </li><li>Create, update, delete, and list your Azure SQL Servers&#x2019; Firewall Rules
        </li><li>Import and Export SQL databases to &amp; from Azure Blob Storage</li>
</ul>
<p>Let&#x2019;s dive in to some of these features by exploring their usage in the sample code I&#x2019;ve written to demonstrate all these cool new WAML features.
  <a href="https://github.com/bradygaster/WindowsAzureManagementLibraries.Demos">I&#x2019;ve put this code up in GitHub</a>  so you can fork it, add your own functionality and test cases, and <strong>[please, feel free]</strong>  spruce up the UX if you&#x2019;re so inclined. The code for this demonstration includes the demonstrations from my previous
  blog posts on WAML, so if you&#x2019;ve been wanting to see that code, you can dive in as of this latest installment in the blog series. </p>
<p>The WPF app shown below sums up today&#x2019;s code demonstration for using the SQL management library. Once I select my publish settings file and select the subscription in which I want to work WAML makes a REST call out to Azure to retrieve the list of database
  servers in my subscription. I data-bind a list view in the WPF app with the list of servers. Next to each you&#x2019;ll find buttons to allow for convenient access to the databases residing on the servers, as well as a button giving you access to the servers&#x2019;
  firewall rules. </p>
<p>
  <a href="http://www.bradygaster.com/posts/files/12567c75-14bb-4d9c-8a90-2bde28584948.png">
    <img alt="SNAGHTML1a48f73" src="media/5b97831e-b9fe-4043-a9a4-5d537f044d34.png">
  </a> 
</p>
<h3>Creating the SQL Management Client</h3>
<p>As with the other demonstrations, the first step is to create an instance of the <strong>SqlManagementClient</strong>  class using a subscription ID and an X509 Certificate, both of which are available in my publish settings file. The code below is similar
  to the manner in which the other management clients (Compute, Infrastructure, Storage, and so on) are created. </p>
<p>
  <a href="http://www.bradygaster.com/posts/files/888d74d5-9c23-4079-998a-bdbc68d98e51.png">
    <img alt="image" src="media/dd043dcb-df67-4b33-b9af-7d0a0fe649fc.png">
  </a> 
</p>
<h3>SQL Server Operations</h3>
<p>The code below shows how, in two lines of code, I can get back the list of servers from my subscription. Once I&#x2019;ve got the list, I set a property that&#x2019;s data-bound in the WPF app equal to the list of servers the REST API returned for my subscription.
  </p>
<p>
  <a href="http://www.bradygaster.com/posts/files/088407c6-a4a0-4f2d-a3cd-39a8f59257fe.png">
    <img alt="image" src="media/0d27bf16-1fb7-4fc9-9a88-4e726b2fc807.png">
  </a> 
</p>
<h3>Database Operations</h3>
<p>Once a user clicks the &#x201C;Databases&#x201D; button in one of the server lines in the WPF app a subsequent call is made out to the Azure REST API to pull back the list of databases running on the selected server.&#xA0; </p>
<p>
  <a href="http://www.bradygaster.com/posts/files/96f32afa-c577-4b29-aef4-c86c2a0d443f.png">
    <img alt="image" src="media/62f62191-7157-4cf8-b6e2-4c4c9ddc58d0.png">
  </a> 
</p>
<p>You can do more than just list databases using the SQL management library &#x2013; you have full control over the creation and deletion of databases, too, using the easily-discoverable syntax. Below, you&#x2019;ll see how easy it is to figure out using nothing more
  than the Intellisense features of Visual Studio to figure out how to create a new database. </p>
<p>
  <a href="http://www.bradygaster.com/posts/files/944b78d8-2b77-42c1-a676-4d3ef95696f8.png">
    <img alt="image" src="media/a9fc744e-187a-4e0f-b63b-f11b20a8c7e3.png">
  </a> 
</p>
<h3>Firewall Rules</h3>
<p>You no longer need to load up a server in the portal to manage your firewall rules &#x2013; you can do that using the SQL management library, too. The code below demonstrates how easy it is to retrieve the list of firewall rules for a particular SQL Servers.
  When the user hits the &#x201C;Firewall Rules&#x201D; button in the WPF app, this code runs and loads up the rules in the UX. </p>
<p>
  <a href="http://www.bradygaster.com/posts/files/9050ab5c-3dda-4884-8ac3-0cae31446d70.png">
    <img alt="image" src="media/395b0df7-668b-4e56-9475-00cc43086b69.png">
  </a> 
</p>
<p>In addition to the SQL library, we&#x2019;ve also released the preview libraries for
  <a href="http://www.nuget.org/packages/Microsoft.WindowsAzure.Management.MediaServices">Media Services</a>  and
  <a href="http://www.nuget.org/packages/Microsoft.WindowsAzure.Management.ServiceBus/">Service Bus</a>, too. We&#x2019;re continuing to improve the landscape of WAML every day, and have some convenience features we&#x2019;re adding into the overall WAML functionality set, too. Keep watching this blog for more announcements and tutorials on the Azure
  Management Libraries. As always, if you have any suggestions or ideas you&#x2019;d like to see bubble up in the management libraries, feel free to post a comment below. </p>
<p>Happy Coding!</p>
