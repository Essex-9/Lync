1. ÿһ�������˶���Ҫ�˺Ű�(��Ҫ)

2. ��ô�����еĻ���

3. ��ô����uri


sip:tonyxia@o365ms.com;gruu;opaque=app:conf:focus:id:8BBGHSMP

https://meet.lync.com/shlinker-o365ms/tonyxia/8BBGHSMP


https://social.msdn.microsoft.com/Forums/lync/en-US/8252794d-ea59-46ae-a051-b1685269ccaa/joining-the-conference-problem-in-lync-sdk?forum=communicatorsdk

I'm afraid, you can't use Lync API to Join conference without successfully signing into Lync,
 and there is no way to sign-in to Lync as anonymous user. Th
e functionality that Lync Attendee is using to Join anonymously is not exposed through the Lync API.


To join a conf anonymously you need to either dial into the server directly or try to join using Lync API while signing into a different server 
that has no federation relationship with the actual conference server.

4. UI Suppression Mode  not  supprot  contentsharing

5. Video�Ƿ�֧�ֶ��participate? Ŀǰ���ǲ�֧�ֵġ�

6. Խ��Խ����





@��Ϊ������������������˵�������о��ɹ��������������һ�£�
1. ��֪�������ڼ���ʱ���м�����ĸ�����Ҫ���ɵģ�
1����������
    
2����Ƶ���飨��PC���ǿ��Կ�����·��Ƶ�ģ�
    ֻ����ʾ������Ƶ

3��PC�����������˵�

4��PC��PC�˹��������PPT�ĵ�

    ֻ�ܹ������棬����PPT

5��PC��Mobile���黥ͨ��Mobile��Guestģʽ��PC���������ǲ���ҲҪͬ����ģʽ��

    ��֧��Guestģʽ
6��Skype���ı���Ϣ����Ա���ǲ���Ҫ����ʱ��Ϣ������΢���飬��Ա������ͬ��¼��

2. ��UI��SDK����UI��SDK�ֱ�������ʲô���ӣ����Ѿ�˵���ͻ���UI��Ƕ���������û���⣩

    ��UI���޷��Զ�����棬�ܹ������ı���Ϣ��

3. ����Ҫ�̳еĹ��ܣ���Щ��SDK��װ�õģ���Щ��Ҫ�����Լ�д�����߼�


4. Mobile��Gavin�Ѿ��о���������Ժ�Gavin������

