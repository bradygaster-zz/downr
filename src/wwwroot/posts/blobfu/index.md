---
title: BlobFu
slug: blobfu
author: bradygaster
lastModified: 2012-10-05 18:15:15
pubDate: 2012-10-05 18:15:10
categories: Azure
---

<p>I came up with a little side project on the plane ride home from Belgium. The world needed a dirt-simple Fluent wrapper around Azure Blob Storage to make it dirt-simple, I decided, and this is my first pass at making such a handy resource helper available.
  I&apos;ll get this thing on NuGet in the next few days, but here&apos;s a quick run-through of what BlobFu can do for you. Take a closer look at the code, as it&apos;s up
  <a href="https://github.com/bradygaster/BlobFu">on GitHub.com right now</a> .&#xA0;</p>
<p>
  <a></a> 
</p>
BlobFu
<p>
  <a></a> 
</p>
Azure Blob Storage Fluent Wrapper
<p>A library that makes it easy via a Fluent interface, to interact with Windows Blob storage. It gives you a very basic start to storing binary blobs in the Azure cloud.</p>
<p>
  <a></a> 
</p>
<h3>What does BlobFu Do?</h3>
<p>Here&apos;s the current set of functionality, demonstrated by an NUnit output of the unit tests used to design BlobFu. Note, more tests may be added as the project evolves.</p>
<p>
  <img src="https://github.com/bradygaster/BlobFu/blob/master/Images/blobfu-unit-test-run.png?raw=true" alt="BlobFu Unit Test Run">
</p>
<p>
  <a></a> 
</p>
<h3>Using BlobFu Within ASP.NET</h3>
<p>Here&apos;s the Hello World example to demonstrate one of the best uses for Azure Blob Storage - capturing file uploads. BlobFu makes this pretty simple.</p>
<p>
  <a></a> 
</p>
<h4>Step 1 - Configure the Azure Blob Storage Connection String</h4>
<p>Add an application or web configuration setting with the connection string you&apos;ll be using that points to your Azure storage account, as shown below.</p>
<p>
  <img src="https://github.com/bradygaster/BlobFu/blob/master/Images/configuring-a-site-or-app-with-the-blob-conne.png?raw=true" alt="Configuring a site or app with the blob connection string">
</p>
<p>Note: In this, the local storage account will be used, so make sure you&apos;re running your local storage emulator in this example.</p>
<p>
  <img src="https://github.com/bradygaster/BlobFu/blob/master/Images/running-the-storage-emulator.png?raw=true" alt="running the storage emulator">
</p>
<p>
  <a></a> 
</p>
<h3>Step 2 - Create an ASPX Page to Upload Files</h3>
<p>Don&apos;t forget the <em>enctype</em>  attribute. I always forget that, and then the files won&apos;t be uploaded. Just sayin&apos;.</p>
<p>
  <img src="https://github.com/bradygaster/BlobFu/blob/master/Images/html-form-for-uploading.png?raw=true" alt="HTML form for uploading">
</p>
<p>
  <a></a> 
</p>
<h3>Step 3 - Collect the Data</h3>
<p>The code below simply collects the file upload and slams it into Azure Blob Storage.</p>
<p>
  <img src="https://github.com/bradygaster/BlobFu/blob/master/Images/saving-blobs-to-blob-storage.png?raw=true" alt="saving blobs to blob storage">
</p>
<p>
  <a></a> 
</p>
<h3>Really?</h3>
<p>Yes, really. Looking at the Azure blob storage account in ClumsyLeaf&apos;s CloudXPlorer, you&apos;ll see images that are uploaded during testing.</p>
<p>
  <img src="https://github.com/bradygaster/BlobFu/blob/master/Images/checking-the-blob-account-using-cloudxplorer.png?raw=true" alt="checking the blob account using CloudXPlorer">
</p>
<p>
  <a></a> 
</p>
<h3>Have Fun!</h3>
<p>Give BlobFu a try. Hopefully it&apos;ll ease the process of learning how to get your blobs into Azure. These helper methods can also be used as WebMatrix 2 helpers, so give that a spin (and watch this space for more on this) if you get a moment.</p>
<p>Please let me know of any issues or enhancements you observe (or think about) using the Issues link for this project.</p>
