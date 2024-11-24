# StepStopper

为部分任务流程提供GUI无关的 类似IDE中的 中断/断点/逐过程/逐语句 功能

一般在部分图形界面实现某些功能时都会有类似的控制，于是将其简单封装

原理为异步和```TaskCompletionSource```的简单应用

## 常见场景
* 自定义脚本的中断调试
* 棋谱等自动播放控制

## Usage
(目前没发nuget，可对照WPFDemo)
1. 首先取得一个```StepContext```，一般可以直接使用 ```StepContext.Default```;
2. 对目标```IEnumerable<T>```进行一次```.AsSingleStepsObject()```,将原始数据类```T```转化为包装类```SingleStep```;
3. 即将使用对应数据时,使用```await item.StepValue() ```的方式取出数据对象，即可在取得前由单步/断点导致其等等待
4. ```StepContext```提供了```Pause()```,```NextSingle()```,```NextBreakingPoint()```方法，用于提供中断，逐语句，逐过程；
5. 断点由每个对象自身的```NeedBreakingPoint```属性决定，其可以与画面进行绑定方便修改

## TODO
可能追加部分其他条件断点，多条线的同步控制等功能