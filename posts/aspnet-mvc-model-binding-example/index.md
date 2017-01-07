---
title: ASP.NET MVC Model Binding Example
slug: aspnet-mvc-model-binding-example
author: bradygaster
lastModified: 2014-12-04 08:18:36
pubDate: 2012-10-05 18:15:05
categories: .NET
---

<p>Scott does an excellent job in
  <a href="http://weblogs.asp.net/scottgu/archive/2008/10/16/asp-net-mvc-beta-released.aspx" title="ScottGu ASP.NET MVC Beta">his introduction blog post</a>  to the new features in the ASP.NET MVC beta release. The Model Binder support is an excellent feature for which I wanted to put forth a simple example.&#xA0;</p>
<p>In essence, Model Binding allows you to pass an object into your controller methods rather than be required to pass the values of each property for your model that you intend to set within your Controller method. In retrospect, that description sounds
  pretty harsh and confusing, doesn&apos;t it? For now I&apos;ll spare you an introduction to Model Binding, Scott&apos;s already done an excellent job of that via these
  <a href="http://weblogs.asp.net/scottgu/archive/2008/09/02/asp-net-mvc-preview-5-and-form-posting-scenarios.aspx" title="ScottGu - MVC Preview 5">two</a> 
  <a href="http://weblogs.asp.net/scottgu/archive/2008/10/16/asp-net-mvc-beta-released.aspx" title="ScottGu - MVC Beta">posts</a> .&#xA0;</p>
<p>For this example I&apos;ll continue with Scott&apos;s Person class example. I&apos;ll have my Controller method, which takes instances of Person classes. The code below contains all of the Controller and Person Model code for this form-posting scenario. Pay special
  attention to the <strong>Save </strong> method, as it demonstrates usage of Model Binding; an instance of the Person class is passed into this method via a parameter named <strong>person</strong> .&#xA0;</p>
<p>&#xA0;</p>
<pre>using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
namespace LearningMvcBeta
{
&#xA0;&#xA0;&#xA0; namespace LearningMvcBeta.Controllers
&#xA0;&#xA0;&#xA0; {
&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0; public class PeopleController : Controller
&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0; {
&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0; public ActionResult Create()
&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0; {
&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0; return View();
&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0; }
&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0; public ActionResult Save(Person person)
&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0; {
&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0; return View(person);
&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0; }
&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0; }
&#xA0;&#xA0;&#xA0; }
&#xA0;&#xA0;&#xA0; public class Person
&#xA0;&#xA0;&#xA0; {
&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0; public string FirstName { get; set; }
&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0; public string LastName { get; set; }
&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0; public Address Address { get; set; }
&#xA0;&#xA0;&#xA0; }
&#xA0;&#xA0;&#xA0; public class Address
&#xA0;&#xA0;&#xA0; {
&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0; public string Street { get; set; }
&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0; public string City { get; set; }
&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0; public string State { get; set; }
&#xA0;&#xA0;&#xA0; }
}
</pre>
<p>&#xA0;</p>
<p>Then (this is the part I think most examples won&apos;t be as clear on as I&apos;m trying to be herein) I&apos;ll create my Create view HTML code. This will present the user with a form. Take note, the Person class parameter from the Save method above is named <strong>person, </strong> so
  the names of the HTML text elements have to be in the <strong>person.<em>PropertyName </em> </strong> format. If I&apos;d named the Person parameter <strong>p</strong>, I&apos;d want to name the HTML text elements using the <strong>p.<em>PropertyName </em> </strong> format.&#xA0;</p>
<p>&#xA0;</p>
<pre>
&#xA0;&#xA0;&#xA0; 
&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0; <b>Your Name</b>
&#xA0;&#xA0;&#xA0; 
&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0; 
&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0; First Name 
&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0; 
&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0; 
&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0; 
&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0; 
&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0; Last Name 
&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0; 
&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0; 
&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0; 
&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0; <b>Your Address</b>
&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0; 
&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0; 
&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0; Street 
&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0; 
&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0; 
&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0; 
&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0; 
&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0; City 
&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0; 
&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0; 
&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0; 
&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0; 
&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0; State 
&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0; 
&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0; 
&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0; 
&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0; 
&#xA0;&#xA0;&#xA0; 
&#xA0;&#xA0;&#xA0; 
</pre>
<p>&#xA0;</p>
<p>The image below is a screenshot I took of my debugger while testing this code with sample data. This is really powerful stuff - the child Address object defined by the instance of this Person class is sent in as well as was the First and Last name properties,
  because the Model Binding support trickles down the object graph!</p>
<p>
  <img src="/image.axd?picture=2008%2f10%2fmodel_binding_debug_scrn.png">
</p>
<p>Using the nomenclature MVC expects in your HTML form elements results in Model Binding automatically figuring out how to map the HTML form values to the parameter&apos;s properties just before the resulting object is fed into the controller method.&#xA0;</p>
<p>This is one of my favorite new features of ASP.NET MVC, for sure. Thanks a lot, ASP.NET MVC overlords!</p>
<p><strong>Update: </strong> Sorry if you&apos;ve tried to find this post and received an <em>Index out of range </em> error. The syntax highlighter I&apos;ve been using seems to have pooped the bed. So, I had to make a few changes to the supporting code and I think
  all&apos;s well now.&#xA0;</p>
