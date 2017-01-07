---
title: ASP.Net, Ajax and Web Services Using Non-primitive Method Parameters
slug: aspnet-ajax-and-web-services-using-non-primitive-method-parameters
author: bradygaster
lastModified: 2014-04-23 04:45:19
pubDate: 2012-10-05 18:14:43
categories: .NET
---

<p>
  I&apos;ve seen quite a few examples on communicating with web services from JavaScript code placed in ASPX pages. This morning I came back to work and decided to rework an existing example of my company&apos;s web service API so that it uses this feature set. In
  the process it occurred to me that our API has certain methods that require the passing of non-primitive data types (for example, a search critera object a-la CSLA, which is a whole other conversation). I did a little Googling and came up with very
  little, placing me once again into &quot;try it and see&quot; mode. The good news, there&apos;s very little work to be done on your part, as most of this stuff gets automagically serialized by the framework. So do the snoopy dance while you&apos;re reading this tutorial,
  because this is one of those magical times when the framework works wonders for us.
</p>
<p>
  Take a look at this C# code, which embodies a relatively simplistic web service. In particular, take note of the custom BankAccount class, which we&apos;ll be using in our example to prove the point.
</p>
<p>

  
</p>
<div>
  <p>
    using  System;
  </p>
  <p>
    using  System.Web;
  </p>
  <p>
    using  System.Collections;
  </p>
  <p>
    using  System.Web.Services;
  </p>
  <p>
    using  System.Web.Services.Protocols;
  </p>
  <p>
    using  System.Web.Script.Services;
  </p>
  <p>
  
  </p>
  <p>
  
  </p>
  <p>
    [ScriptService ]
  </p>
  <p>
    public  class  BankService  : WebService 
  </p>
  <p>
    {
  </p>
  <p>
    [WebMethod, ScriptMethod ]
  </p>
  <p>
    public  BankAccount  GetAccountById(int  id)
  </p>
  <p>
    {
  </p>
  <p>
    BankAccount  b = new  BankAccount ();
  </p>
  <p>
    b.AccountId = id;
  </p>
  <p>
    b.Balance = 3443.23D;
  </p>
  <p>
    return  b;
  </p>
  <p>
    }
  </p>
  <p>
  
  </p>
  <p>
    [WebMethod, ScriptMethod ]
  </p>
  <p>
    public  bool  DeleteBankAccount(BankAccount  account)
  </p>
  <p>
    {
  </p>
  <p>
    if  (account.AccountId == 1) return  false ;
  </p>
  <p>
    return  true ;
  </p>
  <p>
    }
  </p>
  <p>
    }
  </p>
  <p>
  
  </p>
  <p>
    public  class  BankAccount 
  </p>
  <p>
    {
  </p>
  <p>
    public  double  Balance;
  </p>
  <p>
    public  int  AccountId;
  </p>
  <p>
    }
  </p>
</div>
<p>
</p>
<p>
  Not too real-world of an example service, of course, but it&apos;ll do for now.
</p>
<p>
  In addition, we&apos;ve got to examine our ASPX code. I&apos;ll take a top-down approach to this examination, covering each fragment individually. This first segment shows the beginning of the ASPX client page, where you can see that the ScriptManager is being
  used to inform our page that we have a ScriptService (our web service from earlier) with which the page will be communicating.
</p>
<p>

  
</p>
<div>
  <p>
     @  Page     Language =&quot;C#&quot;  AutoEventWireup =&quot;true&quot;  CodeFile =&quot;Default.aspx.cs&quot;     Inherits =&quot;_Default&quot;  %&gt; 
  </p>
  <p>
  
  </p>
  <p>
     DOCTYPE  html  PUBLIC  &quot;-//W3C//DTD XHTML 1.1//EN&quot;  &quot;http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd&quot;&gt; 
  </p>
  <p>
    &lt; html  xmlns =&quot;http://www.w3.org/1999/xhtml&quot;&gt; 
  </p>
  <p>
    &lt; head  runat =&quot;server&quot;&gt; 
  </p>
  <p>
    &lt; title &gt; Untitled Page title &gt; 
  </p>
  <p>
     head &gt; 
  </p>
  <p>
    &lt; body &gt; 
  </p>
  <p>
    &lt; form  id =&quot;form1&quot;  runat =&quot;server&quot;&gt; 
  </p>
  <p>
  
  </p>
  <p>
    &lt; asp : ScriptManager  ID =&quot;ScriptManager1&quot;     runat =&quot;server&quot;&gt; 
  </p>
  <p>
    &lt; Services &gt; 
  </p>
  <p>
    &lt; asp : ServiceReference  <strong>Path =&quot;~/BankService.asmx&quot; </strong>     /&gt; 
  </p>
  <p>
     Services &gt; 
  </p>
  <p>
     asp : ScriptManager &gt; 
  </p>
