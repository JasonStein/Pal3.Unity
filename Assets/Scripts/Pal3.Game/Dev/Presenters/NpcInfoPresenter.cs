// ---------------------------------------------------------------------------------------------
//  Copyright (c) 2021-2023, Jiaqi Liu. All rights reserved.
//  See LICENSE file in the project root for license information.
// ---------------------------------------------------------------------------------------------

namespace Pal3.Game.Dev.Presenters
{
    using Core.DataReader.Scn;
    using Engine.Abstraction;
    using UnityEngine;

    /// <summary>
    /// ScnNpcInfo holder component to present ScnNpcInfo in the Unity inspector.
    /// </summary>
    public sealed class NpcInfoPresenter : GameEntityScript
    {
        [SerializeField] public ScnNpcInfo npcInfo;
    }
}