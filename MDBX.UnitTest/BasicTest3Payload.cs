using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;


namespace MDBX.UnitTest
{
    [DataContract]
    public class BasicTest3Payload
    {
        [DataMember]
        public string Person { get; set; }

        [DataMember]
        public int Age { get; set; }
    }


    class BasicTest3PayloadSerializer : ISerializer<BasicTest3Payload>
    {
        public BasicTest3Payload Deserialize(byte[] buffer)
        {
            using (MemoryStream stream = new MemoryStream(buffer, false))
            {
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(BasicTest3Payload));
                return ser.ReadObject(stream) as BasicTest3Payload;
            }
        }

        public byte[] Serialize(BasicTest3Payload payload)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(BasicTest3Payload));
                ser.WriteObject(stream, payload);
                stream.Flush();
                stream.Position = 0;
                return stream.ToArray();
            }
        }
    }
}
