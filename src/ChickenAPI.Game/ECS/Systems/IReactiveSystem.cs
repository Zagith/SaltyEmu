﻿using ChickenAPI.Game.ECS.Entities;

namespace ChickenAPI.Game.ECS.Systems
{
    /// <summary>
    ///     System that is designed to be executed everytime it's called
    /// </summary>
    public interface IReactiveSystem
    {
        void Execute(IEntity entity);
    }
}