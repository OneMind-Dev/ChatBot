using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOs;

public class Message
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int MessageId { get; set; }
    public int ConservationId { get; set; }
    public string ModelResponse { get; set; }
    public string UserResquest { get; set; }

    public Conversation Conservation { get; set; }
}
