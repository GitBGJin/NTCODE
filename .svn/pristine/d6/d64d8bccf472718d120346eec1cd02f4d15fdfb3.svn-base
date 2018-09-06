using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.CodeDom;
using System.Net;
using System.Web.Services.Description;
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using System.IO;

namespace Service
{
    public class WebServiceHelper
    {
        /// <summary>
        /// 动态调用WEB服务
        /// </summary>
        /// <param name="url">
        /// string：WEB服务的URL
        /// </param>
        /// <param name="methodname">
        /// string：WEB服务中的方法名
        /// </param>
        /// <param name="args">
        /// object[]：WEB服务方法中的参数
        /// </param>
        /// <history>
        /// Lifei 2015/03/25 Create
        /// </history>
        /// <returns>
        /// WEB服务方法的返回值
        /// </returns>
        public static object InvokeWebService(string url, string methodname, object[] args)
        {
            return WebServiceHelper.InvokeWebService(url, null, methodname, args);
        }
        /// <summary>
        /// 动态调用WEB服务
        /// </summary>
        /// <param name="url">
        /// string：WEB服务的URL
        /// </param>
        /// <param name="classname">
        /// string：WEB服务类名
        /// </param>
        /// <param name="methodname">
        /// string：WEB服务中的方法名
        /// </param>
        /// <param name="args">
        /// object[]：WEB服务方法中的参数
        /// </param>
        /// <history>
        /// Lifei 2015/03/25 Create
        /// </history>
        /// <returns>
        /// WEB服务方法的返回值
        /// </returns>
        public static object InvokeWebService(string url, string classname, string methodname, object[] args)
        {
            string @namespace = "EnterpriseServerBase.WebService.DynamicWebCalling";
            if ((classname == null) || (classname == ""))
            {
                classname = GetWsClassName(url);
            }

            try
            {
                //获取WSDL
                WebClient wc = new WebClient();
                Stream stream = wc.OpenRead(url + "?WSDL");
                ServiceDescription sd = ServiceDescription.Read(stream);
                ServiceDescriptionImporter sdi = new ServiceDescriptionImporter();
                sdi.AddServiceDescription(sd, "", "");
                CodeNamespace cn = new CodeNamespace(@namespace);

                //生成客户端代理类代码
                CodeCompileUnit ccu = new CodeCompileUnit();
                ccu.Namespaces.Add(cn);
                sdi.Import(cn, ccu);

                CodeDomProvider csc = new CSharpCodeProvider();

                //设定编译参数
                CompilerParameters cplist = new CompilerParameters();
                cplist.GenerateExecutable = false;
                cplist.GenerateInMemory = true;
                cplist.ReferencedAssemblies.Add("System.dll");
                cplist.ReferencedAssemblies.Add("System.XML.dll");
                cplist.ReferencedAssemblies.Add("System.Web.Services.dll");
                cplist.ReferencedAssemblies.Add("System.Data.dll");

                //编译代理类
                CompilerResults cr = csc.CompileAssemblyFromDom(cplist, ccu);
                if (true == cr.Errors.HasErrors)
                {
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    foreach (System.CodeDom.Compiler.CompilerError ce in cr.Errors)
                    {
                        sb.Append(ce.ToString());
                        sb.Append(System.Environment.NewLine);
                    }
                    throw new Exception(sb.ToString());
                }

                //生成代理实例，并调用方法
                System.Reflection.Assembly assembly = cr.CompiledAssembly;
                Type t = assembly.GetType(@namespace + "." + classname, true, true);
                object obj = Activator.CreateInstance(t);
                System.Reflection.MethodInfo mi = t.GetMethod(methodname);

                return mi.Invoke(obj, args);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException.Message, new Exception(ex.InnerException.StackTrace));
            }
        }
        /// <summary>
        /// 获取URL当中的类名（当类名为空的情况下默认取URL中文件名）
        /// </summary>
        /// <param name="wsUrl"></param>
        /// <returns></returns>
        private static string GetWsClassName(string wsUrl)
        {
            string[] parts = wsUrl.Split('/');
            string[] pps = parts[parts.Length - 1].Split('.');

            return pps[0];
        }

    }
}
