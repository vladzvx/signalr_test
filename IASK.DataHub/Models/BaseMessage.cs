using IASK.DataHub.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace IASK.DataHub.Models
{
    public abstract class BaseMessage
    {
        public DateTime DateTime { get; set; } = DateTime.UtcNow;
        public long Id { get; set; }
        public long GroupId { get; set; }
        public long UserId { get; set; }
        public MessageType MessageType { get; set; }


        public string Text { get; set; }
        public virtual void SetAcknowledged()
        {
            MessageType = MessageType.Acknowledged;
            Text = null;
        }
    }
}
