---
title: Dependency Injection Example - Constructor Injection and Service Orientation
slug: dependency-injection-example-constructor-injection-and-service-orientation
author: bradygaster
lastModified: 2011-05-21 02:22:21
pubDate: 2012-10-05 18:15:06
categories: .NET
---

<p>With all the talk on weblogs or technical conversations within my own organization about DI it&apos;s difficult to ignore it as little more than the latest &quot;new black&quot; pattern. I&apos;ve given it some considerable thought and until quite recently didn&apos;t really
  comprehend the overall niftiness of the DI approach. As with anything else it took a moment of &quot;a-HA&quot; to really grasp the power of DI; I was developing a custom CruiseControl.Net build plugin and realized that the plugins are injected dynamically at
  construction time. If you debug&#xA0; one of these plugins, you&apos;ll notice that the plugin constructors don&apos;t match a common parameter structure. The one commonality throughout all the plugins I was investigating as examples for my own education seemed to
  be in the parameter types - they were all interfaces, implementations of which were usually stored in the CCNet server application domain.</p>
<p>Services, basically. My interest in service orientation perked my interest in DI; the two concepts appeared mutually beneficial. So I opened the laptop and started coding away on my own implementation of a DI framework, with tests (since it&apos;s virtually
  impossible to talk about DI without talking about tests, too). This blog post consists of an investigation into my own implementation. It doesn&apos;t offer up DI as a &quot;holier-than-thou&quot; approach nor a dismissable coding trend. Rather, it is my first attempt
  to prove to myself that I am getting this DI stuff and to implement it in my own words. *cracks knuckles*</p>
<p><strong>First Things First - Support Laziness<br> </strong> </p>
<p>As with any framework, I anticipate that the easier it is to use the more chance I&apos;ll have of talking someone else into giving this a shot or a glance. So I knew that I wanted my approach to be very simple to use, quick to implement, and hopefully, make
  the approach of using DI more interesting. My first question to every pattern is &quot;does it make my coding process easier and more flexible?&quot; I know that:</p>
<ul>
  <li>I&apos;m going to implement DI here, so my dependent classes will &quot;just pop up&quot; because the &quot;stuff&quot; they need to &quot;live&quot; will be provided to them by some service layer.&#xA0; </li>
  <li>So I know I will need to implement ServiceContainer in some fashion, and the services I put into it will implement interfaces, since the Service approach implies the use of interfaces to define contracts. </li>
</ul>
<p>My discomfort with the ServiceContainer approach is that, someone actually has to &quot;Add&quot; the service implementations to the service container. Hence, they have to:</p>
<ul>
  <li>Know how to do that.</li>
  <li>Do it the way you expect they&apos;re going to do it .</li>
  <li>Write their code in such a way that they account for my service container implementation. </li>
</ul>
<p>That point, most times, is where I get pretty aggravated when I&apos;m using someone else&apos;s framework. That is a requirement that I must not impose on my audience, but one that I can&apos;t get started without. So a compromise is in order, and I&apos;ll use the idea
  of simple metadata to notify me of interfaces the developer intends need to be added to the service layer at run-time.</p>
<div>
  <p>&#xA0;&#xA0;&#xA0;&#xA0;1 &#xA0;&#xA0;&#xA0;&#xA0; ///    </p>
  <p>&#xA0;&#xA0;&#xA0;&#xA0;2 &#xA0;&#xA0;&#xA0;&#xA0; ///  Defines an interface as one that should be created and hosted by the  </p>
  <p>&#xA0;&#xA0;&#xA0;&#xA0;3 &#xA0;&#xA0;&#xA0;&#xA0; ///  service host during application run-time.  </p>
  <p>&#xA0;&#xA0;&#xA0;&#xA0;4 &#xA0;&#xA0;&#xA0;&#xA0; ///    </p>
  <p>&#xA0;&#xA0;&#xA0;&#xA0;5 &#xA0;&#xA0;&#xA0;&#xA0; [AttributeUsage (AttributeTargets .Interface, AllowMultiple= 
    false )]</p>
  <p>&#xA0;&#xA0;&#xA0;&#xA0;6 &#xA0;&#xA0;&#xA0;&#xA0; public  class  DependencyServiceInterfaceAttribute  : Attribute </p>
  <p>&#xA0;&#xA0;&#xA0;&#xA0;7 &#xA0;&#xA0;&#xA0;&#xA0; {</p>
  <p>&#xA0;&#xA0;&#xA0;&#xA0;8 &#xA0;&#xA0;&#xA0;&#xA0; }</p>
