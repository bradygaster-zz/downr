---
title: Testing ASP.NET MVC with QUnit - Part 1
slug: testing-aspnet-mvc-with-qunit-part-1
author: bradygaster
lastModified: 2014-10-31 12:19:23
pubDate: 2012-10-05 18:15:04
categories: JavaScript
---

<p>One of the problems that face any web developer is their ability to properly test the GUI components of their sites. A few options exist, specifically
  <a>Watin</a>, with varying degrees of success. Now that I&apos;ve been experimenting with the ASP.NET MVC framework I&apos;ve modified a good deal of my GUI work in such a way that it minimizes form-posts and makes use of the AJAX goodies packed into jQuery. Since
  a lot of my work has moved to the client following my adoption of this approach I needed to investigate new options for testing. This morning I had the luck to stumble across QUnit, a jQuery testing plugin that makes life pretty easy.&#xA0;</p>
<p>I won&apos;t spend a lot of time explaining how QUnit works. That job has been done already by the
  <a>jQuery documentation team</a>  and in a
  <a>great tutorial by Chad Myers</a> . I&apos;d encourage you take a look at those links to get a little more familiar with how QUnit works. It&apos;s quite simple and elegant, like most other plugins in the jQuery world. What I&apos;ll be demonstrating below is how you
  can use QUnit to unit-test ASP.NET MVC client functionality.</p>
<p>The first example will offer up a much-needed scenario - testing the output of controller methods that return JSON data in the form of JsonResult class instances. The TestJsonMessage() method below exemplifies a typical AJAX request. The client uses jQuery
  to make the call, the JsonResult is built within the Action method and returned to the client, where it can be accessed in an object-oriented manner via jQuery.&#xA0;</p>
<pre>public JsonResult TestJsonMessage()
{
&#xA0;&#xA0;return new JsonResult
&#xA0;&#xA0;{
&#xA0;&#xA0;&#xA0;&#xA0;Data = new
&#xA0;&#xA0;&#xA0;&#xA0;{
&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;Status = true
&#xA0;&#xA0;&#xA0;&#xA0;}
&#xA0;&#xA0;};
}
</pre>
<p>In the View&apos;s HTML code JavaScript will be added to perform the test. The TestJsonMessage view is loaded via jQuery&apos;s
  <a>getJSON</a>  method. When the data is returned to the client it is used in the actual test code.&#xA0;</p>
<pre>$(document).ready(function()
{
&#xA0;&#xA0;$.getJSON(&apos;/Home/TestJsonMessage&apos;, function(data)
&#xA0;&#xA0;{
&#xA0;&#xA0;&#xA0;&#xA0;test(&apos;TestJsonMessage status should be true&apos;, function()
&#xA0;&#xA0;&#xA0;&#xA0;{
&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;equals(Boolean(data.Status), true, &apos;The server should return a status of true&apos;);
&#xA0;&#xA0;&#xA0;&#xA0;});
&#xA0;&#xA0;});
});
</pre>
<p>Once the test runs, the output below is generated, indicating the test&apos;s success.&#xA0;</p>
<p>
  <img src="/image.axd?picture=2009%2f2%2ftesting_mvc_qunit_pt1_1.png">
</p>
<p>This example demonstrates how one might use QUnit to test their MVC actions, specifically those actions that return JSON data to the client. In part 2 of this series I&apos;ll provide an example of using QUnit to test changes that might be made to an HTML
  GUI as a result of MVC actions being called.&#xA0;</p>
