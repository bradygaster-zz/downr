---
title: Automating the build process using Visual Studio.Net 2003, CruiseControl.Net, NAnt, and NDoc
slug: automating-the-build-process-using-visual-studionet-2003-cruisecontrolnet-nant-and-ndoc
author: bradygaster
lastModified: 2009-12-21 07:24:09
pubDate: 2012-10-05 18:14:46
categories: .NET
---

I like the idea of automated builds a lot.
<a>Cory Foy</a>  taught me a lot about the idea, and I&apos;ve only recently really had a lot of time to dedicate to learning more about the whole process of setting up an automated build. Today I decided to fire off
<a>NDoc</a>  during a CCNet build. It took awhile to get it down-pat (and required a little help from Cory, to boot). In the spirit of trying to make life as easy as possible, here&apos;s the code to do it in as simple-and-direct-a-method as I could come up with.
<br>
<br> First of all, here&apos;s the CruiseControl.Net server configuration file in its entirety.
<br>
<br>
<div>
  <p>
    &#xA0;&#xA0;&#xA0;&#xA0;1 &#xA0;&lt; cruisecontrol &gt; 
  </p>
  <p>
    &#xA0;&#xA0;&#xA0;&#xA0;2 &#xA0;&#xA0;&#xA0;&#xA0; &lt; project   name =&quot;CCNetSample&quot;&gt; 
  </p>
  <p>
    &#xA0;&#xA0;&#xA0;&#xA0;3 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &lt; tasks &gt; 
  </p>
  <p>
    &#xA0;&#xA0;&#xA0;&#xA0;4 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &lt; devenv &gt; 
  </p>
  <p>
    &#xA0;&#xA0;&#xA0;&#xA0;5 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &lt; solutionfile &gt; C:\CCNetSample\CCNetSample.sln solutionfile 
    &gt; 
  </p>
  <p>
    &#xA0;&#xA0;&#xA0;&#xA0;6 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &lt; configuration &gt; Debug configuration 
    &gt; 
  </p>
  <p>
    &#xA0;&#xA0;&#xA0;&#xA0;7 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0;  devenv &gt; 
  </p>
  <p>
    &#xA0;&#xA0;&#xA0;&#xA0;8 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &lt; nant &gt; 
  </p>
  <p>
    &#xA0;&#xA0;&#xA0;&#xA0;9 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &lt; executable &gt; c:\nant\bin\nant.exe executable 
    &gt; 
  </p>
  <p>
    &#xA0;&#xA0;&#xA0;10 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &lt; baseDirectory &gt; C:\CCNetSample\ baseDirectory 
    &gt; 
  </p>
  <p>
    &#xA0;&#xA0;&#xA0;11 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &lt; buildFile &gt; build.xml buildFile 
    &gt; 
  </p>
  <p>
    &#xA0;&#xA0;&#xA0;12 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &lt; targetList &gt; 
  </p>
  <p>
    &#xA0;&#xA0;&#xA0;13 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &lt; target &gt; doc target 
    &gt; 
  </p>
  <p>
    &#xA0;&#xA0;&#xA0;14 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0;  targetList &gt; 
  </p>
  <p>
    &#xA0;&#xA0;&#xA0;15 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0;  nant &gt; 
  </p>
  <p>
    &#xA0;&#xA0;&#xA0;16 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0;  tasks &gt; 
  </p>
  <p>
    &#xA0;&#xA0;&#xA0;17 &#xA0;&#xA0;&#xA0;&#xA0;  project &gt; 
  </p>
  <p>
    &#xA0;&#xA0;&#xA0;18 &#xA0; cruisecontrol &gt; 
  </p>
</div>
<br> Finally, the build.xml file.
<br>
<br>
<div>
  <p>
    &#xA0;&#xA0;&#xA0;&#xA0;1 &#xA0;&lt; project   name =&quot;CCNetSample&quot;   
    default =&quot;doc&quot;   basedir =&quot;.&quot;&gt; 
  </p>
  <p>
    &#xA0;&#xA0;&#xA0;&#xA0;2 &#xA0;&#xA0;&#xA0;&#xA0; &lt; target   name =&quot;doc&quot;&gt; 
  </p>
  <p>
    &#xA0;&#xA0;&#xA0;&#xA0;3 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &lt; ndoc &gt; 
  </p>
  <p>
    &#xA0;&#xA0;&#xA0;&#xA0;4 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &lt; assemblies   basedir =&quot;C:\CCNetSample\CCNetSample.Client\bin\Debug\&quot;&gt; 
  </p>
  <p>
    &#xA0;&#xA0;&#xA0;&#xA0;5 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &lt; include   name =&quot;CCNetSample.Client.exe&quot; 
      /&gt; 
  </p>
  <p>
    &#xA0;&#xA0;&#xA0;&#xA0;6 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &lt; include   name =&quot;CCNetSample.Lib.dll&quot; 
      /&gt; 
  </p>
  <p>
    &#xA0;&#xA0;&#xA0;&#xA0;7 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0;  assemblies &gt; 
  </p>
  <p>
    &#xA0;&#xA0;&#xA0;&#xA0;8 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &lt; documenters &gt; 
  </p>
  <p>
    &#xA0;&#xA0;&#xA0;&#xA0;9 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &lt; documenter   name =&quot;MSDN&quot;&gt; 
  </p>
  <p>
    &#xA0;&#xA0;&#xA0;10 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0;  documenter &gt; 
  </p>
  <p>
    &#xA0;&#xA0;&#xA0;11 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0;  documenters &gt; 
  </p>
  <p>
    &#xA0;&#xA0;&#xA0;12 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0;  ndoc &gt; 
  </p>
  <p>
    &#xA0;&#xA0;&#xA0;13 &#xA0;&#xA0;&#xA0;&#xA0;  target &gt; 
  </p>
  <p>
    &#xA0;&#xA0;&#xA0;14 &#xA0; project &gt; 
  </p>
</div>
<br> Hope that helps someone who&apos;s ever in the same need.
