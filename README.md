# Farm Life Demo｜2D 农场生活模拟原型

基于团结引擎与 C# 开发的个人学习项目，目标是逐步制作 Stardew Valley 风格的 2D 农场生活模拟游戏。

目前处于基础系统与交互原型阶段，已连接场景启动、玩家控制、交互文本和 UI 显示流程。美术使用占位内容，完整的采集、背包物品和种植玩法正在规划与实现。

## 开发环境

- 团结引擎 1.8.5，工程记录的 Unity API 版本为 2022.3.62t7。
- C#、UGUI、TextMeshPro、Git。

## 当前实现

| 模块 | 实现内容 |
| --- | --- |
| 启动与管理器 | Bootstrap 保持唯一入口，运行时创建管理器，传入依赖并跨场景保留。 |
| 资源与场景 | 从 Resources 加载并实例化预制体；使用协程异步加载场景，检查场景可用性及重复加载请求。 |
| 玩家控制 | Rigidbody2D 移动、奔跑、方向归一化、相机跟随，以及附近最近可交互目标的检测。 |
| 文本配置 | 从 CSV 加载文本，按整数 ID 查询；支持单行记录中的引号内逗号和转义双引号。 |
| 事件通信 | 提供无参、int、string 三类事件，用于连接输入、交互、场景请求和 UI 响应。 |
| UI 管理 | 预制体加载、组件缓存、界面互斥切换与玩家操作权限管理；交互提示约 3 个现实秒后隐藏，重复交互重新计时。 |
| 日期与时间 | 从第 1 天 06:00 开始，每 0.7 个现实秒推进一个游戏分钟；午夜自动换日。常驻时钟分别显示“第1天”和十分钟粒度时间。日期使用文本表模板，UI 打开暂停、关闭恢复，切换期间持续暂停。 |

背包目前提供界面展示与开关控制，尚未实现物品存取。

## 如何运行

1. 使用团结引擎 1.8.5 打开工程，等待资源导入和脚本编译完成。
2. 打开 `Assets/Scenes/SampleScene.scene`，也可通过菜单“工具 → 打开启动场景”进入。
3. 点击运行，Bootstrap 会初始化管理器并加载 FarmScene。
4. 当前启动流程会显示日期与时钟并自动打开背包；背包打开期间时间暂停，按 Tab 关闭后即可操作玩家并继续计时。

SampleScene 是完整启动入口；FarmScene 依赖启动入口创建的管理器。

| 输入 | 功能 |
| --- | --- |
| WASD / 方向键 | 移动 |
| 左 Shift | 奔跑 |
| E | 与附近最近的可交互目标交互 |
| Tab | 打开、关闭背包，或从其他界面切换到背包 |
| Esc | 打开、关闭设置，或从其他界面切换到设置 |

## 关键代码入口

脚本位于 `Assets/ScriptsCshrp`，可以按下面的顺序阅读：

| 入口 | 关注内容 |
| --- | --- |
| [Bootstrap.cs](Assets/ScriptsCshrp/Bootstrap/Bootstrap.cs) | 对象创建、初始化顺序与依赖传递。 |
| [PlayerController.cs](Assets/ScriptsCshrp/Gameplay/PlayerController.cs) | Update 读取输入、FixedUpdate 移动刚体，以及最近目标选择。 |
| [InteractableObject.cs](Assets/ScriptsCshrp/Gameplay/InteractableObject.cs) | 根据文本 ID 查询内容并发布提示事件。 |
| [TextManager.cs](Assets/ScriptsCshrp/Managers/TextManager.cs) | CSV 解析、字典缓存与文本查询。 |
| [UIManager.cs](Assets/ScriptsCshrp/Managers/UIManager.cs) | 界面创建与复用、当前界面切换、提示计时与关闭目标校验。 |
| [TimeManager.cs](Assets/ScriptsCshrp/Managers/TimeManager.cs) | 时钟权限、现实秒累计、游戏分钟推进与日序号/时分查询。 |
| [TimeUI.cs](Assets/ScriptsCshrp/UI/TimeUI.cs) | 日期模板查询、占位符替换，以及独立日期和时间文字刷新。 |

一条完整的交互流程：

```text
玩家按 E
  → PlayerController 查找最近的交互目标
  → InteractableObject 根据文本 ID 向 TextManager 查询文字
  → EventManager 发布显示提示事件
  → UIManager 创建或复用提示界面
  → InteractionUI 更新文字
  → 等待约 3 个现实秒后统一关闭
```

## 开发中与近期目标

- 按现有学习进度继续确定季节、年份及睡眠规则，并分步实现。
- 后续制作采集物品、背包数量变化与目标反馈的可玩演示闭环。
- 整理演示视频、截图与可运行版本。

后续继续实现物品与背包数据、存档、种植等内容，再逐步扩展地图、NPC 与任务系统。
