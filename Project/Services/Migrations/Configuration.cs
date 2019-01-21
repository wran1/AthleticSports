using System.Data.Entity.Migrations;
using System.Linq;
using Models.SysModels;

namespace Services.Migrations
{
    public class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            #region 企业

            //var sysEnterprises = new[]
            //{
            //    new SysEnterprise
            //    {
            //        Id = "defaultEnt",
            //        EnterpriseName = "天津经济技术开发区投资促进三局"
            //    },
            //    //new SysEnterprise
            //    //{
            //    //    Id = "TestEnt",
            //    //    EnterpriseName = "测试企业"
            //    //}
            //};
            //context.SysEnterprises.AddOrUpdate(a => new {a.Id}, sysEnterprises);

            #endregion

            #region SysArea

            var sysAreas = new[]
            {
                new SysArea
                {
                    AreaName = "Platform",
                    Name = "管理平台",
                    SystemId = "002"
                }
            };
            context.SysAreas.AddOrUpdate(a => new {a.AreaName, a.Name, a.SystemId}, sysAreas);

            #endregion

            #region SysAction

            var sysActions = new[]
            {
                new SysAction
                {
                    Name = "列表",
                    ActionName = "Index",
                    SystemId = "001",
                    System = true
                },
                new SysAction
                {
                    Name = "详细",
                    ActionName = "Details",
                    SystemId = "003",
                    System = true
                },
                new SysAction
                {
                    Name = "新建",
                    ActionName = "Create",
                    SystemId = "004",
                    System = true
                },
                new SysAction
                {
                    Name = "编辑",
                    ActionName = "Edit",
                    SystemId = "005",
                    System = true
                },
                new SysAction
                {
                    Name = "删除",
                    ActionName = "Delete",
                    SystemId = "006",
                    System = true
                },
                new SysAction
                {
                    Name = "导出",
                    ActionName = "Report",
                    SystemId = "007",
                    System = true
                }
            };
            context.SysActions.AddOrUpdate(a => new {a.ActionName}, sysActions);

            #endregion

            #region SysController

