# HebyET

基于 [ET框架](https://github.com/egametang/ET) 的 **2D 多人联机 ARPG 游戏**项目，采用 Entity-Component 架构，支持客户端热更新与服务端分布式部署。

## 项目概述

HebyET 是一款俯视角 2D 动作角色扮演游戏，核心特色包括：

- 🎮 **实时多人战斗** — 支持多玩家同屏战斗，AOI（Area of Interest）视野管理
- ⚔️ **技能与 Buff 系统** — 可配置的技能释放、Buff 叠加与状态管理
- 🤖 **AI 行为树** — 基于 Behaviac 的怪物 AI，支持状态机驱动
- 🔥 **热更新** — 基于 HybridCLR 的 C# 代码热更新，无需重启客户端
- 🌐 **分布式服务端** — 基于 ET 框架的多进程架构，支持横向扩展

## 技术栈

| 类别 | 技术 |
|------|------|
| 游戏引擎 | Unity 2022.3.51f1c1 |
| 渲染管线 | URP (Universal Render Pipeline) |
| 框架 | ET 8.x (Entity-Component) |
| 热更新 | HybridCLR |
| 网络协议 | TCP/KCP (ET 内置网络层) |
| 序列化 | MemoryPack |
| 资源管理 | YooAsset 2.1.1 |
| UI 框架 | FairyGUI 5.1.0 |
| AI 行为树 | Behaviac 3.6.39 |
| 动画 | Spine + 精灵帧动画 |
| 数据库 | MongoDB |
| 服务发现 | etcd |
| 缓存 | Redis |
| 配置表 | Excel → Proto 导表 |
| 语言 | C# (.NET Framework 4.7.1 / .NET 8) |

## 架构设计

项目遵循 ET 框架的经典分层架构，客户端与服务端共享核心逻辑代码：

```
HebyET/
├── Unity/                          # Unity 客户端工程
│   ├── Assets/
│   │   ├── Scripts/
│   │   │   ├── Model/              # 模型层（数据定义、实体、组件）
│   │   │   │   ├── Share/          # 客户端与服务端共享模型
│   │   │   │   ├── Client/         # 客户端专属模型
│   │   │   │   └── Server/         # 服务端专属模型
│   │   │   ├── ModelView/          # 客户端视图模型层
│   │   │   ├── Hotfix/             # 热更新逻辑层
│   │   │   │   ├── Share/          # 共享热更逻辑（战斗、Buff、AI）
│   │   │   │   └── Client/         # 客户端热更逻辑
│   │   │   ├── HotfixView/         # 热更新视图层（UI、场景、动画）
│   │   │   ├── Core/               # 核心框架层（网络、序列化、定时器）
│   │   │   ├── Loader/             # 启动加载层
│   │   │   ├── Editor/             # 编辑器工具
│   │   │   └── ThirdParty/         # 第三方库（Spine 等）
│   │   ├── Res/                    # 美术资源（角色精灵、场景、特效）
│   │   ├── Config/                 # 配置表（Excel 导出）
│   │   └── CombatEditor/           # 战斗编辑器资源
│   └── Packages/                   # Unity 包管理
├── DotNet/                         # 服务端 .NET 工程
│   ├── App/                        # 服务端启动入口
│   ├── Core/                       # 核心框架（网络、数据库、etcd）
│   ├── Hotfix/                     # 服务端热更逻辑（战斗、大厅、匹配）
│   ├── Model/                      # 服务端数据模型
│   ├── Loader/                     # 服务端加载器
│   └── ThirdParty/                 # 第三方依赖
├── Share/                          # 共享工具
│   ├── Analyzer/                   # Roslyn 代码分析器
│   ├── Share.SourceGenerator/      # 源代码生成器
│   └── Tool/                       # 共享工具
├── Config/                         # 配置表源文件
│   ├── Excel/                      # Excel 配置表
│   └── Proto/                      # Protobuf 消息定义
├── Tools/                          # 开发工具
│   ├── MongoDb/                    # MongoDB 数据库
│   ├── Redis/                      # Redis 缓存
│   ├── etcd/                       # etcd 服务发现
│   └── BehaviacSetup_3.6.39.exe   # AI 行为树编辑器
└── ET.sln                          # 主解决方案文件
```

### 分层架构说明

```
┌──────────────────────────────────────────────┐
│                  HotfixView                   │  ← UI、动画、特效表现
├──────────────────────────────────────────────┤
│                   Hotfix                      │  ← 业务逻辑（可热更新）
├──────────────────────────────────────────────┤
│                 ModelView                     │  ← 视图数据模型
├──────────────────────────────────────────────┤
│                   Model                       │  ← 数据定义、实体、组件
├──────────────────────────────────────────────┤
│                    Core                       │  ← 底层框架（网络、序列化）
├──────────────────────────────────────────────┤
│                   Loader                      │  ← 启动引导
└──────────────────────────────────────────────┘
```

## 核心系统

### 战斗系统
- **AOI 视野管理** — 基于网格的 Area of Interest，管理玩家可见范围
- **技能系统** — 可配置的技能释放流程（前摇 → 释放 → 后摇）
- **Buff 系统** — 支持伤害 Buff、僵直 Buff、吟唱 Buff 等
- **状态机** — 怪物 AI 状态机（默认 → 追击 → 战斗 → 返回）
- **战斗匹配** — 支持自定义匹配规则（测试匹配、超时匹配）

### 网络架构
- **Gate** — 网关服务，负责客户端连接管理
- **Lobby** — 大厅服务，管理玩家登录、角色数据
- **Map** — 地图服务，管理场景实例
- **Battle** — 战斗服务，处理实时战斗逻辑
- **Realm** — 登录认证服务
- **Router** — 路由服务，服务间消息转发

### AI 系统
- 基于 **Behaviac** 行为树编辑器设计 AI 行为
- 怪物 AI 状态机：默认巡逻 → 发现目标追击 → 进入战斗 → 脱离返回
- 支持自定义 Agent 属性配置

### 配置系统
| 配置表 | 说明 |
|--------|------|
| BattleGlobalConfig | 战斗全局参数 |
| BattleMapConfig | 战斗地图配置 |
| BattleSceneConfig | 战斗场景配置 |
| Monster@怪物配置 | 怪物属性配置 |
| Skill@技能配置 | 技能参数配置 |
| UnitConfig | 单位基础配置 |
| GameGlobalConfig | 游戏全局参数 |
| MatchGlobalConfig | 匹配全局参数 |

## 快速开始

### 环境要求

- **Unity** 2022.3.51f1c1
- **.NET SDK** 8.0+
- **MongoDB** 7.0+
- **Redis** 7.0+
- **etcd** 3.5+
- **Rider / Visual Studio** 2022+

### 启动步骤

1. **克隆项目**
   ```bash
   git clone <repository-url>
   cd HebyET
   ```

2. **启动基础服务**
   ```bash
   # 启动 MongoDB
   cd Tools/MongoDb
   # 运行 mongod

   # 启动 Redis
   cd Tools/Redis
   # 运行 redis-server

   # 启动 etcd
   cd Tools/etcd
   # 运行 etcd
   ```

3. **编译服务端**
   ```bash
   dotnet build DotNet/DotNet.sln -c Release
   ```

4. **启动服务端**
   ```bash
   dotnet DotNet/App/bin/Release/net8.0/DotNet.App.dll
   ```

5. **打开 Unity 工程**
   - 使用 Unity Hub 打开 `Unity` 目录
   - 等待资源导入完成
   - 点击 Play 运行游戏

### 导表工具

配置表位于 `Config/Excel/`，使用 ET 框架自带的导表工具将 Excel 导出为 Proto 和 C# 代码。

## 开发指南

### 代码规范

- 遵循 ET 框架的 Entity-Component 编程范式
- Model 层定义数据结构，Hotfix 层编写业务逻辑
- 使用 `[MessageSessionHandler]` 特性标记网络消息处理器
- 使用 Roslyn Analyzer 进行编译期代码检查

### 热更新流程

1. 在 `Hotfix` / `HotfixView` 层编写业务代码
2. 编译生成 DLL
3. 通过 HybridCLR 加载热更新 DLL
4. 客户端无需重启即可应用更新

### 添加新功能

1. **数据定义** → `Model/Share/` 中定义 Entity、Component
2. **业务逻辑** → `Hotfix/Share/` 中实现 System（逻辑处理）
3. **UI 表现** → `HotfixView/Client/` 中实现 UI 交互
4. **网络协议** → `Config/Proto/` 中定义消息格式

## 项目状态

- 当前分支：`dev_8.3`
- 主分支：`release9.0`
- 活跃开发中，核心战斗系统持续迭代

## 许可证

本项目基于 [MIT License](LICENSE) 开源。

基于 [ET框架](https://github.com/egametang/ET) 构建，版权归原作者 [tanghai](https://github.com/egametang) 所有。
