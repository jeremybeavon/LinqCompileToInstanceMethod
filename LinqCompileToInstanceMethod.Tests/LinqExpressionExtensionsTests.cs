using System;
using System.Collections;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LinqCompileToInstanceMethod.Tests
{
    [TestClass]
    public class LinqExpressionExtensionsTests
    {
        private AssemblyBuilder assemblyBuilder;
        private ModuleBuilder moduleBuilder;

        [TestInitialize]
        public void FixtureSetUp()
        {
            //assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(new AssemblyName("TestAssembly"), AssemblyBuilderAccess.RunAndSave);
            //moduleBuilder = assemblyBuilder.DefineDynamicModule("TestAssembly", "TestAssembly.dll");
            assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(new AssemblyName("TestAssembly"), AssemblyBuilderAccess.Run);
            moduleBuilder = assemblyBuilder.DefineDynamicModule("TestAssembly");
        }

        [TestMethod]
        public void TestSimpleAdd()
        {
            TypeBuilder typeBuilder = moduleBuilder.DefineType("SimpleAdd", TypeAttributes.Public | TypeAttributes.Sealed | TypeAttributes.AutoClass | TypeAttributes.AnsiClass);
            MethodBuilder methodBuilder = typeBuilder.DefineMethod(
                "TestAdd",
                MethodAttributes.Public | MethodAttributes.HideBySig,
                typeof(int),
                Type.EmptyTypes);
            Expression<Func<int>> expression = () => 1 + 2;
            expression.CompileToInstanceMethod(methodBuilder);
            Type type = typeBuilder.CreateType();
            object instance = Activator.CreateInstance(type);
            MethodInfo method = type.GetMethod("TestAdd");
            method.Invoke(instance, null).Should().Be(3);
        }

        [TestMethod]
        public void TestSimpleAddWithParameters()
        {
            TypeBuilder typeBuilder = moduleBuilder.DefineType("SimpleAddWithParameters", TypeAttributes.Public | TypeAttributes.Sealed | TypeAttributes.AutoClass | TypeAttributes.AnsiClass);
            MethodBuilder methodBuilder = typeBuilder.DefineMethod(
                "TestAdd",
                MethodAttributes.Public | MethodAttributes.HideBySig,
                typeof(int),
                new Type[] { typeof(int), typeof(int) });
            Expression<Func<object, int, int, int>> expression = (local, int1, int2) => int1 + int2;
            expression.CompileToInstanceMethod(methodBuilder);
            Type type = typeBuilder.CreateType();
            object instance = Activator.CreateInstance(type);
            MethodInfo method = type.GetMethod("TestAdd");
            method.Invoke(instance, new object[] { 1, 2 }).Should().Be(3);
        }

        [TestMethod]
        public void TestCreateArrayList()
        {
            TypeBuilder typeBuilder = moduleBuilder.DefineType("TestCreateArrayList", TypeAttributes.Public | TypeAttributes.Sealed | TypeAttributes.AutoClass | TypeAttributes.AnsiClass);
            MethodBuilder methodBuilder = typeBuilder.DefineMethod(
                "TestMethod",
                MethodAttributes.Public | MethodAttributes.HideBySig,
                typeof(IList),
                Type.EmptyTypes);
            Expression<Func<IList>> expression = () => new ArrayList();
            expression.CompileToInstanceMethod(methodBuilder);
            Type type = typeBuilder.CreateType();
            object instance = Activator.CreateInstance(type);
            MethodInfo method = type.GetMethod("TestMethod");
            method.Invoke(instance, null).Should().BeOfType<ArrayList>();
        }
    }
}
