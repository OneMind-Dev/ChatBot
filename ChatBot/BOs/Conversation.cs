using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOs;

public class Conversation
{
    public int ConversationId { get; set; }
    public string ConservationName { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
}
