using System;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Assertions;

namespace Forno.HelenHayes
{
    // This need prefab. You shuold attach it from MonoBehaviour
    [DisableAutoCreation]
    public class HelenHayesModelRendererSystem : SystemBase
    {
        public RigBuilder Prefab;
        public int RigIndex;
        public float3 HipsOffset;
        private GameObject parent;
        private List<HelenHayesRig> models;
        private EntityQuery query;

        protected override void OnCreate()
        {
            parent = new GameObject("HelenHayesModelRendererSystem Managed Object");
            models = new List<HelenHayesRig>();
        }

        protected override void OnUpdate()
        {
            var modelCount = query.CalculateEntityCount();
            if (models.Count > modelCount) {
                for (var i = modelCount; i < models.Count; ++i) {
                    models[i].Dispose();
                }
                models.RemoveRange(modelCount, models.Count - modelCount);
            } else if (models.Count < modelCount) {
                for (var i = models.Count; i < modelCount; ++i) {
                    var instance = UnityEngine.Object.Instantiate(Prefab);
                    instance.transform.parent = parent.transform;
                    var rig = new HelenHayesRig(instance, RigIndex);
                    models.Add(rig);
                }
            }
            Assert.AreEqual(models.Count, modelCount);

            var curIndex = 0;
            Entities
                .WithStoreEntityQueryInField(ref query)
                .WithoutBurst()
                .ForEach((in HelenHayesPositions positions) =>
                {
                    var model = models[curIndex];

                    var hipsPosition = positions.LeftAsis / 2 + positions.RightAsis / 2 + HipsOffset;
                    model.Hips.position = hipsPosition;

                    var hips2Up = positions.FrontHead - hipsPosition;
                    var hipVector = positions.RightAsis - positions.LeftAsis;
                    model.Hips.rotation = Quaternion.LookRotation(Quaternion.AngleAxis(-90, Vector3.Cross(hipVector, Vector3.forward)) * hipVector, hips2Up);
                    model.Spine.rotation = Quaternion.LookRotation(Quaternion.AngleAxis(-90, Vector3.Cross(hips2Up, Vector3.forward)) * hips2Up, hips2Up);

                    model.LeftArmTarget.position = positions.LeftWrist;
                    model.LeftArmHint.position = positions.LeftElbow;
                    model.LeftArmTarget.rotation = Quaternion.AngleAxis(90, Vector3.up) * Quaternion.LookRotation(positions.LeftWrist - positions.LeftElbow);
                    model.LeftArmHint.rotation = Quaternion.LookRotation(positions.LeftElbow - positions.LeftShoulder);

                    model.RightArmTarget.position = positions.RightWrist;
                    model.RightArmHint.position = positions.RightElbow;
                    model.RightArmTarget.rotation = Quaternion.AngleAxis(-90, Vector3.up) * Quaternion.LookRotation(positions.RightWrist - positions.RightElbow);
                    model.RightArmHint.rotation = Quaternion.LookRotation(positions.RightElbow - positions.RightShoulder);

                    var leftFoot = positions.LeftAnkle / 2 + positions.LeftAnkleMed / 2;
                    model.LeftLegTarget.position = leftFoot;
                    model.LeftLegHint.position = positions.LeftKnee / 2 + positions.LeftKneeMed / 2;
                    model.LeftLegTarget.rotation = Quaternion.LookRotation(positions.LeftToe - leftFoot);
                    model.LeftLegHint.rotation = Quaternion.LookRotation(positions.LeftAsis - positions.LeftToe);

                    var rightFoot = positions.RightAnkle / 2 + positions.RightAnkleMed / 2;
                    model.RightLegTarget.position = rightFoot;
                    model.RightLegHint.position = positions.RightKnee / 2 + positions.RightKneeMed / 2;
                    model.RightLegTarget.rotation = Quaternion.LookRotation(positions.RightToe - rightFoot);
                    model.RightLegHint.rotation = Quaternion.LookRotation(positions.RightAsis - positions.RightToe);
                }).Run();
        }
    }

    public class HelenHayesModelRenderer : MonoBehaviour
    {
        public RigBuilder Prefab;
        public int RigIndex;
        public float3 HipOffset;

        public void Start()
        {
            var world = World.DefaultGameObjectInjectionWorld;
            var helenHayesModelRendererSystem = world.GetOrCreateSystem<HelenHayesModelRendererSystem>();
            helenHayesModelRendererSystem.Prefab = Prefab;
            helenHayesModelRendererSystem.RigIndex = RigIndex;
            helenHayesModelRendererSystem.HipsOffset = HipOffset;
            var simulationSystemGroup = world.GetOrCreateSystem<SimulationSystemGroup>();
            simulationSystemGroup.AddSystemToUpdateList(helenHayesModelRendererSystem);
            simulationSystemGroup.SortSystemUpdateList();
            // Destroy itself
            Destroy(this);
        }
    }

    public class HelenHayesRig : IDisposable
    {
        public GameObject root;
        public Transform Hips;
        public Transform Spine;
        public Transform LeftArmTarget;
        public Transform LeftArmHint;
        public Transform RightArmTarget;
        public Transform RightArmHint;
        public Transform LeftLegTarget;
        public Transform LeftLegHint;
        public Transform RightLegTarget;
        public Transform RightLegHint;

        public HelenHayesRig(RigBuilder instance, int rigIndex = 0)
        {
            var rig = instance.layers[rigIndex].rig;
            var multiParentConstraint = rig.GetComponentsInChildren<MultiParentConstraint>();
            foreach (var constraint in multiParentConstraint) {
                if (constraint.CompareTag("Hips")) {
                    Hips = constraint.data.sourceObjects[0].transform;
                }
            }
            var multiRotationConstraint = rig.GetComponentsInChildren<MultiRotationConstraint>();
            foreach (var constraint in multiRotationConstraint) {
                if (constraint.CompareTag("Spine")) {
                    Spine = constraint.data.sourceObjects[0].transform;
                }
            }
            var twoBoneIKConstraints = rig.GetComponentsInChildren<TwoBoneIKConstraint>();
            foreach (var constraint in twoBoneIKConstraints) {
                if (constraint.CompareTag("LeftArm")) {
                    LeftArmTarget = constraint.data.target;
                    LeftArmHint = constraint.data.hint;
                } else if (constraint.CompareTag("RightArm")) {
                    RightArmTarget = constraint.data.target;
                    RightArmHint = constraint.data.hint;
                } else if (constraint.CompareTag("LeftLeg")) {
                    LeftLegTarget = constraint.data.target;
                    LeftLegHint = constraint.data.hint;
                } else if (constraint.CompareTag("RightLeg")) {
                    RightLegTarget = constraint.data.target;
                    RightLegHint = constraint.data.hint;
                }
            }
        }

        public void Dispose()
        {
            UnityEngine.Object.Destroy(root);
        }
    }
}
