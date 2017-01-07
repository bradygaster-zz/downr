---
title: Netduino Controlled by Kinect
slug: netduino-controlled-by-kinect
author: bradygaster
lastModified: 2011-07-28 13:01:11
pubDate: 2012-10-05 18:15:07
categories: Kinect,Microcontrollers
---

<p><strong>Update: </strong> I&apos;ve uploaded the code I wrote for this demonstration project to GitHub into my
  <a href="https://github.com/bradygaster/KinectControlledNetduino">KinectControlledNetduino</a>  public repository. I also forgot to mention that the Kinect code makes use of the <em>excellent </em> gesturing engine created by David Catuhe, which you can
  <a href="http://blogs.msdn.com/b/eternalcoding/archive/2011/07/04/gestures-and-tools-for-kinect.aspx">read about on his blog</a>  or
  <a href="http://kinecttoolkit.codeplex.com/">download from CodePlex</a> .&#xA0;</p>
<p>I&apos;ll put the code up here tomorrow once time and energy permit, but for the time being the title says it all. There&apos;s a Kinect, it controls a WPF app, that app sends messages to an HTTP server running on a Netduino, which is connected to a servo.</p>
<p>Using hand gestures like &quot;SwipeRight&quot; or &quot;SwipeLeft,&quot; a user can literally wave to the Kinect to tell it how to tell the Netduino server how to angle the servo. Pretty neat, and quite easier than I&apos;d expected. I&apos;ll post the code ASAP but for now here&apos;s
  a video demonstrating how it works.&#xA0;</p>
<p>
  <iframe frameborder="0" src="http://www.youtube.com/embed/QWiRGT58BoQ" height="349" width="425"></iframe>
</p>
