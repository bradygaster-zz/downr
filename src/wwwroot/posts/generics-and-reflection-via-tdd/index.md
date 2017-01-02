---
title: Generics and Reflection via TDD
slug: generics-and-reflection-via-tdd
author: bradygaster
lastModified: 2012-10-05 18:15:15
pubDate: 2012-10-05 18:15:02
categories: .NET
---

<p>Pluggable development requires trickery. Sometimes naivety is a requirement in the development of pluggable solutions and, though not always the best idea, dynamically-dynamic code is the only way these problems can be solved. If for no other reason than
  to have a record of how to solve certain situations when they arise in my own life again, I&#x2019;m going to try to put together a series of posts on how reflection and generic usage can be pretty neat together.</p>
<p>
  <img src="/posts/generics-and-reflection-via-tdd/media/image_3.png" alt="image">
</p>
<p>This first post is to answer a question from a colleague tonight. He&#x2019;s reading a type via a custom configuration element and then using that type as a generic parameter to a method call.</p>
<blockquote>
  <p>How do I call a generic method if I don&#x2019;t know the type I&#x2019;ll be providing the generic argument at run-time?</p>
</blockquote>
<p>The question&#x2019;s one I&#x2019;ve had to remember how to do a few times, so here&#x2019;s to hoping it helps someone else. The screen shot below contains a unit test class with a sample generic method. The test points to a type&#x2019;s generic method using reflection via the
  <a>MethodInfo.MakeGenericMethod</a> <em></em>  method and then calls the method using the
  <a>MethodInfo.Invoke</a>  method.</p>
<p>Happy coding!</p>