            var sysControllers = new[]
            {
                #region 基础内容 100
                new SysController
                {
                    SysAreaId = sysAreas.Single(a => a.AreaName == "Platform").Id,
                    Name = "登陆管理平台",
                    ControllerName = "Index",
                    SystemId = "100",
                    Display = false
                },
                new SysController
                {
                    SysAreaId = sysAreas.Single(a => a.AreaName == "Platform").Id,
                    Name = "桌面",
                    ControllerName = "Desktop",
                    SystemId = "100100",
                    Display = false
                },
                //new SysController
                //{
                //    SysAreaId = sysAreas.Single(a => a.AreaName == "Platform").Id,
                //    Name = "使用帮助",
                //    ControllerName = "Help",
                //    SystemId = "100200",
                //    Display = false
                //},
                //new SysController
                //{
                //    SysAreaId = sysAreas.Single(a => a.AreaName == "Platform").Id,
                //    Name = "消息中心",
                //    ControllerName = "MyMessage",
                //    SystemId = "100300",
                //    Display = false
                //},
                new SysController
                {
                    SysAreaId = sysAreas.Single(a => a.AreaName == "Platform").Id,
                    Name = "修改密码",
                    ControllerName = "ChangePassword",
                    SystemId = "100400",
                    Display = false
                },

                #endregion other                

                //#region 网站管理 200
                new SysController
                {
                    SysAreaId = sysAreas.Single(a => a.AreaName == "Platform").Id,
                    Name = "网站管理",
                    ControllerName = "Index",
                    SystemId = "200",
                    Ico = "fa-globe",
                    Display = true
                },
                //new SysController
                //{
                //    SysAreaId = sysAreas.Single(a => a.AreaName == "Platform").Id,
                //    Name = "栏目管理",
                //    ControllerName = "CmsCategory",
                //    SystemId = "200005",
                //    Ico = "fa-newspaper-o"
                //},
                //new SysController
                //{
                //    SysAreaId = sysAreas.Single(a => a.AreaName == "Platform").Id,
                //    Name = "资讯管理",
                //    ControllerName = "CmsArtical",
                //    SystemId = "200100",
                //    Ico = "fa-newspaper-o",
                //    Display = false,
                //},
                //  new SysController
                //{
                //    SysAreaId = sysAreas.Single(a => a.AreaName == "Platform").Id,
                //    Name = "国务院信息",
                //    ControllerName = "StateCouncil",
                //    SystemId = "200105",
                //    Ico = "fa-newspaper-o",
                    
                //},
                
                //new SysController
                //{
                //    SysAreaId = sysAreas.Single(a => a.AreaName == "Platform").Id,
                //    Name = "企业之窗",
                //    ControllerName = "CompanyInfo",
                //    SystemId = "200110",
                //    Ico = "fa-newspaper-o",
                //},
                //new SysController
                //{
                //    SysAreaId = sysAreas.Single(a => a.AreaName == "Platform").Id,
                //    Name = "产品管理",
                //    ControllerName = "TedaProductManage",
                //    SystemId = "200200",
                //    Ico = "fa-newspaper-o",
                //},
                //new SysController
                //{
                //    SysAreaId = sysAreas.Single(a => a.AreaName == "Platform").Id,
                //    Name = "年度计划",
                //    ControllerName = "AnnualPlan",
                //    SystemId = "200250",
                //    Ico = "fa-newspaper-o",
                //},
                //new SysController
                //{
                //    SysAreaId = sysAreas.Single(a => a.AreaName == "Platform").Id,
                //    Name = "产品册",
                //    ControllerName = "ProductBook",
                //    SystemId = "200300",
                //    Ico = "fa-newspaper-o",
                //},
                //new SysController
                //{
                //    SysAreaId = sysAreas.Single(a => a.AreaName == "Platform").Id,
                //    Name = "关于我们",
                //    ControllerName = "AboutUs",
                //    SystemId = "200750",
                //    Ico = "fa-list-ul",
                //    Display = false,
                //},

                //new SysController
                //{
                //    SysAreaId = sysAreas.Single(a => a.AreaName == "Platform").Id,
                //    Name = "会员管理",
                //    ControllerName = "MemberManagement",
                //    SystemId = "200800",
                //    Ico = "fa-address-card"
                //},
                //  new SysController
                //{
                //    SysAreaId = sysAreas.Single(a => a.AreaName == "Platform").Id,
                //    Name = "留言板管理",
                //    ControllerName = "Consultation",
                //    SystemId = "200850",
                //    Ico = "fa-list-ul"
                //},
                #endregion

                #region 系统管理 900
                new SysController
                {
                    SysAreaId = sysAreas.Single(a => a.AreaName == "Platform").Id,
                    Name = "系统管理",
                    ControllerName = "Index",
                    SystemId = "900",
                    Ico = "fa-cogs",
                    Display = true
                },
                   new SysController
                {
                    SysAreaId = sysAreas.Single(a => a.AreaName == "Platform").Id,
                    Name = "组织架构",
                    ControllerName = "SysDepartment",
                    SystemId = "900100",
                    Ico = "fa-building-o",
                },
                new SysController
                {
                    SysAreaId = sysAreas.Single(a => a.AreaName == "Platform").Id,
                    Name = "角色管理",
                    ControllerName = "SysRole",
                    SystemId = "900300",
                    Ico = "fa-street-view"
                },
                new SysController
                {
                    SysAreaId = sysAreas.Single(a => a.AreaName == "Platform").Id,
                    Name = "用户管理",
                    ControllerName = "SysUser",
                    SystemId = "900400",
                    Ico = "fa-user-circle-o"
                },
               
                #endregion
                #region 系统开发配置 950
                new SysController
                {
                    SysAreaId = sysAreas.Single(a => a.AreaName == "Platform").Id,
                    Name = "系统设置",
                    ControllerName = "Index",
                    SystemId = "950",
                    Ico = "fa-cog"
                },
                //new SysController
                //{
                //    SysAreaId = sysAreas.Single(a => a.AreaName == "Platform").Id,
                //    Name = "操作类型",
                //    ControllerName = "SysAction",
                //    SystemId = "950300",
                //    Ico = "fa-th-large"
                //},
                //new SysController
                //{
                //    SysAreaId = sysAreas.Single(a => a.AreaName == "Platform").Id,
                //    Name = "系统模块",
                //    ControllerName = "SysController",
                //    SystemId = "950400",
                //    Ico = "fa-puzzle-piece"
                //},
                //new SysController
                //{
                //    SysAreaId = sysAreas.Single(a => a.AreaName == "Platform").Id,
                //    Name = "帮助信息",
                //    ControllerName = "SysHelp",
                //    SystemId = "950900",
                //    Ico = "fa-info-circle"
                //},
                //new SysController
                //{
                //    SysAreaId = sysAreas.Single(a => a.AreaName == "Platform").Id,
                //    Name = "帮助信息分类",
                //    ControllerName = "SysHelpClass",
                //    SystemId = "950950",
                //    Ico = "fa-info-circle"
                //},
                new SysController
                {
                    SysAreaId = sysAreas.Single(a => a.AreaName == "Platform").Id,
                    Name = "用户日志",
                    ControllerName = "SysUserLog",
                    SystemId = "950990",
                    Ico = "fa-calendar"
                },

                #endregion
            };

