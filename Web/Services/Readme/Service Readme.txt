
业务层各个工程说明：


1、SmartEP.Service.AutoMonitoring        自动监控平台：包括实时监控、远程控制、数据查询、数据比对、站点基本信息、采集仪信息、分析仪信息、配置信息等功能


2、SmartEP.Service.Core                  通用功能    ：包括通用接口、枚举类型、读写配置、缓存、日志、事件、异常、封装通用控件等功能


3、SmartEP.Service.DataAuditing          数据审核平台：主要面向数据和规则配置，具有通用数据审核功能


4、SmartEP.Service.ExchangePlatForm      数据交换平台：主要根据数据交换规则、方式、格式等信息实现数据无缝交互


5、SmartEP.Service.Framework             后台管理平台：人员、角色、基本参数、权限等管理功能


6、SmartEP.Service.OperatingMaintenance  运行维护平台：现场维护人员定期维护现场子站信息系统


7、SmartEP.Service.PublishingPlatform    发布平台    ：根据国家标准发布实时数据及日数据，可以基于GIS发布及预测空气质量或污染物（蓝藻）扩散情况


8、SmartEP.Service.QualityControl        质量控制平台：主要是现场仪器校准功能


9、SmartEP.Service.ReportLibrary         报表平台    ：通用数据报表开发系统


10、SmartEP.Service.Communication        通讯平台    ：服务器端与现场端基于Socket（WCF）技术的通讯平台

环境质量系统分为基本版、专业版、高级版三个版本，其中
基本版：涵盖自动监控平台、通用功能、数据审核平台（基本版）、后台管理平台、报表平台、通讯平台
专业版：基于基本版功能上，增加数据审核平台（专业版）、数据交换平台、发布平台
高级版：涵盖系统所有功能，其中数据审核平台使用高级版功能