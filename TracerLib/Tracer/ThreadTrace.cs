﻿using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace TracerLib.Tracer
{
    
    [XmlType("thread")]
    public class ThreadTrace
    {
        [DataMember] [JsonProperty,XmlAttribute("id")] public int ThreadId { get; set; }
        [DataMember] [JsonProperty,XmlAttribute("time")] public long ThreadTime { get; set; }
        [DataMember] [JsonProperty,XmlElement("methods")] public List<MethodInfo> MethodInfo { get; set; }

        public ThreadTrace(int threadId)                                                                                                        
        {
            ThreadId = threadId;
            MethodInfo = new List<MethodInfo>();
        }
        public ThreadTrace () {}

        public void PushMethod(string methodName, string className, string allMethodPath)
        {
            MethodInfo.Add(new MethodInfo(methodName, className, allMethodPath));
        }

        public void PopMethod(string allMethodPath)
        {
            var index = MethodInfo.FindLastIndex(item => item.GetAllMethodPath() == allMethodPath);
            ThreadTime += MethodInfo[index].GetTime();
            MethodInfo[index].CalculateTime();
        }

    }
    
}