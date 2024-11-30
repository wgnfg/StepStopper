# StepStopper

为部分任务流程提供GUI无关的 类似IDE中的 中断/断点/逐过程/继续 功能

一般在部分图形界面实现某些功能时都会有类似的控制，于是将其简单封装

原理为异步和```TaskCompletionSource```的简单应用

## 常见场景
* 自定义脚本的中断调试
* 棋谱等自动播放控制

## Usage
(引入方面目前没发nuget。具体使用方法如下，可对照WPFDemo食用)
1. 对于流程的暂停控制将使用```StepContext```对象来完成，它提供了 ```中断``` ```BreakAll()```,```逐语句``` ```StepOver()```,```继续``` ```Continue()```方法。同一个```StepContext```的将共享```断点```和```单步```导致的暂停，如果可能要开启多个流程且互相独立控制可以使用多个```StepContext```
2. 对于比较简单的情形，如一个普通序列```IEnumerable<T>```的执行，可以对目标进行一次```.AsSingleStepsObject()```，随后此集合即可与UI绑定。```.AsSingleStepsObject()```可以指定使用的```StepContext```，并通过```StepContext```对象来控制其暂停。不指定时将直接使用 ```StepContext.Default```;
3. 如果执行过程中本身可能存在循环等情况，可能需要单独遍历语法树来给这些节点一一使用```AsSingleStepObject```转化为包装类```SingleStep```随后再使用```StepContext```对象来控制其暂停;
4. 流程中即将使用对应数据时,需要使用```await item.StepValue() ```的方式取出数据对象，即可在取得前，由单步/断点导致其等等待。
5. 断点由每个对象自身的```NeedBreakpoint```属性决定，其可以与画面进行绑定方便修改

## TODO
可能追加部分其他条件断点，多条线的同步控制等功能