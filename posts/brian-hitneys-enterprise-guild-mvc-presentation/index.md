---
title: Brian Hitney's Enterprise Guild MVC Presentation
slug: brian-hitneys-enterprise-guild-mvc-presentation
author: bradygaster
lastModified: 2011-05-21 02:11:51
pubDate: 2012-10-05 18:15:04
categories: Community
---

<p>Tonight I attended the local
  <a href="http://www.developersguild.org/Default.aspx?tabid=32&amp;Event=123" title="Link to the ASP.NET MVC Presentation information page. ">Enterprise Guild&apos;s ASP.NET MVC presentation</a>  by
  <a href="http://www.structuretoobig.com/home/" title="Brian Hitney&apos;s Site">Brian Hitney</a> . I don&apos;t know if I&apos;ve ever given a review of a colleague&apos;s presentation (so this might suck) but I feel one&apos;s in order.&#xA0; Brian&apos;s doing his best in this area for Microsoft to spread this and other information and I want to take a moment
  to recognize, suggest some critique of, and applaud his efforts.</p>
<p>Brian&apos;s an excellent speaker and a major contributor to the .NET and Microsoft community in the southeast. He was patient, careful, and did his best to keep things on track without pandering to or disregarding the audience when they requested more information
  be given to specific areas of interest. He has a way with any collection of subject matter; he adds humor to the presentation, keeps the pace moving along, and overall seems to feel really cozy in front of a crowd. A truly good presenter knows the value
  in saying something akin to &quot;I don&apos;t know, dude, that&apos;s an interesting point you bring up,&quot; and Brian&apos;s not afraid to do just this thing. I think that may be his biggest strength as a developer evangelist - he <em>doesn&apos;t</em>  preach and he <em>isn&apos;t</em>   a know-it-all.</p>
<p>I got the impression that most of the people in the audience tonight benefitted from the presentation, which covered some fundamental aspects of the MVC &quot;idea,&quot; rather than focus on some of the lower-level points concerning the actual execution of a coding
  experience using the ASP.NET MVC framework. So when Brian made it clear that the presentation would be covering the &quot;why&apos;s&quot; and not the &quot;how&apos;s,&quot; I have to admit my colleague and I had a mutual eye-roll. MVC isn&apos;t an easy concept to grasp, I know. It
  took me about 3 years to really understand how it&apos;s supposed to work. Even now I still struggle with some of the innerworkings of the approach. So that portion of my review is, I admit, somewhat selfish in nature. I would&apos;ve like to have seen more low-level
  discussions of the technicalities.&#xA0;</p>
<p>During the initial chat, Brian brought up
  <a href="http://en.wikipedia.org/wiki/Inversion_of_control" title="Wikipedia article on IoC">Inversion of Control</a>  and
  <a href="http://en.wikipedia.org/wiki/Dependency_injection" title="Wikipedia article on Dependency Injection">Dependency Injection</a> . I think a better approach for Brian when dealing with these more complex and abstract concepts would&apos;ve been to defer to the audience&apos;s willfullness to learn more on them; maybe say something about them being important ideas
  that are &quot;outside the scope&quot; of the presentation (because, well, they <em>are outside the scope of a presentation like this</em> ). Instead, Brian gave a few examples of IoC and/or DI. One in particular was to use the event model - a button-click event
  handler, to be precise - as an example of IoC. I don&apos;t claim to be a guru at IoC, but I am <em>positive </em> that I can&apos;t think of how IoC could be represented via such an example. I really can&apos;t, and I tried. My co-worker agreed with me on this matter,
  too, so I know I&apos;m not nuts (or that I have a partner in my misunderstanding of <em>something</em> ). That&apos;s my one major &quot;you didn&apos;t capitalize the &apos;N&apos; and the &apos;E&apos; and the &apos;T&apos; when you put &apos;.NET&apos; on your resume&apos;&quot;-type complaint with Brian&apos;s presentation.
  My guess is that Brian understands all of these ideas quite well and could discuss them rather fluently <em>under the appropriate context. </em> In that, when he put them on the slide he had excellent intentions that completely escaped him when he was
  in front of a crowd of geeks. It happens.&#xA0;</p>
