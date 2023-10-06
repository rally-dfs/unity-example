#nullable enable

using System;

using UnityEngine.UIElements;

public abstract class StatefulVisualElement : VisualElement
{
    protected abstract void Build();

    protected void InitState(Action action)
    {
        Build();
        action();
    }

    protected void SetState(Action action)
    {
        action();
        schedule.Execute(Rebuild);
    }

    void Rebuild()
    {
        Clear();
        Build();
    }
}