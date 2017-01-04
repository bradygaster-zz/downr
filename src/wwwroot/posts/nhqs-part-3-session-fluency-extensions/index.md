---
title: NHQS Part 3- Session Fluency Extensions
slug: nhqs-part-3-session-fluency-extensions
author: bradygaster
lastModified: 2012-10-05 18:15:13
pubDate: 2012-10-05 18:15:07
categories: .NET,NHibernate
---

<p>Though this might be the poorliest-named aspect of NHQS it is my favorite part, for it takes a huge part of the complexities associated with NHibernate and makes them simple. Dirt simple. Like, &#x201C;you&#x2019;re kidding, right? That couldn&#x2019;t possibly work,&#x201D; simple.
  </p>
<p>In part 2, the session factory containment methodology was discussed and demonstrated. During the stage of session factory containment NHQS does a wee little trick; it associates all of the domain (or if you prefer, <em>entity</em> ) types with the session
  factory responsible for performing that domain type&#x2019;s CRUD responsibilities. </p>
<p>What? Huh? </p>
<p>In other words, during containment, NHQS learns that, when asked for a Person, to hand back session factory A. When asked for an Order hand back session factory B. Whatever domain is requested, the framework should know which session factory to ask for
  that domain object; it should be transparent to the developer using NHQS. </p>
<p>Take a look again at the test fixture setup for the NHQS demonstration tests. This setup function was introduced in the previous post but has been augmented to help explain the topic in this post. The relevant addition has been highlighted for simplicity.
  </p>
<p>
  <img alt="image" src="/posts/nhqs-part-3-session-fluency-extensions/media/image_3.png">
</p>
<p>To further explain what&#x2019;s going on, let&#x2019;s walk through that highlighted section, piece by piece, and get a good understanding the steps taking place at execution time. </p>
<p>
  <img alt="image" src="/posts/nhqs-part-3-session-fluency-extensions/media/image_6.png">
</p>
<p>This first part looks in the session factory container and finds the session factory that has been set to perform CRUD operations against the SQLCE database that stores Person class instances. The return of the <em>For</em>  method is, obviously, the
  instance of the <em>ISessionFactory</em>  interface that was previously stored in the container. Note &#x2013; this isn&#x2019;t <em>creating a new session factory each time, </em> because, as pointed out in a previous post, that&#x2019;s the probably single most expensive
  [RE: <em>painful</em> ] thing you can do with NHibernate. Since each session factory is created once and maintained within the session factory container, the container is just handing it back based on the type of domain object needed by the calling code.
  </p>
<p>
  <img alt="image" src="/posts/nhqs-part-3-session-fluency-extensions/media/image_11.png">
</p>
<p>Since the <em>For</em>  method hands back a session factory instance, this line should be relatively self-explanatory &#x2013; it creates an NHibernate <em>ISession</em>  instance and hands it back to the calling code. Now for the good stuff, the session extension
  methods. </p>
<p>
  <img alt="image" src="/posts/nhqs-part-3-session-fluency-extensions/media/image_14.png">
</p>
<p>Hanging off of the session object are a few NHQS extension methods. Located within the root NHQS namespace, these methods do pretty much what their normal method equivalents do &#x2013; the CRUD operations against the database. The <em>Save</em>  method takes
  an instance of the domain object type represented in the generic argument and saves it to the NHibernate session and eventually, the SQLCE database. </p>
<p>Moving on, we need to have a unit test to make sure we stored some data. As with the <em>Save</em>  extension method, NHQS provides a retrieval methodology that uses LINQ Expressions to get to the data in the database. </p>
<p>
  <img alt="image" src="/posts/nhqs-part-3-session-fluency-extensions/media/image_25.png">
</p>
<p>This test will, obviously, pass, as the setup routine saved some data to the database. We can also ask for the specific data we saved to make sure the test isn&#x2019;t returning other data that&#x2019;s been persisted at some other time. The second unit test below
  demonstrates this in action. </p>
<p>
  <img alt="image" src="/posts/nhqs-part-3-session-fluency-extensions/media/image_26.png">
</p>
<p>Most of the extension methods have overloaded options, too, if you want to continue to chain together operations in a fluent manner. The test below demonstrates an example of this approach in action.</p>
<p>
  <img alt="image" src="/posts/nhqs-part-3-session-fluency-extensions/media/image_29.png">
</p>
<p>Just take a look at all the work NHibernate&#x2019;s doing under the hood from the debugging log generated at run-time when this unit test (and the setup) are executed. As you&#x2019;ll see, NHQS makes CRUD activities a lot simpler than doing multiple methods of database
  logic. Obviously, it makes such processes a little more testable, too. </p>
<p>
  <a href="/Media/Default/Windows-Live-Writer/ec0ad61d4aad_CF5B/image_31.png">
    <img alt="Click to zoom" src="/posts/nhqs-part-3-session-fluency-extensions/media/image_thumb_10.png">
  </a> 
</p>
<p>This post sums up probably the biggest benefit NHQS could provide most developers &#x2013; easy, fluent access to their domain persistence via simple, domain-centric language and chainable methods. </p>
<p>The final post in this series (which may be split into two posts) will demonstrate two more important facets of NHQS &#x2013; multiple-database access and the big kahuna topic in most database/ORM discussions &#x2013; transaction management. We&#x2019;ll peek at these two
  topics in the next few days and wrap up this NHQS introduction. </p>
<p>I hope you&#x2019;ve enjoyed the series so far!</p>