</div>
<p>Using the DSI attribute I can mark any interface that I intend to be added to a service container instance within the application domain. This makes it really easy to use, as the code below indicates.&#xA0;</p>
<div>
  <p>&#xA0;&#xA0;&#xA0;76 &#xA0;[DependencyServiceInterface ]</p>
  <p>&#xA0;&#xA0;&#xA0;77 &#xA0;&#xA0;&#xA0;&#xA0; public  interface  IMockServiceA </p>
  <p>&#xA0;&#xA0;&#xA0;78 &#xA0;&#xA0;&#xA0;&#xA0; {</p>
  <p>&#xA0;&#xA0;&#xA0;79 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; void  DoMockServiceWork();</p>
  <p>&#xA0;&#xA0;&#xA0;80 &#xA0;&#xA0;&#xA0;&#xA0; }</p>
  <p>&#xA0;&#xA0;&#xA0;81 &#xA0;</p>
  <p>&#xA0;&#xA0;&#xA0;82 &#xA0;&#xA0;&#xA0;&#xA0; [DependencyServiceInterface ]</p>
  <p>&#xA0;&#xA0;&#xA0;83 &#xA0;&#xA0;&#xA0;&#xA0; public  interface  IMockServiceB </p>
  <p>&#xA0;&#xA0;&#xA0;84 &#xA0;&#xA0;&#xA0;&#xA0; {</p>
  <p>&#xA0;&#xA0;&#xA0;85 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; void  DoMoreWork();</p>
  <p>&#xA0;&#xA0;&#xA0;86 &#xA0;&#xA0;&#xA0;&#xA0; }</p>
  <p>&#xA0;&#xA0;&#xA0;87 &#xA0;</p>
  <p>&#xA0;&#xA0;&#xA0;88 &#xA0;&#xA0;&#xA0;&#xA0; public  class  MockServiceA  : IMockServiceA </p>
  <p>&#xA0;&#xA0;&#xA0;89 &#xA0;&#xA0;&#xA0;&#xA0; {</p>
  <p>&#xA0;&#xA0;&#xA0;90 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; #region  IMockService Members</p>
  <p>&#xA0;&#xA0;&#xA0;91 &#xA0;</p>
  <p>&#xA0;&#xA0;&#xA0;92 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; public  void  DoMockServiceWork()</p>
  <p>&#xA0;&#xA0;&#xA0;93 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; {</p>
  <p>&#xA0;&#xA0;&#xA0;94 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; System.Diagnostics.Debug .WriteLine( &quot;Doing Mock Service A&apos;s Job&quot; );</p>
  <p>&#xA0;&#xA0;&#xA0;95 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; }</p>
  <p>&#xA0;&#xA0;&#xA0;96 &#xA0;</p>
  <p>&#xA0;&#xA0;&#xA0;97 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; #endregion </p>
  <p>&#xA0;&#xA0;&#xA0;98 &#xA0;&#xA0;&#xA0;&#xA0; }</p>
  <p>&#xA0;&#xA0;&#xA0;99 &#xA0;</p>
  <p>&#xA0;&#xA0;100 &#xA0;&#xA0;&#xA0;&#xA0; public  class  MockServiceB  : IMockServiceB </p>
  <p>&#xA0;&#xA0;101 &#xA0;&#xA0;&#xA0;&#xA0; {</p>
  <p>&#xA0;&#xA0;102 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; #region  IMockServiceB Members</p>
  <p>&#xA0;&#xA0;103 &#xA0;</p>
  <p>&#xA0;&#xA0;104 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; public  void  DoMoreWork()</p>
  <p>&#xA0;&#xA0;105 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; {</p>
  <p>&#xA0;&#xA0;106 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; System.Diagnostics.Debug .WriteLine( &quot;Doing Mock Service B&apos;s Job&quot; );</p>
  <p>&#xA0;&#xA0;107 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; }</p>
  <p>&#xA0;&#xA0;108 &#xA0;</p>
  <p>&#xA0;&#xA0;109 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; #endregion </p>
  <p>&#xA0;&#xA0;110 &#xA0;&#xA0;&#xA0;&#xA0; }</p>
</div>
<p>I create a new interfaces and mark them with the DSI attribute. Then, I implement each interface with a custom class containing some basic functionality. Notice that I don&apos;t have to do anything to the classes themselves; the DI layer we&apos;ll begin to investigate
  next will do that work for us.&#xA0;</p>
<p><strong>Comprehensive - Sure, Why Not?!</strong> </p>
<p>If you can see where I&apos;m going with this you&apos;re most likely scratching your head and saying &quot;no way, not everything...&quot; Looking through each assembly for implementations of interfaces marked with metadata isn&apos;t the most performant approach to doing type-loading
  but for now it will serve it&apos;s purpose. Think of it this way - I&apos;m making exhaustively sure I&apos;m not going to miss any service implementation that I might need later by a dependent class.&#xA0; If I suspect that any interface I&apos;ve got implementations of will
  ever be needed by any dependent class that might be required later, I can just slap the DSI attribute onto the interface and off we go.</p>
<p>Just to take a quick look at the code that&apos;d perform this exhaustive search it&apos;s right here. The DependencyServiceContainer does just what you suspected - it looks through everything in the application domain to find any implementations of any interfaces
  that have been marked with the DSI attribute. Whenever it finds such an implementation, an instance of it is created and added via the base method ServiceContainer.AddService.</p>
