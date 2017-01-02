---
title: Introducing NDecision.Aspects
slug: ndecision-with-aop
author: bradygaster
lastModified: 2011-08-06 02:53:02
pubDate: 2012-10-05 18:15:08
categories: NDecision
---

<p>If you&apos;ve not gotten up to speed with the core functionality provided via NDecision it might be good to take a look at the release announcement,
  <a>which you can find here.</a> 
</p>
<p>If you&apos;re into Aspect-Oriented Programming or you like things to be a little more dynamic and automatic, a second option exists that you might find interesting. The NDecision.Aspects package makes use of the amazing AOP product
  <a>PostSharp</a>, by
  <a>SharpCrafters</a>, to enable logic execution automatically at run-time. Rather than write your logical specifications inline, you implement the IHasSpec
   interface. This interface&apos;s single method, GetSpec(), allows you the freedom of wiring up your logic in one place (or in many places, if you prefer to have your specifications split into individual classes based on your own need or preferences).&#xA0;</p>
Refactoring the Example for NDecision.Aspects
<p>I&apos;ll build on the
  <a>previous example</a>  and refactor the code to use the NDecision.Aspects package and to provide some insight into the difference. First of all, the logic from the original code has been moved into an implementation of the IHasSpec
   interface.&#xA0;</p>
<p>
  <img src="/posts/ndecision-with-aop/media/bunny_spec_implementation.png" alt="Implementing the rules for an object in an IHasSpec implementation">
</p>
<p>Once your IHasSpec
   implementation has been written, the majority of the work is complete. To use the specifications outlined in the IHasSpec
     implementation, the ApplySpecsBeforeExecution (or ApplySpecsAfterExecution) attribute class can be used to decorate the methods whenever the specifications need to be applied. The screen shot below demonstrates how, using the ApplySpecsBeforeExecution
      attribute, the rules will automatically be applied prior to the method&apos;s logic being executed.&#xA0;</p>
<p>
  <img alt="Method with a parameter decorated for automatic specification application" src="/posts/ndecision-with-aop/media/decorated_method_with_target.png">
</p>
<p>If a second parameter for which there existed IHasSpec
   implementations were placed on this method, specifications for that parameter would be picked up too. In this way, AOP takes over - for any parameter added to this method for which there exists an IHasSpec
     implementation, the specifications will be dynamically found and executed on the parameter instance at run-time.</p>
<p>Your rules, in a sense, are automatically applied whenever you want them to be applied.&#xA0;</p>
NDecision with AOP, Option Two - Self-Awareness
<p>Now, there may exist a circumstance where your object instances - in this case our favorite childhood cartoon character - should be allowed to do their own work based on the specifications and how they should be applied. In these cases it would be more
  optimal to have the capability of applying the ApplySpecsBeforeExecution or ApplySpecsAfterExecution attributes to the target class itself (or specifically, to methods of the target class). The code from above has been refactored with this idea in mind.
  Specifically, we&apos;ve given Bugs his own method, also decorated with the ApplySpecsBeforeExecution attribute.&#xA0;</p>
<p>
  <img alt="Self-driven functionality" src="/posts/ndecision-with-aop/media/decorated_self_driven_method.png">
</p>
<p>Likewise, the program code will need to be changed so that it allows Bugs his autonomy.</p>
<p>
  <img src="/posts/ndecision-with-aop/media/main_method_with_self_driven_changes.png" alt="Allowing the target class to drive itself">
</p>
<p>Both methods result with the following obvious output - a console window showing the directions Bugs takes on his journey, in the order they were taken.&#xA0;</p>
<p>
  <img src="/posts/ndecision-with-aop/media/ndecision_demo_output.png" alt="Output of the demonstration code">
</p>
<p>Just like the NDecision core project,
  <a>NDecision.Aspects is available for free via NuGet</a> . Don&apos;t worry about the dependencies or having to install both packages; simply installing NDecision.Aspects will bring down everything you need, including the PostSharp assembly, as SharpCrafters
  so generously offer it up as a NuGet package too.&#xA0;</p>
<p>
  <img alt="NDecision.Aspects NuGet install " src="/posts/ndecision-with-aop/media/ndecision-aspects-install.png">
</p>
<p>Happy coding! I hope you give NDecision a spin. If you do take the time to play with it, please let me know how it works out for you via the comments form. Or, you can find me on
  <a>Twitter at @bradygaster.</a> 
</p>
