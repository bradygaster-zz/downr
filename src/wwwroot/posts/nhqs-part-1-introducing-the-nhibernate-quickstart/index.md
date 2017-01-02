---
title: Introducing the NHibernate QuickStart (NHQS, Part 1)
slug: nhqs-part-1-introducing-the-nhibernate-quickstart
author: bradygaster
lastModified: 2011-05-29 02:45:40
pubDate: 2012-10-05 18:15:07
categories: .NET,NHibernate
---

<p>If you&#x2019;ve worked with me over the past year or you&#x2019;ve seen one of my alt group meetings, chances are I&#x2019;ve mentioned, demonstrated, or maybe stuffed down your throat, the NHQS thingamabob. With the improvements in NHibernate following .NET 4.0 and the
  experiences I learned in a few projects in the previous release I&#x2019;ve decided to rewrite NHQS for NHibernate 3.xx. I like the improvements in NHibernate and with those and a slightly renewed focus on my goal for NHQS I felt it might be worthwhile to
  introduce it on its own into the wild. This post will introduce the world to NHQS and provide a guideline for its usage.</p>
<p>As usual the project is only as effective as is the goal it intends to serve so let&#x2019;s get that out of the way.</p>
<blockquote>
  <p>A lot of developers want to use NHiibernate, or Fluent NHibernate, or some flavor therein, but don&#x2019;t want to learn the layers and layers of how-and-why it works just to get up and running. NHQS should mitigate this confusion and ease the transition into usage of NHibernate. A secondary goal of NHQS is to make it easy for a developer to have access to any and all persistent storage mechanisms supported by NHibernate in a fluent and domain-centric manner. At the highest level, a developer facilitating object persistence via NHQS should neither need to know nor be exposed to the underlying persistence mechanism.  </p>
</blockquote>
<p>To accomplish that goal we&#x2019;re going to walk through the steps of setting up and using NHQS to access multiple databases via the almost-too-simple-to-be-legit fluent/generic interfaces exposed almost solely via extension methods. I&#x2019;m sure I&#x2019;ll get burned
  at the stake for failing to follow <em>someone&#x2019;s </em> NHibernate Best Practice 101 list, so I&#x2019;ll apologize up front. With that, here&#x2019;s the synopsis of the blog series, which I aim to complete by the end of this week.</p>
<ol>
  <li>
    <a>Introduction (this post) </a> 
    </li><li>
      <a>Session Factory Creation and Containment </a> 
      </li><li>
        <a>Session Fluency Extensions </a> 
        </li><li> <a>Transactions and Multiple Database Support</a>  </li>
</ol>
NHQS in 30 Seconds
<p>There are a few steps one must follow to get their persistence mechanism accessible via NHQS. Here they are:</p>
<ol>
  <li>Implement the <em>ISessionFactoryCreator</em>  interface via a class of your own (we&#x2019;ll cover this in a moment)
    </li><li>Add code to the startup of the calling application (or in your <em>TestFixtureSetup</em>  method) that calls the <em>Create</em>  method of that class to create an NHibernate <em>ISessionFactory</em>  object instance
      </li><li>Take the resultant <em>ISessionFactory </em> and store it using the <em>SessionFactoryContainer</em>  class
        </li><li>Write DDD-like code to get objects from databases and perform CRUD operations via an arsenal of extension methods attached to areas of NHibernate </li>
</ol>
<p>The second post in this series will begin the process of using NHQS in a real-world situation by simplifying the art of creating <em>ISessionFactories </em> and storing them in a container, so check back soon.</p>
<p>If you&#x2019;d like to skip the blogs and look at the source, feel free, I keep the
  <a>NHQS source in Github</a> .</p>
