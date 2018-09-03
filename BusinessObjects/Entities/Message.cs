using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public sealed class Messages
    {
        public int MessageId { get; set; }
        public int SenderId { get; set; }
        public int RecieverId { get; set; }
        public string IP { get; set; }
        public bool IsRead { get; set; }
        public DateTime ? MessageDate { get; set; }
    }
    public sealed class MessageRecipeint
    {
        public int MsgRecipeint { get; set; }
        public string MessageContent { get; set; }

    }
}
