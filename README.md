# 定时任务完整Demo -QuarzNetProject
包含下面几个部分：
- JobCommon 定时任务公共类库，包含Job的基本定义，可被其他项目引用。
  
- JobEntities 定时任务的具体Job，必须引用JobCommon来继承BaseJob来完成自定义的job。
- JobAPI 负责Quartz.net任务的具体调度。
- JobHost 基于Topshelf将JobAPI包装为exe或者windows服务。如果直接运行则可以在控制台实时查看任务状态，如果是window 服务则可以后端静默运行。
- Portal 为了配置job而搭建的demo MVC门户，负责job的创建和维护。
