---
title: PhluffyFotos on Azure
slug: phluffyfotos-on-windows-azure
author: bradygaster
lastModified: 2012-10-05 18:15:15
pubDate: 2012-10-05 18:15:11
categories: Azure
---

<p>In keeping with the Azure Evangelism Team&#x2019;s mission of providing samples that demonstrate how to use various aspects of the Azure platform, I&#x2019;d like to introduce you to the PhluffyFotos application. The idea behind PhluffyFotos is to offer an image-sharing
  application that allows users to upload, tag, and share images on line. The
  <a>MyPictures</a>  application sample introduced you to the idea of storing images in Azure Blob Storage via Web API, but PhluffyFotos takes the idea of an image-sharing application a few steps further architecturally. Take a look at this sample if you
  want to see how various Azure components &#x2013; Web Sites, Cloud Services, and Azure Storage &#x2013; to see how to add background processing to your web application, all hosted in the cloud.</p>
Architecture
<p>The PhluffyFotos application runs as a Azure Web Site used to let visitors upload images. Those images and their metadata are sent over to Azure for processing. The Cloud Service picks up information stored in Azure Storage Queues, and processes that
  information so it can be stored in Azure Table Storage. The image content itself, like was done in MyPictures, is stored in binary blobs using Azure Blob Storage. The web site allows for multiple user profiles, which are stored in a Azure SQL Database
  and accessed using the
  <a>Universal Profile Providers</a>, which are available via
  <a>NuGet</a> . Everything image-centric is actually stored in Azure Storage, once it has been processed via the Cloud Service. </p>
Links and Video
<p>As with all the other Azure Evangelism Team samples and demonstrations,
  <a>the source code for PhluffyFotos is stored in its very own GitHub.com repository</a> . If you notice any issues with the code, please feel free to use
  <a>repository&#x2019;s Issues feature</a>  to let me know what you find. Likewise, if you feel you have a change that&#x2019;d improve the sample, feel free to make your own fork in which you can change the code, and submit a pull request.
  <a>The sample is available on the MSDN samples site</a>, too, so you can use that link to submit questions and to download a zip file containing the code for the sample. I&#x2019;ve also published
  <a>a Channel9 video that demonstrates how you can get the sample running, step-by-step</a> .</p>
<p>I and the rest of the Azure Evangelism Team hope you enjoy this example of what&apos;s possible with Azure! Happy Coding!</p>
