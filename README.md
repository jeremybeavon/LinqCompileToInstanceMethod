# Overview
Provides an extension method to LINQ to compile an expression to an instance method. If the method has parameters, the first parameter of the expression must be represent "this".

# Example

This example uses reflection emit, LINQ expression and LinqCompileToInstanceMethod to create the following code:
```csharp
public sealed class SimpleAdd
{
  public int Add(int parameter1, int parameter2)
  {
    return parameter1 + parameter2;
  }
}
```

```csharp
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using LinqCompileToInstanceMethod;

public static class CompileToInstanceMethodExample
{
  public static void Main(string[] args)
  {
    AssemblyBuilder assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(
      new AssemblyName("TestAssembly"),
      AssemblyBuilderAccess.Run);
    ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule("TestAssembly");
    TypeBuilder typeBuilder = moduleBuilder.DefineType(
      "SimpleAdd",
      TypeAttributes.Public | TypeAttributes.Sealed | TypeAttributes.AutoClass | TypeAttributes.AnsiClass);
    MethodBuilder methodBuilder = typeBuilder.DefineMethod(
        "Add",
        MethodAttributes.Public | MethodAttributes.HideBySig,
        typeof(int),
        new Type[] { typeof(int), typeof(int) });
    Expression<Func<object, int, int, int>> expression = (local, int1, int2) => int1 + int2;
    expression.CompileToInstanceMethod(methodBuilder);
    Type type = typeBuilder.CreateType();
    object instance = Activator.CreateInstance(type);
    MethodInfo method = type.GetMethod("Add");
    Console.WriteLine(method.Invoke(instance, new object[] { 1, 2 }));
  }
}
```
