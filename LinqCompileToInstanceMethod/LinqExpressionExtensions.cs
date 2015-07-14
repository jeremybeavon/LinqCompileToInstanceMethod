using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using ClrTest.Reflection;

namespace LinqCompileToInstanceMethod
{
    public static class LinqExpressionExtensions
    {
        private static readonly ModuleBuilder moduleBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(
            new AssemblyName("StaticGenerator"),
            AssemblyBuilderAccess.Run).DefineDynamicModule("StaticGenerator");

        public static void CompileToInstanceMethod(this LambdaExpression expression, MethodBuilder method)
        {
            string uniqueId = Guid.NewGuid().ToString("N");
            TypeBuilder typeBuilder = moduleBuilder.DefineType("t" + uniqueId);
            MethodBuilder methodBuilder = typeBuilder.DefineMethod(
                "m" + uniqueId,
                MethodAttributes.Public | MethodAttributes.Static,
                expression.Type,
                expression.Parameters.Select(parameter => parameter.Type).ToArray());
            expression.CompileToMethod(methodBuilder);
            Type type = typeBuilder.CreateType();
            MethodInfo staticMethod = type.GetMethod("m" + uniqueId);
            new ILReader(staticMethod).Accept(new ILTransform(method.GetILGenerator()));
        }
    }
}
