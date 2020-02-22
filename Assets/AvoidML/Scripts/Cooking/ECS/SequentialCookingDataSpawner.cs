using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using Forno.HelenHayes;

namespace AvoidML.Cooking
{
    [DisallowMultipleComponent]
    [RequiresEntityConversion]
    public class SequentialCookingDataSpawner : SequentialHelenHayesDataSpawner { }

    public class SequentialCookingDataConversionSystem : SequentialHelenHayesDataConversionSystemBase<SequentialCookingDataSpawner>
    {
        protected override void InitHelenHayes(NativeArray<Entity> entities)
        {
            var entity = DstEntityManager.CreateEntity(typeof(HelenHayesEntitiesHolder));
            DstEntityManager.SetComponentData(entity, new HelenHayesEntitiesHolder
            {
                FrontHead = entities[0],
                TopHead = entities[1],
                RearHead = entities[2],
                RightShoulder = entities[3],
                LeftShoulder = entities[4],
                RightOffset = entities[5],
                LeftElbow = entities[6],
                RightElbow = entities[7],
                LeftWrist = entities[8],
                RightWrist = entities[9],
                RightAsis = entities[10],
                LeftAsis = entities[11],
                VSacral = entities[12],
                RightTight = entities[13],
                LeftTight = entities[14],
                RightKnee = entities[15],
                LeftKnee = entities[16],
                LeftKneeMed = entities[17],
                RightKneeMed = entities[18],
                RightShank = entities[19],
                LeftShank = entities[20],
                RightAnkle = entities[21],
                LeftAnkle = entities[22],
                LeftAnkleMed = entities[24],
                RightAnkleMed = entities[24],
                RightToe = entities[25],
                LeftToe = entities[26],
                LeftToeMed = entities[27],
                RightToeMed = entities[28],
            });
        }
    }
}
