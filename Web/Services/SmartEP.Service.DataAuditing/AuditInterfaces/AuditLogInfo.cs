﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartEP.Service.DataAuditing.AuditInterfaces
{
    public class AuditLogInfo
    {
        public DateTime tstamp
        {
            get;
            set;
        }
        public string PointName
        {
            get;
            set;
        }
        public string CreatUser
        {
            get;
            set;
        }
        public string AuditType
        {
            get;
            set;
        }

        public string PollutantName
        {
            get;
            set;
        }
        public string SourcePollutantDataValue
        {
            get;
            set;
        }
        public string AuditPollutantDataValue
        {
            get;
            set;
        }
        public string AuditReason
        {
            get;
            set;
        }
        public string Description
        {
            get;
            set;
        }
        public DateTime? AuditTime
        {
            get;
            set;
        }
        public string OperationTypeEnum
        {
            get;
            set;
        }
        public DateTime? UpdateTime
        {
            get;
            set;
        }
        public string InstrumentName
        {
            get;
            set;
        }
    }
}
