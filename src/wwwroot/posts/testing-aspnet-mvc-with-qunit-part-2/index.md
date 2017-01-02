---
title: Testing ASP.NET MVC with QUnit - Part 2
slug: testing-aspnet-mvc-with-qunit-part-2
author: bradygaster
lastModified: 2011-05-21 02:07:54
pubDate: 2012-10-05 18:15:04
categories: .NET,JavaScript
---

<p>In
  <a>Part 1 of this series</a>  I demonstrated how QUnit can be used to test JsonResult action methods in ASP.NET MVC applications. Part 2 will take the idea a little further by showing an example of how QUnit can be used to inspect potential user-input areas
  on your MVC forms and to use those values in tests that will verify the requirements have been met.</p>
<p>The scenario will be a search form that will search a database of people. To use the mindset from a post I made earlier this week, the application will need to meet the following requirements:</p>
<ul>
  <li>People will consist of first and last names. </li>
  <li>The search will need to allow for a free-form text entry field. Each person record whose first or last names contain the string being searched will be returned to the user.</li>
</ul>
<p>Not too difficult a set of requirements but clearly-enough stated so that tests can be provided. Of course, a stub of functionality will be created to provide the <em>database </em> of people. The code below demonstrates a PersonFactory class that will
  return instances of the Person class in a generic List.</p>
<pre>public class PersonFactory
{
&#xA0;&#xA0;public static List GetAll()
&#xA0;&#xA0;{
&#xA0;&#xA0;&#xA0;&#xA0;List people = new List();
&#xA0;&#xA0;&#xA0;&#xA0;people.Add(new Person { FirstName = &quot;Al&quot;, LastName = &quot;Pacino&quot; });
&#xA0;&#xA0;&#xA0;&#xA0;people.Add(new Person { FirstName = &quot;Val&quot;, LastName = &quot;Kilmer&quot; });
&#xA0;&#xA0;&#xA0;&#xA0;people.Add(new Person { FirstName = &quot;Robert&quot;, LastName = &quot;DeNiro&quot; });
&#xA0;&#xA0;&#xA0;&#xA0;return people;
&#xA0;&#xA0;}
}
public class Person
{
&#xA0;&#xA0;public string FirstName { get; set; }
&#xA0;&#xA0;public string LastName { get; set; }
}
</pre>
<p>Next, a controller method is added that will make use of the PersonFactory and return the search results to the client in a JsonResult instance.</p>
<pre>public JsonResult FindPerson(string name)<br>{
&#xA0;&#xA0;List people = PersonFactory.GetAll();
&#xA0;&#xA0;JsonResult result = new JsonResult();
&#xA0;&#xA0;people = 
&#xA0;&#xA0;people.FindAll(x =&gt; x.FirstName.Contains(name) || x.LastName.Contains(name)).ToList();
&#xA0;&#xA0;result.Data = people;
&#xA0;&#xA0;return result;
}<br></pre>
<p>Since the expectation is that the person-searching functionality will be performed from a web page on which a textbox is provided to the user for free-form entry a mock GUI will be created to drive the test itself. The HTML code below provides a form
  that the tests will use in a moment.</p>
<pre><div><br>FindPerson Parameter<br></div><br></pre>
<p>Think of it this way. If a web application needs to first go through a rigid QA process to succeed a checkpoint, what better way to make sure the QA process runs as smoothly as possible by automating the use-cases agreed on by the team in the form of
  unit tests? The obvious next step is to do the very thing we&apos;d expect a QA person to do - enter some text into the specified text box and perform the search with the expectation that our search would return a result.&#xA0;</p>
<pre>$(&apos;#findPersonName&apos;).val(&apos;Pacino&apos;)<br>&#xA0;&#xA0;$.getJSON(&apos;/Home/FindPerson&apos;, { name: $(&apos;#findPersonName&apos;).val() }, function(data)<br>&#xA0;&#xA0;{<br>&#xA0;&#xA0;&#xA0;&#xA0;module(&apos;FindPerson&apos;);<br>&#xA0;&#xA0;&#xA0;&#xA0;test(&apos;FindPerson with known value for last name returns matching person records&apos;, function()<br>&#xA0;&#xA0;&#xA0;&#xA0;{<br>&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;equals((data.length &gt; 0), true, &apos;At least one person should return from the search&apos;);<br>&#xA0;&#xA0;&#xA0;&#xA0;});<br>&#xA0;&#xA0;});<br>&#xA0;&#xA0;$(&apos;#findPersonName&apos;).val(&apos;Robert&apos;)<br>&#xA0;&#xA0;$.getJSON(&apos;/Home/FindPerson&apos;, { name: $(&apos;#findPersonName&apos;).val() }, function(data)<br>&#xA0;&#xA0;{<br>&#xA0;&#xA0;&#xA0;&#xA0;module(&apos;FindPerson&apos;);<br>&#xA0;&#xA0;&#xA0;&#xA0;test(&apos;FindPerson with known value for first name returns matching person records&apos;, function()<br>&#xA0;&#xA0;&#xA0;&#xA0;{<br>&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;equals((data.length &gt; 0), true, &apos;At least one person should return from the search&apos;);<br>&#xA0;&#xA0;&#xA0;&#xA0;});<br>&#xA0;&#xA0;});<br></pre>
<p>Of course, any good testing process will test the process&apos;s ability to fail gracefully as well. To accomplish that, another string will be entered that logically would always fail to return a result.&#xA0;</p>
<pre>$(&apos;#findPersonName&apos;).val(&apos;NOWAY&apos;)<br>&#xA0;&#xA0;$.getJSON(&apos;/Home/FindPerson&apos;, { name: $(&apos;#findPersonName&apos;).val() }, function(data)<br>&#xA0;&#xA0;{<br>&#xA0;&#xA0;&#xA0;&#xA0;module(&apos;FindPerson&apos;);<br>&#xA0;&#xA0;&#xA0;&#xA0;test(&apos;FindPerson with value not found in list returns no records&apos;, function()<br>&#xA0;&#xA0;&#xA0;&#xA0;{<br>&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;equals((data.length == 0), true, &apos;No results should be returned from the search&apos;);<br>&#xA0;&#xA0;&#xA0;&#xA0;});<br>&#xA0;&#xA0;});<br></pre>
<p>The HTML output would appear like the screenshot below.&#xA0;</p>
<p>
  <img src="/image.axd?picture=2009%2f2%2ftesting_mvc_qunit_pt2_1.png">
</p>
<p>In this example, QUnit was used to automate the modification of form data and to perform unit tests with that user data. Hopefully this gives you some ideas of how you might be able to automate your own client-side GUI experiences. Writing this blog raised
  an interesting question for me. If a software product&apos;s QA lifecycle involves end-to-end system and user-acceptance testing, could an approach like this the use of QUnit or any other client-automation/scripting tool reduce testing and acceptance times?&#xA0;</p>
