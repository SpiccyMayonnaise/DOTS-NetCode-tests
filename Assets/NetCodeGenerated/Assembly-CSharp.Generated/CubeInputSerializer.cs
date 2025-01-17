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
    public struct CubeInputSerializer : ICommandDataSerializer<CubeInput>
    {
        public void Serialize(ref DataStreamWriter writer, in CubeInput data)
        {
            writer.WriteInt((int) data.horizontal);
            writer.WriteInt((int) data.vertical);
            writer.WriteUInt(data.up ? 1u : 0);
        }

        public void Deserialize(ref DataStreamReader reader, ref CubeInput data)
        {
            data.horizontal = (int) reader.ReadInt();
            data.vertical = (int) reader.ReadInt();
            data.up = (reader.ReadUInt() != 0) ? true : false;
        }

        public void Serialize(ref DataStreamWriter writer, in CubeInput data, in CubeInput baseline, NetworkCompressionModel compressionModel)
        {
            writer.WritePackedIntDelta((int) data.horizontal, (int) baseline.horizontal, compressionModel);
            writer.WritePackedIntDelta((int) data.vertical, (int) baseline.vertical, compressionModel);
            writer.WritePackedUInt(data.up ? 1u : 0, compressionModel);
        }

        public void Deserialize(ref DataStreamReader reader, ref CubeInput data, in CubeInput baseline, NetworkCompressionModel compressionModel)
        {
            data.horizontal = (int) reader.ReadPackedIntDelta((int) baseline.horizontal, compressionModel);
            data.vertical = (int) reader.ReadPackedIntDelta((int) baseline.vertical, compressionModel);
            data.up = (reader.ReadPackedUInt(compressionModel) != 0) ? true : false;
        }
    }
    public class CubeInputSendCommandSystem : CommandSendSystem<CubeInputSerializer, CubeInput>
    {
        [BurstCompile]
        struct SendJob : IJobEntityBatch
        {
            public SendJobData data;
            public void Execute(ArchetypeChunk chunk, int orderIndex)
            {
                data.Execute(chunk, orderIndex);
            }
        }
        protected override void OnUpdate()
        {
            var sendJob = new SendJob{data = InitJobData()};
            ScheduleJobData(sendJob);
        }
    }
    public class CubeInputReceiveCommandSystem : CommandReceiveSystem<CubeInputSerializer, CubeInput>
    {
        [BurstCompile]
        struct ReceiveJob : IJobEntityBatch
        {
            public ReceiveJobData data;
            public void Execute(ArchetypeChunk chunk, int orderIndex)
            {
                data.Execute(chunk, orderIndex);
            }
        }
        protected override void OnUpdate()
        {
            var recvJob = new ReceiveJob{data = InitJobData()};
            ScheduleJobData(recvJob);
        }
    }
}
