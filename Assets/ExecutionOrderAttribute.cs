using System;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class ExecutionOrderAttribute : Attribute
{
    public string Group;
}
