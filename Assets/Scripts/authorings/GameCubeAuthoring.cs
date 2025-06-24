using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;


public class GameCubeAuthoring : MonoBehaviour
{
    public GameObject Cube;
    private class GamekeyBuildBaker : Baker<GameCubeAuthoring>
    {
        public override void Bake(GameCubeAuthoring authoring)
        {
            Entity cube = GetEntity(authoring.Cube, TransformUsageFlags.Renderable);         
            Entity mainEntity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(mainEntity, new GameCubeAuthoringSingleton
            {
                Cube = cube
            }
            );

            AddComponent(mainEntity, new CubePoolComponent
            {
                CurrentIndex = 0
            }
          );

        }
    }


   

}

public struct GameCubeAuthoringSingleton : IComponentData
{
    public Entity Cube;

}

public struct CubeTag : IComponentData { }

public struct CubePoolComponent : IComponentData 
{
    public int CurrentIndex;
}