            context.SysControllers.AddOrUpdate(
                a => new {a.SysAreaId, a.SystemId}, sysControllers);

     

            #region SysControllerSysAction

            var sysControllerSysActions = (from sysAction in sysActions.Where(a => a.System)
                from sysController in sysControllers
                select
                new SysControllerSysAction
                {
                    SysActionId = sysAction.Id,
                    SysControllerId = sysController.Id
                }).ToArray();

            context.SysControllerSysActions.AddOrUpdate(a => new {a.SysActionId, a.SysControllerId},
                sysControllerSysActions);

            #endregion

            #region 模块特殊Action

            context.SysControllerSysActions.AddOrUpdate(a => new {a.SysActionId, a.SysControllerId});

            #endregion

            #region 角色定义

            //自动清理不需要的权限设置功能
            var ids4SysControllerSysActions = sysControllerSysActions.Select(a => a.Id).ToList();
            context.SysRoleSysControllerSysActions.RemoveRange(
                context.SysRoleSysControllerSysActions.Where(
                    rc =>
                        rc.SysControllerSysAction.SysAction.System &&
                        !ids4SysControllerSysActions.Contains(rc.SysControllerSysActionId)).ToList());
            //context.Commit(); 实时提交一下是否能解决重复的问题？

            //生成系统管理员
            var sysRole = new SysRole
            {
                Id = "admin",
                Name = "系统管理员",
                RoleName = "系统管理员",
                SysDefault = true,
                SystemId = "999",
                //EnterpriseId = ent.Id
                //系统管理员自动获得所有权限
                //SysRoleSysControllerSysActions = (from aa in sysControllerSysActions select new SysRoleSysControllerSysAction { SysControllerSysActionId = aa.Id, RoleId = "admin-" + ent.Id }).ToArray()
            };
            context.SysRoles.AddOrUpdate(sysRole);
            //系统管理员自动获得所有权限
            var sysRoleSysControllerSysActions = (from aa in sysControllerSysActions
                                                  select
                                                  new SysRoleSysControllerSysAction
                                                  {
                                                      SysControllerSysActionId = aa.Id,
                                                      RoleId = sysRole.Id
                                                  }).ToArray();
            context.SysRoleSysControllerSysActions.AddOrUpdate(rc => new { rc.RoleId, rc.SysControllerSysActionId },
                sysRoleSysControllerSysActions);
            //foreach (var ent in sysEnterprises)
            //{
            //    //生成系统管理员
            //    var sysRole = new SysRole
            //    {
            //        Id = "admin-" + ent.Id,
            //        Name = ent.EnterpriseName + "系统管理员",
            //        RoleName = "系统管理员",
            //        SysDefault = true,
            //        SystemId = "999",
            //        EnterpriseId = ent.Id
            //        //系统管理员自动获得所有权限
            //        //SysRoleSysControllerSysActions = (from aa in sysControllerSysActions select new SysRoleSysControllerSysAction { SysControllerSysActionId = aa.Id, RoleId = "admin-" + ent.Id }).ToArray()
            //    };
            //    context.SysRoles.AddOrUpdate(sysRole);
            //    //系统管理员自动获得所有权限
            //    var sysRoleSysControllerSysActions = (from aa in sysControllerSysActions
            //        select
            //        new SysRoleSysControllerSysAction
            //        {
            //            SysControllerSysActionId = aa.Id,
            //            RoleId = sysRole.Id
            //        }).ToArray();
            //    context.SysRoleSysControllerSysActions.AddOrUpdate(rc => new {rc.RoleId, rc.SysControllerSysActionId},
            //        sysRoleSysControllerSysActions);
            //}

            #endregion
        }
    }
}