<div>
  <p>&#xA0;&#xA0;&#xA0;&#xA0;8 &#xA0;public  class  DependencyServiceContainer  : ServiceContainer </p>
  <p>&#xA0;&#xA0;&#xA0;&#xA0;9 &#xA0;&#xA0;&#xA0;&#xA0; {</p>
  <p>&#xA0;&#xA0;&#xA0;10 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; public  static  DependencyServiceContainer  Instance</p>
  <p>&#xA0;&#xA0;&#xA0;11 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; {</p>
  <p>&#xA0;&#xA0;&#xA0;12 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; get  { return  _instance; }</p>
  <p>&#xA0;&#xA0;&#xA0;13 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; }</p>
  <p>&#xA0;&#xA0;&#xA0;14 &#xA0;</p>
  <p>&#xA0;&#xA0;&#xA0;15 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; static  DependencyServiceContainer  _instance;</p>
  <p>&#xA0;&#xA0;&#xA0;16 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; static  DependencyServiceContainer()</p>
  <p>&#xA0;&#xA0;&#xA0;17 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; {</p>
  <p>&#xA0;&#xA0;&#xA0;18 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; _instance = new  DependencyServiceContainer ();</p>
  <p>&#xA0;&#xA0;&#xA0;19 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; }</p>
  <p>&#xA0;&#xA0;&#xA0;20 &#xA0;</p>
  <p>&#xA0;&#xA0;&#xA0;21 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; internal  DependencyServiceContainer()</p>
  <p>&#xA0;&#xA0;&#xA0;22 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; {</p>
  <p>&#xA0;&#xA0;&#xA0;23 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; Preload();</p>
  <p>&#xA0;&#xA0;&#xA0;24 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; }</p>
  <p>&#xA0;&#xA0;&#xA0;25 &#xA0;</p>
  <p>&#xA0;&#xA0;&#xA0;26 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; private  int  _svcCount;</p>
  <p>&#xA0;&#xA0;&#xA0;27 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; public  int  ServiceCount</p>
  <p>&#xA0;&#xA0;&#xA0;28 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; {</p>
  <p>&#xA0;&#xA0;&#xA0;29 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; get  { return  _svcCount; }</p>
  <p>&#xA0;&#xA0;&#xA0;30 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; }</p>
  <p>&#xA0;&#xA0;&#xA0;31 &#xA0;</p>
  <p>&#xA0;&#xA0;&#xA0;32 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; void  Preload()</p>
  <p>&#xA0;&#xA0;&#xA0;33 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; {</p>
  <p>&#xA0;&#xA0;&#xA0;34 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; foreach  (Assembly  assm in  AppDomain 
    .CurrentDomain.GetAssemblies()) 
  </p>
  <p>&#xA0;&#xA0;&#xA0;35 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; {</p>
  <p>&#xA0;&#xA0;&#xA0;36 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; SearchAssemblyForDSIAttributes(assm);</p>
  <p>&#xA0;&#xA0;&#xA0;37 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; }</p>
  <p>&#xA0;&#xA0;&#xA0;38 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; }</p>
  <p>&#xA0;&#xA0;&#xA0;39 &#xA0;</p>
  <p>&#xA0;&#xA0;&#xA0;40 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; void  SearchAssemblyForDSIAttributes(Assembly  assm)</p>
  <p>&#xA0;&#xA0;&#xA0;41 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; {</p>
  <p>&#xA0;&#xA0;&#xA0;42 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; foreach  (Type  tp in  assm.GetTypes())</p>
  <p>&#xA0;&#xA0;&#xA0;43 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; {</p>
  <p>&#xA0;&#xA0;&#xA0;44 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; if  (!tp.IsInterface)</p>
  <p>&#xA0;&#xA0;&#xA0;45 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; {</p>
  <p>&#xA0;&#xA0;&#xA0;46 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; SearchTypeForInterfaces(tp);</p>
  <p>&#xA0;&#xA0;&#xA0;47 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; }</p>
  <p>&#xA0;&#xA0;&#xA0;48 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; }</p>
  <p>&#xA0;&#xA0;&#xA0;49 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; }</p>
  <p>&#xA0;&#xA0;&#xA0;50 &#xA0;</p>
  <p>&#xA0;&#xA0;&#xA0;51 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; void  SearchTypeForInterfaces(Type  t)</p>
  <p>&#xA0;&#xA0;&#xA0;52 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; {</p>
  <p>&#xA0;&#xA0;&#xA0;53 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; foreach  (Type  intrfc in  t.GetInterfaces())</p>
  <p>&#xA0;&#xA0;&#xA0;54 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; {</p>
  <p>&#xA0;&#xA0;&#xA0;55 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; if  (IsInterfaceDSI(intrfc))</p>
  <p>&#xA0;&#xA0;&#xA0;56 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; {</p>
  <p>&#xA0;&#xA0;&#xA0;57 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; AddService(</p>
  <p>&#xA0;&#xA0;&#xA0;58 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; intrfc,</p>
  <p>&#xA0;&#xA0;&#xA0;59 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; Activator .CreateInstance(t) </p>
  <p>&#xA0;&#xA0;&#xA0;60 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; );</p>
  <p>&#xA0;&#xA0;&#xA0;61 &#xA0;</p>
  <p>&#xA0;&#xA0;&#xA0;62 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; _svcCount++;</p>
  <p>&#xA0;&#xA0;&#xA0;63 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; }</p>
  <p>&#xA0;&#xA0;&#xA0;64 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; }</p>
  <p>&#xA0;&#xA0;&#xA0;65 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; }</p>
  <p>&#xA0;&#xA0;&#xA0;66 &#xA0;</p>
  <p>&#xA0;&#xA0;&#xA0;67 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; internal  static  bool  IsInterfaceDependencyInjectable(Type     intrfc)</p>
  <p>&#xA0;&#xA0;&#xA0;68 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; {</p>
  <p>&#xA0;&#xA0;&#xA0;69 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; return  (</p>
  <p>&#xA0;&#xA0;&#xA0;70 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; (intrfc.GetCustomAttributes(typeof (DependencyServiceInterfaceAttribute ), false ).Length
    &gt; 0 )</p>
  <p>&#xA0;&#xA0;&#xA0;71 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &amp;&amp;</p>
  <p>&#xA0;&#xA0;&#xA0;72 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; (intrfc.IsInterface)</p>
  <p>&#xA0;&#xA0;&#xA0;73 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; );</p>
  <p>&#xA0;&#xA0;&#xA0;74 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; }</p>
  <p>&#xA0;&#xA0;&#xA0;75 &#xA0;</p>
  <p>&#xA0;&#xA0;&#xA0;76 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; bool  IsInterfaceDSI(Type  intrfc)</p>
  <p>&#xA0;&#xA0;&#xA0;77 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; {</p>
  <p>&#xA0;&#xA0;&#xA0;78 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; return  DependencyServiceContainer .IsInterfaceDependencyInjectable(intrfc); </p>
  <p>&#xA0;&#xA0;&#xA0;79 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; }</p>
  <p>&#xA0;&#xA0;&#xA0;80 &#xA0;&#xA0;&#xA0;&#xA0; }</p>
