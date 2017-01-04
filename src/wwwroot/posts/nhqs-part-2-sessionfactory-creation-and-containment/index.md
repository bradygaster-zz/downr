---
title: NHQS Part 2- SessionFactory Creation and Containment
slug: nhqs-part-2-sessionfactory-creation-and-containment
author: bradygaster
lastModified: 2012-10-05 18:15:15
pubDate: 2012-10-05 18:15:06
categories: .NET,NHibernate
---

<p>As explained in various posts all over the internet (
  <a href="http://stackoverflow.com/questions/2931036/multiple-sessionfactories-in-windows-service-with-nhibernate">like this one at StackOverflow</a> ) the <em>ISessionFactory </em> interface is fundamental to how NHibernate works. As the StackOverflow links points out, creating a session factory is possibly the most expensive of the things NHibernate does during
  execution. A frequent misstep of developers new to NHibernate is to write code that will frequently &#x2013; <em>sometimes prior to each database call for the more heinous abusers</em>  &#x2013; create session factory instances. As with any technology, that which
  isn&#x2019;t used properly will probably result in less-than-favorable outcomes. NHQS simplifies this step for the developer, as well as facades the complexity associated with session factory storage once the application using the session factory has instantiated
  it.</p>
Example Domains
<p>
  <a href="/Media/Default/Windows-Live-Writer/NHQS-Part-2_8C38/image_18.png">
    <img alt="image" src="/posts/nhqs-part-2-sessionfactory-creation-and-containment/media/image_thumb_6.png">
  </a> Throughout this blog series the Visual Studio 2010 solution pictured to the left will be used. The solution consists of the NHQS framework, two domain projects, two data access projects to accommodate those domain projects, and a unit test project.
  To demonstrate how NHQS can connect not only to multiple database instances, but to multiple database platforms agnostically, the domain/data-access examples use 2 different databases; the <em>People </em> scenario will be backed by a SQL Server Compact
  Edition database and the <em>Orders </em> scenario will be backed by a SQL Server 2008 database. NHQS has support for many other database platforms so you should be covered in virtually all RDBMS situations.</p>
Session Factory Creation via a Convention
<p>Within NHQS there exists an interface named <em>ISessionFactoryCreator. </em> Obviously, that&#x2019;s the spot at which our investigation will begin. The interface contract is shown below. The idea behind it is quite simple &#x2013; just give a developer an easy way
  of handing their application a session factory and let them do pretty much whatever they want to do to create it.</p>
<p>
  <img alt="The contract for creating a session factory" src="/posts/nhqs-part-2-sessionfactory-creation-and-containment/media/image_3.png">
</p>
<p>The implementation of the session factory creator interface isn&#x2019;t too difficult. For the People domain we&#x2019;re using SQLCE, so the code below set up the session factory instance for that particular data source using the SQLCE persistence configuration and
  fluent auto-mapping. An in-depth exploration into these topics is beyond the scope of this post; I can assure you there are
  <a title="from the Fluent NHibernate documentation - how to fluently configure NHibernate" href="http://wiki.fluentnhibernate.org/Getting_started#Configuration">far</a> 
  <a title="Fluent NHibernate configuration in-depth" href="http://wiki.fluentnhibernate.org/Fluent_configuration">better</a>  resources out there to explain such techniques. For now, take a look at the implementation.</p>
<p>
  <img alt="Creating a SQL Compact Edition-backed NHibernate Session Factory" src="/posts/nhqs-part-2-sessionfactory-creation-and-containment/media/image_10.png">
</p>
<p>Now that the session factory for the People data source can be created it&#x2019;ll need a place to <em>hang out </em> in the application&#x2019;s domain. The next aspect of NHQS, session factory containment, solves this problem for developers.</p>
Session Factory Containment
<p>One of the goals of NHQS is to provide multiple database access to a single application or web site. Inspired by the idea posited in the DAAB of &#x201C;accessing multiple databases by name,&#x201D; this goal was an interesting one to solve. As mentioned previously,
  the act of creating a session factory is an expensive one and must be done sparingly. Therefore, session factories can be thought of as <em>things that you might want to make Singleton instances, </em> so their storage is quite important.</p>
<p>Once a session factory is created it must be added to the session factory containment class. This isn&#x2019;t too difficult and can be demonstrated in the unit test setup method below. When the application (in this case a unit test execution) starts up, all
  of the session factories used by the application should be created and added to the container.</p>
<p>
  <img alt="Adding a session factory to the container" src="/posts/nhqs-part-2-sessionfactory-creation-and-containment/media/image_13.png">
</p>
<p>The <em>SessionFactoryContainer </em> class does a little more than just <em>contain </em> the session factories created by each <em>ISessionFactoryCreator</em>  implementation, too, but we&#x2019;ll cover those in slightly more detail in the subsequent posts.
  For now, consider the multi-database goal alongside the domain centric access strategy goal. Since NHQS will be containing the session factories for you, chances are it will have the ability to do some mild interrogation of the entity domains it wraps.</p>
<p>Consider the code below, which modifies the test setup function slightly to accommodate a second session factory that is also created by an implementation of the <em>ISessionFactoryCreator </em> interface.</p>
<p>
  <img alt="Adding two session factories to the container" src="/posts/nhqs-part-2-sessionfactory-creation-and-containment/media/image_16.png">
</p>
<p>The next post in this series will begin to explain how, once the session factories have been created and contained, the domain-centric language can provide the CRUD operations necessary to work with the entities comprising these two domains. That&#x2019;s when
  the power and simplicity of NHQS becomes obvious, so stay tuned!</p>
