//THIS FILE IS AUTOGENERATED BY GHOSTCOMPILER. DON'T MODIFY OR ALTER.
using AOT;
using Unity.Burst;
using Unity.Networking.Transport;
using Unity.Entities;
using Unity.Collections;
using Unity.NetCode;
using Unity.Transforms;
using Unity.Mathematics;


namespace Assembly_CSharp.Generated
{
    [BurstCompile]
    public struct TestRPCCommandSerializer : IComponentData, IRpcCommandSerializer<TestRPCCommand>
    {
        public void Serialize(ref DataStreamWriter writer, in TestRPCCommand data)
        {
            writer.WriteInt((int) data.data);
        }

        public void Deserialize(ref DataStreamReader reader, ref TestRPCCommand data)
        {
            data.data = (int) reader.ReadInt();
        }
        [BurstCompile]
        [MonoPInvokeCallback(typeof(RpcExecutor.ExecuteDelegate))]
        private static void InvokeExecute(ref RpcExecutor.Parameters parameters)
        {
            RpcExecutor.ExecuteCreateRequestComponent<TestRPCCommandSerializer, TestRPCCommand>(ref parameters);
        }

        static PortableFunctionPointer<RpcExecutor.ExecuteDelegate> InvokeExecuteFunctionPointer =
            new PortableFunctionPointer<RpcExecutor.ExecuteDelegate>(InvokeExecute);
        public PortableFunctionPointer<RpcExecutor.ExecuteDelegate> CompileExecute()
        {
            return InvokeExecuteFunctionPointer;
        }
    }
    class TestRPCCommandRpcCommandRequestSystem : RpcCommandRequestSystem<TestRPCCommandSerializer, TestRPCCommand>
    {
        [BurstCompile]
        protected struct SendRpc : IJobEntityBatch
        {
            public SendRpcData data;
            public void Execute(ArchetypeChunk chunk, int orderIndex)
            {
                data.Execute(chunk, orderIndex);
            }
        }
        protected override void OnUpdate()
        {
            var sendJob = new SendRpc{data = InitJobData()};
            ScheduleJobData(sendJob);
        }
    }
}