<p>There were a few technical items of note during the presentation. The first was when Brian created an ASP.NET MVC project in VS2K8. When the &quot;do you want to create a test project&quot; screen was presented and the MS Test the only option in the &quot;what type
  of test project do you want to create&quot; drop-down, Brian said &quot;If you install NUnit you&apos;ll see it in here too.&quot; I have both VS2K8 installed and NUnit, and I don&apos;t see an NUnit option. So I conclude that there&apos;s some additional <em>schnizzle </em> that
  I need to download and install to tie NUnit to VS2K8, that I haven&apos;t yet read <em>that particular blog post </em> (note no link, feel free to hook me up in the comments), or that his statement was incorrect. Brian? Bueller? Anyone? What is it? How <em>do </em> I
  get NUnit to appear in that list?</p>
<p>Later occurred what I think was the only major and potentially heinous blunder of the presentation. A question was raised about the variance between ASP.NET MVC&apos;s behavior on IIS6 and IIS7. It was late in the presentation when an audience member asked
  about the differences between the two server versions. The meeting was in a new room, started late, and had a huge crowd, so I&apos;m sure Brian was exhausted and ready to call it quits. So, when this question was asked, Brian&apos;s answer was simply to state
  that there was no difference in the way the routes had to be set up and that it&apos;d all work just fine under either version. I&apos;d been bitten by this variance myself (and I have a complete lack of being able to keep my trap shut) so I raised my hand and
  commented that this wasn&apos;t the case. For the record, I said this not to <em>call Brian out and make him look like a dimwit </em> but rather <em>to make sure the correct information was given to the audience. </em> I mean, that&apos;s why we&apos;re all there, and
  it is called a <em>community group </em> so I figure that&apos;s the best thing one could do in the situation. I specifically mentioned the approach taken in the
  <a href="http://www.codeplex.com/Kigg" title="Kigg">Kigg</a>  framework, and how it seemed to accomplish this for both versions pretty elegantly. The crowd seemed generally cool with this and we moved on, Brian relatively unscathed. No harm, no foul, but I urge (read: <em>plead with</em> ) Brian and the
  rest of the evangelists - don&apos;t forget that many of us in the community hinge on most of the things you say. If you tell me it works, I think it works, and when it doesn&apos;t work I think it&apos;s something I did. When I find out it&apos;s something you guys did
  and that I&apos;ve wasted many hours trying to make something work that just isn&apos;t supposed to work, I want to forget I ever loved coding I get so irritated - nothing plagues a coder like time lost on poor documentation, so just keep doing your best to keep
  the facts straight. The whole <em>with great power comes great responsibility </em> speech is implied here.&#xA0;</p>
<p>My favorite moment during the presentation was when Brian talked about the collection of form data in the &quot;new MVC way.&quot; He specifically mentioned getting back down to basics and dealing more directly with the Request object. I&apos;m a huge fan of this, and
  of removing a lot of the gunk between me and my HTTP protocol, so I was all about this commentary. Additionally helpful was his code example demonstrating form-collection using the BindingHelperExtensions class. This has been one of the things I&apos;ve
  not yet learned how to do with MVC - dealing with forms. I&apos;ve been more focused on link-based approaches using MVC and on JSON transmission, so the traditional forms approach had been something that&apos;d eluded me. Brian&apos;s example of this was great.&#xA0;</p>
<p>Brian did mention
  <a href="http://haacked.com/archive/2008/03/13/url-routing-debugger.aspx" title="Phil Haack&apos;s Routing Debugger">Phil Haack&apos;s ASP.NET Routing Debugger</a>  during his presentation, which I&apos;m <em>so downloading tomorrow </em> it&apos;s not even funny. Thanks for that, Brian and
  <a href="http://haacked.com/" title="Phil Haack">Phil</a> .&#xA0;</p>
<p>Overall, a good presentation. I would like to see a round two of this presentation (or to provide it, as sometimes the requirement of teaching something enables/forces you to learn it pretty well) or to be provided something comperable to Brian&apos;s presentation
  on a more technical, &quot;how-to&quot; level. I look forward to his evolution as a DE, his upcoming travelling show with some of the other guys in his squad, and to learning more about ASP.NET MVC.&#xA0;</p>
