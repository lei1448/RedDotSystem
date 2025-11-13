# Unity 红点提示系统 

一个用于 Unity 的可配置的红点通知系统。

## 主要特性

* **配置驱动:** 使用 `ScriptableObject` 平铺列表 (`Key`, `ParentKey`) 来定义整个红点层级树。
* **状态冒泡:** 子节点的状态（红点、计数）会自动刷新并冒泡至所有父节点。
* **数据视图分离:**
    * `RedDotNode` (数据): 纯 C# 类，处理所有状态计算。
    * `RedDotView` (视图): `MonoBehaviour` 预制件，监听全局事件以更新显示。
* **计数:**
    * `DotAndCount` (红点+数字): 默认类型，显示红点并累加数字。
    * `DotOnly` (仅红点): 特殊类型，仅显示红点，**其计数值不会被父节点累加**。
* **父级显示:**
    * 当父节点的红点状态*仅*来源于 `DotOnly` 子节点时，父节点自身也将只显示红点（计数为0）。

## 架构

1.  **配置 (`RedDotNodeConfig.asset`)**: `ScriptableObject` 资产，用于在 Inspector 中定义树结构和每个节点的 `DisplayType` (显示类型)。
2.  **管理器 (`RedDotManager.cs`)**: 单例，在启动时读取配置，在内存中构建 `RedDotNode` 树。
3.  **数据 (`RedDotNode.cs`)**: 运行时的数据节点。
    * `SetSelfCount(int count)`: 业务逻辑的触发入口。
    * `RefreshState()`: 核心的状态计算与冒泡方法。
    * `BranchCount`: 计算的分支总数（会忽略 `DotOnly` 子节点的计数）。
4.  **视图 (`RedDotView.cs`)**: UI 预制件的挂载脚本。
    * `Init()`: 初始化时，订阅全局事件。
    * `UpdateView()`: 根据节点的 `DisplayType` 和 `BranchCount` 更新红点和数字。
5.  **事件 (`RedDotEvents.cs`)**: 静态事件总线，用于 `RedDotNode` 广播状态变更，以及 `RedDotView` 广播点击事件。


## 功能演示

| 背包系统 (Backpack System) | 任务 & 社交系统 (Task & Social System) |
| :---: | :---: |
| ![背包系统演示](https://github.com/user-attachments/assets/01702fd7-46c0-4db0-b509-d06bfb537f57) | ![任务与社交系统演示](https://github.com/user-attachments/assets/e2c0e4df-09a4-4ab2-bdea-5a75078b2cf9) |

| 邮箱系统 (Mail System) | |
| :---: | :---: |
| ![邮箱系统演示](https://github.com/user-attachments/assets/94142add-b923-463f-85ca-0936f410cc32) | |

---

## 如何使用

### 1. 配置

1.  在 Project 窗口创建 `RedDotNodeConfig` 资产。
2.  在 Inspector 中配置 `Nodes` 列表：
    * `Key: Root`, `ParentKey: (None)`
    * `Key: 邮箱`, `ParentKey: Root`, `DisplayType: DotAndCount`
    * `Key: 附件`, `ParentKey: 邮件`, `DisplayType: DotOnly`
    * `Key: 合成`, `ParentKey: 背包`, `DisplayType: DotOnly`
    * ... 等等

### 2. 场景设置

1.  在场景中放置一个空物体，挂载 `RedDotManager.cs`。
2.  将创建的配置文件拖拽到 `RedDotManager` 的 `M Root Config` 上。

### 3. 绑定 UI

1.  制作一个包含 `Image` (红点) 和 `TextMeshPro - Text` (数字) 的 `Button` 预制件。
2.  将 `RedDotView.cs` 挂载到预制件根物体，并链接 `Image` 和 `Text`。
3.  在游戏运行时 (例如，通过 `MainMenuRedDotGenerator.cs` 脚本)：
    * `Instantiate` 该预制件。
    * 调用 `redDotView.Init(key, isRed, branchCount)` 来完成数据与视图的绑定。

### 4. 触发红点

在任何游戏系统 (如 `TaskManager.cs`, `MailManager.cs`) 中，通过管理器获取节点并设置其计数值：

```csharp
// 示例：收到一个可接任务
RedDotNode node = RedDotManager.Instance.GetNode("可接受");
node.SetSelfCount(node.GetCount() + 1);

// 示例：清除了一个任务
RedDotNode node = RedDotManager.Instance.GetNode("可接受");
node.SetSelfCount(node.GetCount() - 1);
