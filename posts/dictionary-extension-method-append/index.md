---
title: Dictionary Extension Method - Append
slug: dictionary-extension-method-append
author: bradygaster
lastModified: 2011-05-21 02:12:39
pubDate: 2012-10-05 18:15:05
categories: .NET
---

<p>I&apos;ve been creating a few extension methods here and there recently and figured I&apos;d share one of the ones I&apos;m using in a lot of places. This one is an extension method named Append that you can use with a generic IDictionary implementor to do quick-and-dirty
  creations of the object. I&apos;ve found this to be really useful when I&apos;m writing tests that use the generic IDictionary class in my tests. The first block of code below will point out the extension method.</p>
<p>&#xA0;</p>
<pre>public static class DictionaryExtensions
{
&#xA0; public static IDictionary Append(this IDictionary dictionary,
&#xA0;&#xA0;&#xA0; TKey key, TValue value)
&#xA0; {
&#xA0;&#xA0;&#xA0; dictionary.Add(key, value);
&#xA0;&#xA0;&#xA0; return dictionary;
&#xA0; }
}
</pre>
<p>&#xA0;</p>
<p>This second block demonstrates one potential use for it. In this case I was creating a generic Dictionary to store name/value pairs of data for an HTTP post.&#xA0;</p>
<p>&#xA0;</p>
<pre>IDictionary prms = new Dictionary().Append(&quot;status&quot;, status);
</pre>
<p>&#xA0;</p>
<p>Happy coding!</p>
