1. 每一个参与人都需要账号吧(需要)

2. 怎么打开现有的会议

3. 怎么生成uri


sip:tonyxia@o365ms.com;gruu;opaque=app:conf:focus:id:8BBGHSMP

https://meet.lync.com/shlinker-o365ms/tonyxia/8BBGHSMP


https://social.msdn.microsoft.com/Forums/lync/en-US/8252794d-ea59-46ae-a051-b1685269ccaa/joining-the-conference-problem-in-lync-sdk?forum=communicatorsdk

I'm afraid, you can't use Lync API to Join conference without successfully signing into Lync,
 and there is no way to sign-in to Lync as anonymous user. Th
e functionality that Lync Attendee is using to Join anonymously is not exposed through the Lync API.


To join a conf anonymously you need to either dial into the server directly or try to join using Lync API while signing into a different server 
that has no federation relationship with the actual conference server.

4. UI Suppression Mode  not  supprot  contentsharing

5. Video是否支持多个participate? 目前看是不支持的。

6. 需要安装Skype for bussiness 2016，才能正常使用分享桌面的功能。


7. 显示RenderWideoWindow的怪问题！难道是手机能显示PC Video,PC 才能正常显示手机的 Video。