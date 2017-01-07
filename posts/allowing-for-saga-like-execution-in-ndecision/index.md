---
title: Allowing for Saga-like Execution in NDecision
slug: allowing-for-saga-like-execution-in-ndecision
author: bradygaster
lastModified: 2014-01-25 03:32:44
pubDate: 2012-10-05 18:15:02
categories: NDecision
---

<p>After the first post on NDecision I realized there&#x2019;s room for improvement &#x2013; allow for multiple directions when a test expression passes. This way one .Run() call on a target results in the ability to chain multiple processes together. Kind of like a Saga
  in ESB, but maybe not quite. Anyway, here&#x2019;s the test that demonstrates the ability.</p>
<p>
  <img src="media/image_3.png" alt="image">
</p>
