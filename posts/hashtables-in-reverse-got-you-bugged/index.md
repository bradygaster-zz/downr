---
title: Hashtables in reverse got you bugged?
slug: hashtables-in-reverse-got-you-bugged
author: bradygaster
lastModified: 2009-12-21 07:24:34
pubDate: 2012-10-05 18:14:54
categories: .NET
---

well, it was driving me nuts too. apparently using a foreach loop in a custom Hashtable has a strange &quot;reverse-order&quot; implementation. so i looked at the class library - finally, a reason to use the HybridDictionary object. More lightweight than a Hashtable,
and it iterates as expected (from-first-to-last in sequential order) - something truly important when developing Provider implementations.
