# THMonitor - 多站点多线程温湿度智能监控系统

[![.NET Version](https://img.shields.io/badge/.NET-6.0-blue.svg)](https://dotnet.microsoft.com/)
[![WPF](https://img.shields.io/badge/UI-WPF-orange.svg)](https://github.com/dotnet/wpf)
[![Prism](https://img.shields.io/badge/Framework-Prism.DryIoc-purple.svg)](https://prismlibrary.com/)
[![Database](https://img.shields.io/badge/Database-SQL_Server-red.svg)](https://www.microsoft.com/sql-server)

基于 **.NET 6.0 WPF** 平台开发的高性能工业上位机系统。集成 **Prism** 模块化设计、**Modbus TCP** 工业协议、以及**高性能异步日志持久化引擎**。面向多工位/多站点温湿度实时数据采集、高清图表渲染、故障实时报警、配方管理及数据追溯。

---

## 🛠️ 技术栈

*   **核心开发环境**：.NET 6.0 (WPF) / C# 10 / Visual Studio 2022
*   **MVVM 架构 & 依赖注入**：Prism.DryIoc (8.1+) / CommunityToolkit.Mvvm
*   **UI 控件库**：Material Design In XAML Toolkit (现代扁平化样式)
*   **数据可视化**：LiveChartsCore.SkiaSharpView.WPF (高清仪表盘、历史与实时双向曲线渲染)
*   **工业通信协议**：自定义 Modbus TCP 客户端驱动 (支持输入/输出线圈、输入/输出寄存器)
*   **数据转换引擎**：`DataConvertLib` (支持 ABCD/DCBA/BADC/CDAB 大小端转换与点位线性系数缩放)
*   **高频数据库访问与优化**：
    *   `SQLHelper`：支持瞬态故障判定、退避指数级重试逻辑与克隆 `SqlParameter` 机制
    *   `SysLogManage`：**基于 `BlockingCollection<T>` 实现的生产者-消费者异步后台写入服务**，彻底消除数据库I/O导致的UI线程卡顿
*   **配置解析器**：MiniExcel (用于 Excel 格式点位配置的高速流式解析)

---

## 📂 项目结构说明

本系统采用经典的多层架构，配合 Prism 框架解耦视图与逻辑层：

```
THMonitor/
│
├── THMonitor.App/                      # 主程序/外壳层 (WPF / Prism App)
│   ├── Views/                          # 系统视图组件 (MonitorView, AlarmView, RecipeView, HistoryView 等)
│   ├── ViewModels/                     # 视图模型层 (负责 UI 交互、LiveCharts 数据更新及 Prism 导航参数透传)
│   └── Command/                        # 通用控制方法 (PLC 通信线程管理、数据转换拦截等)
│
├── THMonitor.BLL/                      # 业务逻辑层 (BLL)
│   └── SysLogManage.cs                 # 异步日志队列管理 (生产者-消费者缓冲区)
│
├── THMonitor.DAL/                      # 数据访问层 (DAL)
│   ├── SQLHelper.cs                    # 深度优化的数据库交互核心 (支持异步重试、高并发缓冲与事务控制)
│   └── SysLogService.cs                # 系统日志底层持久化服务
│
├── THMonitor.Models/                   # 数据模型层
│   ├── Config/                         # 变量/设备配置模型 (Device.cs 包含报警检测逻辑)
│   ├── Recipe/                         # 配方数据模型
│   └── SQL/                            # SQL 数据库实体定义
│
├── THMonitor.Helper/                   # 工业驱动与工具辅助层
│   ├── ModbusTCP.cs                    # Modbus TCP 标准客户端底层驱动
│   └── IniConfigHelper.cs              # INI 设备物理配置文件读取助手
│
└── DataConvertLib/                     # 高性能数据解析核心库
    ├── ByteArrayLib.cs                 # 字节流拆装与逆序重组引擎
    └── MigrationLib.cs                 # 点位线性偏移值计算与工程值转换层
```

---

## 🚀 核心功能模块

### 1. 多线程轮询采集
采用后台独立线程（`Task.Run` + `CancellationToken`）对 PLC 点位进行异步高频轮询采集，避免 UI 线程阻塞。

### 2. 实时看板 & 折线图
使用 LiveChartsCore 进行 SkiaSharp 高清渲染，以动态 Gauge 仪表盘直观展现 6 个站点的温湿度，并基于折线图实现最近多次采集的平滑趋势预览。

### 3. 高性能异步日志持久化 (Producer-Consumer)
针对高频报警和操作日志写入数据库的操作，系统在 `SysLogManage` 中引入了静态 `BlockingCollection<SysLog>` 队列：
*   **生产者**：UI 操作或 PLC 报警触发时，直接将日志压入队列，立即返回，**实现主线程 0 阻塞**。
*   **消费者**：长驻后台的任务（`ProcessLogQueue`）负责将队列中的日志项批量、顺序写入数据库。
*   **UI 同步**：报警触发时，通过 `Dispatcher.Invoke` 安全地将报警项同步到主界面列表。

### 4. 用户认证与安全权限 (RBAC)
支持多用户登录体系，密码通过 MD5 密文存储；具备细粒度的权限控制，支持对用户管理、参数设置、配方管理、报警追溯和历史趋势等导航页面实施角色控制。

### 5. 配方管理
支持在线创建、编辑、另存及下发设备生产工艺配方参数。

### 6. 历史数据趋势与故障追溯
通过多维 SQL 条件检索历史记录，支持温湿度数据表格化分析及 SkiaSharp 折线图可视化呈现。

---

## 🔧 运行与配置准备

### 1. 配置文件
*   **INI 基础配置文件** (`Config/Device.ini`):
    配置 PLC 连接 IP、Port 端口号、心跳包配置及断线重连时间间隔等。
*   **PLC 点位配置** (`Config/Group.xlsx` & `Config/Variable.xlsx`):
    使用 MiniExcel 解析，定义通信组寄存器区域 (输入线圈/输出线圈/输入寄存器/输出寄存器) 及变量名、寄存器地址、类型、放大缩小比例和偏移量。

### 2. 数据库配置
在 `THMonitor.App/App.config` 中，配置对应的 `connString` 数据库连接串：
```xml
<connectionStrings>
    <add name="connString" connectionString="Data Source=localhost\SQLEXPRESS;Initial Catalog=MultiTHMonitorDB;User ID=sa;Password=123456;TrustServerCertificate=True" providerName="System.Data.SqlClient" />
</connectionStrings>
```

### 3. 数据表结构参考
系统运行需要包含以下几张核心表：
*   `ActualData`：温湿度历史数据存储（包含 6 个 Station 站点的 Temp 与 Humidity 字段）。
*   `SysAdmin`：管理员表（包含 `LoginId`, `LoginName`, `LoginPwd`, 权限控制位如 `UserManage`, `ParamSet` 等）。
*   `SysLog`：操作和异常日志记录表。
*   `AlarmLog` / `HistoryAlarm`：报警状态存储及追溯表。
