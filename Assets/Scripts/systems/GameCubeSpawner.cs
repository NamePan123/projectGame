using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

partial struct GameCubeSpawner : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {

    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var entityManager = state.EntityManager;

        var singleton = SystemAPI.GetSingleton<GameCubeAuthoringSingleton>();

        var query = state.GetEntityQuery(ComponentType.ReadOnly<GameCubeAuthoringSingleton>());
        var mainEntity = query.GetSingletonEntity();

        var buffer = entityManager.AddBuffer<CubePoolElement>(mainEntity);


        for (int i = 0; i < 10000; i++)
        {
            Entity cube = CreateKey(entityManager.Instantiate(singleton.Cube), state.EntityManager, i);
            buffer.Add(new CubePoolElement { Cube = cube});
        }

        state.Enabled = false;
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }

    private Entity CreateKey(Entity key, EntityManager manager, int index)
    {     
        var transform = manager.GetComponentData<LocalTransform>(key);
        transform.Position = new float3(-1000, 0, 0);
        manager.SetComponentData(key, transform);
        return key;
    }

}
