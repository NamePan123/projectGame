using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.VisualScripting;
using UnityEngine;
using Unity.Mathematics;
using System;
using Unity.Entities.UniversalDelegates;
partial struct BlockSystemMoveScript : ISystem
{
    private NativeArray<Entity> _entitys;
    private NativeArray<int> _datas;
    private int Witdh;
    private int Height;
    private double _lastUpdateTime;
    private double _index;
    private double _speed;
    private bool _lastIsDone;
    private Tetromino _currentDropData;

    private static readonly int[][] Shapes =
    {
        new int[] { 0,0,0,0, 1,1,1,1, 0,0,0,0, 0,0,0,0 }, // I
        new int[] { 0,1,1,0, 0,1,1,0, 0,0,0,0, 0,0,0,0 }, // O
        new int[] { 0,1,0,0, 1,1,1,0, 0,0,0,0, 0,0,0,0 }, // T
        new int[] { 0,1,1,0, 1,1,0,0, 0,0,0,0, 0,0,0,0 }, // S
        new int[] { 1,1,0,0, 0,1,1,0, 0,0,0,0, 0,0,0,0 }, // Z
        new int[] { 1,0,0,0, 1,1,1,0, 0,0,0,0, 0,0,0,0 }, // J
        new int[] { 0,0,1,0, 1,1,1,0, 0,0,0,0, 0,0,0,0 }  // L
    };

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        Witdh = 10;
        Height = 20;
        _speed = 1;
        _index = 0;
        _lastUpdateTime = SystemAPI.Time.ElapsedTime;
        _datas = new NativeArray<int>(Witdh * Height, Allocator.Persistent);
        _lastIsDone = true;
    }



    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        if (SystemAPI.Time.ElapsedTime - _lastUpdateTime > _speed)
        {
            _lastUpdateTime = SystemAPI.Time.ElapsedTime;
            _index++;
            if (_lastIsDone) Create();
            var poolEntity = SystemAPI.GetSingletonEntity<CubePoolComponent>();
            var pool = SystemAPI.GetSingletonRW<CubePoolComponent>();
            var buffer = state.EntityManager.GetBuffer<CubePoolElement>(poolEntity);
            Dropping();
            if (pool.ValueRW.CurrentIndex >= buffer.Length) pool.ValueRW.CurrentIndex = 0;
            //buffer[pool.ValueRW.CurrentIndex].Cube;
            pool.ValueRW.CurrentIndex++;
        }
    }

    [BurstCompile]
    private void Create()
    {
        _lastIsDone = false;
        Unity.Mathematics.Random random = new Unity.Mathematics.Random((uint)System.DateTime.Now.Ticks);
        var randomIndex = 0;//random.NextInt(0, Shapes.Length);
        _currentDropData = new Tetromino();
        _currentDropData.data = new int[16];
        //拷贝
        for (int i = 0; i < Shapes[randomIndex].Length; i++)
        {
            _currentDropData.data[i] = Shapes[randomIndex][i];
        }


        Debug.LogError(_currentDropData.GetValue(0, 0));
        Debug.LogError(_currentDropData.GetValue(1, 0));
        Debug.LogError(_currentDropData.GetValue(2, 0));
        Debug.LogError(_currentDropData.GetValue(3, 0));
    }

    [BurstCompile]
    private void Dropping()
    {
                                                                                        
    }



    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        if (_datas.IsCreated)
            _datas.Dispose();
    }
     
    public void SetData(int x, int y, int value)
    {
        int index = y * Witdh + x;
        _datas[index] = value;
    }
    
    public int GetData(int x, int y)
    {
        int index = y * Witdh + x;
        return _datas[index];
    }

    public int[] RotateShapeClockwise(int[] shape)
    {
        int[] result = new int[16];
        for (int y = 0; y < 4; y++)
        {
            for (int x = 0; x < 4; x++)
            {
                int srcIndex = y * 4 + x;
                int dstIndex = x * 4 + (3 - y);
                result[dstIndex] = shape[srcIndex];
            }
        }
        return result;        
    }

}


public struct Tetromino
{
    public int[] data;     // 长度为16的1维数组
    public int width;      // 宽度4
    public int height;     // 高度4
    public int posX;       // 当前在大地图上的 X 位置
    public int posY;       // 当前在大地图上的 Y 位置

    public int GetValue(int x, int y)
    {
        return data[y * width + x];
    }
}
