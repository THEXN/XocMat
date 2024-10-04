﻿namespace Lagrange.XocMat.Commands;

public class Command<T> where T: BaseCommandArgs
{
    public delegate ValueTask CommandCallBack(T args);

    public List<string> Name { get; }

    public CommandCallBack CallBack { get; }

    public List<string> Permission { get; }

    public Command(List<string> name, CommandCallBack callBack, params string[] permission)
    {
        Name = name;
        CallBack = callBack;
        Permission = [.. permission];
    }

    public Command(string name, CommandCallBack callBack, params string[] permission)
    {
        Name = [name];
        CallBack = callBack;
        Permission = [.. permission];
    }
}
