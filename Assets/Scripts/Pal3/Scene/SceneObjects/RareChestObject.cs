﻿// ---------------------------------------------------------------------------------------------
//  Copyright (c) 2021-2022, Jiaqi Liu. All rights reserved.
//  See LICENSE file in the project root for license information.
// ---------------------------------------------------------------------------------------------

namespace Pal3.Scene.SceneObjects
{
    using Command;
    using Command.InternalCommands;
    using Command.SceCommands;
    using Core.DataReader.Scn;

    [ScnSceneObject(ScnSceneObjectType.RareChest)]
    public class RareChestObject : SceneObject
    {
        private const float MAX_INTERACTION_DISTANCE = 3f;

        public RareChestObject(ScnObjectInfo objectInfo, ScnSceneInfo sceneInfo)
            : base(objectInfo, sceneInfo)
        {
        }
        
        public override bool IsInteractable(InteractionContext ctx)
        {
            return Activated && ctx.DistanceToActor < MAX_INTERACTION_DISTANCE;
        }

        public override void Interact()
        {
            if (!IsInteractableBasedOnTimesCount()) return;

            CommandDispatcher<ICommand>.Instance.Dispatch(new PlaySfxCommand("wa006", 1));
            
            for (int i = 0; i < 6; i++)
            {
                if (Info.Parameters[i] != 0)
                {
                    CommandDispatcher<ICommand>.Instance.Dispatch(new InventoryAddItemCommand(Info.Parameters[i], 1));
                }
            }
            
            if (ModelType == SceneObjectModelType.CvdModel)
            {
                GetCvdModelRenderer().StartOneTimeAnimation(() =>
                {
                    CommandDispatcher<ICommand>.Instance.Dispatch(new SceneActivateObjectCommand(Info.Id, 0));
                    CommandDispatcher<ICommand>.Instance.Dispatch(new SceneChangeObjectActivationStateCommand(Info.Id, 0));
                });
            }
            else
            {
                CommandDispatcher<ICommand>.Instance.Dispatch(new SceneActivateObjectCommand(Info.Id, 0));
                CommandDispatcher<ICommand>.Instance.Dispatch(new SceneChangeObjectActivationStateCommand(Info.Id, 0));
            }
        }
    }
}