</div>
<p><strong>Activation of Dependent Objects - the Point</strong> </p>
<p>Now that the DI framework has a service container into which services required by dependent classes have been added the logic to create dependent objects must be created. Basically, we&apos;re going to use reflection to inspect dependent classes. During reflection
  each constructor signature will be observed. When a constructor is found that has parameters of interface types that are all being held within the DependencyServiceContainer, the constructor will be called and the resulting object returned.&#xA0;</p>
<div>
  <p>&#xA0;&#xA0;&#xA0;&#xA0;8 &#xA0;public  class  DependentClassActivator </p>
  <p>&#xA0;&#xA0;&#xA0;&#xA0;9 &#xA0;&#xA0;&#xA0;&#xA0; {</p>
  <p>&#xA0;&#xA0;&#xA0;10 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; static  DependentClassActivator  _instance;</p>
  <p>&#xA0;&#xA0;&#xA0;11 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; static  DependentClassActivator()</p>
  <p>&#xA0;&#xA0;&#xA0;12 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; {</p>
  <p>&#xA0;&#xA0;&#xA0;13 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; _instance = new  DependentClassActivator ();</p>
  <p>&#xA0;&#xA0;&#xA0;14 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; }</p>
  <p>&#xA0;&#xA0;&#xA0;15 &#xA0;</p>
  <p>&#xA0;&#xA0;&#xA0;16 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; public  static  DependentClassActivator  Instance</p>
  <p>&#xA0;&#xA0;&#xA0;17 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; {</p>
  <p>&#xA0;&#xA0;&#xA0;18 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; get  { return  _instance; }</p>
  <p>&#xA0;&#xA0;&#xA0;19 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; }</p>
  <p>&#xA0;&#xA0;&#xA0;20 &#xA0;</p>
  <p>&#xA0;&#xA0;&#xA0;21 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; public  T CreateInstance
    () where  T : class </p>
  <p>&#xA0;&#xA0;&#xA0;22 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; {</p>
  <p>&#xA0;&#xA0;&#xA0;23 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; Type  tp = typeof (T);</p>
  <p>&#xA0;&#xA0;&#xA0;24 &#xA0;</p>
  <p>&#xA0;&#xA0;&#xA0;25 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; foreach  (ConstructorInfo  ctor in  tp.GetConstructors())</p>
  <p>&#xA0;&#xA0;&#xA0;26 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; {</p>
  <p>&#xA0;&#xA0;&#xA0;27 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; if  (Observe(ctor))</p>
  <p>&#xA0;&#xA0;&#xA0;28 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; {</p>
  <p>&#xA0;&#xA0;&#xA0;29 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; return  InvokeConstructor
    (ctor);</p>
  <p>&#xA0;&#xA0;&#xA0;30 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; }</p>
  <p>&#xA0;&#xA0;&#xA0;31 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; }</p>
  <p>&#xA0;&#xA0;&#xA0;32 &#xA0;</p>
  <p>&#xA0;&#xA0;&#xA0;33 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; return  null ;</p>
  <p>&#xA0;&#xA0;&#xA0;34 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; }</p>
  <p>&#xA0;&#xA0;&#xA0;35 &#xA0;</p>
  <p>&#xA0;&#xA0;&#xA0;36 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; #region  Private Helper Methods</p>
  <p>&#xA0;&#xA0;&#xA0;37 &#xA0;</p>
  <p>&#xA0;&#xA0;&#xA0;38 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; bool  Observe(ConstructorInfo  ctor)</p>
  <p>&#xA0;&#xA0;&#xA0;39 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; {</p>
  <p>&#xA0;&#xA0;&#xA0;40 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; foreach  (ParameterInfo  prm in  ctor.GetParameters())</p>
  <p>&#xA0;&#xA0;&#xA0;41 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; {</p>
  <p>&#xA0;&#xA0;&#xA0;42 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; if  (!Observe(prm)) return  false ;</p>
  <p>&#xA0;&#xA0;&#xA0;43 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; }</p>
  <p>&#xA0;&#xA0;&#xA0;44 &#xA0;</p>
  <p>&#xA0;&#xA0;&#xA0;45 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; return  true ;</p>
  <p>&#xA0;&#xA0;&#xA0;46 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; }</p>
  <p>&#xA0;&#xA0;&#xA0;47 &#xA0;</p>
  <p>&#xA0;&#xA0;&#xA0;48 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; bool  Observe(ParameterInfo  prm)</p>
  <p>&#xA0;&#xA0;&#xA0;49 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; {</p>
  <p>&#xA0;&#xA0;&#xA0;50 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; return  (</p>
  <p>&#xA0;&#xA0;&#xA0;51 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; DependencyServiceContainer .IsInterfaceDependencyInjectable(prm.ParameterType) &amp;&amp; </p>
  <p>&#xA0;&#xA0;&#xA0;52 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; GetTypeFromServiceContainer(prm.ParameterType)</p>
  <p>&#xA0;&#xA0;&#xA0;53 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; );</p>
  <p>&#xA0;&#xA0;&#xA0;54 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; }</p>
  <p>&#xA0;&#xA0;&#xA0;55 &#xA0;</p>
  <p>&#xA0;&#xA0;&#xA0;56 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; bool  GetTypeFromServiceContainer(Type  intrfc)</p>
  <p>&#xA0;&#xA0;&#xA0;57 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; {</p>
  <p>&#xA0;&#xA0;&#xA0;58 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; return  (DependencyServiceContainer .Instance.GetService(intrfc) !=  
    null );</p>
  <p>&#xA0;&#xA0;&#xA0;59 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; }</p>
  <p>&#xA0;&#xA0;&#xA0;60 &#xA0;</p>
  <p>&#xA0;&#xA0;&#xA0;61 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; T InvokeConstructor
    (ConstructorInfo  ctor) where  T : class </p>
  <p>&#xA0;&#xA0;&#xA0;62 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; {</p>
  <p>&#xA0;&#xA0;&#xA0;63 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; object [] prms = GetConstructorParametersFromServiceContainer(ctor);</p>
  <p>&#xA0;&#xA0;&#xA0;64 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; return  ctor.Invoke(prms) as  T;</p>
  <p>&#xA0;&#xA0;&#xA0;65 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; }</p>
  <p>&#xA0;&#xA0;&#xA0;66 &#xA0;</p>
  <p>&#xA0;&#xA0;&#xA0;67 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; object [] GetConstructorParametersFromServiceContainer(ConstructorInfo  ctor)</p>
  <p>&#xA0;&#xA0;&#xA0;68 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; {</p>
  <p>&#xA0;&#xA0;&#xA0;69 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; List &lt; object &gt; prms =  
    new  List &lt; object &gt;(); </p>
  <p>&#xA0;&#xA0;&#xA0;70 &#xA0;</p>
  <p>&#xA0;&#xA0;&#xA0;71 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; foreach  (ParameterInfo  prm in  ctor.GetParameters())</p>
  <p>&#xA0;&#xA0;&#xA0;72 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; {</p>
  <p>&#xA0;&#xA0;&#xA0;73 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; prms.Add(DependencyServiceContainer .Instance.GetService(prm.ParameterType)); </p>
  <p>&#xA0;&#xA0;&#xA0;74 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; }</p>
  <p>&#xA0;&#xA0;&#xA0;75 &#xA0;</p>
  <p>&#xA0;&#xA0;&#xA0;76 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; return  prms.ToArray();</p>
  <p>&#xA0;&#xA0;&#xA0;77 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; }</p>
  <p>&#xA0;&#xA0;&#xA0;78 &#xA0;</p>
  <p>&#xA0;&#xA0;&#xA0;79 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; #endregion </p>
  <p>&#xA0;&#xA0;&#xA0;80 &#xA0;&#xA0;&#xA0;&#xA0; }</p>
