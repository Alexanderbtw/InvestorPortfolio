﻿namespace Core.Interfaces;

public interface IBuilder<out T>
{
    T Build();
}