</div>
<p>
  Notice in particular the bolded section in the code above. That Path attribute points to the URL of the web service the page will use to &quot;do stuff&quot; on the server. Without it things just won&apos;t function (I mention this because I&apos;ve seen quite a few examples
  that leave it out entirely). Next&#xA0; we&apos;ll have a little more HTML code to place our elements nicely about the page and to give the user some clues as to how to use the page:
</p>
<p>

  
</p>
<div>
  <p>
    &#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0; &lt; input  type =&quot;button&quot;  onclick =&quot;getAccount();&quot;     value =&quot;Get Account&quot;  /&gt;&lt; br  /&gt; 
  </p>
  <p>
    &lt; input  type =&quot;button&quot;  onclick =&quot;deleteAccount();&quot;     value =&quot;Delete Account&quot;  /&gt;&lt; br  /&gt; 
  </p>
  <p>
  
  </p>
  <p>
    &lt; div  style =&quot;width: 200px; font-family: Arial;&quot;&gt; 
  </p>
  <p>
  
  </p>
  <p>
    &lt; div  id =&quot;accountLbl&quot;  style =&quot;float: left;&quot;&gt; Account
    Id: div &gt; 
  </p>
  <p>
    &lt; div  id =&quot;accountId&quot;  style =&quot;color:red; float:right;&quot;&gt; 
    div &gt; 
  </p>
  <p>
    &lt; br  /&gt; 
  </p>
  <p>
  
  </p>
  <p>
    &lt; div  id =&quot;balanceLbl&quot;  style =&quot;float: left;&quot;&gt; Balance:
     div &gt; 
  </p>
  <p>
    &lt; div  id =&quot;balance&quot;  style =&quot;color:red; float:right;&quot;&gt; 
    div &gt; 
  </p>
  <p>
    &lt; br  /&gt; 
  </p>
  <p>
  
  </p>
  <p>
    &lt; div  id =&quot;resultLbl&quot;  style =&quot;float: left;&quot;&gt; Result:
     div &gt; 
  </p>
  <p>
    &lt; div  id =&quot;delResult&quot;  style =&quot;color:red; float:right;&quot;&gt; 
    div &gt; 
  </p>
  <p>
    &lt; br  /&gt; 
  </p>
  <p>
  
  </p>
  <p>
     div &gt; 
  </p>
</div>
<p>
  Not too much going on until we see the final piece, the JavaScript that ties it all together. Note in particular again the bolded code below. It creates a variable of type BankAccount so that our delete functionality will work properly.

  
</p>
<div>
  <p>
    &#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0; &lt; script  language =&quot;javascript&quot;  type =&quot;text/javascript&quot;&gt; 
  </p>
  <p>
    function  getAccount()
  </p>
  <p>
    {
  </p>
  <p>
    BankService.GetAccountById(2,getAccountCallback);
  </p>
  <p>
    }
  </p>
  <p>
  
  </p>
  <p>
    function  getAccountCallback(result)
  </p>
  <p>
    {
  </p>
  <p>
    $get(&quot;accountId&quot; ).innerHTML = result.AccountId;
  </p>
  <p>
    $get(&quot;balance&quot; ).innerHTML = result.Balance;
  </p>
  <p>
    }
  </p>
  <p>
  
  </p>
  <p>
    function  deleteAccount()
  </p>
  <p>
    {
  </p>
  <p>
    <strong>var  account = new  BankAccount();</strong> 
  </p>
  <p>
    account.AccountId = 1;
  </p>
  <p>
    BankService.DeleteBankAccount(account,deleteAccountCallback);
  </p>
  <p>
    }
  </p>
  <p>
  
  </p>
  <p>
    function  deleteAccountCallback(result)
  </p>
  <p>
    {
  </p>
  <p>
    delResult.innerHTML = result;
  </p>
  <p>
    }
  </p>
  <p>
     script &gt; 
  </p>
  <p>
  
  </p>
  <p>
     form &gt; 
  </p>
  <p>
     body &gt; 
  </p>
  <p>
     html &gt; 
  </p>
</div>
<p>
  As you can see, the ASP.Net Ajax extensions automatically serialize everything in your web service so that you have access to everything as simplistically as you would in C#. Just create the object as you normally would, set the instance&apos;s properties
  by name, and send the message!
</p>
<p>
  Happy coding!
</p>