</div>
<p>Here&apos;s the basic run-down. The DependentClassActivator creates looks at all of a class&apos;s constructors. Specifically, at each constructor&apos;s parameter. When it finds a constructor that has parameters it sees in the DependencyServiceContainer, it calls that
  constructor. So you don&apos;t have to use your class&apos;s constructors any longer. Instead, you tell the new DependentClassActivator to give you an instance, instead. Something like this:</p>
<div>&#xA0;&#xA0; 27 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; MockServiceA  a = DependentClassActivator .Instance.CreateInstance&lt; 
  MockServiceA &gt;(); 
    <p>&#xA0;</p>
</div>
<p>To use a metaphor, it&apos;s like walking into a kitchen in which everything you ever need to make anything is already there. You need a spatula, you got it, you need a mixer, you got it. And so on.</p>
<p><strong>How Do You Test It?</strong> </p>
<p>Possibly the most important aspect of all this is the ability to test it. In fact, code written using this DI framework is rather simplistic to test. To explain how you&apos;d use this approach we&apos;ll consider the ever-relevant banking scenario. Below you&apos;ll
  see the test code for all the functionality described earlier. You&apos;ll see some interfaces that have been marked with the DSI attribute, some implementations to create and use, and tie it all up with a mock banking execution example.</p>
