---
title: The Robot Factory Kata
slug: the-robot-factory-kata
author: bradygaster
lastModified: 2012-10-05 18:15:12
pubDate: 2012-10-05 18:15:08
categories: .NET
---

<p>On the drive home from my last Behavior Driven Development talk, I began thinking about the idea of
  <a href="http://codekata.pragprog.com/2007/01/code_kata_backg.html" title="Great introduction to coding katas">Code Katas</a>  and how one might be appropriate in my future disucssions of Behavior Driven Development. Given that BDD tries to solve things in as simple and direct a path as possible, and given that BDD takes some of the lessons learned via TDD and
  applies them in slightly more business-centric language, a Kata would demonstrate well the effectiveness of BDD when applied to a problem domain.&#xA0;</p>
<p>So, I took the example problem domain of a robotic assembly line that I&apos;ve been using since I was training full-time under the guidance of
  <a href="http://weblogs.asp.net/palermo4/" title="my mentoring mentor">J. Michael Palermo IV</a> &#xA0;and implemented it using
  <a href="http://specflow.org/" title="the SpecFlow project page">SpecFlow</a>  and
  <a href="http://code.google.com/p/moq/" title="Moq is my favorite mocking library for .NET">Moq</a> . So far, the Robot Factory Kata video series has two videos.&#xA0;</p>
<p>The first of these demonstrates the project setup and configuration, and starts solving the problem using SpecFlow specifications and NUnit tests.&#xA0;</p>
<p>
  <iframe width="560" height="315" src="http://www.youtube.com/embed/PAeIuqt2Nf0" frameborder="0" allowfullscreen></iframe>
</p>
<p>The second part helps demonstrate how mocking method verification using callback expectations to demonstrate testing interaction between two objects. Sure, this is getting into integration testing, but the idea is to demonstrate BDD using a legitimate
  problem domain that&apos;s slightly more interesting than the construction of a calculator. Not that there&apos;s anything wrong with that, but you know, it helps to have variety.&#xA0;</p>
<p>
  <iframe width="560" height="315" src="http://www.youtube.com/embed/CZl9Ic3R4_E" frameborder="0" allowfullscreen></iframe>
</p>
<p>Hope you enjoy these video demonstrations and that they motivate you to start BDD&apos;ing today.&#xA0;</p>
