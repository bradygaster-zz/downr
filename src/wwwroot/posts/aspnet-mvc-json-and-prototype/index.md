---
title: ASP.NET MVC, JSON, and Prototype
slug: aspnet-mvc-json-and-prototype
author: bradygaster
lastModified: 2011-05-21 02:20:50
pubDate: 2012-10-05 18:15:06
categories: .NET
---

<p>I&apos;ve tinkered with the method of posting JSON between the browser and the server via a generic handler. When the new MVC Preview came out and had native support for JSON I knew I needed to do some further tinkering. This post will describe in brief how
  to perform such a thing. I&apos;ll demonstrate how to perform a login process using JSON communication between an MVC Controller&apos;s action method which returns a JsonResult instance. Sounds tricky, but that&apos;s how Prototype helps us. It makes the communication
  process pretty easy. I&apos;ll try to be pretty short and sweet here and will keep the code discussion to a bare minimum.</p>
<h3>Environment Setup&#xA0;</h3>
<p>First and foremost, you&apos;ll need to learn the basics of the ASP.NET MVC approach and find the two downloads over at
  <a>ScottGu&apos;s blog post on the topic</a> . That&apos;s about the best place to start.&#xA0; Once you&apos;ve got everything installed you should create a new MVC project. Add a folder to the <em>Views </em> folder called <em>Login</em> . This is a relatively simple topic
  that all must implement at some point or another. I&apos;ll use the Prototype JavaScript framework for this. So make sure you
  <a>download Prototype</a>  and put it into your web project. Finally, add a reference to the script in your <em>Site.Master</em>  page, which should be in the <em>Views/Shared</em>  folder.&#xA0;</p>
<h3>Creating the Login Index View and Controller</h3>
<p>Within the new Login folder, create an <em>MVC View Content Page </em> named Index.aspx.&#xA0; This page will contain some HTML code, as you&apos;ll see below. This code contains some form elements and some JavaScript code. The JavaScript code is what will perform
  the duty of packing up the data collected from the form in the structure of a JavaScript object called Login. Once the object is created, it will be shipped over HTTP to the server.</p>
<p>&#xA0;</p>
<pre>

&#xA0;&#xA0;&#xA0; <div>
&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0; <div>
&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0; username
&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0; 
&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0; </div>
&#xA0;&#xA0;&#xA0; </div>
&#xA0;&#xA0;&#xA0; <div>
&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0; <div>
&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0; password
&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0; 
&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0; </div>
&#xA0;&#xA0;&#xA0; </div>
&#xA0;&#xA0;&#xA0; <div>
&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0; <div>
&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0; 
&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0; </div>
&#xA0;&#xA0;&#xA0; </div>
&#xA0;&#xA0;&#xA0; 

</pre>
<p>&#xA0;</p>
<h3>Controlling Things</h3>
<p>Take note of the URL in the call to the Ajax.Request constructor. The call is going to be made to /Login/Authenticate. Understanding that step will give you a better comprehension of how the URL&apos;s are converted into useful routes on the server to view
  pages. The first step is to create a Controller class. Since I&apos;ve created a Views folder named Login, I must create the class useful for controlling all login-related procedures. This class, LoginController, will contain action methods that perform
  various actions the user will need to execute during the Login routines. The first of these - the Index view - needs to be controlled first.&#xA0;</p>
<p>&#xA0;</p>
<pre>public class LoginController : Controller
{
&#xA0;&#xA0;&#xA0; public ActionResult Index()
&#xA0;&#xA0;&#xA0; {
&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; return View();
&#xA0;&#xA0;&#xA0; }
<p>&#xA0;</p>
<p>
}
</p>
</pre>
<p>&#xA0;</p>
<p>This one&apos;s pretty obvious - the call to View() will just render the Index view I already created, with the login form and JavaScript call. The next one isn&apos;t so obvious and requires a quick glance at some helper methods I&apos;ve written into a JsonHelper
  class. This class just makes some of the heavy lifting easier in a moment. The ToJson extension method wouldn&apos;t have been possible without a
  <a>post from ScottGu&apos;s blog</a> .</p>
<p>&#xA0;</p>
<pre>public static class JsonHelper
{
&#xA0;&#xA0;&#xA0; public static string ToJson(this object target)
&#xA0;&#xA0;&#xA0; {
&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; JavaScriptSerializer ser = new JavaScriptSerializer();
&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; return ser.Serialize(target);
&#xA0;&#xA0;&#xA0; }
&#xA0;&#xA0;&#xA0; public static T FromJson(string json)
&#xA0;&#xA0;&#xA0; {
&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; JavaScriptSerializer ser = new JavaScriptSerializer();
&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; return ser.Deserialize(json);
&#xA0;&#xA0;&#xA0; }
&#xA0;&#xA0;&#xA0; public static T FromJson(Stream stream)
&#xA0;&#xA0;&#xA0; {
&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; StreamReader rdr = new StreamReader(stream);
&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; return FromJson(rdr.ReadToEnd());
&#xA0;&#xA0;&#xA0; }
}
</pre>
<p>&#xA0;</p>
<p>The last step in our controller class is the actual <em>niftyness </em> of the whole post. Within this method the code takes a peek at the current <em>Request, </em> specifically the incoming JSON string contained within the request&apos;s InputStream property.
  This is where I&apos;ll use my JsonHelper class, which makes it a snap to parse an HTTP Request into a JSON object.&#xA0;</p>
<p>&#xA0;</p>
<pre>public JsonResult Authenticate()
{
&#xA0;&#xA0;&#xA0; Login login = JsonHelper.FromJson(this.Request.InputStream);
&#xA0;&#xA0;&#xA0; return new JsonResult
&#xA0;&#xA0;&#xA0; {
&#xA0;&#xA0;&#xA0; &#xA0;&#xA0;&#xA0; Data = (login.Username == &quot;username&quot; &amp;&amp; login.Password == &quot;password&quot;)
&#xA0;&#xA0;&#xA0; };
<p>&#xA0;</p>
<p>
}
</p>
</pre>
<p>&#xA0;</p>
<h3>The Final Touch</h3>
<p>Last, I&apos;ll need to give the user some way of finding the Login&apos;s Index view so I&apos;ll add a link to the view in the <em>Site.Master</em>  file&apos;s navigation section.</p>
<p>&#xA0;</p>
<pre>&#xA0;&#xA0;&#xA0; 

</pre>
<p>&#xA0;</p>
<p>Once you&apos;ve done all this you should be able to run the code with success. I&apos;ll get around to uploading this sample onto the server so you can see it working in the next few days, but I couldn&apos;t wait to share this technique. Using Prototype as a means
  to communicate with a ASP.NET MVC Controller class&apos; JsonResult methods makes for a powerful combination of tools to build truly rich, interactive applications that run in all modern browsers.</p>
<p>Happy Coding!&#xA0;</p>
<p>&#xA0;</p>
