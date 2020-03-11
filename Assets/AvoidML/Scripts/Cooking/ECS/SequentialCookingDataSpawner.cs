using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using Xmaho.HelenHayes;

namespace AvoidML.Cooking
{
    [DisallowMultipleComponent]
    [RequiresEntityConversion]
    [System.Obsolete()]
    public class SequentialCookingDataSpawner : SequentialDataAsHelenHayesSpawner { }

    [System.Obsolete()]
    public class SequentialCookingDataConversionSystem : SequentialDataAsHelenHayesConversionSystemBase<SequentialCookingDataSpawner>
    {
        protected override void InitHelenHayes(NativeArray<Entity> entities)
        {
            var entity = DstEntityManager.CreateEntity(typeof(HelenHayesEntitiesHolder), typeof(HelenHayesPositions), typeof(HelenHayesPositionsFromEntitiesHolder));
            DstEntityManager.SetComponentData(entity, new HelenHayesEntitiesHolder
            {
                TopHead       = entities[0],
                FrontHead     = entities[1],
                RearHead      = entities[2],
                RightShoulder = entities[3],
                RightOffset   = entities[4],
                RightElbow    = entities[5],
                RightWrist    = entities[6],
                LeftShoulder  = entities[7],
                LeftElbow     = entities[8],
                LeftWrist     = entities[9],
                RightAsis     = entities[10],
                LeftAsis      = entities[11],
                VSacral       = entities[12],
                RightTight    = entities[13],
                RightKnee     = entities[14],
                RightShank    = entities[15],
                RightAnkleMed = entities[16],
                RightHeel     = entities[17],
                RightToe      = entities[18],
                LeftTight     = entities[19],
                LeftKnee      = entities[20],
                LeftShank     = entities[21],
                LeftAnkle     = entities[22],
                LeftHeel      = entities[23],
                LeftToe       = entities[24],
                RightKneeMed  = entities[25],
                RightAnkle    = entities[26],
                LeftKneeMed   = entities[27],
                LeftAnkleMed  = entities[28],
            });
        }
    }
}
