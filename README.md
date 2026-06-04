# MultiTHMonitorProject

> **多站点多线程温湿度智能监控系统**  
> 基于 **.NET 6.0 WPF** 平台开发，集成 **Prism** 架构与 **Modbus TCP** 工业协议，面向多工位/多站点温湿度实时数据采集、图表渲染、故障报警、配方管理及数据持久化的工业上位机控制系统。

---

## 🛠️ 技术栈

*   **核心框架**：.NET 6.0 (WPF) / C# 10
*   **MVVM & 依赖注入**：Prism.DryIoc (8.1+) / CommunityToolkit.Mvvm
*   **UI 框架 & 控件库**：Material Design In XAML Toolkit
*   **数据可视化**：LiveChartsCore.SkiaSharpView.WPF (仪表盘、历史与实时双向曲线渲染)
*   **工业通信协议**：自定义 Modbus TCP 客户端驱动 (支持输入线圈、输出线圈、输入寄存器、输出寄存器)
*   **数据转换引擎**：`DataConvertLib` (支持 ABCD/DCBA/BADC/CDAB 等多种大小端格式转换与点位线性系数缩放)
*   **数据持久化**：Microsoft.Data.SqlClient (面向 SQL Server) + 事务处理机制
*   **高频数据库访问优化**：支持瞬态故障判定、退避指数级重试逻辑与克隆 SqlParameter 防异常机制
*   **配置解析器**：MiniExcel (用于 Excel 格式点位的高速动态解析)

---

## 📂 项目结构说明

本系统采用经典的多层架构，配合 Prism 框架解耦视图与逻辑层：

```
thinger.WPF.MultiTHMonitorProject/
│
├── THMonitor.App/                      # 主程序/外壳层 (WPF / Prism App)
│   ├── Views/                          # 系统视图组件 (MonitorView, AlarmView, RecipeView, HistoryView 等)
│   ├── ViewModels/                     # 视图模型层 (负责 UI 交互、LiveCharts 数据更新及 Prism 导航参数透传)
│   └── Command/                        # 通用控制方法 (PLC 通信线程管理、数据转换拦截等)
│
├── THMonitor.BLL/                      # 业务逻辑层 (BLL)
│   └── 负责控制各业务功能的流转与事务逻辑校验
│
├── THMonitor.DAL/                      # 数据访问层 (DAL)
│   ├── SQLHelper.cs                    # 深度优化的数据库交互核心 (支持异步重试、高并发缓冲与事务控制)
│   └── SysAdminService/SysLogService   # 用户信息、系统日志与温湿度记录持久化服务
│
├── THMonitor.Models/                   # 数据模型层
│   ├── Config/                         # 变量/设备配置模型
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

1. **多线程轮询采集**：采用后台独立线程（`Task.Run`）对 PLC 点位进行异步高频轮询采集，避免 UI 线程阻塞。
2. **实时看板 & 折线图**：使用 LiveChartsCore 进行 SkiaSharp 高清渲染，以动态 Gauge 仪表盘直观展现 6 个站点的温湿度，并基于折线图实现最近多次采集的平滑趋势预览。
3. **用户认证与安全权限 (RBAC)**：支持多用户登录体系，密码通过 MD5 密文存储；具备细粒度的权限控制，支持对用户管理、参数设置、配方管理、报警追溯和历史趋势等导航页面实施角色控制。
4. **配方管理**：支持在线创建、编辑、另存及下发设备生产工艺配方参数。
5. **历史数据趋势**：通过多维 SQL 条件检索历史记录，支持温湿度数据表格化分析及 SkiaSharp 折线图可视化呈现。
6. **故障追溯与日志**：具备完整的报警监控，实时展示报警列表并支持历史报警检索与确认机制；全生命周期操作日志追溯。

---

## 🔧 运行与配置准备

### 1. 配置文件
* **INI 基础配置文件** (`Config/Device.ini`):
  配置 PLC 连接 IP、Port 端口号、心跳包配置及断线重连时间间隔等。
* **PLC 点位配置** (`Config/Group.xlsx` & `Config/Variable.xlsx`):
  使用 MiniExcel 解析，定义通信组寄存器区域 (输入线圈/输出线圈/输入寄存器/输出寄存器) 及变量名、寄存器地址、类型、放大缩小比例和偏移量。

### 2. 数据库配置
在 `THMonitor.App/App.config` 中，配置对应的 `connString` 数据库连接串：
```xml
<connectionStrings>
    <add name="connString" connectionString="Data Source=服务器IP;Initial Catalog=数据库名称;User ID=用户名;Password=密码;TrustServerCertificate=True" providerName="System.Data.SqlClient" />
</connectionStrings>
```

### 3. 数据表结构参考
系统运行需要包含以下几张核心表：
* `ActualData`：温湿度历史数据存储（包含 6 个 Station 站点的 Temp 与 Humidity 字段）。
* `SysAdmin`：管理员表（包含 `LoginId`, `LoginName`, `LoginPwd`, 权限控制位如 `UserManage`, `ParamSet` 等）。
* `SysLog`：操作和异常日志记录表。
* `AlarmLog` / `HistoryAlarm`：报警状态存储及追溯表。
