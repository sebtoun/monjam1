using System;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class ExecutionOrderAttribute : Attribute
{
    public string Group = "Default";

    public ExecutionOrderAttribute(string group)
    {
        Group = group;
    }
}
