# fastPomeloUnity
## 背景介绍
Pomelo是网易出品的一个基于node.js的游戏服务器框架（已经很久没维护了）
对于一些小型的游戏而言，socket方案采用node.js是一种成本较低的选择。

Pomelo官方虽有unity的实现库与demo，但是在dns转换和socket连接的时候比较耗时，
会阻塞Unity的主线程，造成卡顿。
※fastPomeloUnity\Assets\Scripts\Networks\pomelo\client\PomeloClient.cs.old 是官方版本的代码，使用它替换PomeloClient.cs后再运行本项目，会发现转动的地球在初始化Pomelo时会卡顿
本项目对这方面进行了改善。
同时，项目还对Pomelo的消息收发进行了一定程度的封装，将游戏逻辑的代码与Pomelo部分剥离开。

## pomelo的修改
因此fastPomeloUnity\Assets\Scripts\Networks\pomelo\client\PomeloClient.cs做了如下修改：
1. 注释了Dns.GetHostEntry相关操作（这部分操作耗时
2. 通过BeginInvoke操作来完成socket的创建与连接，多线程，速度更快

## 流程原理
游戏启动后，Main脚本创建出Game对象（该对象负责控制游戏的总体逻辑，包括网络，音频，UI等）
Game对象中会创建NetworkMgr对象，并在Update方法中执行networkMgr的相应更新方法，从消息队列中提取数据，然后根据配置好的路由捡出相应的处理器进行处理

那么，所谓的封装就是：
A：把pomelo送来的消息放入 消息队列中，
B：并且，在游戏启动初，就要为各个类型的消息 指派 对应的处理器

对于A部分，是在Pomelo.request和Pomelo.on 时做了手脚，让它的回调函数进入我们自己定义的逻辑
而B部分，则是通过BroadcastFact和CallbackFact 两个类来完成的。

##使用方法
1. 通过networkMgr的newSocket方法创建socket连接
2. 连接成功后就可以发送（request,notify）与接收数据（广播pomelo.on）了
3. 创建了Senders文件夹，下面的代码都是用来编写 发送逻辑的
4. 创建了Handlers文件夹，下面的代码都是用来编写 数据响应逻辑的
5. 在Routes类中罗列出各个route和响应的广播事件名称
6. 对于route的回调，在CallbackFact的registCallbacks方法中注册
7. 对于广播事件的回调，在BroadcastFact的registEvents中注册

## 联系方式
LSD751@qq.com