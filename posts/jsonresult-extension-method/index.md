---
title: JsonResult Extension Method
slug: jsonresult-extension-method
author: bradygaster
lastModified: 2011-05-21 02:05:24
pubDate: 2012-10-05 18:15:03
categories: .NET
---

<p>Mildly silly and maybe making too many assumptions though it is, I worked up a little extension method to generically evaluate (and return) the Data property of a JsonResult class. Makes life a little easier when testing JsonResult action methods on controller
  instances.</p>
<p>
  <img alt="image" src="media/image.png">
</p>
<p>Helps during testing, for sure. See below for a demonstration of how I&#x2019;m using this in a pet project.</p>
<p>
  <img alt="image" src="media/image_3.png">
</p>
