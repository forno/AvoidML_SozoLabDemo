using Forno.HelenHayes;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace AvoidML.Cooking
{
    [DisallowMultipleComponent]
    [RequiresEntityConversion]
    public class SequentialCookingDataSpawner : SequentialHelenHayesDataSpawner { }

    public class SequentialCookingDataConversionSystem : SequentialHelenHayesDataConversionSystemBase<SequentialCookingDataSpawner>
    {
        protected override void InitHelenHayes(NativeArray<Entity> entities)
        {
            var entity = DstEntityManager.CreateEntity(typeof(HelenHayesEntitiesHolder), typeof(HelenHayesPositions));
            DstEntityManager.SetComponentData(entity, new HelenHayesEntitiesHolder
            {
                TopHead       = entities[0],
                FrontHead     = entities[1],
                RearHead      = entities[2],
                RightShoulder = entities[3],
                RightOffset   = entities[4],
                RightElbow    = entities[5],
                LeftElbow     = entities[6],
                LeftShoulder  = entities[7],
                LeftWrist     = entities[8],
                RightWrist    = entities[9],
                RightAsis     = entities[10],
                LeftAsis      = entities[11],
                VSacral       = entities[12],
                RightTight    = entities[13],
                LeftTight     = entities[14],
                RightKnee     = entities[15],
                LeftKnee      = entities[16],
                LeftKneeMed   = entities[17],
                RightKneeMed  = entities[18],
                RightShank    = entities[19],
                LeftShank     = entities[20],
                RightAnkle    = entities[21],
                LeftAnkle     = entities[22],
                LeftAnkleMed  = entities[24],
                RightAnkleMed = entities[24],
                RightToe      = entities[25],
                LeftToe       = entities[26],
                LeftHeel      = entities[27],
                RightHeel     = entities[28],
            });
        }
    }
}