<p>&#xA0;</p>
<div>
  <p>&#xA0;&#xA0;&#xA0;10 &#xA0;#region  Bank Service Interfaces and Implementations</p>
  <p>&#xA0;&#xA0;&#xA0;11 &#xA0;</p>
  <p>&#xA0;&#xA0;&#xA0;12 &#xA0;&#xA0;&#xA0;&#xA0; public  class  Account </p>
  <p>&#xA0;&#xA0;&#xA0;13 &#xA0;&#xA0;&#xA0;&#xA0; {</p>
  <p>&#xA0;&#xA0;&#xA0;14 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; private  int  _accountId;</p>
  <p>&#xA0;&#xA0;&#xA0;15 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; private  decimal  _bal;</p>
  <p>&#xA0;&#xA0;&#xA0;16 &#xA0;</p>
  <p>&#xA0;&#xA0;&#xA0;17 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; public  decimal  Balance</p>
  <p>&#xA0;&#xA0;&#xA0;18 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; {</p>
  <p>&#xA0;&#xA0;&#xA0;19 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; get  { return  _bal; }</p>
  <p>&#xA0;&#xA0;&#xA0;20 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; set  { _bal = value ; }</p>
  <p>&#xA0;&#xA0;&#xA0;21 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; }</p>
  <p>&#xA0;&#xA0;&#xA0;22 &#xA0;</p>
  <p>&#xA0;&#xA0;&#xA0;23 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; public  int  AccountId</p>
  <p>&#xA0;&#xA0;&#xA0;24 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; {</p>
  <p>&#xA0;&#xA0;&#xA0;25 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; get  { return  _accountId; }</p>
  <p>&#xA0;&#xA0;&#xA0;26 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; set  { _accountId = value ; }</p>
  <p>&#xA0;&#xA0;&#xA0;27 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; }</p>
  <p>&#xA0;&#xA0;&#xA0;28 &#xA0;&#xA0;&#xA0;&#xA0; }</p>
  <p>&#xA0;&#xA0;&#xA0;29 &#xA0;</p>
  <p>&#xA0;&#xA0;&#xA0;30 &#xA0;&#xA0;&#xA0;&#xA0; [DependencyServiceInterface ]</p>
  <p>&#xA0;&#xA0;&#xA0;31 &#xA0;&#xA0;&#xA0;&#xA0; public  interface  IAccountLookupService </p>
  <p>&#xA0;&#xA0;&#xA0;32 &#xA0;&#xA0;&#xA0;&#xA0; {</p>
  <p>&#xA0;&#xA0;&#xA0;33 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; Account  FindAccount(int  accountId);</p>
  <p>&#xA0;&#xA0;&#xA0;34 &#xA0;&#xA0;&#xA0;&#xA0; }</p>
  <p>&#xA0;&#xA0;&#xA0;35 &#xA0;</p>
  <p>&#xA0;&#xA0;&#xA0;36 &#xA0;&#xA0;&#xA0;&#xA0; [DependencyServiceInterface ]</p>
  <p>&#xA0;&#xA0;&#xA0;37 &#xA0;&#xA0;&#xA0;&#xA0; public  interface  IWithdrawalService </p>
  <p>&#xA0;&#xA0;&#xA0;38 &#xA0;&#xA0;&#xA0;&#xA0; {</p>
  <p>&#xA0;&#xA0;&#xA0;39 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; bool  Withdraw(Account  account, decimal  amount);</p>
  <p>&#xA0;&#xA0;&#xA0;40 &#xA0;&#xA0;&#xA0;&#xA0; }</p>
  <p>&#xA0;&#xA0;&#xA0;41 &#xA0;</p>
  <p>&#xA0;&#xA0;&#xA0;42 &#xA0;&#xA0;&#xA0;&#xA0; [DependencyServiceInterface ]</p>
  <p>&#xA0;&#xA0;&#xA0;43 &#xA0;&#xA0;&#xA0;&#xA0; public  interface  IDepositService </p>
  <p>&#xA0;&#xA0;&#xA0;44 &#xA0;&#xA0;&#xA0;&#xA0; {</p>
  <p>&#xA0;&#xA0;&#xA0;45 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; bool  Deposit(Account  account, decimal  amount);</p>
  <p>&#xA0;&#xA0;&#xA0;46 &#xA0;&#xA0;&#xA0;&#xA0; }</p>
  <p>&#xA0;&#xA0;&#xA0;47 &#xA0;</p>
  <p>&#xA0;&#xA0;&#xA0;48 &#xA0;&#xA0;&#xA0;&#xA0; public  class  AccountLookup  : IAccountLookupService </p>
  <p>&#xA0;&#xA0;&#xA0;49 &#xA0;&#xA0;&#xA0;&#xA0; {</p>
  <p>&#xA0;&#xA0;&#xA0;50 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; #region  IAccountLookupService Members</p>
  <p>&#xA0;&#xA0;&#xA0;51 &#xA0;</p>
  <p>&#xA0;&#xA0;&#xA0;52 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; public  Account  FindAccount(int  accountId)</p>
  <p>&#xA0;&#xA0;&#xA0;53 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; {</p>
  <p>&#xA0;&#xA0;&#xA0;54 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; if  (accountId != 1234 ) return  null ;</p>
  <p>&#xA0;&#xA0;&#xA0;55 &#xA0;</p>
  <p>&#xA0;&#xA0;&#xA0;56 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; Account  mockAccount = new  Account ();</p>
  <p>&#xA0;&#xA0;&#xA0;57 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; mockAccount.AccountId = 1234 ;</p>
  <p>&#xA0;&#xA0;&#xA0;58 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; mockAccount.Balance = 100 ;</p>
  <p>&#xA0;&#xA0;&#xA0;59 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; return  mockAccount;</p>
  <p>&#xA0;&#xA0;&#xA0;60 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; }</p>
  <p>&#xA0;&#xA0;&#xA0;61 &#xA0;</p>
  <p>&#xA0;&#xA0;&#xA0;62 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; #endregion </p>
  <p>&#xA0;&#xA0;&#xA0;63 &#xA0;&#xA0;&#xA0;&#xA0; }</p>
  <p>&#xA0;&#xA0;&#xA0;64 &#xA0;</p>
  <p>&#xA0;&#xA0;&#xA0;65 &#xA0;&#xA0;&#xA0;&#xA0; public  class  AccountWithdrawer  : IWithdrawalService </p>
  <p>&#xA0;&#xA0;&#xA0;66 &#xA0;&#xA0;&#xA0;&#xA0; {</p>
  <p>&#xA0;&#xA0;&#xA0;67 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; #region  IWithdrawalService Members</p>
  <p>&#xA0;&#xA0;&#xA0;68 &#xA0;</p>
  <p>&#xA0;&#xA0;&#xA0;69 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; public  bool  Withdraw(Account  account, decimal     amount)</p>
  <p>&#xA0;&#xA0;&#xA0;70 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; {</p>
  <p>&#xA0;&#xA0;&#xA0;71 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; if  (amount &gt; account.Balance) return  false ;</p>
  <p>&#xA0;&#xA0;&#xA0;72 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; account.Balance -= amount;</p>
  <p>&#xA0;&#xA0;&#xA0;73 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; return  true ;</p>
  <p>&#xA0;&#xA0;&#xA0;74 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; }</p>
  <p>&#xA0;&#xA0;&#xA0;75 &#xA0;</p>
  <p>&#xA0;&#xA0;&#xA0;76 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; #endregion </p>
  <p>&#xA0;&#xA0;&#xA0;77 &#xA0;&#xA0;&#xA0;&#xA0; }</p>
  <p>&#xA0;&#xA0;&#xA0;78 &#xA0;</p>
  <p>&#xA0;&#xA0;&#xA0;79 &#xA0;&#xA0;&#xA0;&#xA0; public  class  AccountDepositer  : IDepositService </p>
  <p>&#xA0;&#xA0;&#xA0;80 &#xA0;&#xA0;&#xA0;&#xA0; {</p>
  <p>&#xA0;&#xA0;&#xA0;81 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; #region  IDepositService Members</p>
  <p>&#xA0;&#xA0;&#xA0;82 &#xA0;</p>
  <p>&#xA0;&#xA0;&#xA0;83 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; public  bool  Deposit(Account  account, decimal     amount)</p>
  <p>&#xA0;&#xA0;&#xA0;84 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; {</p>
  <p>&#xA0;&#xA0;&#xA0;85 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; account.Balance += amount;</p>
  <p>&#xA0;&#xA0;&#xA0;86 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; return  true ;</p>
  <p>&#xA0;&#xA0;&#xA0;87 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; }</p>
  <p>&#xA0;&#xA0;&#xA0;88 &#xA0;</p>
  <p>&#xA0;&#xA0;&#xA0;89 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; #endregion </p>
  <p>&#xA0;&#xA0;&#xA0;90 &#xA0;&#xA0;&#xA0;&#xA0; }</p>
  <p>&#xA0;&#xA0;&#xA0;91 &#xA0;</p>
  <p>&#xA0;&#xA0;&#xA0;92 &#xA0;&#xA0;&#xA0;&#xA0; public  class  Bank </p>
  <p>&#xA0;&#xA0;&#xA0;93 &#xA0;&#xA0;&#xA0;&#xA0; {</p>
  <p>&#xA0;&#xA0;&#xA0;94 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; IAccountLookupService  lookupService;</p>
  <p>&#xA0;&#xA0;&#xA0;95 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; IWithdrawalService  withdrawalService;</p>
  <p>&#xA0;&#xA0;&#xA0;96 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; IDepositService  depositService;</p>
  <p>&#xA0;&#xA0;&#xA0;97 &#xA0;</p>
  <p>&#xA0;&#xA0;&#xA0;98 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; public  Bank(IAccountLookupService  lookupService,</p>
  <p>&#xA0;&#xA0;&#xA0;99 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; IWithdrawalService  withdrawalService,</p>
  <p>&#xA0;&#xA0;100 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; IDepositService  depositService)</p>
  <p>&#xA0;&#xA0;101 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; {</p>
  <p>&#xA0;&#xA0;102 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; this .lookupService = lookupService; </p>
  <p>&#xA0;&#xA0;103 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; this .withdrawalService = withdrawalService; </p>
  <p>&#xA0;&#xA0;104 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; this .depositService = depositService; </p>
  <p>&#xA0;&#xA0;105 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; }</p>
  <p>&#xA0;&#xA0;106 &#xA0;</p>
  <p>&#xA0;&#xA0;107 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; public  Account  GetAccount(int  id)</p>
  <p>&#xA0;&#xA0;108 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; {</p>
  <p>&#xA0;&#xA0;109 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; return  this .lookupService.FindAccount(id); </p>
  <p>&#xA0;&#xA0;110 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; }</p>
  <p>&#xA0;&#xA0;111 &#xA0;</p>
  <p>&#xA0;&#xA0;112 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; public  bool  Withdraw(Account  account, decimal     amount)</p>
  <p>&#xA0;&#xA0;113 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; {</p>
  <p>&#xA0;&#xA0;114 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; return  this .withdrawalService.Withdraw(account, amount); </p>
  <p>&#xA0;&#xA0;115 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; }</p>
  <p>&#xA0;&#xA0;116 &#xA0;</p>
  <p>&#xA0;&#xA0;117 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; public  bool  Deposit(Account  account, decimal     amount)</p>
  <p>&#xA0;&#xA0;118 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; {</p>
  <p>&#xA0;&#xA0;119 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; return  this .depositService.Deposit(account, amount); </p>
  <p>&#xA0;&#xA0;120 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; }</p>
  <p>&#xA0;&#xA0;121 &#xA0;&#xA0;&#xA0;&#xA0; }</p>
  <p>&#xA0;&#xA0;122 &#xA0;</p>
  <p>&#xA0;&#xA0;123 &#xA0;&#xA0;&#xA0;&#xA0; #endregion </p>
  <p>&#xA0;&#xA0;124 &#xA0;</p>
  <p>&#xA0;&#xA0;125 &#xA0;&#xA0;&#xA0;&#xA0; #region  Tests</p>
  <p>&#xA0;&#xA0;126 &#xA0;</p>
  <p>&#xA0;&#xA0;127 &#xA0;&#xA0;&#xA0;&#xA0; [TestFixture ]</p>
  <p>&#xA0;&#xA0;128 &#xA0;&#xA0;&#xA0;&#xA0; public  class  BankAccountTests </p>
  <p>&#xA0;&#xA0;129 &#xA0;&#xA0;&#xA0;&#xA0; {</p>
  <p>&#xA0;&#xA0;130 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; [SetUp ]</p>
  <p>&#xA0;&#xA0;131 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; public  void  Setup()</p>
  <p>&#xA0;&#xA0;132 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; {</p>
  <p>&#xA0;&#xA0;133 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; }</p>
  <p>&#xA0;&#xA0;134 &#xA0;</p>
  <p>&#xA0;&#xA0;135 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; [TearDown ]</p>
  <p>&#xA0;&#xA0;136 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; public  void  TearDown()</p>
  <p>&#xA0;&#xA0;137 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; {</p>
  <p>&#xA0;&#xA0;138 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; }</p>
  <p>&#xA0;&#xA0;139 &#xA0;</p>
  <p>&#xA0;&#xA0;140 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; [Test ]</p>
  <p>&#xA0;&#xA0;141 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; public  void  CanBankClassBeCreated()</p>
  <p>&#xA0;&#xA0;142 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; {</p>
  <p>&#xA0;&#xA0;143 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; Bank  bank = DependentClassActivator .Instance.CreateInstance&lt; 
    Bank &gt;(); </p>
  <p>&#xA0;&#xA0;144 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; Assert .IsNotNull(bank); </p>
  <p>&#xA0;&#xA0;145 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; }</p>
  <p>&#xA0;&#xA0;146 &#xA0;</p>
  <p>&#xA0;&#xA0;147 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; [Test ]</p>
  <p>&#xA0;&#xA0;148 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; public  void  CanBankHandBackAccount()</p>
  <p>&#xA0;&#xA0;149 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; {</p>
  <p>&#xA0;&#xA0;150 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; Bank  bank = DependentClassActivator .Instance.CreateInstance&lt; 
    Bank &gt;(); </p>
  <p>&#xA0;&#xA0;151 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; Account  account&#xA0; = bank.GetAccount(1234 );</p>
  <p>&#xA0;&#xA0;152 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; Assert .IsNotNull(account ); </p>
  <p>&#xA0;&#xA0;153 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; account = bank.GetAccount(1000 );</p>
  <p>&#xA0;&#xA0;154 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; Assert .IsNull(account ); </p>
  <p>&#xA0;&#xA0;155 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; }</p>
  <p>&#xA0;&#xA0;156 &#xA0;</p>
  <p>&#xA0;&#xA0;157 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; [Test ]</p>
  <p>&#xA0;&#xA0;158 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; public  void  CanBankAccountWithdrawMoney()</p>
  <p>&#xA0;&#xA0;159 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; {</p>
  <p>&#xA0;&#xA0;160 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; Bank  bank = DependentClassActivator .Instance.CreateInstance&lt; 
    Bank &gt;(); </p>
  <p>&#xA0;&#xA0;161 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; Account  account = bank.GetAccount(1234 );</p>
  <p>&#xA0;&#xA0;162 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; Assert .IsNotNull(account); </p>
  <p>&#xA0;&#xA0;163 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; decimal  bal = account.Balance;</p>
  <p>&#xA0;&#xA0;164 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; decimal  amt = 42 ;</p>
  <p>&#xA0;&#xA0;165 &#xA0;</p>
  <p>&#xA0;&#xA0;166 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; bool  result = bank.Withdraw(account , amt);</p>
  <p>&#xA0;&#xA0;167 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; Assert .IsTrue(result); </p>
  <p>&#xA0;&#xA0;168 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; Assert .AreEqual((bal - amt), account .Balance); </p>
  <p>&#xA0;&#xA0;169 &#xA0;</p>
  <p>&#xA0;&#xA0;170 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; result = bank.Withdraw(account , 99999999 );</p>
  <p>&#xA0;&#xA0;171 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; Assert .IsFalse(result); </p>
  <p>&#xA0;&#xA0;172 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; }</p>
  <p>&#xA0;&#xA0;173 &#xA0;</p>
  <p>&#xA0;&#xA0;174 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; [Test ]</p>
  <p>&#xA0;&#xA0;175 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; public  void  CanBankAccountDepositMoney()</p>
  <p>&#xA0;&#xA0;176 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; {</p>
  <p>&#xA0;&#xA0;177 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; Bank  bank = DependentClassActivator .Instance.CreateInstance&lt; 
    Bank &gt;(); </p>
  <p>&#xA0;&#xA0;178 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; Account  account = bank.GetAccount(1234 );</p>
  <p>&#xA0;&#xA0;179 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; Assert .IsNotNull(account ); </p>
  <p>&#xA0;&#xA0;180 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; decimal  bal = account .Balance;</p>
  <p>&#xA0;&#xA0;181 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; decimal  amt = 42 ;</p>
  <p>&#xA0;&#xA0;182 &#xA0;</p>
  <p>&#xA0;&#xA0;183 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; bool  result = bank.Deposit(account , amt);</p>
  <p>&#xA0;&#xA0;184 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; Assert .IsTrue(result); </p>
  <p>&#xA0;&#xA0;185 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; Assert .AreEqual((bal + amt), account .Balance); </p>
  <p>&#xA0;&#xA0;186 &#xA0;&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; }</p>
  <p>&#xA0;&#xA0;187 &#xA0;&#xA0;&#xA0;&#xA0; }</p>
  <p>&#xA0;&#xA0;188 &#xA0;</p>
  <p>&#xA0;&#xA0;189 &#xA0;&#xA0;&#xA0;&#xA0; #endregion </p>
</div>
<p>I welcome any comments on this approach. Is this DI or have I completely missed the boat on this whole concept. Hopefully this look at one appraoch DI has been as enlightening as it has been for me. Happy coding!</p>
