using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOs;

public class Message
{
    public int MessageId { get; set; }
    public int ConservationId { get; set; }
    public string ModelResponse { get; set; }
    public string UserResquest { get; set; }
    public Conversation Conservation { get; set; }


}
